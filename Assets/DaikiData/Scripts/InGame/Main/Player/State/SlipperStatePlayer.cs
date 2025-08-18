using UnityEngine;

/// <summary>
/// 滑る状態
/// </summary>
public class SlipperStatePlayer : PlayerState
{
    public GridPos m_directionBaseGrid; // グリッド基準のスライドする方向の

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
        Vector3 velocityNormal = owner.GetRigidbody().linearVelocity.normalized;

        // プレイヤーの移動方向をグリッド基準に変換
        m_directionBaseGrid = new GridPos(
            Mathf.RoundToInt(velocityNormal.x),
            Mathf.RoundToInt(-velocityNormal.z)
        );
    }

    /// <summary>
    /// 滑る状態中のUpdateで毎フレーム呼ばれる
    /// </summary>
    public override void OnUpdateState()
    {
        //// 持ち運ぶオブジェクトを拾う処理を試みる
        //owner.TryPickUp();
        //// 持ち運ぶオブジェクトを置く処理を試みる
        //owner.TryPutDown();
      
        //if (owner.GetCarryingObject() != null)
        //{
        //    // test
        //    owner.TryForwardFloorSetting();
        //    // 編む処理を試みる
        //    owner.TryKnit();
        //}
        //else
        //{
        //    // test
        //    owner.TryForwardObjectSetting();

        //    owner.TryPushBlock();

        //    owner.TryUnknit();
        //}

    }
    /// <summary>
    /// 滑る状態中のFixedUpdateで物理演算フレームごとに呼ばれる
    /// </summary>
    public override void OnFixedUpdateState()
    {
        Slide();

        //// プレイヤーの移動処理
        //if (owner.IsMoving())
        //{
        //    // 移動処理
        //    owner.Move();
        //}
        //else
        //{
        //    // 滑る状態から待機状態に遷移
        //    owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);
        //    // 移動を停止
        //    owner.StopMove();
        //}
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
        owner.transform.position += direction * 2.0f * Time.deltaTime;
    }


}
