using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// メニューのコンテンツセレクター
/// </summary>
public class MenuContentSelector : MonoBehaviour
{


    /// <summary>
    /// タイトルメニューの情報構造体
    /// </summary>
    [Serializable]
    struct TitleMenuInfo
    {
        public string menuName; // タイトルメニューの種類
        public GameObject gameObject; // タイトルメニューの位置
    }

    [Header("親(親が有効の時に入力可能)")]
    [SerializeField] private GameObject m_parentObject;


    [Header("初期位置")]
    [SerializeField] private string m_startPointMenu; // タイトルセレクターの初期位置

    [Header("タイトルセレクター")]
    [SerializeField] private GameObject m_titleSelector; // タイトルセレクター

    [Header("タイトルメニュー")]
    [SerializeField] private List<TitleMenuInfo> m_titleMenu = new (); // タイトルセレクターのリスト

    [Header("Canvas")]
    [SerializeField]
    private CanvasScaler m_canvasScalar;


    private Dictionary<string, GameObject> m_titleMenuDict = new();  // タイトルメニューの辞書
    
    private string m_currentTitleMenu; // 現在のタイトルメニュー

    private float m_swayingDuration = 1.0f; // 点滅の周期

    private float m_currentTime = 0.0f; // 現在の時間

    bool m_isMoving = false; // 移動中かどうか



    /// <summary>
    /// 現在のタイトルメニュー名を取得
    /// </summary>
    public string CurrentTitleMenuName
    {
        get { return m_currentTitleMenu; }
    }

    /// <summary>
    /// タイトルメニューのリストを取得
    /// </summary>
    public string[] TitleMenuList
    {
        get
        {
            List<string> menuNames = new List<string>();
            foreach (var menuInfo in m_titleMenu)
            {
                menuNames.Add(menuInfo.menuName);
            }
            return menuNames.ToArray();
        }
    }

    void Awake()
    {
        // タイトルメニューの辞書を初期化
        foreach (var menuInfo in m_titleMenu)
        {
            m_titleMenuDict[menuInfo.menuName] = menuInfo.gameObject;
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

        selectorRectTransform.anchoredPosition= GetSelectorPosition(m_currentTitleMenu) + new Vector2(amount * ratio - amount / 2.0f, 0);



    }

    // 指したい場所へセレクターを移動す座標を取得する
    private Vector2 GetSelectorPosition(string menu)
    {
        var rectTransform = m_titleMenuDict[menu].gameObject.GetComponent<RectTransform>();

        var selectorRectTransform = m_titleSelector.GetComponent<RectTransform>();

        // 位置を取得
        Vector2 pos = rectTransform.anchoredPosition;


        // セレクターの幅を考慮して、左端に合わせる
        //pos.x = rectTransform.anchoredPosition.x - rectTransform.rect.width / 2.0f;

        //pos.x -= selectorRectTransform.rect.width / 2.0f;

        //pos.x = rectTransform.anchoredPosition.x;

        
        pos.x = rectTransform.anchoredPosition.x - ((rectTransform.localScale.x * rectTransform.rect.width) / 2.0f + selectorRectTransform.rect.width * selectorRectTransform.localScale.x / 2.0f);
        pos.x += 15.0f;
        pos.y = rectTransform.anchoredPosition.y;

        return (Vector3)pos;
        //return rectTransform.position;
    }

    /// <summary>
    /// セレクターの位置を更新
    /// </summary>
    private void UpdateSelectorPosition()
    {
        var newPos = GetSelectorPosition(m_currentTitleMenu);

        var selectorRectTransform = m_titleSelector.GetComponent<RectTransform>();

        m_isMoving = true;

        // ローカル座標にする
    
        // セレクターの位置を更新
        selectorRectTransform.DOAnchorPos(newPos, 0.2f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            m_isMoving = false;
        });

    }

    /// <summary>
    /// 上下の入力に基づいてワールドIDを変更
    /// </summary>
    /// <param name="value"></param>
    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (m_titleMenuDict.Count <= 0 ) { return; }
        if (m_parentObject == null || !m_parentObject.activeSelf) { return; }

        Vector2 input = context.ReadValue<Vector2>();

        int newPointMenu = GetIndex(m_currentTitleMenu);

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

        newPointMenu = Math.Clamp(newPointMenu, 0, m_titleMenuDict.Count - 1);

        if (newPointMenu != GetIndex(m_currentTitleMenu))
        {
            // SEを鳴らす
            SoundManager.GetInstance.PlaySE(SoundID.SE_UI_BUTTON_MOVE);

            m_currentTitleMenu = GetMenuName(newPointMenu);

            // セレクターの位置を更新
            UpdateSelectorPosition();

            Debug.Log($"Switched to {m_currentTitleMenu}");
        }
    }

    /// <summary>
    /// メニュー名からインデックスを取得
    /// </summary>
    /// <param name="menuName"></param>
    /// <returns></returns>
    int GetIndex(string menuName)
    {
        int index = 0;
        foreach (var menuInfo in m_titleMenu)
        {
            if (menuInfo.menuName == menuName)
            {
                return index;
            }
            index++;
        }
        return -1;
    }

    /// <summary>
    /// インデックスからメニュー名を取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    string GetMenuName(int index)
    {
        int currentIndex = 0;
        foreach (var menuInfo in m_titleMenu)
        {
            if (currentIndex == index)
            {
                return menuInfo.menuName;
            }
            currentIndex++;
        }
        return null;
    }

}
