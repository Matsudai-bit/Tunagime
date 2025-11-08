using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// フルスクリーン設定コンテンツ
/// </summary>
public class FullScreenSettingContent : SettingsContent
{
    #region 構造体
    /// <summary>
    /// スプライトペア
    /// </summary>
    [System.Serializable]
    struct SpritePair
    {
        public Sprite active;   // アクティブスプライト
        public Sprite noActine; // 非アクティブスプライト
    }
    #endregion

    [Header("チェックマーク")]
    [SerializeField]
    GameObject m_checkMark;

    [Header("チェックボックスイメージ")]
    [SerializeField]
    Image m_checkBoxImage;

    [Header("チェックボックススプライトペア")]
    [SerializeField]
    SpritePair m_checkBoxSpritePair;



    bool m_prevFullScreenState;


    /// <summary>
    /// 画面サイズの切り替え
    /// </summary>
    private void SwitchScreenSize()
    {
        // フルスクリーンの切り替え
        bool isFullScreen = Screen.fullScreen;
        // フルスクリーンの設定を反転
        Screen.fullScreen = !isFullScreen;
        
        UpdateCheckMark();
    }

    /// <summary>
    /// チェックマークの表示更新
    /// </summary>
    private void UpdateCheckMark()
    {
        // チェックマークの表示更新
        if (Screen.fullScreen)
        {
            m_checkMark.SetActive(true);
        }
        else
        {
            m_checkMark.SetActive(false);
        }
    }

    /// <summary>
    /// 操作不可能にする
    /// </summary>
    public override void DisableInteractable()
    {
        m_checkBoxImage.sprite = m_checkBoxSpritePair.noActine;
    }

    /// <summary>
    /// 操作可能にする
    /// </summary>
    public override void EnableInteractable()
    {
        m_checkBoxImage.sprite = m_checkBoxSpritePair.active;
    }

    /// <summary>
    /// 操作可能かどうか
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public override bool IsInteractable()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// 設定の読み込み
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void LoadSettings()
    {
        m_prevFullScreenState = GameContext.GetInstance.GetGameSettingParameters().isFullScreen;
       Screen.fullScreen =  GameContext.GetInstance.GetGameSettingParameters().isFullScreen;
    }

    /// <summary>
    /// ナビゲート入力処理
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void OnNavigate(InputAction.CallbackContext context)
    {

    }

    /// <summary>
    /// サブミット入力処理
    /// </summary>
    /// <param name="context"></param>
    public override void OnSubmit(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        SoundManager.GetInstance.RequestPlaying(SoundID.SE_UI_BUTTON_PUSH);

        // 画面サイズの切り替え
        SwitchScreenSize();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void OnUpdate()
    {
    }

    /// <summary>
    /// 設定のリセット
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void ResetSettings()
    {
        Screen.fullScreen = true;
        UpdateCheckMark();
    }

    /// <summary>
    /// 設定の保存
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void SaveSettings()
    {
    }

    public override void CancelSettings()
    {
        Screen.fullScreen = m_prevFullScreenState;
        UpdateCheckMark();
    }

    public override void Initialize()
    {
        // チェックマークの表示更新
        UpdateCheckMark();
    }
}
