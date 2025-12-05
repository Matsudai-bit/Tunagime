using DG.Tweening;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using static MenuContentSelector;

/// <summary>
/// タイトルシーンコントローラー
/// </summary>
public class TitleSceneController : MonoBehaviour
{
    [System.Serializable]
    struct PVData
    {
        public bool        isEnabled; // PV有効フラグ
        public GameObject pvRenderer; // PVレンダラー
        public VideoPlayer pvPlayer; // PVプレイヤー
        public float waitTime; // 再生待機時間
    }


    [Header("PVデータ")]
    [SerializeField]
    private PVData m_pvData; // PVデータ

    public float m_inactivityTime = 0.0f; // 操作していない時間

    [Header("設定ウィンドウコントローラ")]
    [SerializeField]
    private SettingsWindowController m_settingsWindowController; // 設定ウィンドウコントローラ

    [Header("プレイヤー入力")]
    [SerializeField]
    private PlayerInput m_playerInput; // プレイヤー入力

    [Header("タイトルロゴ")]
    [SerializeField] private Image m_titleLogo; // タイトルロゴ

    [Header("タイトルセレクター")]
    [SerializeField] private MenuContentSelector m_titleSelector; // タイトルセレクター

    [Header("画面遷移(フェードアウト)イメージ")]
    [SerializeField]
    private SceneTransitionEffect m_sceneTransitionFadeOutEffect; // 画面遷移(フェードアウト)イメージ


    [Header("画面遷移(フェードイン)イメージ")]
    [SerializeField]
    private SceneTransitionEffect m_sceneTransitionFadeInEffect; // 画面遷移(フェードイン)イメージ

    [Header("初期化警告ウィンドウ")]
    [SerializeField]
    private InitializationWarningWindow m_initializationWarningWindow;

    [Header("背景画像")]
    [SerializeField]
    private Image m_bacgroundImg;

    [Header("背景画像(通常時)")]
    [SerializeField]
    private Sprite m_backgroundNormalSprite;

    [Header("背景画像(クリア時)")]
    [SerializeField]
    private Sprite m_backgroundClearSprite;


    private void Awake()
    {
        //　nullチェック

        if (m_titleLogo == null)
        {
            Debug.LogError("TitleLogoがアタッチされていません。");
        }

        if (m_titleSelector == null)
        {
            Debug.LogError("TitleSelectorがアタッチされていません。");
        }

        if (m_sceneTransitionFadeOutEffect == null)
        {
            Debug.LogError("SceneTransitionFadeOutEffectがアタッチされていません。");
        }

    }

    void Start()
    {
        SoundManager.GetInstance.RequestAllStopping();

        Application.targetFrameRate = 60;

        // タイトルロゴの透明度を0に設定
        var color = m_titleLogo.color;
        color.a = 0.0f;
        m_titleLogo.color = color;
        // タイトルロゴをフェードインさせる
        m_titleLogo.DOFade(1, 4.0f).SetEase(Ease.OutCubic).SetDelay(1.5f);

        // フェードインエフェクトの開始
        m_sceneTransitionFadeInEffect.StartTransition(
            () => { });

        // 背景画像の設定
        if (GameContext.GetInstance.GetSaveData().GetStageStatus(WorldID.World_5, StageID.STAGE_5).isClear)
        {
            m_bacgroundImg.sprite = m_backgroundClearSprite;
        }
        else
        {
            m_bacgroundImg.sprite = m_backgroundNormalSprite;
        }
    }

    void Update()
    {
        if (m_pvData.isEnabled)
        {
            // 操作していない時間を計測
            if (m_playerInput.actions["Move"].WasPerformedThisFrame() ||
                m_playerInput.actions["Submit"].WasPerformedThisFrame() ||
                m_playerInput.actions["Cancel"].WasPerformedThisFrame())
            {
                if (IsPVPlaying())
                {
                    // PV停止
                    StopPV();

                    // BGM再生
                    SoundManager.GetInstance.PlayBGM(SoundID.BGM_TITLE);
                }
                m_inactivityTime = 0.0f;
            }
            else
            {

                m_inactivityTime += Time.deltaTime;
            }

            if (m_inactivityTime >= m_pvData.waitTime && !IsPVPlaying())
            {
                // PV再生
                PlayPV();

                // BGM停止
                SoundManager.GetInstance.StopBGM();


            }
            else if (m_inactivityTime >= m_pvData.waitTime + 0.1f && !m_pvData.pvRenderer.activeSelf)
            {
                // PV表示
                m_pvData.pvRenderer.SetActive(true);
            }

        }

        if (!m_titleSelector.CanInputEnabled() && !m_initializationWarningWindow.gameObject.activeSelf)
        {
            m_titleSelector.SetInputEnabled(true);

        }

    }

    /// <summary>
    /// 現在のワールドIDに対応するボタンをクリック
    /// </summary>
    /// <param name="value"></param>
    public void OnSubmit(InputAction.CallbackContext value)
    {
        if (m_initializationWarningWindow.gameObject.activeSelf) { return; }
        if (!value.performed) { return; }

        // SEを鳴らす
        SoundManager.GetInstance.RequestPlaying(SoundID.SE_UI_BUTTON_PUSH);
        // 各タイトルメニューに対応する処理
        switch (m_titleSelector.CurrentTitleMenuName)
        {
            case "ResetGame":
                TryResetGame();
                break;
            case "ContinueGame":
                if (GameContext.GetInstance.GetSaveData().GetStageStatus(WorldID.World_1, StageID.STAGE_1).isLocked == false)
                {
                    // 最初のステージがロックされている場合、ゲーム開始
                    ContinueGame();
                }
           
                break;
            case "Setting":
                m_playerInput.actions.Disable();
                m_playerInput.SwitchCurrentActionMap("SettingWindow");
                m_playerInput.currentActionMap.Enable();

                m_settingsWindowController.Open(() => {

                    m_playerInput.actions.Disable();
                    m_playerInput.SwitchCurrentActionMap("UI");
                    m_playerInput.currentActionMap.Enable();
                });


                break;
            case "QuitGame":
                {
#if UNITY_EDITOR
                    m_sceneTransitionFadeOutEffect.StartTransition(() =>
                    {
                        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了

                    });
#else
    m_sceneTransitionFadeOutEffect.StartTransition(() =>
                    {
                    Application.Quit();//ゲームプレイ終了
                    });
    
#endif

                    break;
                }
            default:
                Debug.LogError("Invalid TitleMenuID");
                break;
        }
    }

    /// <summary>
    /// 続きからゲーム開始
    /// </summary>
    void ContinueGame()
    {
        // ステージセレクトシーンへ遷移
        SceneTransitionManager.GetInstance.TransitionToScene("StageSelectScene", m_sceneTransitionFadeOutEffect);
    }

    /// <summary>
    /// ゲーム開始（リセット用）
    /// </summary>
    public void StartGameForReset()
    {
        // リセット音を鳴らす
        int id = SoundManager.GetInstance.RequestPlaying(SoundID.SE_GAMERESET_APPLY);

        SoundManager.GetInstance.SetSpeed(id, 1.5f);

        // ゲーム開始
        StartGame();
    }

    /// <summary>
    /// ゲーム開始
    /// </summary>
    private void StartGame()
    {
        // ゲーム初期化
        InitialGame();

        // 物語の状態を導入に設定
        NarrativeContext.GetInstance.SetNarrativeState(NarrativeContext.NarrativeState.INTRODUCTION, () =>
        {
            // 最初のステージのロックを解除
            GameContext.GetInstance.GetSaveData().worldDataDict[WorldID.World_1].isLocked = false;
            GameContext.GetInstance.GetSaveData().GetStageStatus(WorldID.World_1, StageID.STAGE_1).isLocked = false;
            GameContext.GetInstance.SaveGame();

            // 最初のワールド、ステージを設定
            GameProgressManager.Instance.GameProgressData.worldID = WorldID.World_1;
            GameProgressManager.Instance.GameProgressData.stageID = StageID.STAGE_1;

            LoadingSceneRequest.GetInstance.RequestLoadingScene("GameplayScene");
           
        });
        // ステージセレクトシーンへ遷移
        SceneTransitionManager.GetInstance.TransitionToScene("StoryScene", m_sceneTransitionFadeOutEffect);

        // 音の初期化
        SoundManager.GetInstance.StopBGM();
    }

    public void TryResetGame()
    {
        // 最初のステージがロックされている場合、初期化警告ウィンドウを表示
        if (GameContext.GetInstance.GetSaveData().GetStageStatus(WorldID.World_1, StageID.STAGE_1).isLocked)
        {
            // ゲーム初期化
            StartGame();
        }
        else
        {
            m_initializationWarningWindow.Open();
            m_titleSelector.SetInputEnabled(false);
        }

    }

    

    public void CancelInitializationWindow()
    {
        m_initializationWarningWindow.Close();
    }

    /// <summary>
    /// ゲーム初期化
    /// </summary>
    void InitialGame()
    {
        var gameContext = GameContext.GetInstance;
        gameContext.ResetGame();
    }

    /// <summary>
    /// PV再生
    /// </summary>
    private void PlayPV()
    {
        m_pvData.pvPlayer.Play();

     
        // 音量調整
        m_pvData.pvPlayer.SetDirectAudioVolume(0, GameContext.GetInstance.GetGameSettingParameters().bgmVolume);
    }

    /// <summary>
    /// PVが再生中かどうか
    /// </summary>
    /// <returns></returns>
    private bool IsPVPlaying()
    {
        return m_pvData.pvPlayer.isPlaying;
    }

    /// <summary>
    /// PV停止
    /// </summary>
    private void StopPV()
    {
        m_pvData.pvPlayer.Stop();
        m_pvData.pvRenderer.SetActive(false);


    }

}
