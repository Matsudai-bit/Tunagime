using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class GameDirector : MonoBehaviour, IInGameFlowEventObserver
{
    bool m_isFirstUpdate = true;

    [Header("開始状態")]
    [SerializeField]
    InGameFlowEventID m_startState ;

    [Header("ゲーム開始UIパネル")]
    [SerializeField] GameObject m_gameStartUIPanel;

    [Header("クリアUIパネル")]
    [SerializeField] GameObject m_clearUIPanel;

    [Header("プレイヤーの入力システム")]
    [SerializeField] PlayerInput m_playerInput;

    [Header("ゲーム時間")]
    [SerializeField]
    private float m_gameTime = 0.0f;

    private bool m_isGameClear = false;


    void Awake()
    {
        // ゲームフロウイベントの登録
        InGameFlowEventMessenger.GetInstance.RegisterObserver(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 60fpsに設定
        Application.targetFrameRate = 60;

        m_gameTime = 0.0f;
    }

    void OnDestroy()
    {
        // ゲームフロウイベントの登録解除
        InGameFlowEventMessenger.GetInstance.RemoveObserver(this);
    }   

    // Update is called once per frame
    void Update()
    {
        if (m_isFirstUpdate)
        {
            m_isFirstUpdate = false;

            // ゲーム開始のイベントを通知
            InGameFlowEventMessenger.GetInstance.Notify(m_startState);
            return;
        }

        // Escキーが押されたらゲームを終了
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("ゲームを終了しました。");
        }

        // Tabが押されたらステージ選択にいく
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // タイトルシーンに遷移する処理
            UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelectScene");
            Debug.Log("タイトルシーンに戻ります。");
        }


        // リロード
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        }


        if (m_isGameClear == false)
        {
            // ゲーム時間を更新
            m_gameTime += Time.deltaTime;
        }

    }

    // ゲームがクリアした時に呼ばれる
    public void OnGameClear()
    {
        // クリアUIを表示する処理
        Debug.Log("ゲームクリア！");
        // ここにゲームクリアの処理を追加

        m_isGameClear = true;

        GameProgressManager.Instance.GameProgressData.clearTime = m_gameTime;

        // クリアUIのパネルを表示
        m_clearUIPanel.SetActive(true);


    }

    /// <summary>
    /// ステージセレクトシーンに遷移
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    async UniTask WaitAndLoadStageSelectScene(int waitTime)
    {
        await UniTask.Delay(waitTime);
       // InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.GAME_END);
    }

    public void LoadStageSelectScene()
    {
        SceneManager.LoadScene("ResultScene");
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
                m_playerInput.actions.Disable();
                break;

            // ズームアウト終了イベント
            case InGameFlowEventID.ZOOM_OUT_PLAYER_END:
                // イントロシーケンス開始イベントを通知
                InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.INTRO_SEQUENCE_START);
                break;

            // イントロシーケンス終了イベント
            case InGameFlowEventID.INTRO_SEQUENCE_END:
                // ゲーム開始イベントを通知
                InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.GAME_START_EFFECT_START);
                m_playerInput.actions.Enable();
                break;
            case InGameFlowEventID.GAME_START_EFFECT_START:
                // ゲーム開始UIパネルを表示する
                m_gameStartUIPanel.SetActive(true);
                break;

            case InGameFlowEventID.GAME_START_EFFECT_END:
                // ゲームプレイ開始イベントを通知
                InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.GAME_PLAYING_START);
                break;
            case InGameFlowEventID.GAME_CLEAR:
                // ゲームクリアイベントを通知
                OnGameClear();
                break;

            case InGameFlowEventID.GAME_PLAYING_END:
                var s = WaitAndLoadStageSelectScene(300);
                // ゲーム終了イベントを通知
                LoadStageSelectScene();
                break;

            case InGameFlowEventID.GAME_CLEAR_EFFECT_START:
                m_playerInput.actions.Disable();
                break;
            case InGameFlowEventID.GAME_CLEAR_EFFECT_END:
                // ゲームプレイ終了イベントを通知

                break;

            case InGameFlowEventID.GOING_GET_FEELING_PIECE_START:
                m_playerInput.actions.Enable();
                break;
            case InGameFlowEventID.GOING_GET_FEELING_PIECE_END:
                InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.GAME_PLAYING_END);
                break;
        }
    }
}

