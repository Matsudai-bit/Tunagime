
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// �X�C�b�`�̎��
/// </summary>
public enum SwitchType
{
    YELLOW,
    RED,
    PURPLE
}

public class SwitchTypeController : MonoBehaviour
{
    private List<SwitchType> m_wallType;    // �ǂ̎�ލ����\

    
    /// <summary>
    /// ��ނ̎擾
    /// </summary>
    /// <returns>�ǂ̎��</returns>
    public List<SwitchType> GetSwitchType()
    {
        return m_wallType;
    }

    /// <summary>
    /// ��ނ̐ݒ�
    /// </summary>
    public void SetSwitchType(List<SwitchType> switchType)
    {
        m_wallType = switchType;
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Erase()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �o������
    /// </summary>
    public void Appearance()
    {
        gameObject.SetActive(true);

    }


}