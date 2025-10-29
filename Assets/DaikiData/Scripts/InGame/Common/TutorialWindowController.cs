using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// チュートリアルの操作コンポーネント
/// </summary>
public class TutorialWindowController : MonoBehaviour
{


    #region　データ構造
    class TutorialImageForDisplay
    {
        public List<Image> keyboardImages;  // キーボード　　用
        public List<Image> gamepadImages;   // ゲームパッド  用

        public TutorialImageForDisplay()
        {
            keyboardImages = new();
            gamepadImages = new();
        }
    }
    enum State
    {
        FADE,       // フェード中
        NORMAL,     // 通常状態
        ANIMATION,  // アニメーション状態
    }

    #endregion

    #region 変数宣言
    [SerializeField]
    private GameObject m_tutorialImagePrefab; // チュートリアル画像プレハブ
    
    [SerializeField]
    private List<Sprite> m_tutorialPageSprites ; // チュートリアルスプライト


    [Header("ページの親トランスフォーム")]
    [SerializeField]
    private Transform m_pageImageParent;

    [Header("次のページに行く時のアニメーション時間")]
    [SerializeField]
    private float NEXT_PAGE_ANIMATION_TIME = 1.0f;

    [Header("前のページに行く時のアニメーション時間")]
    [SerializeField]
    private float PREVIOUS_PAGE_ANIMATION_TIME = 0.5f;

    [Header("上げるY座標の量")]
    [SerializeField]
    private float UPPER_Y = 20.0f;

    [Header("背景イメージ")]
    [SerializeField]
    private Image m_backgroundImage;

    [Header("右矢印UIイメージ")]
    [SerializeField]
    private Image m_rightArrowPageGuide;

    [Header("左矢印UIイメージ")]
    [SerializeField]
    private Image m_leftArrowPageGuide;

    [Header("終了ガイドUI（キーボード）")]
    [SerializeField]
    private Image m_exitGuideKeyboardImage;

    [Header("終了ガイドUI（ゲームパッド）")]
    [SerializeField]
    private Image m_exitGuideGamepadImage;

    private Vector3 m_centerPosition;   // 中心座標

    private State m_currentState;       // 現在の状態


    private List<Image> m_currentTutorialPageImages = new();   // インスタンス化されたスプライト

    private Image m_currentImage;       // 現在のイメージ
    private Image m_nextImage;       // 現在のイメージ
    private int m_currentImageIndex;    // 現在のイメージ画像

    //private int m_nextImageIndex;       // 次のイメージ画像

    private TutorialImageForDisplay m_displayTutorialImageData; // 表示するチュートリアル画像データ

    #endregion

    private void Awake()
    {
        if (m_tutorialImagePrefab == null)
        {
            Debug.LogError("チュートリアル画像プレハブが設定されていません");
        }

        gameObject.SetActive(false);

        // イージングの設定
        m_leftArrowPageGuide.GetComponent<RectTransform>().DOBlendableLocalMoveBy(new Vector3(25.0f, 0.0f), 1.5f).SetLoops(-1, LoopType.Yoyo);
        m_rightArrowPageGuide.GetComponent<RectTransform>().DOBlendableLocalMoveBy(new Vector3(-25.0f, 0.0f), 1.5f).SetLoops(-1, LoopType.Yoyo);

        m_exitGuideKeyboardImage.DOFade(0.7f, 1.0f).SetLoops(-1, LoopType.Yoyo);
        m_exitGuideGamepadImage.DOFade(0.7f, 1.0f).SetLoops(-1, LoopType.Yoyo);

        // 一時停止する
        PauseEaseForGuideUI();
    }


    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize(TutorialEventData.InputDataForTutorialSprite tutorialSpriteData)
    {
        ResetTutorial();

        m_displayTutorialImageData = new();

        // キーボードイメージのセットアップ
        SetUpImage(tutorialSpriteData.pageKeyboardSprites, m_displayTutorialImageData.keyboardImages, m_tutorialImagePrefab, m_pageImageParent);

        // ゲームパッドイメージのセットアップ
        SetUpImage(tutorialSpriteData.pageGamepadSprites, m_displayTutorialImageData.gamepadImages, m_tutorialImagePrefab, m_pageImageParent);
        // 現在表示するリストを設定する
        m_currentTutorialPageImages = m_displayTutorialImageData.keyboardImages;
        // 初期インデックスの設定
        m_currentImageIndex = 0;
        // 現在の画像を設定する
        m_currentImage = m_currentTutorialPageImages[m_currentImageIndex];
        m_currentImage.gameObject.SetActive(true);

        m_centerPosition = m_currentTutorialPageImages[m_currentImageIndex].gameObject.GetComponent<RectTransform>().position;
        m_currentState = State.NORMAL;

        
    }

    public void StartTutorial()
    {
        if (m_currentImage == null) { return; }

        gameObject.SetActive(true);

        UpdatePageImages();
        UpdateGuideUI();

        PlayEaseForGuideUI();
        StartFadeIn();
    }

    
    /// <summary>
    /// 次のページに切り替える
    /// </summary>
    void ChangeNextPage()
    {
        if (m_currentState == State.ANIMATION) { return; }

        // 次のインデックスへ加算する
        int nextImageIndex = Mathf.Min( (m_currentImageIndex + 1), m_currentTutorialPageImages.Count - 1);
        
        if (nextImageIndex == m_currentImageIndex || nextImageIndex < 0) { return; }

        m_currentState = State.ANIMATION;

        // 現在のページのトランスフォームの取得
        Image currentImage = m_currentImage;
        RectTransform currentImageTransform = currentImage.GetComponent<RectTransform>();

        // 次のページの取得
        m_nextImage = m_currentTutorialPageImages[nextImageIndex];
        // 次のページを表示する
        m_nextImage.gameObject.SetActive(true);
        
        // 透過フェード
        currentImage.DOFade(0.0f, NEXT_PAGE_ANIMATION_TIME - NEXT_PAGE_ANIMATION_TIME * 0.6f).SetEase(Ease.OutSine).SetDelay(NEXT_PAGE_ANIMATION_TIME * 0.6f);
        // 少し上にずらす
        currentImageTransform.DOBlendableMoveBy(new Vector3(0.0f, UPPER_Y, 0.0f), NEXT_PAGE_ANIMATION_TIME).SetEase(Ease.OutCubic);
        // Y軸のスケールを0にする
        currentImageTransform.DOScaleY(0.0f, NEXT_PAGE_ANIMATION_TIME).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            // 元の状態に戻す
            currentImage.color = new Color(currentImage.color.r, currentImage.color.g, currentImage.color.b, 1.0f);
            currentImageTransform.position = m_centerPosition;
            currentImageTransform.localScale = new Vector3(currentImageTransform.localScale.x, currentImageTransform.localScale.x, currentImageTransform.localScale.x);

            ApplyPage(nextImageIndex);
            m_currentState = State.NORMAL;
        });

    }

  

    /// <summary>
    /// 前のページにいく
    /// </summary>
    void ChangePreviousPage()
    {
        if (m_currentState == State.ANIMATION) { return; }

        // 前のインデックスへ減算する
        int nextImageIndex = Mathf.Max((m_currentImageIndex - 1), 0);

        if (nextImageIndex == m_currentImageIndex || nextImageIndex < 0){ return; }

        m_currentState = State.ANIMATION;

        // 現在のページのトランスフォームの取得
        Image currentImage = m_currentTutorialPageImages[m_currentImageIndex];

        // 次のページの取得
        m_nextImage = m_currentTutorialPageImages[nextImageIndex];
        // 次のページを表示する
        m_nextImage.gameObject.SetActive(true);
        RectTransform nextTransform = m_nextImage.GetComponent<RectTransform>(); ;

        float scale = m_nextImage.transform.localScale.x;


        m_nextImage.transform.localScale = new Vector3 (scale, 0.0f, scale);
        // 透過フェード
        m_nextImage.DOFade(1.0f, PREVIOUS_PAGE_ANIMATION_TIME - PREVIOUS_PAGE_ANIMATION_TIME * 0.1f).SetEase(Ease.OutSine);
        // 少し上にずらす
        nextTransform.position = new Vector3(nextTransform.position.x, m_centerPosition.y + UPPER_Y, nextTransform.position.z);
        nextTransform.DOBlendableMoveBy(new Vector3(0.0f, -UPPER_Y, 0.0f), PREVIOUS_PAGE_ANIMATION_TIME).SetEase(Ease.OutCubic);
        // Y軸のスケールを0にする
        nextTransform.DOScaleY(0.2f, PREVIOUS_PAGE_ANIMATION_TIME).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            // 元の状態に戻す
            currentImage.color = new Color(currentImage.color.r, currentImage.color.g, currentImage.color.b, 1.0f);
            nextTransform.position = m_centerPosition;
            nextTransform.localScale = new Vector3(nextTransform.localScale.x, nextTransform.localScale.x, nextTransform.localScale.x);

            ApplyPage(nextImageIndex);
            m_currentState = State.NORMAL;
        });
    }

    /// <summary>
    /// チュートリアルを終了する
    /// </summary>
    void ResetTutorial()
    {
        gameObject.SetActive(false);

        foreach(var pageImage in m_currentTutorialPageImages)
        {
            Destroy(pageImage.gameObject);
        }

        m_tutorialPageSprites.Clear();
        m_currentTutorialPageImages.Clear();
    }
    void ApplyPage(int nextImageIndex)
    {
        m_currentImage  .gameObject.SetActive(false);
        m_nextImage     .gameObject.SetActive(true);

        m_currentImageIndex = nextImageIndex;
        m_currentImage = m_currentTutorialPageImages[m_currentImageIndex];
    }

    public void OnNextPage(InputAction.CallbackContext context )
    {
        if (m_currentState == State.NORMAL)
        {
             // 次のページに行く
            ChangeNextPage();
        }


    }

    public void OnPreviousPage(InputAction.CallbackContext context)
    {
        if (m_currentState == State.NORMAL)
        {
            // 次のページに行く
            ChangePreviousPage();
        }


    }

    public void OnEndTutorial(InputAction.CallbackContext context)
    {
        if (State.FADE == m_currentState) { return; }
        if (m_displayTutorialImageData.keyboardImages.Count - 1 > m_currentImageIndex) { return; }

        StartEndFade();
        
    }
    private void Update()
    {
        if (m_currentState == State.FADE) { return; }

        UpdatePageImages();

        UpdateGuideUI();
    }

    /// <summary>
    /// 現在のイメージの更新
    /// </summary>
    private void UpdateCurrentPage()
    {
        // 現在のイメージを非表示にする
        m_currentImage.gameObject.SetActive(false);

        // 新しく取得して表示にする
        m_currentImage = m_currentTutorialPageImages[m_currentImageIndex];
        m_currentImage.gameObject.SetActive(true);
    }

    /// <summary>
    /// イメージの設定
    /// </summary>
    /// <param name="pageSprites"></param>
    /// <param name="setImages"></param>
    /// <param name="tutorialImagePrefab"></param>
    /// <param name="parent"></param>
    static private void  SetUpImage(List<Sprite> pageSprites,  List<Image> setImages, GameObject tutorialImagePrefab, Transform parent)
    {
        // チュートリアルページの作成
        for (int i = 0; i < pageSprites.Count; i++)
        {
            // チュートリアルのイメージの作成
            var imageObject = Instantiate(tutorialImagePrefab, parent);

            imageObject.transform.SetAsFirstSibling();

            // イメージコンポーネントにスプライトを設定する
            var image = imageObject.GetComponent<Image>();
            image.sprite = pageSprites[i];

            // 非表示にする
            imageObject.SetActive(false);

            // 配列に追加
            setImages.Add(image);
        }
    }

    void StartFadeIn()
    {
        m_currentState = State.FADE;

        // 透明度を0にする
        float backAlpha = m_backgroundImage.color.a;
        m_backgroundImage.color = new Color(m_backgroundImage.color.r, m_backgroundImage.color.g, m_backgroundImage.color.b, 0.0f);
        m_currentImage.color = new Color(m_currentImage.color.r, m_currentImage.color.g, m_currentImage.color.b, 0.0f);

        // フェードする
        m_backgroundImage.DOFade(backAlpha, 1.5f).SetEase(Ease.OutCubic);
        m_currentImage.DOFade(1.0f, 1.5f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            m_currentState = State.NORMAL;
        });
    }
    void StartEndFade()
    {
        m_currentState = State.FADE;

        UpdateGuideUI();

        // 透明度を1にする
        m_backgroundImage.DOFade(0.0f, 1.0f).SetEase(Ease.OutCubic);
        m_currentImage.DOFade(0.0f, 1.0f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            PauseEaseForGuideUI();
            ResetTutorial();
            // 終了したことを通知
            InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.TUTORIAL_END);
        });
    }

    void UpdatePageImages()
    {
        if (IsConnectedGamepad())
        {

            m_currentTutorialPageImages = m_displayTutorialImageData.gamepadImages;
            UpdateCurrentPage();

        }
        else
        {
            m_currentTutorialPageImages = m_displayTutorialImageData.keyboardImages;
            UpdateCurrentPage();
        }
    }

    /// <summary>
    /// ガイドUIの更新処理
    /// </summary>
    void UpdateGuideUI()
    {
        // 一旦すべて見えなくする
        m_leftArrowPageGuide    .gameObject.SetActive(false);
        m_rightArrowPageGuide   .gameObject.SetActive(false);
        m_exitGuideKeyboardImage.gameObject.SetActive(false);
        m_exitGuideGamepadImage .gameObject.SetActive(false);

        if (m_currentState != State.NORMAL) { return; }

        // 最後のページかどうか
        if (m_displayTutorialImageData.keyboardImages.Count - 1 <= m_currentImageIndex )
        {
            if (m_displayTutorialImageData.keyboardImages.Count != 1)
            {
                m_leftArrowPageGuide.gameObject.SetActive(true);
            }


            if (IsConnectedGamepad())
            {
                m_exitGuideGamepadImage.gameObject.SetActive(true);
            }
            else
            {
                m_exitGuideKeyboardImage.gameObject.SetActive(true);
            }
        }
        else
        {
            m_leftArrowPageGuide.gameObject.SetActive(true);
            m_rightArrowPageGuide.gameObject.SetActive(true);

          

        }
    }

    /// <summary>
    /// ゲームパッドに接続されているかどうか
    /// </summary>
    /// <returns></returns>
    private bool IsConnectedGamepad()
    {
        string[] padNames = Input.GetJoystickNames();

        return (padNames.Length > 0 && padNames[0] != "");
    }

    void PlayEaseForGuideUI()
    {
        // 一時停止する
        m_leftArrowPageGuide.GetComponent<RectTransform>().DOPlay();
        m_rightArrowPageGuide.GetComponent<RectTransform>().DOPlay();

        m_exitGuideGamepadImage.DOPlay();
        m_exitGuideKeyboardImage.DOPlay();
    }
    void PauseEaseForGuideUI()
    {
        // 一時停止する
        m_leftArrowPageGuide.GetComponent<RectTransform>().DOPause();
        m_rightArrowPageGuide.GetComponent<RectTransform>().DOPause();

        m_exitGuideGamepadImage.DOPause();
        m_exitGuideKeyboardImage.DOPause();
    }
}
