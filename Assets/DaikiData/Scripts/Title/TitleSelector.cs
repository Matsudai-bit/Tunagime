using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトルセレクター
/// </summary>
public class TitleSelector : MonoBehaviour
{
    /// <summary>
    /// タイトルメニューの種類
    /// </summary>
    [Serializable]
    public enum TitleMenuID
    {
        RESET_GAME,
        CONTINUE_GAME,
        SETTING,
        QUIT_GAME
    }

    /// <summary>
    /// タイトルメニューの情報構造体
    /// </summary>
    [Serializable]
    struct TitleMenuInfo
    {
        public TitleMenuID menu; // タイトルメニューの種類
        public GameObject gameObject; // タイトルメニューの位置
    }

    [Header("初期位置")]
    [SerializeField] private TitleMenuID m_startPointMenu; // タイトルセレクターの初期位置

    [Header("タイトルセレクター")]
    [SerializeField] private GameObject m_titleSelector; // タイトルセレクター

    [Header("タイトルメニュー")]
    [SerializeField] private List<TitleMenuInfo> m_titleMenu = new (); // タイトルセレクターのリスト


    private Dictionary<TitleMenuID, GameObject> m_titleMenuDict = new();  // タイトルメニューの辞書
    
    private TitleMenuID m_currentTitleMenu; // 現在のタイトルメニュー

    private float m_swayingDuration = 1.0f; // 点滅の周期

    private float m_currentTime = 0.0f; // 現在の時間

    bool m_isMoving = false; // 移動中かどうか

    public TitleMenuID CurrentTitleMenuID
    {
        get { return (TitleMenuID)m_currentTitleMenu; }
    }

    void Awake()
    {
        // タイトルメニューの辞書を初期化
        foreach (var menuInfo in m_titleMenu)
        {
            m_titleMenuDict[menuInfo.menu] = menuInfo.gameObject;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_currentTitleMenu = m_startPointMenu;
        UpdateSelectorPosition();

        var selectorRectTransform = m_titleSelector.GetComponent<RectTransform>();

 
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isMoving)
        {
            return;
        }
        m_currentTime += Time.deltaTime;

        // セレクターを揺らす
        float amount = 7.0f; // 揺れの大きさ

        float ratio = (Mathf.Sin((m_currentTime / m_swayingDuration) * Mathf.PI * 2) + 1) / 2.0f;

        var selectorRectTransform = m_titleSelector.GetComponent<RectTransform>();

        selectorRectTransform.anchoredPosition = GetSelectorPosition(m_currentTitleMenu) + new Vector2(amount * ratio - amount / 2.0f, 0);


        
    }

    // 指したい場所へセレクターを移動す座標を取得する
    private Vector2 GetSelectorPosition(TitleMenuID menu)
    {
        var rectTransform = m_titleMenuDict[menu].gameObject.GetComponent<RectTransform>();

        var selectorRectTransform = m_titleSelector.GetComponent<RectTransform>();

        // 位置を取得
        Vector2 pos = rectTransform.anchoredPosition;

        Debug.Log($"Menu: {menu}, Pos: {pos}, RectWidth: {rectTransform.rect.width}, SelectorWidth: {selectorRectTransform.rect.width}");

        // セレクターの幅を考慮して、左端に合わせる
        pos.x = rectTransform.anchoredPosition.x - rectTransform.rect.width / 2.0f;

        pos.x -= selectorRectTransform.rect.width / 2.0f ;

        pos.x += 15.0f; // 少し内側に寄せる

        return (Vector3)pos;
    }

    /// <summary>
    /// セレクターの位置を更新
    /// </summary>
    private void UpdateSelectorPosition()
    {
        var newPos = GetSelectorPosition(m_currentTitleMenu);

        var selectorRectTransform = m_titleSelector.GetComponent<RectTransform>();

        m_isMoving = true;

        // セレクターの位置を更新
        selectorRectTransform.DOLocalMove(newPos, 0.2f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            m_isMoving = false;
        });

    }

    /// <summary>
    /// 上下の入力に基づいてワールドIDを変更
    /// </summary>
    /// <param name="value"></param>
    public void OnNavigate(InputValue value)
    {

        Vector2 input = value.Get<Vector2>();

        int newPointMenu = (int)m_currentTitleMenu;

        // 上下の入力に基づいてワールドIDを変更

        // 下入力
        if (input.y < 0)
        {
            // 循環させるために剰余を取る
            newPointMenu = (newPointMenu + 1);
        }
        // 上入力
        else if (input.y > 0)
        {
            // 負の数に対応するために、長さを足してから剰余を取る
            newPointMenu = (newPointMenu - 1);
        }

        newPointMenu = Math.Clamp(newPointMenu, 0, Enum.GetValues(typeof(TitleMenuID)).Length - 1);

        if (newPointMenu != (int)m_currentTitleMenu)
        {
            m_currentTitleMenu = (TitleMenuID)newPointMenu;

            // セレクターの位置を更新
            UpdateSelectorPosition();

            Debug.Log($"Switched to {m_currentTitleMenu}");
        }
    }

    
}
