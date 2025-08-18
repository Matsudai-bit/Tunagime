using UnityEngine;

public class PutDownStatePlayer : PlayerState
{

    private AnimationEventHandler m_animationEventHandler; // アニメーションイベントハンドラー

    public PutDownStatePlayer(Player owner) : base(owner)
    {
        // アニメーションイベントハンドラーを初期化
        m_animationEventHandler = new AnimationEventHandler(owner.GetAnimator());
    }
    /// <summary>
    /// 歩行状態の開始時に一度だけ呼ばれる
    /// </summary>
    public override void OnStartState()
    {
        // 移動を停止
        owner.StopMove();
        StartPutDown();

      

    }

    /// <summary>
    /// 歩行状態中のUpdateで毎フレーム呼ばれる
    /// </summary>
    public override void OnUpdateState()
    {
    
        m_animationEventHandler.OnUpdate(); // アニメーションイベントハンドラーの更新


        // アニメーションの終了を待つ
        if ( m_animationEventHandler.HasAnimationPlayed() && m_animationEventHandler.IsPlaying() == false)
        {
            // アニメーションが終了したら持ち運ぶオブジェクトを拾う処理を実行
            ApplyPutDown();
            // 待機状態に遷移
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);


         
        }


    }


    /// <summary>
    /// 持ち運ぶオブジェクトを置く処理を開始する
    /// </summary>
    private void StartPutDown()
    {
        // レイヤーの変更中フラグをリセット
        m_animationEventHandler.PlayAnimationTrigger("PutDown", "Carry", "PutDown"); // 置くアニメーションを再生

        // レイヤーのウェイトを変更するためのコールバックを設定
        m_animationEventHandler.SetTargetTimeAction(0.7f, () => { owner.RequestTransitionLayerWeight("Carry", 0, 0.4f); }); // Carryレイヤーのウェイトを0に設定
    }

    /// 持ち運ぶオブジェクトを置く処理を実行する
    /// </summary>
    private void ApplyPutDown()
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



}
