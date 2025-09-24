using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour, IInGameFlowEventObserver
{
    bool m_isFirstUpdate = true;

    [SerializeField] GameObject m_clearUI;

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
            InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.ZOOM_OUT_PLAYER_START);
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


    }

    // ゲームがクリアした時に呼ばれる
    public void OnGameClear()
    {
        // クリアUIを表示する処理
        Debug.Log("ゲームクリア！");
        // ここにゲームクリアの処理を追加
        m_clearUI.SetActive(true);


    }

    public void LoadStageSelectScene()
    {
        SceneManager.LoadScene("StageSelectScene");
    }

    /// <summary>
    /// ゲームフロウイベント
    /// </summary>
    /// <param name="eventID"></param>
    public void OnEvent(InGameFlowEventID eventID)
    {
        switch (eventID)
        {
            // ズームアウト終了イベント
            case InGameFlowEventID.ZOOM_OUT_PLAYER_END:
                // イントロシーケンス開始イベントを通知
                InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.INTRO_SEQUENCE_START);
                break;

            // イントロシーケンス終了イベント
            case InGameFlowEventID.INTRO_SEQUENCE_END:
                // ゲーム開始イベントを通知
                InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.GAME_START);
                break;

            case InGameFlowEventID.GAME_END:
                // ゲーム終了イベントを通知
                LoadStageSelectScene();
                break;
        }
    }
}

