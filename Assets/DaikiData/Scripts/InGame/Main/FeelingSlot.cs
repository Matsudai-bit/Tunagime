using UnityEngine;

/// <summary>
/// 想いの型
/// </summary>
public class FeelingSlot : MonoBehaviour
{
    [SerializeField] private FeelingCore m_feelingCore; // 想いの核

    [Header("マテリアル種類の設定")]
    [SerializeField] private YarnMaterialLibrary.MaterialType m_materialType; // マテリアルタイプ

    /// <summary>
    /// スタートメソッド
    /// </summary>

    private void Start()
    {
        if (m_materialType != YarnMaterialLibrary.MaterialType.NONE)
        {
            m_feelingCore.SetMaterial(m_materialType); // 初期マテリアルを設定
        }
    }


    /// <summary>
    /// 想いの核のマテリアルを取得する
    /// </summary>
    /// <returns></returns>
    public Material GetCoreMaterial()
    {
        return m_feelingCore.GetMaterial(); // 想いの核のマテリアルを取得
    }

    /// <summary>
    /// 想いの核のマテリアルを設定する
    /// </summary>
    /// <param name="material"></param>
    public void SetCoreMaterial(YarnMaterialLibrary.MaterialType materialType)
    {
        m_feelingCore.SetMaterial(materialType); // 想いの核のマテリアルを設定
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
