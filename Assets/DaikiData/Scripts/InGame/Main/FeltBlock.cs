using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.GridLayoutGroup;


/// <summary>
/// フェルトブロック
/// </summary>
public class FeltBlock : MonoBehaviour
{
    private StageBlock m_stageBlock; // ステージブロック

    private MeshRenderer m_meshRenderer = null; // メッシュレンダラー

    private readonly float TARGET_TIME = 0.3f; // 動かすターゲット時間
 
    private GridPos m_prevVelocity;

   private PairBadge m_pairBadge; // ペアワッペン

    [SerializeField]
    private GameObject m_model; // フェルトブロックのモデル

    enum State
    {
        IDLE, // 何もしない状態
        MOVE, // 移動状態 <- プレイヤに依存
        SLIDE, // スライド状態
    }

    private State m_state;

    /// <summary>
    /// Awake is called when the script instance is being loaded
    /// </summary>
    void Awake()
    {
        m_stageBlock = GetComponent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("FeltBlock requires a StageBlock component.");
        }

        m_meshRenderer = m_model.GetComponent<MeshRenderer>();
        if (m_meshRenderer == null)
        {
            Debug.LogError("FeltBlock requires a MeshRenderer component.");
        }
    }

    private void Start()
    {
     

        m_state = State.IDLE; // 初期状態は何もしない状態

    }

    public bool CheckCanMove(GridPos moveDirection)
    {
        // ペアワッペンがある場合はペアワッペンの移動可能かチェック
        if (m_pairBadge != null)
        {
            return m_pairBadge.CanMove(moveDirection);
        }
    
        return CanMove(moveDirection); // ペアワッペンがない場合は自分自身の移動可能かチェック
    }

    public bool CanMove(GridPos moveDirection)
    {
        // ステージブロックが移動可能かチェック
        GridPos currentGridPos = m_stageBlock.GetGridPos();

        GridPos targetGridPos = currentGridPos + moveDirection;

        // StageBlockのグリッド位置を取得
        MapData map = MapData.GetInstance; // マップデータを取得
        TileObject targetTileObject = map.GetStageGridData().GetTileObject(targetGridPos);

        return (targetTileObject.gameObject == null);
    }

    private void Update()
    {

    }

    /// <summary>
    /// ペアワッペンコンポーネントを設定します。
    /// </summary>
    public void SetPairBadge(PairBadge pairBadge)
    {
        m_pairBadge = pairBadge;
        transform.SetParent(pairBadge.transform); // ペアワッペンの子オブジェクトとして設定
    }

    public Transform GetParentTransform()
    {
        // フェルトブロックのTransformを取得
        return (m_pairBadge != null) ? m_pairBadge.transform : transform;
    }


    /// <summary>
    /// ステージブロックを取得します。
    /// </summary>
    public StageBlock stageBlock
    {
        get { return m_stageBlock; }

    }

    /// <summary>
    /// フェルトブロックを移動する
    /// </summary>
    /// <param name="velocity"></param>
    public void RequestMove(GridPos velocity)
    {
        if (m_pairBadge != null)
        {
            // ペアワッペンがある場合はペアワッペンに移動を依頼
            m_pairBadge.Move(velocity);
            return;
        }

        Move(velocity); // ペアワッペンがない場合は自分自身を移動

    }

    ///// <summary>
    ///// フェルトブロックを移動する
    ///// </summary>
    ///// <param name="velocity"></param>
    public void Move(GridPos velocity)
    {

        GridPos newGridPos = m_stageBlock.GetGridPos() + velocity;

        // ステージブロックの位置を更新
        m_stageBlock.UpdatePosition(newGridPos);

        m_prevVelocity = velocity;

        if (CheckCanSlide()) ChangeState(State.SLIDE);
        else ChangeState(State.IDLE);

    }

    public MeshRenderer meshRenderer
    {
        get { return m_meshRenderer; }
        set { m_meshRenderer = value; }
    }

    private void RequestSlide()
    {
        if (m_pairBadge != null)
        {
            // ペアワッペンがある場合はペアワッペンにスライドを依頼
            m_pairBadge.Slide(m_prevVelocity);
            return;
        }

        // ペアワッペンがない場合は自分自身をスライド
        StartSlide(m_prevVelocity);
    }

    public void StartSlide(GridPos velocity)
    {
        

        var map = MapData.GetInstance; // マップデータを取得

        // ブロックを押す前の開始位置を設定
        var blockPos = m_stageBlock.GetGridPos(); // ブロックのグリッド位置を取得

        // 念のため再配置
        transform.position = map.ConvertGridToWorldPos(blockPos); 

        var endGridPos = blockPos + velocity; // ブロックを押した後のグリッド位置を計算

        // ブロックを押した後の目標位置を設定
        var endPosition = map.ConvertGridToWorldPos(endGridPos);

     

        transform.DOMove(endPosition, TARGET_TIME)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // 念のため再配置
                // 先に座標を設定してから移動
                stageBlock.UpdatePosition(endGridPos);
                GameInteractionEventMessenger.GetInstance.Notify(InteractionEvent.PUSH_FELTBLOCK);

                if (CheckCanSlide()) ChangeState(State.SLIDE);
                else ChangeState(State.IDLE);
            });
    }

    public bool CheckCanSlide()
    {
        if (m_pairBadge != null)
        {
            // ペアワッペンがある場合はペアワッペンのスライド可能かチェック
            return m_pairBadge.CanSlide();
        }

        return CanSlide(); // ペアワッペンがない場合は自分自身のスライド可能かチェック
    }

    public bool CanSlide()
    {
        if (CheckCanMove(m_prevVelocity) == false) return false;

        // **** 床の種類でチェック ****
        var gridPos = m_stageBlock.GetGridPos();

        var stageGrid = MapData.GetInstance.GetStageGridData();

        var currentTileFloor = stageGrid.GetFloorObject(gridPos);

        if (currentTileFloor == null) return false;

        var satainFloorOfCurrentTile = currentTileFloor?.GetComponent<SatinFloor>();
        if (satainFloorOfCurrentTile)  return true;
        

        return false;
    }
    private void ChangeState(State newState)
    {
        m_state = newState;
        switch (m_state)
        {
            case State.IDLE:
                // 何もしない状態
                break;
            case State.MOVE:
            
                break;
            case State.SLIDE:
                // スライド状態の処理
                RequestSlide();
                break;
        }
    }

}
