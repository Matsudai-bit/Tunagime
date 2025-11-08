using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 設定画面のコントローラー
/// </summary>
public class SettingsWindowController : MonoBehaviour
{
    [Header("設定画面のコンテンツリスト（順番大事)")]
    [SerializeField]
    private List<SettingsContent> settingsContents = new List<SettingsContent>();   // 設定画面のコンテンツリスト

    // 保存ボタン
    [Header("保存ボタン")]
    [SerializeField]
    private SelectButton m_saveButton;

    private int m_currentContentIndex = 0;   // 現在表示しているコンテンツのインデックス

    private int m_totalContentsCount = 0;    // コンテンツの総数

    private Action m_onExit; // 終了時のアクション

    private float m_initialScale = 1.0f; // 初期スケール

    private bool m_canInput = false; // 入力可能フラグ


    private void Awake()
    {
        // コンテンツの総数を設定する
        m_totalContentsCount = settingsContents.Count + 1;

        // 初期スケールを取得する
        m_initialScale = transform.localScale.x;



    }

    /// <summary>
    /// 設定画面のコントローラーの表示を開始する
    /// </summary>
    public void Open(Action onExit)
    {

        // 最初はすべてのコンテンツを操作不可能にする
        foreach (var content in settingsContents)
        {
            content.Initialize();
        }

        // 終了時のアクションを設定する
        m_onExit = onExit;

        // 設定画面を表示する
        gameObject.SetActive(true);

        // 最初のコンテンツを表示する
        m_currentContentIndex = 0;
        // 現在のコンテンツを操作可能にする
        settingsContents[m_currentContentIndex].EnableInteractable(); ;

        float targetScale = m_initialScale;
        gameObject.transform.localScale = Vector3.zero;
        gameObject.transform.DOScale(targetScale, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            m_canInput = true;
        });

        m_saveButton.SetSelected(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeSelf) return;

        if (m_currentContentIndex < settingsContents.Count - 1)
        {
            settingsContents[m_currentContentIndex].OnUpdate();
        }
    }

    // 切り替える
    void SwitchContent(bool nextContent)
    {
        SoundManager.GetInstance.RequestPlaying(SoundID.SE_UI_BUTTON_MOVE);

        // 現在のコンテンツを操作不可能にする
        if (m_currentContentIndex == settingsContents.Count)
        {
            // 保存ボタンの選択状態を解除する
            m_saveButton.SetSelected(false);
        }
        else
        {
            settingsContents[m_currentContentIndex].DisableInteractable();
        }

        // 次のコンテンツのインデックスを計算する
        if (nextContent)
        {
            m_currentContentIndex++;
        }
        else
        {
            m_currentContentIndex--;
        }

        // インデックスが範囲外の場合、ループさせる
        m_currentContentIndex = (m_currentContentIndex + m_totalContentsCount) % m_totalContentsCount;


        // 新しいコンテンツを操作可能にする
        if (m_currentContentIndex == settingsContents.Count)
        {
            // 保存ボタンを選択状態にする
            m_saveButton.SetSelected(true);
            return;
        }
        else
        {
            settingsContents[m_currentContentIndex].EnableInteractable();
        }
    }

    /// <summary>
    /// ナビゲート入力処理
    /// </summary>
    /// <param name="context"></param>
    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (m_canInput == false )return;

        if (m_currentContentIndex < settingsContents.Count)
        {
            settingsContents[m_currentContentIndex].OnNavigate(context);

        }


        if (context.performed)
        {
            Vector2 navigation = context.ReadValue<Vector2>();
            if (navigation.y > 0.1f)
            {
                // 上に移動
                SwitchContent(false);
            }
            else if (navigation.y < -0.1f)
            {
                // 下に移動
                SwitchContent(true);
            }
        }


    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (m_canInput == false )return;
        if (!context.performed) return;

        if (m_currentContentIndex == settingsContents.Count)
        {
            SoundManager.GetInstance.RequestPlaying(SoundID.SE_UI_BUTTON_PUSH);

            // 保存ボタンが選択されている場合、すべてのコンテンツの設定を保存する
            foreach (var content in settingsContents)
            {
                content.SaveSettings();
            }
            // 設定データを保存する
            GameContext.GetInstance.SaveSettingData();
            OnExitComplete();
        }
        else
        {
            settingsContents[m_currentContentIndex].OnSubmit(context);
        }
    }

    public void OnReset(InputAction.CallbackContext context)
    {
        if (m_canInput == false )return;
        if (!context.performed) return;
        SoundManager.GetInstance.RequestPlaying(SoundID.SE_UI_BUTTON_PUSH);
        // すべてのコンテンツの設定をリセットする
        foreach (var content in settingsContents)
        {
            content.ResetSettings();
        }


    }

    public void OnCancel(InputAction.CallbackContext context)
    {

        if (m_canInput == false )return;
        SoundManager.GetInstance.RequestPlaying(SoundID.SE_UI_BUTTON_BACK);
        // すべてのコンテンツの設定をキャンセルする
        foreach (var content in settingsContents)
        {
            content.CancelSettings();
        }



        OnExitComplete();
    }

    private void OnExitComplete()
    {
        m_canInput = false;
        float targetScale = 0.0f;
        gameObject.transform.DOScale(targetScale, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            // 設定画面を非表示にする
            gameObject.SetActive(false);

            // 終了時のアクションを呼び出す
            m_onExit?.Invoke();
        });
    }
}
