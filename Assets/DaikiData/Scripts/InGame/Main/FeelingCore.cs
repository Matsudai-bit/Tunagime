using UnityEngine;

/// <summary>
///  想いの核
/// </summary>
public class FeelingCore : MonoBehaviour
{

    private YarnMaterialLibrary.MaterialType m_materialType; // マテリアルのタイプ

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
    /// マテリアルを取得する
    /// </summary>
    /// <returns></returns>
    public Material GetMaterial()
    {
        return YarnMaterialLibrary.Instance.GetMaterial(m_materialType); // マテリアルライブラリからマテリアルを取得
    }

    /// <summary>
    /// マテリアルを設定する
    /// </summary>
    public void SetMaterial(YarnMaterialLibrary.MaterialType materialType)
    {
        m_materialType = materialType; // マテリアルを設定
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
