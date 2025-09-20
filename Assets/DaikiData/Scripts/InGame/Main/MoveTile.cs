using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 
/// </summary>
public class MoveTile : MonoBehaviour , IMoveTile
{
    enum State
    {
        IDLE, // 何もしない状態
        MOVE, // 移動状態 <- プレイヤに依存
        SLIDE, // スライド状態
    }

    [Header("監視用")]
    [SerializeField]
    private StageBlock m_stageBlock; // ステージブロック

    private readonly float TARGET_TIME = 0.3f; // 動かすターゲット時間

    private GridPos m_prevVelocity;

    private State m_state;



    /// <summary>
    /// Awake is called when the script instance is being loaded
    /// </summary>
    public virtual void Awake()
    {
        OnAwake();
    }

    public virtual void Start()
    {
        OnStart();
    }

    protected void OnAwake()
    {
        m_stageBlock = GetComponent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("FeltBlock requires a StageBlock component.");
        }
    }
        

    protected void OnStart()
    {
        m_prevVelocity = new GridPos(0, 0);
        m_state = State.IDLE; // 初期状態は何もしない状態
    }


    /// <summary>
    /// 移動要求
    /// </summary>
    /// <param name="velocity"></param>
    public virtual void RequestMove(GridPos velocity)
    {
        Move(velocity);
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

        if (CanSlide()) ChangeState(State.SLIDE);
        else ChangeState(State.IDLE);

    }

    /// <summary>
    /// スライド要求
    /// </summary>
    public virtual void RequestSlide(GridPos velocity)
    {
        StartSlide(velocity);
    }

    /// <summary>
    /// スライド開始
    /// </summary>
    /// <param name="velocity"></param>
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
                m_stageBlock.UpdatePosition(endGridPos);
                GameInteractionEventMessenger.GetInstance.Notify(InteractionEvent.PUSH_FELTBLOCK);

                if (CanSlide()) ChangeState(State.SLIDE);
                else ChangeState(State.IDLE);
            });
    }

    /// <summary>
    /// 指定方向に移動可能かチェック
    /// </summary>
    /// <param name="moveDirection"></param>
    /// <returns></returns>
    public virtual bool CanMove(GridPos moveDirection)
    {
        return IsObstacleInPath(moveDirection);
    }

    /// <summary>
    /// 指定方向に障害物があるかチェック
    /// </summary>
    /// <param name="moveDirection"></param>
    /// <returns></returns>
    public bool IsObstacleInPath(GridPos moveDirection)
    {
        // ステージブロックが移動可能かチェック
        GridPos currentGridPos = m_stageBlock.GetGridPos();

        GridPos targetGridPos = currentGridPos + moveDirection;

        // StageBlockのグリッド位置を取得
        MapData map = MapData.GetInstance; // マップデータを取得
        TileObject targetTileObject = map.GetStageGridData().GetTileObject(targetGridPos);

        return (targetTileObject.gameObject == null);
    }

    /// <summary>
    /// 滑りやすい床の上にいるかチェック
    /// </summary>
    /// <returns></returns>
    public bool IsSlippery()
    {
        if (IsObstacleInPath(m_prevVelocity) == false) return false;

        // **** 床の種類でチェック ****
        var gridPos = m_stageBlock.GetGridPos();

        var stageGrid = MapData.GetInstance.GetStageGridData();

        var currentTileFloor = stageGrid.GetFloorObject(gridPos);

        if (currentTileFloor == null) return false;

        var satainFloorOfCurrentTile = currentTileFloor?.GetComponent<SatinFloor>();
        if (satainFloorOfCurrentTile) return true;


        return false;
    }


    /// <summary>
    /// スライド可能かチェック
    /// </summary>
    /// <returns></returns>
    public virtual bool CanSlide()
    {
        return IsSlippery();
    }

    /// <summary>
    /// 状態変更
    /// </summary>
    /// <param name="newState"></param>
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
                RequestSlide(m_prevVelocity);
                break;
        }
    }

    /// <summary>
    /// 移動するTransformを取得
    /// </summary>
    /// <returns></returns>
    public virtual Transform GetMoveTransform()
    {
        return this.transform;
    }

}
