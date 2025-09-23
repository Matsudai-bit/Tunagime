using UnityEngine;

public class FeelingCoreInserter : MonoBehaviour
{
    [SerializeField] private FeelingCore m_feelingCore; // 想いの核

    private void Awake()
    {
        if (m_feelingCore == null)
        {
            Debug.LogError("FeelingCoreがアタッチされていません。");
        }
    }

    /// <summary>
    /// 想いの核を挿入する
    /// </summary>
    /// <param name="feelingCore"></param>
    public void InsertCore(FeelingCore feelingCore)
    {
        if (IsInsertCore())
        {
            Debug.LogError("既に想いの核が挿入されています。");
            return; // 既に想いの核が挿入されている場合は処理を中断
        }

        m_feelingCore.MeshRenderer.material = feelingCore.MeshRenderer.material;    // 想いの核のマテリアルを設定
        m_feelingCore.SetEmotionType(feelingCore.GetEmotionType());                 // 想いの核の感情タイプを設定
        m_feelingCore.SetActive(true);                                               // 想いの核をアクティブにする
    }

    /// <summary>
    /// 想いの核が挿入されているかどうかを判定
    /// </summary>
    /// <returns></returns>
    public bool IsInsertCore()
    {
        return m_feelingCore.GetEmotionType() != EmotionCurrent.Type.NONE; // 想いの核が挿入されているかどうかを判定
    }
}
