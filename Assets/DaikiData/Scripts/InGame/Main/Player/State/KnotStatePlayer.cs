using UnityEngine;

public class KnotStatePlayer : PlayerState
{
    public KnotStatePlayer(Player owner) : base(owner)
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
        //// アニメーションの終了を待つ
        //if (owner.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        //{
        //    // アニメーションが終了したら持ち運ぶオブジェクトを拾う処理を実行
        //    PickDown();
        //    // 待機状態に遷移
        //    owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);


        //    // 持ち運ぶオブジェクトを持っている場合、アニメーションレイヤーを有効化
        //    owner.GetAnimator().SetLayerWeight(owner.GetAnimator().GetLayerIndex("TakeFluffBall"), 0);
        //}

        if (owner.CanKnit())
            Knot();
        // 待機状態に遷移
        owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);

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

    /// <summary>
    /// 編む
    /// </summary>
    private void Knot()
    {
        // 置く位置
        GridPos knottingPos = owner.GetForwardGridPos(); // 前方のグリッド位置
        // アミダ橋の生成
        var generateAmida =AmidaTubeGenerator.GetInstance.GenerateAmidaBridge(knottingPos);


        owner.DropCarryingObject();

        // 持ち運ぶオブジェクトを持つアニメーションレイヤーを無効化
        owner.GetAnimator().SetLayerWeight(owner.GetAnimator().GetLayerIndex("TakeFluffBall"), 0); 
        
    }




}
