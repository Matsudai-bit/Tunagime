using UnityEngine;

/// <summary>
/// 終点にある型
/// </summary>
public class TerminusFeelingSlot : MonoBehaviour, IGameInteractionObserver
{
    private StageBlock m_stageBlock;    // ステージブロック

    private FeelingSlot m_feelingSlot; // 想いの型

    [SerializeField]
    bool m_isConnection;


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

        // オブザーバーとして登録
        GameInteractionEventMessenger.GetInstance.RegisterObserver(this);

        // モニターに登録
        FeelingSlotStateMonitor.GetInstance.RegisterMonitorObject(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 初期状態で接続をチェック
        m_isConnection = false;

        CheckConnection();
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
        return m_isConnection;
    }
    /// <summary>
    /// 接続状態をチェックするメソッド
    /// </summary>

    private void CheckConnection()
    {
        //　グリッドデータの取得
        StageGridData gridData = MapData.GetInstance.GetStageGridData();

        //　グリッド位置の取得
        GridPos gridPos = m_stageBlock.GetGridPos();

        // 左隣のグリッドをチェック
        GridPos checkGridPos = gridPos + new GridPos(-1, 0);

        // あみだチューブを取得
        var amidaTube = gridData.GetAmidaTube(checkGridPos);

        if (amidaTube == null)
        {
           // Debug.LogWarning($"AmidaTube not found at position ({checkGridPos.x}, {checkGridPos.y}).");
            m_isConnection = false;
            return;
        }

        // 接続状態の確認
        if (amidaTube.GetEmotionType(YarnMaterialGetter.MaterialType.OUTPUT) == m_feelingSlot.GetEmotionType() && 
            m_feelingSlot.IsInsertCore())
  
            // 終点の感情と一致している場合は繋がっている
            m_isConnection = true;
        else
            m_isConnection = false;
    }


    public void OnEvent(InteractionEvent eventID)
    {
        switch(eventID)
        { 
            case InteractionEvent.FLOWWING_AMIDAKUJI:
                CheckConnection();
                break;
        }
    }

    public EmotionCurrent.Type emotionType
    {
        get
        {
            return m_feelingSlot.GetEmotionType(); // 想いの型の感情タイプを取得
        }
    }

    // 削除時
    private void OnDestroy()
    {
        // ゲームインタラクションイベントのオブザーバーを解除
        GameInteractionEventMessenger.GetInstance.RemoveObserver(this);
        // モニターから登録解除
        FeelingSlotStateMonitor.GetInstance.RemoveMonitor(this);
    }


}
