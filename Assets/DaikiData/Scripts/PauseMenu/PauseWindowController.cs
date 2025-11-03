using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    #region インスペクター表示よう構造体

    /// <summary>
    /// 決定時のイベント構造体
    /// </summary>
    [Serializable]
    struct SubmitEventInfo
    {
        public string menuName; // メニュー名
        public UnityEvent submitEvent; // 決定時のイベント
    }
    #endregion

    [Header("イベント辞書 ======================================-")]
    [SerializeField]
    private List<SubmitEventInfo> m_submitEventList = new(); // 決定時のイベントリスト

    private Dictionary<string, UnityEvent> m_submitEvent; // 決定時のイベント辞書


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

    private void Reset()
    {


        if (m_submitEventList == null) return;
        var contentSelector = GetComponent<MenuContentSelector>();
        // ラベル名を設定
        m_submitEventList.Clear();
        foreach (var menuName in contentSelector.TitleMenuList)
        {
            m_submitEventList.Add(new SubmitEventInfo
            {
                menuName = menuName,
                submitEvent = new UnityEvent()
            });
        }


    }
    void Awake()
    {
        // イベント辞書を初期化
        m_submitEvent = new Dictionary<string, UnityEvent>();
        foreach (var submitEventInfo in m_submitEventList)
        {
            m_submitEvent[submitEventInfo.menuName] = submitEventInfo.submitEvent;
        }

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
            if (m_pauseMenuManualController)
            {
                m_pauseMenuManualController.OnOpenPause(m_fadeInTime);
            }
        });
    }

    void OnClosePause()
    {
        // ポーズメニューが開かれたときの処理を呼び出す
        if (m_pauseMenuManualController)
        {
            m_pauseMenuManualController.OnClosePause(m_fadeOutTime);
        }

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
            // 戻る
            default:
                m_submitEvent[contentSelector.CurrentTitleMenuName].Invoke();
                break;


        }

   
    }
}
