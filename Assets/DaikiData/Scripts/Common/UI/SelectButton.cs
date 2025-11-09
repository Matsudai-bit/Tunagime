using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 選択ボタンのクラス
/// </summary>
public class SelectButton : MonoBehaviour
{
    #region 構造体
    [System.Serializable]
    struct InspectorSettings
    {
        public float selectedScaleMultiplier;   // 選択時のスケール倍率
        public Sprite normalSprite;                // 通常時のスプライト
        public Sprite selectedSprite;              // 選択時のスプライト
    }

    #endregion

    [Header("設定")]
    [SerializeField]
    InspectorSettings m_inspectorSettings;   // インスペクター設定

    private Button m_button;    // ボタンコンポーネント

    private bool m_isSelected = false;   // 選択状態かどうか

    private float m_initialScale;    // 初期スケール

    private void Awake()
    {
        m_button = GetComponent<Button>();
        m_initialScale = transform.localScale.x;

    }

    private void Start()
    {
    }

    public void SetSelected(bool isSelected)
    {
        m_isSelected = isSelected;
        var image = m_button.GetComponent<Image>();

        if (m_isSelected)
        {
            // 選択状態の場合、スケールを大きくする
            transform.localScale = Vector3.one * (m_initialScale * m_inspectorSettings.selectedScaleMultiplier);

            // スプライトを選択時のものに変更する
            image.sprite = m_inspectorSettings.selectedSprite;
        }
        else
        {
            // 非選択状態の場合、初期スケールに戻す
            transform.localScale = Vector3.one * m_initialScale;

            // スプライトを通常時のものに変更する
            image.sprite = m_inspectorSettings.normalSprite;
        }
    }

}
