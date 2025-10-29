using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


/// <summary>
/// チュートリアルの操作コンポーネント
/// </summary>
public class TutorialWindowController : MonoBehaviour
{
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
        NORMAL,     // 通常状態
        ANIMATION,  // アニメーション状態
    }

    [SerializeField]
    private GameObject m_tutorialImagePrefab; // チュートリアル画像プレハブ
    
    [SerializeField]
    private List<Sprite> m_tutorialPageSprites ; // チュートリアルスプライト

    private List<Image> m_currentTutorialPageImages = new();   // インスタンス化されたスプライト

    private int m_currentImageIndex;    // 現在のイメージ画像

    private int m_nextImageIndex;       // 次のイメージ画像

    private TutorialImageForDisplay m_displayTutorialImageData; // 表示するチュートリアル画像データ

    [Header("次のページに行く時のアニメーション時間")]
    [SerializeField]
    private float NEXT_PAGE_ANIMATION_TIME = 1.0f;

    [Header("前のページに行く時のアニメーション時間")]
    [SerializeField]
    private float PREVIOUS_PAGE_ANIMATION_TIME = 0.5f;

    [Header("上げるY座標の量")]
    [SerializeField]
    private float UPPER_Y = 20.0f;


    private List<Sprite> m_currentDisplayedTutorialSprite;   // 現在表示するスプライト

    private Vector3 m_centerPosition;   // 中心座標

    private State m_currentState;       // 現在の状態

    private void Awake()
    {
        if (m_tutorialImagePrefab == null)
        {
            Debug.LogError("チュートリアル画像プレハブが設定されていません");
        }

        gameObject.SetActive(false);
    }

    void Start()
    {


    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize(TutorialEventData.InputDataForTutorialSprite tutorialSpriteData)
    {
        ResetTutorial();

        m_displayTutorialImageData = new();

        // キーボードイメージのセットアップ
        SetUpImage(tutorialSpriteData.pageKeyboardSprites, m_displayTutorialImageData.keyboardImages, m_tutorialImagePrefab, transform);

        // ゲームパッドイメージのセットアップ
        SetUpImage(tutorialSpriteData.pageGamepadSprites, m_displayTutorialImageData.gamepadImages, m_tutorialImagePrefab, transform);
        // 現在表示するリストを設定する
        m_currentTutorialPageImages = m_displayTutorialImageData.keyboardImages;
        // 初期インデックスの設定
        m_currentImageIndex = 0;
        // 現在の画像を設定する
        m_currentTutorialPageImages[m_currentImageIndex].gameObject.SetActive(true);



        m_centerPosition = m_currentTutorialPageImages[m_currentImageIndex].gameObject.GetComponent<RectTransform>().position;
        m_currentState = State.NORMAL;
    }

    public void StartTutorial()
    {
        gameObject.SetActive(true);
        m_currentState = State.NORMAL;
    }

    
    /// <summary>
    /// 次のページに切り替える
    /// </summary>
    void ChangeNextPage()
    {
        if (m_currentState == State.ANIMATION) { return; }

        // 次のインデックスへ加算する
        m_nextImageIndex = Mathf.Min( (m_currentImageIndex + 1), m_currentTutorialPageImages.Count - 1);
        
        if (m_nextImageIndex == m_currentImageIndex || m_nextImageIndex < 0) { return; }

        m_currentState = State.ANIMATION;

        // 現在のページのトランスフォームの取得
        Image currentImage = m_currentTutorialPageImages[m_currentImageIndex];
        RectTransform currentImageTransform = currentImage.GetComponent<RectTransform>();

        // 次のページの取得
        Image nextImage = m_currentTutorialPageImages[m_nextImageIndex];
        // 次のページを表示する
        nextImage.gameObject.SetActive(true);
        
        // 透過フェード
        currentImage.DOFade(0.0f, NEXT_PAGE_ANIMATION_TIME - NEXT_PAGE_ANIMATION_TIME * 0.6f).SetEase(Ease.OutSine).SetDelay(NEXT_PAGE_ANIMATION_TIME * 0.6f);
        // 少し上にずらす
        currentImageTransform.DOBlendableMoveBy(new Vector3(0.0f, UPPER_Y, 0.0f), NEXT_PAGE_ANIMATION_TIME).SetEase(Ease.OutCubic);
        // Y軸のスケールを0にする
        currentImageTransform.DOScaleY(0.0f, NEXT_PAGE_ANIMATION_TIME).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            ApplyPage();
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
        m_nextImageIndex = Mathf.Max((m_currentImageIndex - 1), 0);

        if (m_nextImageIndex == m_currentImageIndex || m_nextImageIndex < 0){ return; }

        m_currentState = State.ANIMATION;

        // 現在のページのトランスフォームの取得
        Image currentImage = m_currentTutorialPageImages[m_currentImageIndex];

        // 次のページの取得
        Image nextImage = m_currentTutorialPageImages[m_nextImageIndex];
        // 次のページを表示する
        nextImage.gameObject.SetActive(true);
        RectTransform nextTransform = nextImage.GetComponent<RectTransform>(); ;

        float scale = nextImage.transform.localScale.x;


        nextImage.transform.localScale = new Vector3 (scale, 0.0f, scale);
        // 透過フェード
        nextImage.DOFade(1.0f, PREVIOUS_PAGE_ANIMATION_TIME - PREVIOUS_PAGE_ANIMATION_TIME * 0.1f).SetEase(Ease.OutSine);
        // 少し上にずらす
        nextTransform.position = new Vector3(nextTransform.position.x, m_centerPosition.y + UPPER_Y, nextTransform.position.z);
        nextTransform.DOBlendableMoveBy(new Vector3(0.0f, -UPPER_Y, 0.0f), PREVIOUS_PAGE_ANIMATION_TIME).SetEase(Ease.OutCubic);
        // Y軸のスケールを0にする
        nextTransform.DOScaleY(0.2f, PREVIOUS_PAGE_ANIMATION_TIME).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            ApplyPage();
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
    void ApplyPage()
    {
        m_currentTutorialPageImages[m_currentImageIndex].gameObject.SetActive(false);
        m_currentTutorialPageImages[m_nextImageIndex].gameObject.SetActive(true);

        m_currentImageIndex = m_nextImageIndex;
        m_nextImageIndex = -1;
    }

    public void OnNextPage(InputValue value )
    {
        if (m_currentState == State.NORMAL)
        {
             // 次のページに行く
            ChangeNextPage();
        }


    }

    public void OnPreviousPage(InputValue value)
    {
        if (m_currentState == State.NORMAL)
        {
            // 次のページに行く
            ChangePreviousPage();
        }


    }

    public void OnEndTutorial(InputValue value)
    {
        ResetTutorial();
    }

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
}
