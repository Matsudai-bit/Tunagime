using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.GridLayoutGroup;


/// <summary>
/// �t�F���g�u���b�N
/// </summary>
public class FeltBlock : MonoBehaviour
{
    private StageBlock m_stageBlock; // �X�e�[�W�u���b�N

    private MeshRenderer m_meshRenderer = null; // ���b�V�������_���[

    private readonly float TARGET_TIME = 0.3f; // �������^�[�Q�b�g����
 
    private GridPos m_prevVelocity;

   private PairBadge m_pairBadge; // �y�A���b�y��

    [SerializeField]
    private GameObject m_model; // �t�F���g�u���b�N�̃��f��

    enum State
    {
        IDLE, // �������Ȃ����
        MOVE, // �ړ���� <- �v���C���Ɉˑ�
        SLIDE, // �X���C�h���
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
     

        m_state = State.IDLE; // ������Ԃ͉������Ȃ����

    }

    public bool CheckCanMove(GridPos moveDirection)
    {
        // �y�A���b�y��������ꍇ�̓y�A���b�y���̈ړ��\���`�F�b�N
        if (m_pairBadge != null)
        {
            return m_pairBadge.CanMove(moveDirection);
        }
    
        return CanMove(moveDirection); // �y�A���b�y�����Ȃ��ꍇ�͎������g�̈ړ��\���`�F�b�N
    }

    public bool CanMove(GridPos moveDirection)
    {
        // �X�e�[�W�u���b�N���ړ��\���`�F�b�N
        GridPos currentGridPos = m_stageBlock.GetGridPos();

        GridPos targetGridPos = currentGridPos + moveDirection;

        // StageBlock�̃O���b�h�ʒu���擾
        MapData map = MapData.GetInstance; // �}�b�v�f�[�^���擾
        TileObject targetTileObject = map.GetStageGridData().GetTileObject(targetGridPos);

        return (targetTileObject.gameObject == null);
    }

    private void Update()
    {

    }

    /// <summary>
    /// �y�A���b�y���R���|�[�l���g��ݒ肵�܂��B
    /// </summary>
    public void SetPairBadge(PairBadge pairBadge)
    {
        m_pairBadge = pairBadge;
        transform.SetParent(pairBadge.transform); // �y�A���b�y���̎q�I�u�W�F�N�g�Ƃ��Đݒ�
    }

    public Transform GetParentTransform()
    {
        // �t�F���g�u���b�N��Transform���擾
        return (m_pairBadge != null) ? m_pairBadge.transform : transform;
    }


    /// <summary>
    /// �X�e�[�W�u���b�N���擾���܂��B
    /// </summary>
    public StageBlock stageBlock
    {
        get { return m_stageBlock; }

    }

    /// <summary>
    /// �t�F���g�u���b�N���ړ�����
    /// </summary>
    /// <param name="velocity"></param>
    public void RequestMove(GridPos velocity)
    {
        if (m_pairBadge != null)
        {
            // �y�A���b�y��������ꍇ�̓y�A���b�y���Ɉړ����˗�
            m_pairBadge.Move(velocity);
            return;
        }

        Move(velocity); // �y�A���b�y�����Ȃ��ꍇ�͎������g���ړ�

    }

    ///// <summary>
    ///// �t�F���g�u���b�N���ړ�����
    ///// </summary>
    ///// <param name="velocity"></param>
    public void Move(GridPos velocity)
    {

        GridPos newGridPos = m_stageBlock.GetGridPos() + velocity;

        // �X�e�[�W�u���b�N�̈ʒu���X�V
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
            // �y�A���b�y��������ꍇ�̓y�A���b�y���ɃX���C�h���˗�
            m_pairBadge.Slide(m_prevVelocity);
            return;
        }

        // �y�A���b�y�����Ȃ��ꍇ�͎������g���X���C�h
        StartSlide(m_prevVelocity);
    }

    public void StartSlide(GridPos velocity)
    {
        

        var map = MapData.GetInstance; // �}�b�v�f�[�^���擾

        // �u���b�N�������O�̊J�n�ʒu��ݒ�
        var blockPos = m_stageBlock.GetGridPos(); // �u���b�N�̃O���b�h�ʒu���擾

        // �O�̂��ߍĔz�u
        transform.position = map.ConvertGridToWorldPos(blockPos); 

        var endGridPos = blockPos + velocity; // �u���b�N����������̃O���b�h�ʒu���v�Z

        // �u���b�N����������̖ڕW�ʒu��ݒ�
        var endPosition = map.ConvertGridToWorldPos(endGridPos);

     

        transform.DOMove(endPosition, TARGET_TIME)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // �O�̂��ߍĔz�u
                // ��ɍ��W��ݒ肵�Ă���ړ�
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
            // �y�A���b�y��������ꍇ�̓y�A���b�y���̃X���C�h�\���`�F�b�N
            return m_pairBadge.CanSlide();
        }

        return CanSlide(); // �y�A���b�y�����Ȃ��ꍇ�͎������g�̃X���C�h�\���`�F�b�N
    }

    public bool CanSlide()
    {
        if (CheckCanMove(m_prevVelocity) == false) return false;

        // **** ���̎�ނŃ`�F�b�N ****
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
                // �������Ȃ����
                break;
            case State.MOVE:
            
                break;
            case State.SLIDE:
                // �X���C�h��Ԃ̏���
                RequestSlide();
                break;
        }
    }

}
