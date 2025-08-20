using UnityEngine;

/// <summary>
/// 想いの断片
/// </summary>
public class Fragment : MonoBehaviour
{
    private StageBlock m_stageBlock;
    [SerializeField] private MeshRenderer m_meshRenderer;

    /// <summary>
    /// 移動方向の列挙型
    /// </summary>
    enum MovementDirectionID
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    [SerializeField]
    MovementDirectionID m_currentDirection = MovementDirectionID.RIGHT; // 現在の移動方向

    MovementDirectionID m_currentSideDirection = MovementDirectionID.RIGHT; // 現在の横方向の移動

    readonly float SPEED = 0.1f; // 移動の速さ
    readonly Vector3 m_movementDirection = new(); // 移動方方向

    private void Awake()
    {
        // StageBlockコンポーネントを取得
        m_stageBlock = GetComponent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("Fragment requires a StageBlock component.");
        }
    }

    private void FixedUpdate()
    {
        var map = MapData.GetInstance; // マップデータのインスタンスを取得
        var stageGridData = map.GetStageGridData();

        // あみだに沿って移動し続ける

        // 現在の座標
        GridPos currentGridPos = m_stageBlock.GetGridPos();

        var tile = stageGridData.GetTileObject(currentGridPos);

        // 移動方向にレイを飛ばして、何もない場合は移動
        Vector3 movedDirection = GetMovementDirection(m_currentDirection);

        Ray ray = new Ray(m_stageBlock.transform.position, movedDirection);
        // 衝突した場合反対方向へ向きを切り替える
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 0.1f))
        {
            // 反対方向へ向きを切り替える
            m_currentDirection = m_currentDirection switch
            {
                MovementDirectionID.UP => MovementDirectionID.DOWN,
                MovementDirectionID.DOWN => MovementDirectionID.UP,
                MovementDirectionID.LEFT => MovementDirectionID.RIGHT,
                MovementDirectionID.RIGHT => MovementDirectionID.LEFT,
                _ => m_currentDirection
            };
        }
        // 移動する
        m_stageBlock.transform.position += GetMovementDirection(m_currentDirection) * SPEED;

        // 横方向の移動
        m_stageBlock.UpdatePosition (map.GetClosestGridPos(transform.position), false);




    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Material Material
    {
        get
        {
            return m_meshRenderer != null ? m_meshRenderer.material : null;
        }
    }

    private Vector3 GetMovementDirection(MovementDirectionID movementDirection)
    {
        return movementDirection switch
        {
            MovementDirectionID.UP => Vector3.forward,
            MovementDirectionID.DOWN => Vector3.back,
            MovementDirectionID.LEFT => Vector3.left,
            MovementDirectionID.RIGHT => Vector3.right,
            _ => Vector3.zero
        };
    }
}
