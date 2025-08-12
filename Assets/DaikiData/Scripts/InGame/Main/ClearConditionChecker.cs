using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// �N���A�������`�F�b�N����N���X
/// </summary>
public class ClearConditionChecker : MonoBehaviour , IGameInteractionObserver
{
    [Header("�N���A�����̐ݒ�")]
    [SerializeField]
    private List<TerminusFeelingSlot> terminusFeelingSlots = new List<TerminusFeelingSlot>(); // �I�_�ɂ���^�̃��X�g

    private bool m_isConnectionRejectionSlot = false; // �I�_�̋���̊j���ڑ�����Ă��邩�ǂ���

    
    private bool m_isDelayClearCheck = false;// �N���A�����x�������邩�ǂ���

    private GameDirector m_gameDirector;

    private void Awake()
    {
        // GameDirector�̃C���X�^���X���擾
        m_gameDirector = GetComponent<GameDirector>();
        if (m_gameDirector == null)
        {
            Debug.LogError("GameDirector instance not found in the scene.");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �I�u�U�[�o�[�Ƃ��ēo�^
        GameInteractionEventMessenger.GetInstance.RegisterObserver(this);

        m_isConnectionRejectionSlot = false; // ������Ԃł͐ڑ�����Ă��Ȃ�
        m_isDelayClearCheck = false;
    }


    /// <summary>
    /// �X�V����
    /// </summary>
    void Update()
    {
        if (m_isDelayClearCheck)
        {
            // �N���A�����̃`�F�b�N��x��������ꍇ�͉������Ȃ�
            m_isDelayClearCheck = false;
            return;
        }

        // �N���A�����̃`�F�b�N
        if (CheckClearCondition())
        {
            Debug.Log("�N���A������B�����܂����I");
            // �N���A�����������ɒǉ�
            if (m_gameDirector != null)
            {
                m_gameDirector.OnGameClear(); // �Q�[���N���A�̏������Ăяo��
            }

        }

    }

    private bool CheckClearCondition()
    {
        // �I�_�̋���̊j���ڑ�����Ă���ꍇ�̓N���A�����𖞂����Ȃ�
        if (m_isConnectionRejectionSlot)
            return false;

        // �N���A�����𖞂����Ă��邩�ǂ������`�F�b�N
        foreach (var slot in terminusFeelingSlots)
        {
            if (!slot.IsConnected())
            {
                return false; // 1�ł��q�����Ă��Ȃ��ꍇ�̓N���A�������B��
            }
        }
        return true; // �S�Ă̏I�_���q�����Ă���ꍇ�̓N���A�����B��
    }

    /// <summary>
    /// �I�_�ɂ���^��ǉ����郁�\�b�h
    /// </summary>
    /// <param name="slot"></param>
    public void AddTerminusFeelingSlot(TerminusFeelingSlot slot) // �����C���^�[�t�F�[�X��Ⴄ�`�ɂ�����
    {
        if (!terminusFeelingSlots.Contains(slot))
        {
            terminusFeelingSlots.Add(slot);
        }
    }

    public void OnEvent(InteractionEvent eventID)
    {
        // �C�x���gID�ɉ����ď����𕪊�
        switch (eventID)
        {
            case InteractionEvent.CONNECTED_REJECTION_SLOT:
                m_isConnectionRejectionSlot = true; // �I�_�̋���̊j���ڑ����ꂽ
                break;
            case InteractionEvent.DISCONNECTED_REJECTION_SLOT:
                m_isConnectionRejectionSlot = false; // �I�_�̋���̊j���ؒf���ꂽ
                break;
            case InteractionEvent.FLOWWING_AMIDAKUJI:
                m_isDelayClearCheck = true; // ���݂���H��C�x���g�����������ꍇ�A�N���A�����x��������
                break;
            default:
                break;
        }
    }
    // �폜��
    private void OnDestroy()
    {
        // �Q�[���C���^���N�V�����C�x���g�̃I�u�U�[�o�[������
        GameInteractionEventMessenger.GetInstance.RemoveObserver(this);
    }
}

