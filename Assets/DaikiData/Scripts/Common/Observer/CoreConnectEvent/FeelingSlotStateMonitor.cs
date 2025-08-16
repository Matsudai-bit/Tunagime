using System.Collections.Generic;

/// <summary>
/// �j�̏�Ԃ��Ď����Ă���N���X �V���O���g��
/// </summary>
public class FeelingSlotStateMonitor
{

    private List<TerminusFeelingSlot> m_monitorObjects = new List<TerminusFeelingSlot>(); // �ώ@�҃��X�g

    private static FeelingSlotStateMonitor s_instance;
    /// <summary>
    /// �V���O���g���C���X�^���X���擾
    /// </summary>
    public static FeelingSlotStateMonitor GetInstance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new FeelingSlotStateMonitor();
            }
            return s_instance;
        }
    }
  

    public void RegisterMonitorObject(TerminusFeelingSlot monitor)
    {
        // ���ɓo�^����Ă���I�u�U�[�o�[�͒ǉ����Ȃ�
        if (!m_monitorObjects.Contains(monitor))
        {
            m_monitorObjects.Add(monitor);
        }
    }

    public bool IsConnected(EmotionCurrent.Type coreType)
    {
        foreach (var monitor in m_monitorObjects)
        {
            if (monitor.emotionType == coreType)
            {
                if (monitor.IsConnected()) return true;// �I�u�U�[�o�[������^�C�v�ƈ�v����ꍇ�͐ڑ�����Ă���
            }
        }

        return false; // ��v����I�u�U�[�o�[�����Ȃ��ꍇ�͐ڑ�����Ă��Ȃ�
    }

    /// <summary>
    /// �I�u�U�[�o�[��o�^�������郁�\�b�h
    /// </summary>
    /// <param name="monitor"></param>
    public void RemoveMonitor(TerminusFeelingSlot monitor)
    {
        // �I�u�U�[�o�[���X�g����폜
        if (m_monitorObjects.Contains(monitor))
        {
            m_monitorObjects.Remove(monitor);
        }
    }
}
