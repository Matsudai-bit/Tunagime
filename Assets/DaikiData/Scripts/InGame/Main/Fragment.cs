using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

/// <summary>
/// �v���C���[�i�z���̒f�Ёj�̋����𐧌䂷��N���X�B
/// �}�b�v��������ňړ����A���̒f�Ђƃ}�[�W����@�\�����B
/// </summary>
[RequireComponent(typeof(StageBlock))]
public class Fragment : MonoBehaviour
{
    // === �t�B�[���h�ƃv���p�e�B ===

    // ���̃R���|�[�l���g�ւ̎Q��
    private StageBlock m_stageBlock;
    [SerializeField] private MeshRenderer m_meshRenderer;


    // ���݂̏�Ԃ��Ǘ�����ϐ�
    [Header("=== ��ԊǗ� ===")]
    [SerializeField]
    private State m_currentState = State.MOVING;
    [SerializeField]
    private MovementDirectionID m_currentMovementDirection = MovementDirectionID.RIGHT;

    // �������̈ړ����L�����邽�߂̕ϐ�
    private MovementDirectionID m_currentSideDirection = MovementDirectionID.RIGHT;
    // 1�t���[���O�̃O���b�h���W��ێ�
    private GridPos m_prevGridPos = new GridPos();

    // �v���C���[�̃��x��
    [Header("=== ���x�� ===")]
    [SerializeField]
    private Level m_level = Level.LEVEL_1;

    // === �ݒ�p�����[�^ ===

    [Header("=== �ړ��ݒ� ===")]
    [Tooltip("�f�Ђ��ړ����鑬��")]
    [SerializeField] private float m_speed = 0.1f;
    [Tooltip("���݂��̌��іڔ���Ɏg�p���郌�C�L���X�g�̋���")]
    [SerializeField] private float m_raycastDistance = 0.1f;

    [Header("=== �z���̒f�Ђ̑傫�� ===")]
    [SerializeField] private float LEVEL_1_SIZE = 0.75f; // ���x��1�̒f�Ђ̃T�C�Y
    [SerializeField] private float LEVEL_2_SIZE = 1.0f; // ���x��2�̒f�Ђ̃T�C�Y
    [SerializeField] private float LEVEL_3_SIZE = 1.75f; // ���x��3�̒f�Ђ̃T�C�Y

    [Header("=== �傫���̕ς�鑬�x ===")]
    [SerializeField] private float SIZE_CHANGE_DURATION = 2.0f; // �T�C�Y�ύX�ɂ����鎞��

    // === ���J�v���p�e�B ===

    public MovementDirectionID MovementDirection => m_currentMovementDirection;
    public MovementDirectionID CurrentSideDirection => m_currentSideDirection;
    public Level level => m_level;
    public MeshRenderer MeshRenderer => m_meshRenderer != null ? m_meshRenderer : null;

    // === �񋓌^ ===

    /// <summary>
    /// �ړ������̗񋓌^
    /// </summary>
    public enum MovementDirectionID
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    /// <summary>
    /// �f�Ђ̃��x��
    /// </summary>
    public enum Level
    {
        LEVEL_1 = 1,
        LEVEL_2,
        LEVEL_3,
        LEVEL_4,
    }

    /// <summary>
    /// �f�Ђ̌��݂̏��
    /// </summary>
    public enum State
    {
        MOVING, // �ړ���
        MERGING, // �}�[�W��
    }

    // === MonoBehaviour���C�t�T�C�N�����\�b�h ===

    private void Awake()
    {
        InitializeComponents();
    }

    private void Start()
    {
        // ������Ԃ̐ݒ�
        SetUpSizeForLevel(); // ���x���ɉ����ăT�C�Y��ݒ�
    }

    private void FixedUpdate()
    {
        // ���݂̏�Ԃ�MOVING�̏ꍇ�̂݁A�ړ����������s
        if (m_currentState == State.MOVING)
        {
            MoveOnGrid();
        }
    }

    private void Update()
    {
        if (m_level == Level.LEVEL_4)
        {
            var core =  StageObjectFactory.GetInstance().GenerateCarriableCore(null,m_stageBlock.GetGridPos(), GetComponent<EmotionCurrent>().CurrentType);
            MapData.GetInstance.GetStageGridData().RemoveGridDataGameObject(m_stageBlock.GetGridPos()); // �O���b�h�f�[�^����폜
            MapData.GetInstance.GetStageGridData().TryPlaceTileObject(m_stageBlock.GetGridPos(), core); // �O���b�h�f�[�^�ɓo�^
            gameObject.SetActive(false); // ���x��4�̒f�Ђ͔�A�N�e�B�u�ɂ���
            return;
        }

        if (m_currentState == State.MOVING)
        {
            if (TryFindFragmentToMovementDirection(out Fragment findingFragment))
            {
                // �f�Ђ����������ꍇ�A�}�[�W���N�G�X�g�𑗂�
                if (findingFragment != null)
                {
                    RequestMerge(findingFragment);
                    findingFragment.RequestMerge(this);

                }
            }
        }
    }

    // === ���C���̏������\�b�h ===

    /// <summary>
    /// �X�e�[�W�̃O���b�h�ɉ����Ēf�Ђ��ړ��������v�ȏ���
    /// </summary>
    private void MoveOnGrid()
    {
        var map = MapData.GetInstance;
        var stageGridData = map.GetStageGridData();
        GridPos currentGridPos = m_stageBlock.GetGridPos();

        // �O���b�h���W���ω������ꍇ�A���݂��̌��іڔ�����s��
        if (m_prevGridPos != currentGridPos)
        {
            HandleAmidaTubeLogic(currentGridPos, stageGridData, map);
        }

        // �ړ���ɏ�Q�����Ȃ����m�F���A����Ε����]������
        CheckAndReverseDirection();

        // ���ۂ̈ړ�����
        ApplyMovement(map);

        // ���݂̃O���b�h���W���X�V
        m_stageBlock.UpdatePosition(map.GetClosestGridPos(transform.position), false);
    }

    /// <summary>
    /// �R���|�[�l���g�̏��������s��
    /// </summary>
    private void InitializeComponents()
    {
        m_stageBlock = GetComponent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("Fragment requires a StageBlock component.");
        }
    }

    /// <summary>
    /// ���݂��̌��іڂɓ��B�����ꍇ�A�ړ�������؂�ւ���
    /// </summary>
    /// <param name="currentGridPos">���݂̃O���b�h���W</param>
    /// <param name="stageGridData">�X�e�[�W�̃O���b�h�f�[�^</param>
    /// <param name="map">�}�b�v�f�[�^</param>
    private void HandleAmidaTubeLogic(GridPos currentGridPos, StageGridData stageGridData, MapData map)
    {
        // �^�C���̒��S���W���擾���A�v���C���[�����S�ɓ��B�������𔻒肷��
        if (IsNearTileCenter(currentGridPos, map))
        {
            var amidaTube = stageGridData.GetAmidaTube(currentGridPos);

            if (amidaTube != null)
            {
                // ���݂��̌��іڂ̏�Ԃɉ����ĕ�����؂�ւ���
                switch (amidaTube.GetState())
                {
                    case AmidaTube.State.KNOT_DOWN:
                        // ��������痈���ꍇ�A�������ցB����ȊO�͉�������
                        m_currentMovementDirection = (m_currentMovementDirection == MovementDirectionID.UP) ? m_currentSideDirection : MovementDirectionID.DOWN;
                        break;
                    case AmidaTube.State.KNOT_UP:
                        // ���������痈���ꍇ�A�������ցB����ȊO�͏������
                        m_currentMovementDirection = (m_currentMovementDirection == MovementDirectionID.DOWN) ? m_currentSideDirection : MovementDirectionID.UP;
                        break;
                }
            }
            m_prevGridPos = m_stageBlock.GetGridPos();

        }
    }

    /// <summary>
    /// �v���C���[���^�C���̒��S�ɋ߂Â������𔻒肷��
    /// </summary>
    /// <param name="currentGridPos">���݂̃O���b�h���W</param>
    /// <param name="map">�}�b�v�f�[�^</param>
    /// <returns>���S�ɓ��B�������ǂ����̐^�U�l</returns>
    private bool IsNearTileCenter(GridPos currentGridPos, MapData map)
    {
        Vector3 centerTileWorldPos = map.ConvertGridToWorldPos(currentGridPos.x, currentGridPos.y);

        var movedDirection = GetMovementDirectionVector(m_currentMovementDirection);   

        if (Mathf.Approximately(0.0f, movedDirection.x)) centerTileWorldPos.x = 0.0f;

        if (Mathf.Approximately(0.0f, movedDirection.z)) centerTileWorldPos.z = 0.0f;


        if ((m_currentMovementDirection == MovementDirectionID.RIGHT || m_currentMovementDirection == MovementDirectionID.DOWN))

        {
            var direction = (centerTileWorldPos - transform.position);

            if (m_currentMovementDirection == MovementDirectionID.RIGHT && direction.x >= 0.0f) return true;
            if (m_currentMovementDirection == MovementDirectionID.DOWN && direction.z >= 0.0f) return true;
        }

        else
        {
            var direction = (centerTileWorldPos - transform.position);
            if (m_currentMovementDirection == MovementDirectionID.LEFT && direction.x <= 0.0f) return true;
            if (m_currentMovementDirection == MovementDirectionID.UP && direction.z <= 0.0f) return true;
        }

            return false;
    }

    /// <summary>
    /// �O���ɏ�Q�����Ȃ����m�F���A����Ε����]������
    /// </summary>
    private void CheckAndReverseDirection()
    {
        // ���݂̈ړ������ɉ������x�N�g�����擾
        Vector3 movedDirection = GetMovementDirectionVector(m_currentMovementDirection);
        Ray ray = new Ray(m_stageBlock.transform.position, movedDirection);

        if (Physics.Raycast(ray, out RaycastHit hit, m_raycastDistance))
        {
            if (!hit.collider.gameObject.CompareTag("Player"))
            {
                // �v���C���[�ȊO�ɏՓ˂����ꍇ�A�����𔽓]
                m_currentMovementDirection = ReverseDirection(m_currentMovementDirection);
            }
        }
    }

    /// <summary>
    /// ���ۂ̈ړ���K�p����
    /// </summary>
    /// <param name="map">�}�b�v�f�[�^</param>
    private void ApplyMovement(MapData map)
    {
        // ���x��K�p���Ĉʒu���X�V
        transform.position += GetMovementDirectionVector(m_currentMovementDirection) * m_speed;

        // �������̈ړ����L��
        if (m_currentMovementDirection == MovementDirectionID.LEFT || m_currentMovementDirection == MovementDirectionID.RIGHT)
        {
            m_currentSideDirection = m_currentMovementDirection;
        }

        // �ړ������ɉ�����Z�܂���X���W���O���b�h�ɍ��킹��
        var closestGridPos = map.GetClosestGridPos(transform.position);
        var snappedPosition = map.ConvertGridToWorldPos(closestGridPos);

        if (m_currentMovementDirection == MovementDirectionID.LEFT || m_currentMovementDirection == MovementDirectionID.RIGHT)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, snappedPosition.z);
        }
        else
        {
            transform.position = new Vector3(snappedPosition.x, transform.position.y, transform.position.z);
        }
    }

    // === ���[�e�B���e�B���\�b�h ===

    /// <summary>
    /// �ړ�����ID��Ή�����Vector3�ɕϊ�����
    /// </summary>
    /// <param name="movementDirection">�ړ�����ID</param>
    /// <returns>�Ή�����Vector3�x�N�g��</returns>
    private static Vector3 GetMovementDirectionVector(MovementDirectionID movementDirection)
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

    /// <summary>
    /// �ړ������𔽓]������
    /// </summary>
    /// <param name="direction">���݂̕���ID</param>
    /// <returns>���]��̕���ID</returns>
    private static MovementDirectionID ReverseDirection(MovementDirectionID direction)
    {
        return direction switch
        {
            MovementDirectionID.UP => MovementDirectionID.DOWN,
            MovementDirectionID.DOWN => MovementDirectionID.UP,
            MovementDirectionID.LEFT => MovementDirectionID.RIGHT,
            MovementDirectionID.RIGHT => MovementDirectionID.LEFT,
            _ => direction
        };
    }



    /// <summary>
    /// �ړ������ɂɑz���̒f�Ђ����邩���m�F����
    /// </summary>
    /// <returns></returns>
    private bool TryFindFragmentToMovementDirection(out Fragment findingFragment)
    {
        RaycastHit hit;
        Vector3 movedDirection = GetMovementDirectionVector(m_currentMovementDirection);
        Ray ray = new Ray(m_stageBlock.transform.position, movedDirection);
        if (Physics.Raycast(ray, out hit, m_raycastDistance + 1.0f))
        {
    
            // �f�Ђ����������ꍇ�A�}�[�W���N�G�X�g�𑗂�
            Fragment fragment = hit.collider.GetComponent<Fragment>();
            if (fragment != null && hit.collider.GetComponent<EmotionCurrent>().CurrentType == GetComponent<EmotionCurrent>().CurrentType)
            {
                findingFragment = fragment;
                return true;
            }
            
        }
        findingFragment = null;
        return false;
    }

    /// <summary>
    /// �z���̒f�Ђ��}�[�W���郊�N�G�X�g�𑗂�
    /// </summary>
    /// <param name="fragment">�}�[�W���鑊��̒f��</param>
    public void RequestMerge(Fragment fragment)
    {
        if ((int)(fragment.level) < (int)(m_level))
        {
            MergeFragment(fragment);
            return;
        }

        if (m_currentSideDirection == MovementDirectionID.RIGHT)
        {

            if (fragment.CurrentSideDirection != m_currentSideDirection || m_currentMovementDirection == MovementDirectionID.DOWN)
            {
                MergeFragment(fragment);
                return;
            }
        }

        gameObject.SetActive(false); // �������g���A�N�e�B�u�ɂ���
        MapData.GetInstance.GetStageGridData().RemoveGridDataGameObject(m_stageBlock.GetGridPos()); // �O���b�h�f�[�^����폜


    }

    private void MergeFragment(Fragment fragment)
    {
        // �f�Ђ̃��x�����X�V
        IncrementLevel(fragment.level);
        // ���x���ɉ����ăT�C�Y��ݒ�
        SetUpSizeForLevel(); 

        
    }

    void IncrementLevel(Level level)
    {
        m_level = (Level)(Math.Min((int)(m_level) + (int)(level), (int)(Level.LEVEL_4))); // ���x�����X�V

    }

    /// <summary>
    /// ���x���ɉ����Ēf�Ђ̃T�C�Y��ݒ肷��
    /// </summary>
    void SetUpSizeForLevel()
    {
        float targetSize = LEVEL_1_SIZE; // �f�t�H���g�̓��x��1�̃T�C�Y

        switch (m_level)
        {
            case Level.LEVEL_2:
                targetSize = LEVEL_2_SIZE;
                break;
            case Level.LEVEL_3:
                targetSize = LEVEL_3_SIZE;
                break;
            case Level.LEVEL_4:
                // ���x��4�͍ő�T�C�Y�Ȃ̂œ��ɐݒ肵�Ȃ�
                break;
        }

        

        transform.DOScale(targetSize, 1.0f).SetEase(Ease.OutBounce, SIZE_CHANGE_DURATION);
        m_raycastDistance = 0.5f * targetSize; // ���x���ɉ����ă��C�L���X�g�̋����𒲐�
    }

}