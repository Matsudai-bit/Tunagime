using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// �X�C�b�`�Ɋւ���N���X
/// </summary>
public class Switch : MonoBehaviour
{


    private SwitchType m_switchType;

    [SerializeField] private bool m_isPush = false; // ������Ă��邩�ǂ���

    /// <summary>
    /// ������Ă��邩�ǂ���
    /// </summary>
    /// <returns>true ������Ă���</returns>
    public bool IsPush()
    {
        return m_isPush;
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Push()
    {

        m_isPush = true;
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Release()
    {
        m_isPush = false;
    }


    /// <summary>
    /// ��ނ̎擾
    /// </summary>
    /// <returns>�ǂ̎��</returns>
    public SwitchType GetSwitchType()
    {
        return m_switchType;
    }

    /// <summary>
    /// ��ނ̐ݒ�
    /// </summary>
    public void SetSwitchType(SwitchType switchType)
    {
        m_switchType = switchType;
    }


}
