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
        if (m_feelingCore.GetEmotionType() != EmotionCurrent.Type.NONE)
        {
            var coreMaterial = MaterialLibrary.GetInstance.GetMaterial(MaterialLibrary.MaterialGroup.CORE, m_feelingCore.GetEmotionType()); // マテリアルライブラリの初期化
            m_feelingCore.MeshRenderer.material = coreMaterial; // 想いの核のマテリアルを設定
        }
        else
        {
            m_feelingCore.SetActive(false); // 想いの核が設定されていない場合は非アクティブにする
        }
        
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

    /// <summary>
    /// 想いの核を挿入する
    /// </summary>
    /// <param name="feelingCore"></param>
    public void InsertCore(FeelingCore feelingCore)
    {
        if (m_feelingCore.GetEmotionType() != EmotionCurrent.Type.NONE)
        {
            Debug.LogError("既に想いの核が挿入されています。");
            return; // 既に想いの核が挿入されている場合は処理を中断
        }

        m_feelingCore.MeshRenderer.material = feelingCore.MeshRenderer.material;    // 想いの核のマテリアルを設定
        m_feelingCore.SetEmotionType(feelingCore.GetEmotionType());                 // 想いの核の感情タイプを設定
        m_feelingCore.SetActive(true);                                               // 想いの核をアクティブにする
    }

    public FeelingCore GetFeelingCore()
    {
        return m_feelingCore; // 想いの核を取得
    }

}
