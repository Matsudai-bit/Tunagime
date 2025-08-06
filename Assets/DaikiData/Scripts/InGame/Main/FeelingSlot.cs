using UnityEngine;

/// <summary>
/// 想いの型
/// </summary>
public class FeelingSlot : MonoBehaviour
{
    [SerializeField] private FeelingCore m_feelingCore; // 想いの核


    /// <summary>
    /// スタートメソッド
    /// </summary>

    private void Start()
    {

    }



    /// <summary>
    /// 現在の感情タイプを取得する
    /// </summary>
    /// <returns>想いの種類</returns>
    public EmotionCurrent.Type GetEmotionType()
    {
        return m_feelingCore.GetEmotionType(); // 想いの流れの種類を取得
    }


    /// <summary>
    /// ステージブロックの取得
    /// </summary>
    public StageBlock StageBlock
    {
        get
        {
            return GetComponent<StageBlock>(); // StageBlockコンポーネントを取得
        }
    }
}
