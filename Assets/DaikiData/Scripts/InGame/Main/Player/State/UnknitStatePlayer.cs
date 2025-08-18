using UnityEngine;

public class UnknitStatePlayer : PlayerState
{

    private AnimationEventHandler m_animationEventHandler; // アニメーションイベントハンドラー

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
        // 移動を停止
        owner.StopMove();
        // レイヤーの変更中フラグをリセット
        m_animationEventHandler.PlayAnimationTrigger("Unknit", "Normal", "Unknit"); // 解くアニメーションを再生
                                                                                    // レイヤーのウェイトを変更するためのコールバックを設定
        m_animationEventHandler.SetTargetTimeAction(0.7f, () => { owner.RequestTransitionLayerWeight("Carry", 1, 0.6f); }); // Carryレイヤーのウェイトを0に設定


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
            var frontAmidaPos = owner.GetForwardGridPos(); ;

            var fluffBallObj = stageObjectFactory.GenerateFluffBall(null, frontAmidaPos);


            var carryingObject = fluffBallObj.GetComponent<Carryable>(); // 持ち上げるオブジェクトを取得
            if (carryingObject == null)
            {
                Debug.LogError("Carryable component not found on the object.");
                return; // 持ち上げるオブジェクトが見つからない場合は処理を中断
            }

            // 持ち上げるオブジェクトを非アクティブにする
            fluffBallObj.SetActive(false);

            // 持ち上げる
            carryingObject.OnPickUp();

            // プレイヤーに持ち上げるオブジェクトをセット
            owner.SetCarryingObject(carryingObject);

            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE); // 持ち上げる状態に遷移

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
    /// 編む
    /// </summary>
    private void FinishUnknit()
    {
        var stageGridData = MapData.GetInstance.GetStageGridData();

        // 前方のあみだに取得
        var frontAmidaPos = owner.GetForwardGridPos(); ;

        stageGridData.RemoveAmidaTube(frontAmidaPos); // 前方のあみだを削除

        // 上下のあみだの状態を再設定
        var upAmidaPos = frontAmidaPos + GridPos.UP;
        var downAmidaPos = frontAmidaPos + GridPos.DOWN;

        stageGridData.GetAmidaTube(upAmidaPos).RequestChangedState(AmidaTube.State.NORMAL);
        stageGridData.GetAmidaTube(downAmidaPos).RequestChangedState(AmidaTube.State.NORMAL);

        
    }




}
