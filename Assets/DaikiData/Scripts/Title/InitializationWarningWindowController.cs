using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

/// <summary>
/// Initialization Warning Window Controller
/// </summary>
public class InitializationWarningWindow : MonoBehaviour
{
    [Header("UIボタン")]
    [SerializeField]
    private List<Button> m_buttons = new();

    [Header("決定時のイベントリスト")]
    [SerializeField]
    private List<SubmitEventInfo> m_submitEventList = new(); // 決定時のイベント辞書

    [Header("ルート")]
    [SerializeField]
    private GameObject m_rootObject; // ルートオブジェクト

    /// 決定時のイベント構造体
    /// </summary>
    [Serializable]
    struct SubmitEventInfo
    {
        public string menuName; // メニュー名
        public UnityEvent submitEvent; // 決定時のイベント
    }

    private int m_currentSelectedIndex = 0; // 現在選択されているボタンのインデックス

    private bool m_canInput = false; // 入力可能フラグ

    private float m_initialScale = 1.0f; // 初期スケール

    private float m_initialButtonScale = 1.0f; // ボタンの初期スケール
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_initialScale = m_rootObject.transform.localScale.x;


        // 各ボタンにクリックイベントを追加
        for (int i = 0; i < m_buttons.Count; i++)
        {
            int index = i; // ローカル変数にキャプチャ
            m_buttons[i].onClick.AddListener(() =>
            {
                // クリックされたときの処理
                var submitEventInfo = m_submitEventList.Find(e => e.menuName == m_buttons[index].name);
                if (submitEventInfo.submitEvent != null)
                {
                    submitEventInfo.submitEvent.Invoke();
                }
            });
        }
    }

    private void OnEnable()
    {

    }

    /// <summary>
    /// ウィンドウを開く
    /// </summary>
    public void Open()
    {
        gameObject.SetActive(true);

        // 音を鳴らす
        SoundManager.GetInstance.RequestPlaying(SoundID.SE_GAMERESET_WINDOW_OPEN);

        m_initialButtonScale = m_buttons[0].transform.localScale.x;

        m_canInput = false;

        m_currentSelectedIndex = 0;
        UpdateButtonVisual();

        float targetScale = 1.0f;
        m_rootObject.transform.localScale = Vector3.zero;
        m_rootObject.transform.DOScale(targetScale, 0.5f).SetEase(Ease.OutBack).OnComplete(()=>
        {
            m_canInput = true;
        });
    }

    public void Close()
    {
        SoundManager.GetInstance.RequestPlaying(SoundID.SE_UI_BUTTON_PUSH);


        m_canInput = false;
        float targetScale = 0.0f;
        m_rootObject.transform.DOScale(targetScale, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);

            // ボタンを元のスケールに戻す
            for (int i = 0; i < m_buttons.Count; i++)
            {
                m_buttons[i].transform.localScale = Vector3.one * m_initialButtonScale;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// カーソル移動時の処理
    /// </summary>
    /// <param name="value"></param>
    public void OnMoveCursor(InputAction.CallbackContext value)
    {
        if (m_canInput == false) { return; }
        if (!value.performed){ return; }

        // 入力ベクトルを取得
        Vector2 vector = value.ReadValue<Vector2>();

        // カーソル移動処理
        MoveCursor(vector);
    }

    public void OnSubmit(InputAction.CallbackContext value)
    {
        if (!value.performed) { return; }
        if (m_canInput == false) { return; }
        // 現在選択されているボタンのクリックイベントを呼び出す
        var selectedButton = m_buttons[m_currentSelectedIndex];


        if (selectedButton != null)
        {
            selectedButton.onClick.Invoke();
        }
    }

    public void OnCancel(InputAction.CallbackContext value)
    {
        if (!value.performed) { return; }
        if (m_canInput == false) { return; }


        // ウィンドウを閉じる処理をここに追加
        Close();
    }



    /// <summary>
    /// カーソル移動処理
    /// </summary>
    /// <param name="vector"></param>
    private void MoveCursor(Vector2 vector)
    {
        bool moved = false;

        if (vector.x > 0)
        {
            // 上移動
            m_currentSelectedIndex--;
            if (m_currentSelectedIndex < 0)
            {
                m_currentSelectedIndex = m_buttons.Count - 1;

                moved = true;
            }
        }
        else if (vector.x < 0)
        {
            // 下移動
            m_currentSelectedIndex++;
            if (m_currentSelectedIndex >= m_buttons.Count)
            {
                m_currentSelectedIndex = 0;

                moved = true;
            }
        }

        if (moved)
        {
            // ボタンのビジュアルを更新
            UpdateButtonVisual();

            // サウンド再生
            SoundManager.GetInstance.RequestPlaying(SoundID.SE_UI_BUTTON_MOVE);
        }
    }

    /// <summary>
    /// ボタンのビジュアルを更新
    /// </summary>
    void UpdateButtonVisual()
    {
        for (int i = 0; i < m_buttons.Count; i++)
        {
            var button = m_buttons[i];
            var colors = button.colors;
            if (i == m_currentSelectedIndex)
            {

                button.OnPointerEnter(null);
                // 元に戻す
                button.image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                button.transform.localScale = Vector3.one * (m_initialButtonScale + 0.05f);
            }

            else
            {   
                // 暗くする
                button.image.color = new Color(0.7f, 0.7f, 0.7f, 1.0f);
                button.transform.localScale = Vector3.one * m_initialButtonScale;


                button.OnPointerExit(null);
            }
        }
    }
}
