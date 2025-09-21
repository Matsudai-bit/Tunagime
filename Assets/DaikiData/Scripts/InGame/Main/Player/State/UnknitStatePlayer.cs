using UnityEngine;

/// <summary>
/// 
/// </summary>
public class UnknitStatePlayer : PlayerState
{

    private AnimationEventHandler m_animationEventHandler; // アニメーションイベントハンドラー
    private GameObject m_targetObject = null;

    public UnknitStatePlayer(Player owner) : base(owner)
    {
        // アニメーションイベントハンドラーを初期化
        m_animationEventHandler = new AnimationEventHandler(owner.GetAnimator());
    }
    /// <summary>
    /// 歩行状態の開始時に一度だけ呼ばれる
    /// </summary>
    public override void OnStartState()
    {
        m_targetObject = owner.GetTargetObject();

        if (m_targetObject == null)
        {
            Debug.LogError("Target object not found.");
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE); // 対象オブジェクトが見つからない場合はIDLE状態に戻る
            return; // 対象オブジェクトが見つからない場合は処理を中断
        }

        // 移動を停止
        owner.StopMove();
        // レイヤーの変更中フラグをリセット
        m_animationEventHandler.PlayAnimationTrigger("Unknit", "Normal", "Unknit"); // 解くアニメーションを再生
                                                                                    // レイヤーのウェイトを変更するためのコールバックを設定
      //  m_animationEventHandler.SetTargetTimeAction(0.7f, () => { owner.RequestTransitionLayerWeight("Carry", 1, 0.6f); }); // Carryレイヤーのウェイトを0に設定


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

            // アニメーションが終了したら編む処理を実行
            FinishUnknit();
       

            // 毛糸だまを生成
            var stageObjectFactory = StageObjectFactory.GetInstance();

            // 前方のあみだに取得
            var frontAmidaPos = m_targetObject.GetComponent<StageBlock>().GetGridPos();

            var fluffBallObj = stageObjectFactory.GenerateFluffBall(null, frontAmidaPos);
            
            fluffBallObj.GetComponent<FluffBall>().OnDrop(frontAmidaPos); // 毛糸玉を配置


            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE); 

            // 変更を通知
            GameInteractionEventMessenger.GetInstance.Notify(InteractionEvent.CHANGED_AMIDAKUJI);
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
    /// 解く
    /// </summary>
    private void FinishUnknit()
    {
        var stageGridData = MapData.GetInstance.GetStageGridData();

        // 前方のあみだに取得
        var frontAmidaPos = m_targetObject.GetComponent<StageBlock>().GetGridPos();

        stageGridData.RemoveAmidaTube(frontAmidaPos); // 前方のあみだを削除

        // 上下のあみだの状態を再設定
        var upAmidaPos = frontAmidaPos + GridPos.UP;
        var downAmidaPos = frontAmidaPos + GridPos.DOWN;

        var upAmidaTube = stageGridData.GetAmidaTube(upAmidaPos);
        var downAmidaTube = stageGridData.GetAmidaTube(downAmidaPos);

        if (upAmidaTube == null || downAmidaTube == null)
        {
            //Debug.LogError("Up or Down AmidaTube not found.");
            return; // 上下のあみだが見つからない場合は処理を中断
        }

        stageGridData.GetAmidaTube(upAmidaPos).RequestChangedState(AmidaTube.State.NORMAL);
        stageGridData.GetAmidaTube(downAmidaPos).RequestChangedState(AmidaTube.State.NORMAL);

        
    }




}
