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
        // 滑る状態の開始時にアニメーションを設定
        owner.GetAnimator().SetBool("Slide", true);

        // プレイヤーの移動方向の取得
        Vector3 velocityNormal = owner.GetPreviousMoveVelocity();
        velocityNormal.Normalize();
        m_directionBaseGrid = (Mathf.Abs(velocityNormal.x) > Mathf.Abs(velocityNormal.z))
          ? new GridPos((int)Mathf.Round(velocityNormal.x), 0)
          : new GridPos(0, -(int)Mathf.Round(velocityNormal.z));

        owner.StopMove(); // 物理移動を停止

        Debug.Log(m_directionBaseGrid.x + "," + m_directionBaseGrid.y);
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
