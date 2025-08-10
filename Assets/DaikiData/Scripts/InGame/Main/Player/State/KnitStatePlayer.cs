using UnityEngine;

/// <summary>
/// 解く状態のプレイヤー
/// </summary>
public class KnitStatePlayer : PlayerState
{

    private AnimationEventHandler m_animationEventHandler; // アニメーションイベントハンドラー

    public KnitStatePlayer(Player owner) : base(owner)
    {
        // アニメーションイベントハンドラーを初期化
        m_animationEventHandler = new AnimationEventHandler(owner.GetAnimator());
    }
    /// <summary>
    /// 歩行状態の開始時に一度だけ呼ばれる
    /// </summary>
    public override void OnStartState()
    {
        // レイヤーの変更中フラグをリセット
        m_animationEventHandler.PlayAnimationTrigger("Knit", "Carry", "Knit"); // 置くアニメーションを再生

        // レイヤーのウェイトを変更するためのコールバックを設定
        // Carryレイヤーのウェイトを0に設定
        m_animationEventHandler.SetTargetTimeAction(0.9f, () => { owner.RequestTransitionLayerWeight("Carry", 0, 0.8f); });
    }

    /// <summary>
    /// 歩行状態中のUpdateで毎フレーム呼ばれる
    /// </summary>
    public override void OnUpdateState()
    {

        // アニメーションイベントハンドラーの更新
        m_animationEventHandler.OnUpdate();

        if (m_animationEventHandler.HasAnimationPlayed() && m_animationEventHandler.IsPlaying() == false)
        {

            if (owner.CanKnit())
                // アニメーションが終了したら編む処理を実行
                FinishKnit();
            // 待機状態に遷移
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);
        }

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
    private void FinishKnit()
    {
        // 置く位置
        GridPos knottingPos = owner.GetForwardGridPos(); // 前方のグリッド位置
        // アミダ橋の生成
        var generateAmida = AmidaTubeGenerator.GetInstance.GenerateAmidaBridge(knottingPos);


        owner.DropCarryingObject();


    }




}
