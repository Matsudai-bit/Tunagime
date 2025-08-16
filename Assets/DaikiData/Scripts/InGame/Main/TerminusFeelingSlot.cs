using UnityEngine;

/// <summary>
/// �I�_�ɂ���^
/// </summary>
public class TerminusFeelingSlot : MonoBehaviour, IGameInteractionObserver
{
    private StageBlock m_stageBlock;    // �X�e�[�W�u���b�N

    private FeelingSlot m_feelingSlot; // �z���̌^

    [SerializeField]
    bool m_isConnection;


    private void Awake()
    {
        // �X�e�[�W�u���b�N���擾
        m_stageBlock = GetComponent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("TerminusFeelingSlot requires a StageBlock component.");
        }

        // �z���̌^���擾
        m_feelingSlot = GetComponent<FeelingSlot>();
        if (m_feelingSlot == null)
        {
            Debug.LogError("TerminusFeelingSlot requires a FeelingSlot component.");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �I�u�U�[�o�[�Ƃ��ēo�^
        GameInteractionEventMessenger.GetInstance.RegisterObserver(this);

        // ������ԂŐڑ����`�F�b�N
        m_isConnection = false;

        // ���j�^�[�ɓo�^
        FeelingSlotStateMonitor.GetInstance.RegisterMonitorObject(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// �q�����Ă��邩�ǂ���
    /// </summary>
    /// <returns></returns>
    public bool IsConnected()
    {
        return m_isConnection;
    }
    /// <summary>
    /// �ڑ���Ԃ��`�F�b�N���郁�\�b�h
    /// </summary>

    private void CheckConnection()
    {
        //�@�O���b�h�f�[�^�̎擾
        StageGridData gridData = MapData.GetInstance.GetStageGridData();

        //�@�O���b�h�ʒu�̎擾
        GridPos gridPos = m_stageBlock.GetGridPos();

        // ���ׂ̃O���b�h���`�F�b�N
        GridPos checkGridPos = gridPos + new GridPos(-1, 0);

        // ���݂��`���[�u���擾
        var amidaTube = gridData.GetAmidaTube(checkGridPos);

        if (amidaTube == null)
        {
           // Debug.LogWarning($"AmidaTube not found at position ({checkGridPos.x}, {checkGridPos.y}).");
            m_isConnection = false;
            return;
        }

        // �ڑ���Ԃ̊m�F
        if (amidaTube.GetEmotionType(YarnMaterialGetter.MaterialType.OUTPUT) == m_feelingSlot.GetEmotionType())
  
            // �I�_�̊���ƈ�v���Ă���ꍇ�͌q�����Ă���
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
            return m_feelingSlot.GetEmotionType(); // �z���̌^�̊���^�C�v���擾
        }
    }

    // �폜��
    private void OnDestroy()
    {
        // �Q�[���C���^���N�V�����C�x���g�̃I�u�U�[�o�[������
        GameInteractionEventMessenger.GetInstance.RemoveObserver(this);
        // ���j�^�[����o�^����
        FeelingSlotStateMonitor.GetInstance.RemoveMonitor(this);
    }
}
