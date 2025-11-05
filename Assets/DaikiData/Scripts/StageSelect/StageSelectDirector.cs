using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Stage Select Director
/// </summary>
public class StageSelectDirector : MonoBehaviour
{
    [Header("ステージ選択コントローラ")]
    [SerializeField] private StageSelectController m_stageSelectController; // ステージ選択コントローラ

    [Header("ワールド選択コントローラ")]
    [SerializeField] private WorldSelectButtonController m_worldSelectController; // ワールド選択コントローラ

    [Header("ポーズ画面コントローラ")]
    [SerializeField]
    private PauseWindowController m_pauseWindowController; // ポーズ画面コントローラ

    [Header("プレイヤー入力")]
    [SerializeField]
    private PlayerInput m_playerInput; // プレイヤー入力

    [Header("画面遷移(フェードイン)イメージ")]
    [SerializeField]
    private SceneTransitionEffect m_sceneTransitionFadeInEffect; // 画面遷移(フェードイン)イメージ

    [Header("画面遷移(フェードアウト)イメージ")]
    [SerializeField]
    private SceneTransitionEffect m_sceneTransitionFadeOutEffect; // 画面遷移(フェードアウト)イメージ


    void Awake()
    {
        if (m_stageSelectController == null)
        {
            Debug.LogError("StageSelectControllerがアタッチされていません。");
        }

        if (m_worldSelectController == null)
        {
            Debug.LogError("WorldSelectButtonControllerがアタッチされていません。");
        }

        // nullチェック
        if (m_pauseWindowController == null)
        {
            Debug.LogError("PauseWindowControllerがアタッチされていません。");
        }

        if (m_playerInput == null)
        {
            Debug.LogError("PlayerInputがアタッチされていません。");
        }

        if (m_sceneTransitionFadeInEffect == null)
        {
            Debug.LogError("SceneTransitionFadeOutEffectがアタッチされていません。");
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // フェードインエフェクトの開始
        m_sceneTransitionFadeInEffect.StartTransition(
            () => { });

        // 全て有効化する
        m_playerInput.actions.Enable();
        m_playerInput.SwitchCurrentActionMap("UI");

        // ステージセレクトの入力モードに設定する
        if (GameProgressManager.Instance.GameProgressData.isPrevSceneGamePlayed)
        {
            // ステージセレクトの入力モードに設定する
            var gameProgressData = GameProgressManager.Instance.GameProgressData;
            m_worldSelectController.RequestStartSelectStage(gameProgressData.worldID, gameProgressData.stageID);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 共通の入力設定を行う
    /// </summary>
    void SetUpCommonInput()
    {
        // 全て無効にする
        m_playerInput.actions.Disable();

        // 必要な入力を有効にする
        m_playerInput.actions.FindActionMap("InGameSystem").Enable();
        m_playerInput.actions.FindActionMap("UI").Enable();

        // UIアクションマップに切り替え
        m_playerInput.SwitchCurrentActionMap("UI");
    }

    /// <summary>
    /// ステージセレクトの入力モードに設定する
    /// </summary>
    void SetUpStageSelectInputMode()
    {
        SetUpCommonInput();

        // ステージセレクトの入力を有効にする
        m_stageSelectController.AcceptInput = true;
        // ワールドセレクトの入力を無効にする
        m_worldSelectController.AcceptInput = true;
    }

    /// <summary>
    /// ポーズメニューの入力モードに設定する
    /// </summary>
    void SetUpPauseMenuInputMode()
    {
        SetUpCommonInput();

        // ステージセレクトの入力を無効にする
        m_stageSelectController.AcceptInput = false;
        // ワールドセレクトの入力を無効にする
        m_worldSelectController.AcceptInput = false;
    }

    public void OpenPause(InputAction.CallbackContext context)
    {
        if (!context.performed){ return; }

        SetUpPauseMenuInputMode();
        m_pauseWindowController.RequestOpenPause(m_playerInput, SetUpStageSelectInputMode);
    }

    public void ReturnTitle()
    {
        SceneTransitionManager.GetInstance.TransitionToScene("TitleScene", m_sceneTransitionFadeOutEffect);

        // 前のシーンがゲームプレイだったかどうかをリセットする
        GameProgressManager.Instance.GameProgressData.isPrevSceneGamePlayed = false;
    }

    public void LoadGameplayScene()
    {
       
        SceneTransitionManager.GetInstance.TransitionToScene("GameplayScene", m_sceneTransitionFadeOutEffect);


    }

}
