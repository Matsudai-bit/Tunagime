using UnityEngine;

/// <summary>
/// 滑る状態
/// </summary>
public class SlipperStatePlayer : PlayerState
{
    public GridPos m_directionBaseGrid; // グリッド基準のスライドする方向の

    private readonly float SLIDE_SPEED = 2.0f; // スライド速度 1秒に動く量

    public SlipperStatePlayer(Player owner) : base(owner)
    {

    }
    /// <summary>
    /// 滑る状態の開始時に一度だけ呼ばれる
    /// </summary>
    public override void OnStartState()
    {
        var map = MapData.GetInstance;



        // 滑る状態を開始し、アニメーションを再生します
        owner.GetAnimator().SetBool("Slide", true);
        owner.StopMove(); // 物理移動を即座に停止します

        // プレイヤーの移動方向から主要な方向を特定します
        // XとZ軸のどちらがより支配的かを判断します
        Vector3 velocityNormal = owner.GetPreviousMoveVelocity();
        velocityNormal.Normalize();
        bool isMovingHorizontally = Mathf.Abs(velocityNormal.x) > Mathf.Abs(velocityNormal.z);

        // 特定された方向に合わせて、スライドの基準となるグリッド方向を設定します
        m_directionBaseGrid = isMovingHorizontally
            ? new GridPos((int)Mathf.Round(velocityNormal.x), 0)
            : new GridPos(0, -(int)Mathf.Round(velocityNormal.z));

        // 現在位置を最も近いグリッドの中心に調整します
        // スライド方向と垂直な軸上でのみ、位置をグリッドの中心に合わせます
        var currentGridPos = map.GetClosestGridPos(owner.transform.position);
        Vector3 targetWorldPos = map.ConvertGridToWorldPos(currentGridPos);

        float newX = owner.transform.position.x;
        float newZ = owner.transform.position.z;

        // スライド方向が横（X軸）の場合、Z座標をグリッドの中心に合わせる
        if (isMovingHorizontally)
        {
            newZ = targetWorldPos.z;
        }
        // スライド方向が縦（Z軸）の場合、X座標をグリッドの中心に合わせる
        else
        {
            newX = targetWorldPos.x;
        }

        owner.transform.position = new Vector3(newX, owner.transform.position.y, newZ);

    }

    /// <summary>
    /// 滑る状態中のUpdateで毎フレーム呼ばれる
    /// </summary>
    public override void OnUpdateState()
    {
        if (CanFinish())
        {
            // 滑る状態から待機状態に遷移
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);
         
        }
    }
    /// 滑る状態中のFixedUpdateで物理演算フレームごとに呼ばれる
    /// </summary>
    public override void OnFixedUpdateState()
    {
        Slide();
    }

    /// <summary>
    /// 滑る状態の終了時に一度だけ呼ばれる
    /// </summary>
    public override void OnFinishState()
    {
        // 滑る状態の終了時にアニメーションをリセット
        owner.GetAnimator().SetBool("Slide", false);

        // 移動を停止
        owner.StopMove();
    }


    /// <summary>
    /// プレイヤーを指定した方向にスライドさせる。
    /// </summary>
    /// <param name="direction">  </param>
    public void Slide()
    {
        Vector3 direction = new Vector3(m_directionBaseGrid.x, 0, -m_directionBaseGrid.y);

        // プレイヤーの位置を更新
        owner.transform.position += direction * SLIDE_SPEED * Time.deltaTime;
    }

    /// <summary>
    /// 滑る状態を終了できるかどうかをチェックする。
    /// </summary>
    /// <returns></returns>
    public bool CanFinish()
    {

        // **** 床の種類でチェック ****
        var gridPos = owner.GetGridPosition();

        var stageGrid = MapData.GetInstance.GetStageGridData();

        var currentTileFloor = stageGrid.GetFloorObject(gridPos);

        var satainFloorOfCurrentTile = currentTileFloor?.GetComponent<SatinFloor>();
        if (!satainFloorOfCurrentTile)
        {
            return true;
        }

        // **** 前方3方向のレイに当たったオブジェクトでチェック ****
        RaycastHit[] hits = new RaycastHit[3];
        // 前方3方向にレイを飛ばして、当たったオブジェクトを取得
        for (int i = 0; i < 3; i++)

        {
            int offset = i - 1; // -1, 0, 1 のオフセットを使用


            Vector3 direction = new Vector3(m_directionBaseGrid.x, 0, -m_directionBaseGrid.y);
            direction.Normalize();

            direction = Quaternion.Euler(0, offset * 45, 0) * direction; // 90度ずつ回転

            Debug.DrawRay(owner.transform.position, direction, Color.red, 0.5f); // デバッグ用のレイ表示

            if (Physics.Raycast(owner.transform.position, direction, out hits[i], 0.5f))
            {
                // 当たったオブジェクトのタグをチェック
                if (hits[i].collider?.gameObject.GetComponent<StageBlock>())
                {
                    return true; // 滑る状態を終了できる
                }
            }
        }


        return false;
    }


}
