using UnityEngine;

public class PushBlockStatePlayer : PlayerState
{
    private AnimationEventHandler m_animationEventHandler; // アニメーションイベントハンドラー

    private readonly float TARGET_TIME = 1.0f; // アニメーションのターゲット時間
    private float currentTime = 0.0f; // 現在の時間

    private FeltBlock m_feltBlock;      // 押す対象のブロック
    private Vector3 m_endPosition;   // ブロックを押した後の目標位置
    private Vector3 m_startPosition;    // ブロックを押す前の開始位置
    private float m_lerpValue = 0.0f; // 補間値

    public PushBlockStatePlayer(Player owner) : base(owner)
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

        var map = MapData.GetInstance; // マップを取得
                                       // test
        owner.TryForwardObjectSetting();

        // 押す対象のブロックを取得
        m_feltBlock = GetPushBlock();
        if (m_feltBlock == null)
        {
            // 押す対象のブロックが見つからない場合は待機状態に遷移
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);
            return;
        }



        
        // ブロックを押す前の開始位置を設定
        var blockPos = m_feltBlock.GetComponent<StageBlock>().GetGridPos(); // ブロックのグリッド位置を取得


      

        m_startPosition = map.ConvertGridToWorldPos(blockPos );

        // ブロックを押した後の目標位置を設定
        m_endPosition = map.ConvertGridToWorldPos(owner.GetForwardDirection() + blockPos);

        // レイヤーの変更中フラグをリセット
        m_animationEventHandler.PlayAnimationBool("Push", "Normal", "Push"); // 置くアニメーションを再生


        // プレイヤーの前方に少し離れた位置を計算 
        Vector3 dephPos = map.GetCommonData().tileSize / 2.0f * -owner.GetForwardDirectionForGrid();
        // プレイヤーのグリッド位置を計算
        GridPos playerGridPos = blockPos - owner.GetForwardDirection();

        // 自分の座標を設定 // ブロックのグリッド位置をワールド座標に変換して、プレイヤーの位置を設定
        owner.transform.position = map.ConvertGridToWorldPos(playerGridPos) + dephPos;
        owner.GetComponent<Rigidbody>().linearVelocity = Vector3.zero; // プレイヤーの速度をリセット

        // フェルトブロックの方向を向く
        owner.transform.LookAt(m_feltBlock.transform); // ブロックの目標位置を向くように設定

        // 子として設定
        owner.transform.SetParent(m_feltBlock.transform);
    }

    /// <summary>
    /// 歩行状態中のUpdateで毎フレーム呼ばれる
    /// </summary>
    public override void OnUpdateState()
    {
        // 押す処理
        Push();

        // アニメーションイベントハンドラーの更新
        m_animationEventHandler.OnUpdate();

        if (m_lerpValue >= 1.0f)
        {
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
        // 念のため
        m_feltBlock.transform.position = m_endPosition; // プレイヤーの位置を目標位置に設定
        owner.transform.SetParent(null); // ブロックの親を解除
                                         // ブロックを押す処理
        m_feltBlock.RequestMove(owner.GetForwardDirection());

        m_animationEventHandler.StopAnimation(); // アニメーションを停止


        //フェルトブロックを押したことを通知
        GameInteractionEventMessenger.GetInstance.Notify(InteractionEvent.PUSH_FELTBLOCK); 
    }

    /// <summary>
    /// ブロックを押す処理
    /// </summary>
    private void Push()
    {
        // 押す処理
        Vector3 newBlockPos = Vector3.Lerp(m_startPosition, m_endPosition, m_lerpValue);
        // 補間値を更新
        m_lerpValue = currentTime / TARGET_TIME;

        currentTime += Time.deltaTime; // 現在の時間を更新

        // ブロックの位置を更新
        m_feltBlock.transform.position = newBlockPos;


    }

    private FeltBlock GetPushBlock()
    {
       return owner.GetTargetObject()?.GetComponent<FeltBlock>();
    }

}
