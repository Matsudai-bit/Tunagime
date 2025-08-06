using UnityEngine;

/// <summary>
/// 想いの流れ
/// </summary>

public class EmotionCurrent : MonoBehaviour
{

    /// <summary>
    /// 想いの流れの種類
    /// </summary>
    public enum Type
    {
        NONE,         // 初期状態
        FAITHFULNESS, // 誠実     青
        LOVE,         // 愛       赤　
        KINDNESS,     // 優しさ   緑
        COURAGE,      // 勇気     黄
        LONGING,      // 憧れ     紫
        REJECTION,    // 拒絶     黒
    }

    [SerializeField]
    private Type m_Type = Type.NONE; // 想いの流れの種類


    public Type CurrentType
    {
        get { return m_Type; }
        set { m_Type = value; }
    }

}
