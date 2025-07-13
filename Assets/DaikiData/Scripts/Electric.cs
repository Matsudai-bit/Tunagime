using UnityEngine;

/// <summary>
/// �d�C�Ɋւ���N���X
/// </summary>
public class Electric : MonoBehaviour
{
    /// <summary>
    /// ����Ă���d�C�̎��
    /// </summary>
    [System.Serializable]
    public enum ElectricFlowType
    {
        NO_FLOW,
        NORMAL,
        RED
        
    }

    ElectricFlowType m_electricFlowType;    // �d�C�̎��

    /// <summary>
    /// �d�C�̗���Ă����ނ̎擾
    /// </summary>
    /// <returns>�d�C�̎��</returns>
    public ElectricFlowType  GetElectricFlowType()
    {
        return m_electricFlowType;
    }

    /// <summary>
    /// �d�C�̗���Ă����ނ̐ݒ�
    /// </summary>
    /// <returns>�d�C�̎��</returns>
    public void SetElectricFlowType(ElectricFlowType type)
    {
       m_electricFlowType = type;
    }

}
