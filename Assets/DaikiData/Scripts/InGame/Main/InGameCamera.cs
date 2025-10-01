using DG.Tweening;
using UnityEngine;

/// <summary>
/// インゲームカメラ
/// </summary>
public class InGameCamera 
    : MonoBehaviour
    , IInGameFlowEventObserver

{
    private Vector3 m_targetPosition;        // 目標座標

    GameObject m_player;              // プレイヤー

    [SerializeField]
    private Vector3 m_startFocusPosition;    // 初期注視点

    private Quaternion m_startFocusRotate;   // 初期注視点の回転

    private State m_state;


    private Quaternion m_targetRotate;

    [SerializeField]
    private float START_GAME_STARTING_TIME = 2.0f; // ゲーム開始状態の時間

    /// <summary>
    /// 状態
    /// </summary>
    enum State
    {
        FOCUS_PLAYER,       // プレイヤー注視状態
        GAME_SATARTING,     // ゲーム開始
        GAME_PLAYING,       // ゲーム中
        CLEAR_STATE,      // クリア状態
    }

    void Awake()
    {
        // ゲームフロウイベントの登録
        InGameFlowEventMessenger.GetInstance.RegisterObserver(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // プレイヤーの取得
        m_player = GameObject.FindGameObjectWithTag("Player");

        m_startFocusPosition = m_player.transform.position ;
        m_startFocusRotate = Quaternion.LookRotation(new Vector3(0.0f, 0.0f, 1.0f));

        // 目標座標
        m_targetPosition = transform.position;
        m_targetRotate = transform.rotation;

    }

    void OnDestroy()
    {
        // ゲームフロウイベントの登録解除
        InGameFlowEventMessenger.GetInstance.RemoveObserver(this);
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_state)
        {
            case State.GAME_SATARTING:
                UpdateGameStartingState();
                break;
            case State.GAME_PLAYING:
                // 何もしない
                break;
        }
    }

    /// <summary>
    /// ゲーム開始状態の開始
    /// </summary>
    void StartGameStartingState()
    {

        transform.DOBlendableMoveBy(m_targetPosition - transform.position,START_GAME_STARTING_TIME).SetEase(Ease.InOutSine);
        transform.DORotateQuaternion(m_targetRotate, START_GAME_STARTING_TIME * 0.6f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            // シーケンス完了イベントを通知
            InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.INTRO_SEQUENCE_END);
        });
    }

    /// <summary>
    /// プレイヤー注視状態の開始
    /// </summary>
    void StartFocusPlayerState()
    {
        transform.position = m_startFocusPosition + new Vector3(0.0f, 1.5f, -(m_player.transform.localScale.z - 0.12f));
        transform.rotation = m_startFocusRotate;

        var endpoint = transform.position + new Vector3(0.0f, 10.0f, -8.0f);

        const float DURATION = 1.0f;

        // 注視点を初期位置に
        transform.DOMove(endpoint, DURATION).SetEase(Ease.InOutSine).SetDelay(0.5f).OnComplete(() =>
        {
            // フォーカス完了イベントを通知
            InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.ZOOM_OUT_PLAYER_END);
        });

        Vector3 direction = m_player.transform.position - endpoint;

        var lookAt = Quaternion.LookRotation(direction.normalized);
        transform.DORotateQuaternion(lookAt, DURATION * 0.5f).SetEase(Ease.OutQuart).SetDelay(0.5f);

    }

    /// <summary>
    /// ゲーム中状態の開始
    /// </summary>
    void StartGamePlayingState()
    {
        //// 位置と回転を目標に
        //transform.position = m_targetPosition;
        
        //transform.rotation = m_targetRotate;
    }

    void StartGameClearState() 
    {
        // 位置と回転を目標に
        transform.position = m_targetPosition - new Vector3(0.0f, 0.0f, -4.0f);

        // 目標座標の周りを回転する
        transform.DORotate(new Vector3(0.0f, 360.0f, 0.0f), 5.0f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    /// <summary>
    /// ゲーム開始状態の更新
    /// </summary>
    void UpdateGameStartingState()
    {
 
    }

    void ChangeState(State state)
    {
        m_state = state;
        
        switch (m_state)
        {
            case State.FOCUS_PLAYER:
                StartFocusPlayerState();
                break;
            case State.GAME_SATARTING:
                StartGameStartingState();
                break;
            case State.GAME_PLAYING:
                StartGamePlayingState();
                break;
            case State.CLEAR_STATE:
                StartGameClearState();
                break;
        }
    }

    /// <summary>
    /// ゲームフロウイベント
    /// </summary>
    /// <param name="eventID"></param>
    public void OnEvent(InGameFlowEventID eventID)
    {
        switch (eventID)
        {
            
            case InGameFlowEventID.ZOOM_OUT_PLAYER_START:
                ChangeState(State.FOCUS_PLAYER);
                break;

            case InGameFlowEventID.INTRO_SEQUENCE_START:
                ChangeState(State.GAME_SATARTING);
                break;

            case InGameFlowEventID.GAME_START:
                ChangeState(State.GAME_PLAYING);
                break;


        }
    }

    /// <summary>
    /// 目標座標の設定
    /// </summary>
    /// <param name="targetPosition"></param>
    public void SetTargetPosition(Vector3 targetPosition)
    {
        m_targetPosition = targetPosition;
    }

    public void RequestChangeClearState()
    {
        ChangeState(State.CLEAR_STATE);
    }

}
