using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Stage Select Director
/// </summary>
public class StageSelectDirector : MonoBehaviour
{
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ステージセレクトの入力モードに設定する
    /// </summary>
    void SetUpStageSelectInputMode()
    {
        // 全て有効化する
        m_playerInput.actions.Enable();
        m_playerInput.SwitchCurrentActionMap("UI");
    }

    public void OpenPause(InputAction.CallbackContext context)
    {
        if (!context.performed){ return; }

        m_pauseWindowController.RequestOpenPause(m_playerInput, SetUpStageSelectInputMode);
    }

    public void ReturnTitle()
    {
        SceneTransitionManager.GetInstance.TransitionToScene("TitleScene", m_sceneTransitionFadeOutEffect);
    }

    public void LoadGameplayScene()
    {
        SceneTransitionManager.GetInstance.TransitionToScene("GameplayScene", m_sceneTransitionFadeOutEffect);
    }

}
