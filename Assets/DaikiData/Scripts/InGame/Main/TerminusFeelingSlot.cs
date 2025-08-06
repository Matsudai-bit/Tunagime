using UnityEngine;

/// <summary>
/// 終点にある型
/// </summary>
public class TerminusFeelingSlot : MonoBehaviour
{
    private StageBlock m_stageBlock;    // ステージブロック

    private FeelingSlot m_feelingSlot; // 想いの型


    private void Awake()
    {
        // ステージブロックを取得
        m_stageBlock = GetComponent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("TerminusFeelingSlot requires a StageBlock component.");
        }

        // 想いの型を取得
        m_feelingSlot = GetComponent<FeelingSlot>();
        if (m_feelingSlot == null)
        {
            Debug.LogError("TerminusFeelingSlot requires a FeelingSlot component.");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 繋がっているかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsConnected()
    {
        //　グリッドデータの取得
        StageGridData gridData = MapData.GetInstance.GetStageGridData();

        //　グリッド位置の取得
        GridPos gridPos = m_stageBlock.GetGridPos();

        GridPos checkGridPos = gridPos + new GridPos(-1, 0); // 左隣のグリッドをチェック

        var amidaTube =  gridData.GetAmidaTube(checkGridPos);

        if (amidaTube.GetEmotionType(YarnMaterialGetter.MaterialType.OUTPUT) == m_feelingSlot.GetEmotionType())
        {
            // 終点の感情と一致している場合は繋がっている
            return true;
        }


        return false;
    }
}
