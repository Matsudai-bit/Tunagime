using DG.Tweening;
using UnityEngine;
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

    

    private void OnEnable()
    {
       

        // 透過画像をフェードインさせる
        float targetAlpha = m_backgroundFilterImage.color.a; // 目標の透明度
        m_backgroundFilterImage.color = new Color(m_backgroundFilterImage.color.r, m_backgroundFilterImage.color.g, m_backgroundFilterImage.color.b, 0); // 最初は透明にする
        m_backgroundFilterImage.DOFade(targetAlpha, m_fadeInTime).SetEase(Ease.InSine);

        // 背景画像を左からスライドさせる
        var contentRect = m_contentParent.GetComponent<RectTransform>(); // コンテンツのサイズ
        Vector2 targetPosition = contentRect.anchoredPosition; // 目標の位置
        contentRect.anchoredPosition += new Vector2(-500.0f, 0); // 最初は左に移動させる
        contentRect.DOAnchorPosX(targetPosition.x, m_fadeInTime).SetEase(Ease.OutCubic).OnComplete(() => // アニメーション完了時の処理
        {
            // ポーズメニューが開かれたときの処理を呼び出す
            m_pauseMenuManualController.OnOpenPause();
        });


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
