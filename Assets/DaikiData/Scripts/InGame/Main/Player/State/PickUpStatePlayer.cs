using UnityEngine;

public class PickUpStatePlayer : PlayerState
{

    private AnimationEventHandler m_animationEventHandler; // アニメーションイベントハンドラー


    public PickUpStatePlayer(Player owner) : base(owner)
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
        // 持ち上げる処理を開始
        StartPickUp(); 

    }

    /// <summary>
    /// 歩行状態中のUpdateで毎フレーム呼ばれる
    /// </summary>
    public override void OnUpdateState()
    {
        // 現在のアニメーションのハッシュを取得
        m_animationEventHandler.OnUpdate(); // アニメーションイベントハンドラーの更新


        // アニメーションの終了を待つ
        if (m_animationEventHandler.HasAnimationPlayed() && m_animationEventHandler.IsPlaying() == false)
        {
            // アニメーションが終了したら持ち上げるオブジェクトを拾う処理を実行
            PickUp();
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

    private void PickUp()
    {

        var map = MapData.GetInstance; // マップを取得

        var checkPos = owner.GetForwardGridPos(); // チェックするグリッド位置

        TileObject tileObject = map.GetStageGridData().GetTileData[checkPos.y, checkPos.x].tileObject;


        GameObject gameObj = tileObject.gameObject;
        var carryingObject = gameObj.GetComponent<Carryable>(); // 持ち上げるオブジェクトを取得
        if (carryingObject == null)
        {
            Debug.LogError("Carryable component not found on the object.");
            return; // 持ち上げるオブジェクトが見つからない場合は処理を中断
        }
        // 持ち上げるオブジェクトのグリッド位置を取得
        var stageBlock = carryingObject.stageBlock;

        // グリッドから持ち上げるオブジェクトを削除
        map.GetStageGridData().RemoveGridDataGameObject(stageBlock.GetGridPos()); 

        // デバッグログに持ち上げるオブジェクトの名前を出力
        Debug.Log(gameObj.name); 

        // 持ち上げるオブジェクトを非アクティブにする
        stageBlock.SetActive(false);

        // 持ち上げる
        carryingObject.OnPickUp();

        // プレイヤーに持ち上げるオブジェクトをセット
        owner.SetCarryingObject(carryingObject); 
    }


    /// <summary>
    /// 持ち上げる処理を開始する
    /// </summary>
    private void StartPickUp()
    {
        // レイヤーの変更中フラグをリセット
        m_animationEventHandler.PlayAnimationTrigger("PickUp", "Normal", "PickUp"); // 置くアニメーションを再生

        // レイヤーのウェイトを変更するためのコールバックを設定
        m_animationEventHandler.SetTargetTimeAction(0.5f, () => { owner.RequestTransitionLayerWeight("Carry", 1, 0.4f); }); // Carryレイヤーのウェイトを0に設定
    }
}
