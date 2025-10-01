using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static TitleSelector;

/// <summary>
/// タイトルシーンコントローラー
/// </summary>
public class TitleSceneController : MonoBehaviour
{
    [Header("タイトルロゴ")]
    [SerializeField] private Image m_titleLogo; // タイトルロゴ

    [Header("タイトルセレクター")]
    [SerializeField] private TitleSelector m_titleSelector; // タイトルセレクター

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
    public void OnSubmit(InputValue value)
    {
        // 各タイトルメニューに対応する処理
        switch (m_titleSelector.CurrentTitleMenuID)
        {
            case TitleMenuID.RESET_GAME:
                break;
            case TitleMenuID.CONTINUE_GAME:
                SceneManager.LoadScene("StageSelectScene");
                break;
            case TitleMenuID.SETTING:
                break;
            case TitleMenuID.QUIT_GAME:
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
