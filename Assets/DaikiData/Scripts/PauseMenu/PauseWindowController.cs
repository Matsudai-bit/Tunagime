using DG.Tweening;
using System;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ポーズウィンドウのコントローラー
/// </summary>
public class PauseWindowController : MonoBehaviour
{

    [Header("パラメータ ======================================-")]
    [Header("フェードイン時間")]
    [SerializeField]
    private float m_fadeInTime = 0.5f;          // フェードイン時間
    private float m_fadeOutTime = 0.5f;          // フェードアウト時間

    [Header("コンポーネント ======================================-")]

    [Header("背景フィルター画像(透過画像)")]
    [SerializeField]
    private Image m_backgroundFilterImage;  // 背景フィルター画像

    [Header("コンテンツ親オブジェクト")]
    [SerializeField]
    private GameObject m_contentParent;        // コンテンツ親オブジェクト

    [Header("ポーズメニューのマニュアルコントローラー")]
    [SerializeField]
    private PauseMenuManualController m_pauseMenuManualController; // ポーズメニューのマニュアルコントローラー


    private Vector3 m_initialContentPosition; // コンテンツの初期位置
    private float m_initialBackgroundAlpha; // 背景フィルター画像の初期透明度

    private bool m_isInitialStart = false; // 初回起動フラグ

    private PlayerInput m_playerInput; // プレイヤー入力

    private Action m_onExitPause; // ポーズメニューを閉じたときの処理

  

    public void RequestOpenPause(PlayerInput playerInput, Action onExitPause)
    {

        gameObject.SetActive(true);
        m_isInitialStart = true;
        m_playerInput = playerInput;

        m_onExitPause = onExitPause;
    }

   
    void Awake()
    {
        var contentRect = m_contentParent.GetComponent<RectTransform>(); 
        // 初期位置を保存しておく
        m_initialContentPosition = contentRect.anchoredPosition;

        // 背景フィルター画像の初期透明度を保存しておく
        m_initialBackgroundAlpha = m_backgroundFilterImage.color.a;
    }


    // Update is called once per frame
    void Update()
    {
        if (m_isInitialStart)
        {
            m_isInitialStart = false;
            OnOpenPause();
        }
    }


    void OnOpenPause()
    {
        // プレイヤー入力をUI用に切り替える
        m_playerInput.actions.Disable();
        m_playerInput.actions.FindActionMap("UI").Enable();

        // 透過画像をフェードインさせる
        m_backgroundFilterImage.color = new Color(m_backgroundFilterImage.color.r, m_backgroundFilterImage.color.g, m_backgroundFilterImage.color.b, 0); // 最初は透明にする
        m_backgroundFilterImage.DOFade(m_initialBackgroundAlpha, m_fadeInTime).SetEase(Ease.InSine);

        // 背景画像を左からスライドさせる
        var contentRect = m_contentParent.GetComponent<RectTransform>(); // コンテンツのサイズ

        contentRect.anchoredPosition = m_initialContentPosition; // 初期位置に戻す

        contentRect.anchoredPosition += new Vector2(-500.0f, 0); // 最初は左に移動させる
        contentRect.DOAnchorPosX(m_initialContentPosition.x, m_fadeInTime).SetEase(Ease.OutCubic).OnComplete(() => // アニメーション完了時の処理
        {
            // ポーズメニューが開かれたときの処理を呼び出す
            m_pauseMenuManualController.OnOpenPause(m_fadeInTime);
        });
    }

    void OnClosePause()
    {
        // ポーズメニューが開かれたときの処理を呼び出す
        m_pauseMenuManualController.OnClosePause(m_fadeOutTime);

        // 透過画像をフェードアウトさせる
        m_backgroundFilterImage.DOFade(0, m_fadeOutTime).SetEase(Ease.OutSine);
        // 背景画像を左にスライドさせる
        var contentRect = m_contentParent.GetComponent<RectTransform>(); // コンテンツのサイズ
        contentRect.DOAnchorPosX(contentRect.anchoredPosition.x - 500.0f, m_fadeOutTime).SetEase(Ease.InCubic).OnComplete(() => // アニメーション完了時の処理
        {
            gameObject.SetActive(false);

            m_onExitPause?.Invoke();
        });
    }

    /// <summary>
    /// ポーズメニューを閉じるボタンが押されたときの処理
    /// </summary>
    /// <param name="context"></param>
    public void OnExitPauseButton(InputAction.CallbackContext context)
    {
        if (context.performed == false) return;

        if (!gameObject.activeSelf || m_isInitialStart) return;
        // ポーズメニューを閉じる
        OnClosePause();
    }


    
    public void OnSubmit(InputAction.CallbackContext context)
    {

        if (context.performed == false) return;

        var contentSelector = GetComponent<MenuContentSelector>();

        // 選択されているメニューに応じて処理を分岐
        switch (contentSelector.CurrentTitleMenuName)
        {

            // メニューを閉じる
            case "ExitMenu":
                // ポーズメニューを閉じる
                OnClosePause();
                break;
            // 操作方法
            case "OperationManual":
                break;
            // 設定
            case "Setting":
                break;
            // ステージ選択へ戻る
            case "ReturnStageSelect":
                // ステージシーンへ戻る処理
                SceneManager.LoadScene("StageSelectScene");
                break;


        }

        // ポーズメニューを閉じる
        OnClosePause();
    }
}
