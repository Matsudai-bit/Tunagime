using UnityEngine;

/// <summary>
/// �z���̒f��
/// </summary>
public class Fragment : MonoBehaviour
{
    private StageBlock m_stageBlock;
    [SerializeField] private MeshRenderer m_meshRenderer;

    /// <summary>
    /// �ړ������̗񋓌^
    /// </summary>
    enum MovementDirectionID
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    [SerializeField]
    MovementDirectionID m_currentDirection = MovementDirectionID.RIGHT; // ���݂̈ړ�����

    MovementDirectionID m_currentSideDirection = MovementDirectionID.RIGHT; // ���݂̉������̈ړ�

    readonly float SPEED = 0.1f; // �ړ��̑���
    readonly Vector3 m_movementDirection = new(); // �ړ�������

    private void Awake()
    {
        // StageBlock�R���|�[�l���g���擾
        m_stageBlock = GetComponent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("Fragment requires a StageBlock component.");
        }
    }

    private void FixedUpdate()
    {
        var map = MapData.GetInstance; // �}�b�v�f�[�^�̃C���X�^���X���擾
        var stageGridData = map.GetStageGridData();

        // ���݂��ɉ����Ĉړ���������

        // ���݂̍��W
        GridPos currentGridPos = m_stageBlock.GetGridPos();

        var tile = stageGridData.GetTileObject(currentGridPos);

        // �ړ������Ƀ��C���΂��āA�����Ȃ��ꍇ�͈ړ�
        Vector3 movedDirection = GetMovementDirection(m_currentDirection);

        Ray ray = new Ray(m_stageBlock.transform.position, movedDirection);
        // �Փ˂����ꍇ���Ε����֌�����؂�ւ���
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 0.1f))
        {
            // ���Ε����֌�����؂�ւ���
            m_currentDirection = m_currentDirection switch
            {
                MovementDirectionID.UP => MovementDirectionID.DOWN,
                MovementDirectionID.DOWN => MovementDirectionID.UP,
                MovementDirectionID.LEFT => MovementDirectionID.RIGHT,
                MovementDirectionID.RIGHT => MovementDirectionID.LEFT,
                _ => m_currentDirection
            };
        }
        // �ړ�����
        m_stageBlock.transform.position += GetMovementDirection(m_currentDirection) * SPEED;

        // �������̈ړ�
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
