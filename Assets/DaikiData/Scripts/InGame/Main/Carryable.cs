using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

/// <summary>
/// ゲーム内で持ち運び可能なオブジェクト。
/// </summary>
public class Carryable : MonoBehaviour
{
    
    private StageBlock m_stageBlock; // このCarryableが属するStageBlock

    private void Awake()
    {
        // StageBlockコンポーネントを取得
        m_stageBlock = GetComponent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("Carryable must be attached to a GameObject with a StageBlock component.");
        }
    }

    public StageBlock stageBlock
    {
        get { return m_stageBlock; }
    }

    /// <summary>
    /// このCarryableオブジェクトを拾う処理。
    /// </summary>
    public void OnPickUp()
    {
        gameObject.SetActive(false); // オブジェクトを非表示にする
    }

    /// <summary>
    /// このCarryableオブジェクトを置く処理。
    /// </summary>
    public void OnDrop( GridPos  placementPos)
    {
        gameObject.SetActive(true); // オブジェクトを表示する
        stageBlock.UpdatePosition(placementPos); // StageBlockの位置を更新

        // グリッドデータに綿毛ボールを配置
        MapData.GetInstance.GetStageGridData().TryPlaceTileObject(placementPos, gameObject);
    }
}
