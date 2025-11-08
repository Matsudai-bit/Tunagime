using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


/// <summary>
/// サウンド設定コンテンツ
/// </summary>
public class SoundSettingContent : SettingsContent
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

    [Header("BGMかどうか　falseだとSE設定になる")]
    [SerializeField]
    private bool m_bgmSetting = true;


    [Header("リセット音量値")]
    [SerializeField]
    private float RESET_VOLUME = 0.5f;

    [Header("スライダー")]
    [SerializeField]
    private Slider m_slider;

    [Header("スライダーハンドルイメージ")]
    [SerializeField]
    private Image m_sliderHandleImage;

    [Header("スライダーハンドルスプライトペア")]
    [SerializeField]
    private SpritePair m_sliderHandleSpritePair;

    private bool m_isInteractable = false;

    private float m_previousSliderValue = -1f;


    public override void DisableInteractable()
    {
        m_sliderHandleImage.sprite = m_sliderHandleSpritePair.noActine;
        m_isInteractable = false;
    }

    public override void EnableInteractable()
    {
        m_sliderHandleImage.sprite = m_sliderHandleSpritePair.active;
        m_isInteractable = true;
    }

    public override void LoadSettings()
    {
        float volume = 0f;
        // 初期音量設定
        if (m_bgmSetting)
        {
            volume = GameContext.GetInstance.GetGameSettingParameters().bgmVolume;
        }
        else
        {
            volume = GameContext.GetInstance.GetGameSettingParameters().seVolume;
        }
        m_previousSliderValue = volume;

        ApplyVolume(volume);

    }

    public override void OnNavigate(InputAction.CallbackContext context)
    {

        // ナビゲート入力の値を取得
        float navigateValue = context.ReadValue<Vector2>().x;

        // スライダーの値を更新
        float volume = m_slider.value;
        volume += navigateValue * 0.1f;

        // スライダーの値を0から1の範囲にクランプ
        volume = Mathf.Clamp01(volume);

        ApplyVolume(volume);

    }

    public override void OnSubmit(InputAction.CallbackContext context)
    {
    }

    public override void OnUpdate()
    {
    }

    public override void ResetSettings()
    {
        ApplyVolume(RESET_VOLUME);

    }

    public override void SaveSettings()
    {
    }

    public override bool IsInteractable()
    {
        return m_isInteractable;
    }

    public override void CancelSettings()
    {
        ApplyVolume(m_previousSliderValue);

    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initialize()
    {
        m_isInteractable = false;

        DisableInteractable();
        LoadSettings();

        // 初手スプライトを設定
        m_sliderHandleImage.sprite = m_sliderHandleSpritePair.noActine;
    }


    /// <summary>
    /// 音量を反映
    /// </summary>
    private void ApplyVolume(float volume)
    {
        m_slider.value = volume;

        // 音量を反映
        if (m_bgmSetting)
            GameContext.GetInstance.GetGameSettingParameters().bgmVolume = volume;
        else
            GameContext.GetInstance.GetGameSettingParameters().seVolume = volume;
    }

}
