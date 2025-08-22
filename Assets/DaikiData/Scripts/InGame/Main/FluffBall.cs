using UnityEngine;

public class FluffBall : MonoBehaviour
{
    Carryable m_carriable; // Carriableコンポーネントを参照するための変数
    StageBlock m_stageBlock;

    private void Awake()
    {
        // Carryableコンポーネントを取得
        m_carriable = GetComponent<Carryable>();
        if (m_carriable == null)
        {
            Debug.LogError("FluffBall must be attached to a GameObject with a Carryable component.");
        }

        // StageBlockコンポーネントを取得
        m_stageBlock = GetComponent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("FluffBall must be attached to a GameObject with a StageBlock component.");
        }

        m_carriable.SetOnDropAction(OnDrop); // 置くときの処理を設定
    }

    private void OnDrop(GridPos placementPos)
    {
        gameObject.SetActive(true); // オブジェクトを表示する
        m_stageBlock.UpdatePosition(placementPos); // StageBlockの位置を更新

        // グリッドデータに綿毛ボールを配置
        MapData.GetInstance.GetStageGridData().TryPlaceTileObject(placementPos, gameObject);

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
