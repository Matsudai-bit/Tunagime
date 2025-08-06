using UnityEngine;

/// <summary>
/// �z���̗���
/// </summary>

public class EmotionCurrent : MonoBehaviour
{

    /// <summary>
    /// �z���̗���̎��
    /// </summary>
    public enum Type
    {
        NONE,         // �������
        FAITHFULNESS, // ����     ��
        LOVE,         // ��       �ԁ@
        KINDNESS,     // �D����   ��
        COURAGE,      // �E�C     ��
        LONGING,      // ����     ��
        REJECTION,    // ����     ��
    }

    [SerializeField]
    private Type m_Type = Type.NONE; // �z���̗���̎��


    public Type CurrentType
    {
        get { return m_Type; }
        set { m_Type = value; }
    }

}
