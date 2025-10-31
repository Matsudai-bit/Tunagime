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

    void Start()
    {
        // タイトルロゴの透明度を0に設定
        var color = m_titleLogo.color;
        color.a = 0.0f;
        m_titleLogo.color = color;
        // タイトルロゴをフェードインさせる
        m_titleLogo.DOFade(1, 4.0f).SetEase(Ease.OutCubic).SetDelay(1.5f);
    }

    /// <summary>
    /// 現在のワールドIDに対応するボタンをクリック
    /// </summary>
    /// <param name="value"></param>
    public void OnSubmit(InputAction.CallbackContext value)
    {
        // 各タイトルメニューに対応する処理
        switch (m_titleSelector.CurrentTitleMenuName)
        {
            case "ResetGame":
                break;
            case "ContinueGame":
                SceneManager.LoadScene("StageSelectScene");
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

}
