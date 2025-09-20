using UnityEngine;

/// <summary>
///  想いの核
/// </summary>
public class FeelingCore : MonoBehaviour 
{


    private EmotionCurrent m_emotionCurrent; // 想いの感情タイプ

    [SerializeField] MeshRenderer m_meshRenderer;

    Carryable m_carriable; // Carriableコンポーネントを参照するための変数


    public MeshRenderer MeshRenderer
    {
        get { return m_meshRenderer; }
    }

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

        if (m_meshRenderer == null)
        {
            m_meshRenderer = GetComponent<MeshRenderer>();
        }

        m_carriable = GetComponent<Carryable>(); // Carryableコンポーネントを取得
                                                 // 登録
        if (m_carriable)
            m_carriable.SetOnDropAction(OnDrop); // 置くときの処理を設定  
    }

    private void Start()
    {
      
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

    public void SetEmotionType(EmotionCurrent.Type type)
    {
        m_emotionCurrent.CurrentType = type; // 感情タイプを設定
    }


    public void OnConnectEvent(EmotionCurrent emotionCurrent)
    {
    }

    /// <summary>
    /// 想いの核を配置する処理
    /// </summary>
    /// <param name="placementPos"></param>
    public void OnDrop(GridPos placementPos)
    {
        var map = MapData.GetInstance; // MapDataのインスタンスを取得
        var stageGridData = map.GetStageGridData(); // ステージグリッドデータを取得

        // 配置予定箇所のタイルを取得
        var tile = stageGridData.GetTileObject(placementPos);

        if (tile.stageBlock != null && tile.stageBlock.GetBlockType() == StageBlock.BlockType.FEELING_SLOT)
        {
            var feelingSlot = tile.gameObject.GetComponent<FeelingSlot>();
            if (feelingSlot != null)
            {
                // 想いの核を配置するスロットに感情タイプを設定
                feelingSlot.InsertCore(this);
            }
            else
            {
                Debug.LogError("FeelingSlot コンポーネントが見つかりません。");
            }
            return;
        }

        gameObject.SetActive(true); // オブジェクトを表示する
        var stageBlock = GetComponent<StageBlock>();
        if (stageBlock == null)
        {
            Debug.LogError("StageBlock コンポーネントが見つかりません。");
            return;
        }
        stageBlock.UpdatePosition(placementPos); // StageBlockの位置を更新
        // グリッドデータに綿毛ボールを配置
        MapData.GetInstance.GetStageGridData().TryPlaceTileObject(placementPos, gameObject);
    }



}
