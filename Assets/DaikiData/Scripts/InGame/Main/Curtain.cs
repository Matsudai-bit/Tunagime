using UnityEngine;
using UnityEngine.UIElements.Experimental;
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

    private float m_elapsedTime = 0.0f; // �o�ߎ���

    private float m_openingTime = 2.5f; // �J�[�e�����J������
    private float m_closingTime = 1.5f; // �J�[�e������鎞��


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
    }

    void Start()
    {
        // �}�b�v�̐ݒ�
        var stageGridData = MapData.GetInstance.GetStageGridData();

        // �X�e�[�W�u���b�N�̃O���b�h�ʒu���擾
        GridPos gridPos = m_stageBlock.GetGridPos();

        // ���E�̃t�F���g�u���b�N�̃O���b�h�ʒu��ݒ�
        GridPos forwardDirection = GetForwardDirection();

        m_feltBlock_R.stageBlock.SetGridPos(gridPos + new GridPos(forwardDirection.y, forwardDirection.x));
        m_feltBlock_L.stageBlock.SetGridPos(gridPos + new GridPos(-forwardDirection.y, forwardDirection.x));

        // �t�F���g�u���b�N�̃}�b�v�֒ǉ�
        stageGridData.TryPlaceTileObject(m_feltBlock_R.stageBlock.GetGridPos(), m_curtainModel_R);
        stageGridData.TryPlaceTileObject(m_feltBlock_L.stageBlock.GetGridPos(), m_curtainModel_L);

        m_state = State.OPENING; // �J�[�e���̏�Ԃ��J����Ԃɐݒ�
        StartOpenCurtain(); // �J�[�e�����J���������J�n

    }

    // Update is called once per frame
    void Update()
    {


        switch (m_state)
        {
            case State.OPENING:
                Open();
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
        // ���݂̃J�[�e���̈ʒuX���擾
        m_startPos_R = m_curtainModel_R.transform.localPosition;

        // �J�[�e���̏I���ʒuX��ݒ�
        m_endPos_R = m_startPos_R +  m_movementVector;

        // �J�[�e�����J������


            m_state = State.OPENING;
        float backPower = 2.5f;

        m_curtainModel_R.transform.DOLocalMove(m_endPos_R, m_openingTime).SetEase(Ease.OutBack,  backPower);
        m_curtainModel_L.transform.DOLocalMove(-m_endPos_R, m_openingTime).SetEase(Ease.OutBack, backPower);

        // �X�P�[���𒲐�
        m_curtainModel_R.transform.DOScaleX(m_targetScaleX, m_openingTime).SetEase(Ease.OutBack,  backPower);
        m_curtainModel_L.transform.DOScaleX(m_targetScaleX, m_openingTime).SetEase(Ease.OutBack,  backPower).OnComplete(() =>
        {
            // �J������ԂɈڍs
            m_state = State.OPENING_FINISHED;
            m_elapsedTime = 0.0f; // �o�ߎ��Ԃ����Z�b�g
        });

    }

    private void Open()
    {
        m_elapsedTime += Time.deltaTime;

        // �J�[�e�����J������

        

        //// �ړ�
        //m_curtainModel_R.transform.localPosition = Vector3.Lerp(m_startPos_R, m_endPos_R, m_elapsedTime/ m_openingTime);
        //m_curtainModel_L.transform.localPosition = Vector3.Lerp(-m_startPos_R, -m_endPos_R, m_elapsedTime / m_openingTime);



        //m_curtainModel_R.transform.localScale = new Vector3(Mathf.Lerp(1.0f, m_targetScaleX, m_elapsedTime / m_openingTime), 1.0f, 1.0f);
        //m_curtainModel_L.transform.localScale = new Vector3(Mathf.Lerp(1.0f, m_targetScaleX, m_elapsedTime / m_openingTime), 1.0f, 1.0f);

        //if (m_elapsedTime >= m_openingTime)
        //{
        //    // �J������ԂɈڍs
        //    m_state = State.OPENING_FINISHED;
        //    m_elapsedTime = 0.0f; // �o�ߎ��Ԃ����Z�b�g
        //}
    }


    private void StartCloseCurtain()
    {
        // �J�[�e������鏈��
        m_curtainModel_L.transform.localPosition += new Vector3(0.1f, 0, 0);
        m_curtainModel_R.transform.localPosition += new Vector3(-0.1f, 0, 0);
        // ������ԂɈڍs
        if (m_curtainModel_L.transform.localPosition.x >= 0.0f && m_curtainModel_R.transform.localPosition.x <= 0.0f)
        {
            m_state = State.CLOSING_FINISHED;
        }
    }

}
