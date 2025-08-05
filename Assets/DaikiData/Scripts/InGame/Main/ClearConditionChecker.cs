using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// �N���A�������`�F�b�N����N���X
/// </summary>
public class ClearConditionChecker : MonoBehaviour
{
    [Header("�N���A�����̐ݒ�")]
    [SerializeField]
    private List<TerminusFeelingSlot> terminusFeelingSlots = new List<TerminusFeelingSlot>(); // �I�_�ɂ���^�̃��X�g

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
        
    }


    /// <summary>
    /// �X�V����
    /// </summary>
    void Update()
    {
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
}
