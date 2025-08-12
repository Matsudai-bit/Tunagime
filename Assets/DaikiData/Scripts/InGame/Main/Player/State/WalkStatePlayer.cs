using UnityEngine;

public class WalkStatePlayer : PlayerState
{
    public WalkStatePlayer(Player owner) : base(owner)
    {

    }
    /// <summary>
    /// 歩行状態の開始時に一度だけ呼ばれる
    /// </summary>
    public override void OnStartState()
    {
        // 歩行状態の開始時にアニメーションを設定
        owner.GetAnimator().SetBool("Walk", true);
    }

    /// <summary>
    /// 歩行状態中のUpdateで毎フレーム呼ばれる
    /// </summary>
    public override void OnUpdateState()
    {
        // 持ち運ぶオブジェクトを拾う処理を試みる
        owner.TryPickUp();
        // 持ち運ぶオブジェクトを置く処理を試みる
        owner.TryPutDown();
      
        if (owner.GetCarryingObject() != null)
        {
            // test
            owner.TryForwardFloorSetting();
            // 編む処理を試みる
            owner.TryKnit();
        }
        else
        {
            // test
            owner.TryForwardObjectSetting();

            owner.TryPushBlock();

            owner.TryUnknit();
        }

    }
    /// <summary>
    /// 歩行状態中のFixedUpdateで物理演算フレームごとに呼ばれる
    /// </summary>
    public override void OnFixedUpdateState()
    {
     
        // プレイヤーの移動処理
        if (owner.IsMoving())
        {
            // 移動処理
            owner.Move();
        }
        else
        {
            // 歩行状態から待機状態に遷移
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);
            // 移動を停止
            owner.StopMove();
        }
    }

    /// <summary>
    /// 歩行状態の終了時に一度だけ呼ばれる
    /// </summary>
    public override void OnFinishState()
    {
        // 歩行状態の終了時にアニメーションをリセット
        owner.GetAnimator().SetBool("Walk", false);

        // 移動を停止
        owner.StopMove();
    }


}
