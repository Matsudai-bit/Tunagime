using UnityEngine;

/// <summary>
///  想いの核
/// </summary>
public class FeelingCore : MonoBehaviour
{


    private EmotionCurrent m_emotionCurrent; // 想いの感情タイプ

    /// <summary>
    /// Awakeメソッド
    /// </summary>
    private void Awake()
    {
        m_emotionCurrent = GetComponent<EmotionCurrent>(); // EmotionCurrentコンポーネントを取得
        if (m_emotionCurrent == null)
        {
            Debug.LogError("EmotionCurrent が null です");
        }
    }


    /// <summary>
    /// ゲームオブジェクトのアクティブ状態を設定する
    /// </summary>
    /// <param name="isActive"></param>
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive); // ゲームオブジェクトのアクティブ状態を設定
    }

    public EmotionCurrent.Type GetEmotionType()
    {
        return m_emotionCurrent.CurrentType; // 現在の感情タイプを取得
    }

}
