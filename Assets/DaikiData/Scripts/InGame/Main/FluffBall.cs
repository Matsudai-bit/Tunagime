using UnityEngine;

public class FluffBall : MonoBehaviour
{
    StageBlock m_stageBlock;

    private void Awake()
    {

        // StageBlockコンポーネントを取得
        m_stageBlock = GetComponent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("FluffBall must be attached to a GameObject with a StageBlock component.");
        }

    }

    public void OnDrop(GridPos placementPos)
    {
        var stageGridData = MapData.GetInstance.GetStageGridData();

        gameObject.SetActive(true); // オブジェクトを表示する
        m_stageBlock.Initialize(placementPos); // StageBlockの位置を更新

        var objectInGrid = stageGridData.GetTileObject(placementPos);
        if (objectInGrid.stageBlock != null && objectInGrid.stageBlock.GetBlockType() == StageBlock.BlockType.FRAGMENT)
        {
            var fragment = objectInGrid.gameObject.GetComponent<Fragment>();
            fragment.RequestAdjustingPositionAccordingToAmidaTube();
        }

        // グリッドデータに綿毛ボールを配置
        if (stageGridData.TryPlaceTileObject(placementPos, gameObject) == false)
        {
            Debug.LogWarning("既にタイルにオブジェクトが存在します。綿毛ボールの配置に失敗しました。オブジェクト名 :" + stageGridData.GetTileObject(placementPos).gameObject.name);
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
}
