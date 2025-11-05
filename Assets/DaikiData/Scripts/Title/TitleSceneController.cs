using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MenuContentSelector;

/// <summary>
/// タイトルシーンコントローラー
/// </summary>
public class TitleSceneController : MonoBehaviour
{
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

        // タイトルロゴの透明度を0に設定
        var color = m_titleLogo.color;
        color.a = 0.0f;
        m_titleLogo.color = color;
        // タイトルロゴをフェードインさせる
        m_titleLogo.DOFade(1, 4.0f).SetEase(Ease.OutCubic).SetDelay(1.5f);

        // フェードインエフェクトの開始
        m_sceneTransitionFadeInEffect.StartTransition(
            () => { });
    }

    /// <summary>
    /// 現在のワールドIDに対応するボタンをクリック
    /// </summary>
    /// <param name="value"></param>
    public void OnSubmit(InputAction.CallbackContext value)
    {
        if (!value.performed) { return; }

        // SEを鳴らす
        SoundManager.GetInstance.RequestPlaying(SoundID.SE_UI_BUTTON_PUSH);
        // 各タイトルメニューに対応する処理
        switch (m_titleSelector.CurrentTitleMenuName)
        {
            case "ResetGame":
                StartGame();
                break;
            case "ContinueGame":
                // ステージセレクトシーンへ遷移
                SceneTransitionManager.GetInstance.TransitionToScene("StageSelectScene", m_sceneTransitionFadeOutEffect);
                break;
            case "Setting":
                break;
            case "QuitGame":
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
                    Application.Quit();//ゲームプレイ終了
    
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

    }

    /// <summary>
    /// ゲーム開始
    /// </summary>
    void StartGame()
    {
        // ゲーム初期化
        InitialGame();

        NarrativeContext.GetInstance.SetNarrativeState(NarrativeContext.NarrativeState.INTRODUCTION);
        // ステージセレクトシーンへ遷移
        SceneTransitionManager.GetInstance.TransitionToScene("StoryScene", m_sceneTransitionFadeOutEffect);

        // 音の初期化
        SoundManager.GetInstance.StopBGM();
    }

    /// <summary>
    /// ゲーム初期化
    /// </summary>
    void InitialGame()
    {
        var gameContext = GameContext.GetInstance;
        gameContext.ResetGame();
    }

}
