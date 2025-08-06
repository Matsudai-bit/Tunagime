using UnityEngine;

public class PickDownStatePlayer : PlayerState
{
    public PickDownStatePlayer(Player owner) : base(owner)
    {

    }
    /// <summary>
    /// 歩行状態の開始時に一度だけ呼ばれる
    /// </summary>
    public override void OnStartState()
    {
        //owner.GetAnimator().SetTrigger("PickDown"); // 持ち運ぶオブジェクトを拾うアニメーションをトリガー


        
    }

    /// <summary>
    /// 歩行状態中のUpdateで毎フレーム呼ばれる
    /// </summary>
    public override void OnUpdateState()
    {
        // アニメーションの終了を待つ
        //if (owner.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        //{
            // アニメーションが終了したら持ち運ぶオブジェクトを拾う処理を実行
            PickDown();
            // 待機状態に遷移
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);


            // 持ち運ぶオブジェクトを持っている場合、アニメーションレイヤーを有効化
            owner.GetAnimator().SetLayerWeight(owner.GetAnimator().GetLayerIndex("TakeFluffBall"), 0);
        //}

     
    }
    /// <summary>
    /// 歩行状態中のFixedUpdateで物理演算フレームごとに呼ばれる
    /// </summary>
    public override void OnFixedUpdateState()
    {
     

    }

    /// <summary>
    /// 歩行状態の終了時に一度だけ呼ばれる
    /// </summary>
    public override void OnFinishState()
    {

    }

    private void PickDown()
    {

        var map = MapData.GetInstance; // マップを取得

        // 置く位置
        GridPos placementPos = owner.GetForwardGridPos(); // 前方のグリッド位置

        // 運んでいるオブジェクトを取得
        var carryingObject = owner.GetCarryingObject(); 

        // 持ち運ぶオブジェクトを置く処理を実行
        carryingObject.OnDrop(placementPos); 

       
        owner.SetCarryingObject(null); // 持ち運ぶオブジェクトを解放
    }


}
