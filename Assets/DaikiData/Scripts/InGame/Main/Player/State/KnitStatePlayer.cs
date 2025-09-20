using UnityEngine;

/// <summary>
/// 解く状態のプレイヤー
/// </summary>
public class KnitStatePlayer : PlayerState
{

    private AnimationEventHandler m_animationEventHandler; // アニメーションイベントハンドラー

    private GameObject m_fluffBallObject; // 編む対象のフラッフボール

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
        // 移動を停止
        owner.StopMove();

        // レイヤーの変更中フラグをリセット
        m_animationEventHandler.PlayAnimationTrigger("Knit", "Normal", "Knit");

        GridPos knittingPos = owner.GetForwardGridPos(); // 前方のグリッド位置

        // 正面に毛糸玉があるかどうか
        m_fluffBallObject = MapData.GetInstance.GetStageGridData().GetTileObject(knittingPos).gameObject;

        if (m_fluffBallObject == null || m_fluffBallObject.GetComponent<FluffBall>() == null)
        {
            // 正面に毛糸玉がない場合は待機状態に遷移
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);
            return;
        }

        //// レイヤーのウェイトを変更するためのコールバックを設定
        //// Carryレイヤーのウェイトを0に設定
        //m_animationEventHandler.SetTargetTimeAction(0.9f, () => { owner.RequestTransitionLayerWeight("Carry", 0, 0.8f); });
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
        var stageBlock = m_fluffBallObject.GetComponent<StageBlock>();

        if (stageBlock == null)
        {
            Debug.LogError("FluffBall must be attached to a GameObject with a StageBlock component.");
            return;
        }

        // 編む位置
        GridPos knittingPos = stageBlock.GetGridPos();
        // アミダ橋の生成
        var generateAmida = AmidaTubeGenerator.GetInstance.GenerateAmidaBridge(knittingPos);

        m_fluffBallObject.SetActive(false); // 毛糸玉を非表示にする

        MapData.GetInstance.GetStageGridData().RemoveGridDataGameObject(knittingPos); // グリッドデータから毛糸玉を削除

    }




}
