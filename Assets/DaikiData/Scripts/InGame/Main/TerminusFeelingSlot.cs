using UnityEngine;

/// <summary>
/// �I�_�ɂ���^
/// </summary>
public class TerminusFeelingSlot : MonoBehaviour
{
    private StageBlock m_stageBlock;    // �X�e�[�W�u���b�N

    private void Awake()
    {
        // �X�e�[�W�u���b�N���擾
        m_stageBlock = GetComponent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("TerminusFeelingSlot requires a StageBlock component.");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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


        // �X�e�[�W�u���b�N�����݂��A�ڑ�����Ă��邩�ǂ������m�F
        //return m_stageBlock != null && m_stageBlock.IsConnected();

        return true;
    }
}
