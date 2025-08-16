using UnityEngine;
using DG.Tweening;	//DOTween���g���Ƃ��͂���using������


/// <summary>
/// �J�[�e��
/// </summary>
public class Curtain : MonoBehaviour
{
    /// <summary>
    /// �J�[�e���̏�Ԃ�\���񋓌^
    /// </summary>
    enum State
    {
        NONE = 0, // �������Ȃ����
       
        OPENING, // �J���Ă�����
        CLOSING, // ���Ă�����

        OPENING_FINISHED, // �J�������
        CLOSING_FINISHED, // �������
    }

    [SerializeField] private GameObject m_curtainModel_L = null; // �J�[�e�����f���i���j
    [SerializeField] private GameObject m_curtainModel_R = null; // �J�[�e�����f���i�E�j

    [SerializeField] private FeltBlock m_feltBlock_L = null; // �t�F���g�u���b�N�i�J�[�e���̍��ɂ���u���b�N�j
    [SerializeField] private FeltBlock m_feltBlock_R = null; // �t�F���g�u���b�N�i�J�[�e���̉E�ɂ���u���b�N�j


    private StageBlock m_stageBlock = null; // �X�e�[�W�u���b�N

    private State m_state = State.OPENING; // �J�[�e���̏��

    private Vector3 m_startPos_R ; // �J�[�e���̊J�n�ʒu�iX�������j
    private Vector3 m_endPos_R;    // �J�[�e���̊J�n�ʒu�iX�������j

    private float m_targetScaleX = 0.5f; // �J�[�e���̖ڕW�X�P�[���iX�������j

    private Vector3 m_movementVector = new Vector3(0.22f, 0.0f, 0.0f); // �J�[�e�����J��/����ۂ̈ړ��ʁiX�������j

    [SerializeField]
    private float m_openingTime = 2.5f; // �J�[�e�����J������
    [SerializeField]
    private float m_closingTime = 1.5f; // �J�[�e������鎞��

    private Collider m_collider; // �J�[�e���̃R���C�_�[

    private EmotionCurrent m_emotionCurrent;  // �z���̎��


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        // �X�e�[�W�u���b�N���擾
        m_stageBlock = GetComponentInParent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("Curtain: StageBlock not found in parent.");
        }
        // �J�[�e�����f���̏�����
        if (m_curtainModel_L == null || m_curtainModel_R == null)
        {
            Debug.LogError("Curtain: Curtain models are not assigned.");
        }

        // �t�F���g�u���b�N�̏�����
        if (m_feltBlock_L == null || m_feltBlock_R == null)
        {
            Debug.LogError("Curtain: FeltBlocks are not assigned.");
        }

        // �J�[�e���̃R���C�_�[���擾
        m_collider = GetComponent<Collider>();

        // �z���̎�ނ̎擾
        m_emotionCurrent = GetComponentInParent<EmotionCurrent>();
    }

    void Start()
    {

        // ���݂̃J�[�e���̈ʒuX���擾
        m_startPos_R = m_curtainModel_R.transform.localPosition;
        // �J�[�e���̏I���ʒuX��ݒ�
        m_endPos_R = m_startPos_R + m_movementVector;

        // �}�b�v�̐ݒ�
        var stageGridData = MapData.GetInstance.GetStageGridData();

        // �X�e�[�W�u���b�N�̃O���b�h�ʒu���擾
        GridPos gridPos = m_stageBlock.GetGridPos();

        // ���E�̃t�F���g�u���b�N�̃O���b�h�ʒu��ݒ�
        GridPos forwardDirection = GetForwardDirection();

        m_feltBlock_R.stageBlock.SetGridPos(gridPos + new GridPos(forwardDirection.y, forwardDirection.x));
        m_feltBlock_L.stageBlock.SetGridPos(gridPos + new GridPos(-forwardDirection.y, -forwardDirection.x));

        // �t�F���g�u���b�N�̃}�b�v�֒ǉ�
        stageGridData.TryPlaceTileObject(m_feltBlock_R.stageBlock.GetGridPos(), m_feltBlock_R.gameObject);
        stageGridData.TryPlaceTileObject(m_feltBlock_L.stageBlock.GetGridPos(), m_feltBlock_L.gameObject);

        m_state = State.CLOSING_FINISHED; // ������Ԃ������Ԃɐݒ�

        TryChangeState();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_state == State.OPENING_FINISHED || m_state == State.CLOSING_FINISHED)
        {
            TryChangeState();

        }

        switch (m_state)
        {
            case State.OPENING:
                break;
            case State.CLOSING:
                // �J�[�e������鏈��
                
                break;
            case State.OPENING_FINISHED:
                // �J������Ԃ̏���
                break;
            case State.CLOSING_FINISHED:
                // ������Ԃ̏���
                break;
        }
    }

    /// <summary>
    /// �O���̃O���b�h�������擾
    /// </summary>
    /// <returns></returns>
    public GridPos GetForwardDirection()
    {
        Vector3 forward = transform.forward;
        // �e���̕����̑傫�����r���đ傫�����𐳋K�����A�O���b�h�����Ƃ��đI��
        // ���� : �����_�����ނ��߁A�����ł͂Ȃ��ꍇ������̂�Round���g�p���Ă��邽�ߕs�K�؂Ɣ��f
        GridPos forward2D = (Mathf.Abs(forward.x) > Mathf.Abs(forward.z))
            ? new GridPos((int)Mathf.Round(forward.x), 0)
            : new GridPos(0, -(int)Mathf.Round(forward.z));
        return forward2D; // �`�F�b�N����O���b�h�ʒu
    }


    private void StartOpenCurtain()
    {

        m_collider.enabled = false; // �J�[�e���̃R���C�_�[�𖳌���
        var stageGridData = MapData.GetInstance.GetStageGridData();
        // �X�e�[�W�u���b�N�̃O���b�h�ʒu���擾���āA�J�[�e���̃O���b�h�f�[�^����폜
        stageGridData.RemoveGridDataGameObject(m_stageBlock.GetGridPos()); // �X�e�[�W�u���b�N�̃O���b�h�f�[�^����폜


        // �J�[�e�����J������

        float backPower = 2.5f;

        m_curtainModel_R.transform.DOLocalMove(m_endPos_R, m_openingTime).SetEase(Ease.OutBack,  backPower);
        m_curtainModel_L.transform.DOLocalMove(new Vector3 (-m_endPos_R.x, m_endPos_R.y, m_endPos_R.z), m_openingTime).SetEase(Ease.OutBack, backPower);

        // �X�P�[���𒲐�
        m_curtainModel_R.transform.DOScaleX(m_targetScaleX, m_openingTime).SetEase(Ease.OutBack,  backPower);
        m_curtainModel_L.transform.DOScaleX(m_targetScaleX, m_openingTime).SetEase(Ease.OutBack,  backPower).OnComplete(() =>
        {
            // �J������ԂɈڍs
            ChangeState(State.OPENING_FINISHED);
        });

    }

    private void StartCloseCurtain()
    {
        m_collider.enabled = true; // �J�[�e���̃R���C�_�[�𖳌���

        var stageGridData = MapData.GetInstance.GetStageGridData();
        // �X�e�[�W�u���b�N�̃O���b�h�ʒu���擾���āA�J�[�e���̃O���b�h�f�[�^�ɒǉ�
        stageGridData.TryPlaceTileObject(m_stageBlock.GetGridPos(), gameObject);


        // �J�[�e������鏈��
        float backPower = 1.0f;

        m_curtainModel_R.transform.DOLocalMove(m_startPos_R, m_closingTime).SetEase(Ease.OutBack, backPower);
        m_curtainModel_L.transform.DOLocalMove(new Vector3(-m_startPos_R.x, m_startPos_R.y, m_startPos_R.z), m_closingTime).SetEase(Ease.OutBack, backPower);

        // �X�P�[���𒲐�
        m_curtainModel_R.transform.DOScaleX(1.0f, m_closingTime).SetEase(Ease.OutBack, backPower);
        m_curtainModel_L.transform.DOScaleX(1.0f, m_closingTime).SetEase(Ease.OutBack, backPower).OnComplete(() =>
        {
            // ������ԂɈڍs
            ChangeState(State.CLOSING_FINISHED);
        });
    }

    private void TryChangeState()
    {

        // �^�̌q���胂�j�^�[
        var slotStateMonitor = FeelingSlotStateMonitor.GetInstance;
        if (slotStateMonitor.IsConnected(m_emotionCurrent.CurrentType))
        {
            // �J�[�e���̏�Ԃ��J����Ԃɐݒ�
            if (m_state == State.CLOSING_FINISHED)
            {
                ChangeState(State.OPENING);
            }
        }
        else
        {
            // �J�[�e���̏�Ԃ�����Ԃɐݒ�
            if (m_state == State.OPENING_FINISHED)
            {
                if (CanClose())
                    ChangeState(State.CLOSING);
            }
        }
    }

    private void ChangeState(State newState)
    {
        // �J�[�e���̏�Ԃ�ύX
        m_state = newState;
        switch (m_state)
        {
            case State.OPENING:
                StartOpenCurtain();
                break;
            case State.CLOSING:
                StartCloseCurtain();
                break;
            case State.OPENING_FINISHED:
                // �J������Ԃ̏���
                break;
            case State.CLOSING_FINISHED:
                // ������Ԃ̏���

                break;
        }
    }


    private bool CanClose()
    {
        // �����Ƀ��C���΂��Ď����ȊO�̃I�u�W�F�N�g������΂ł��Ȃ�
        RaycastHit hit;
        if (Physics.Raycast(m_curtainModel_L.transform.position, -m_curtainModel_L.transform.right, out hit, 0.5f))
        {
            
            if (hit.collider.gameObject != null && hit.collider.gameObject != gameObject)
            {
                return false; // ���̃t�F���g�u���b�N������ꍇ�͕����Ȃ�
            }
        }
        return true;
    }

    public void SetMaterial(Material material)
    {
        if (m_curtainModel_L != null && m_curtainModel_R != null)
        {
            m_curtainModel_L.GetComponent<MeshRenderer>().material = material;
            m_curtainModel_R.GetComponent<MeshRenderer>().material = material;
        }
        else
        {
            Debug.LogError("Curtain: Curtain models are not assigned.");
        }
    }
}
