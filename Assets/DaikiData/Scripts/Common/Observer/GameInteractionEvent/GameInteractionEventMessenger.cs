using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// �Q�[�����̃C���^���N�V�����C�x���g���Ǘ�����N���X �V���O���g��
/// </summary>
public class GameInteractionEventMessenger 
{
    
    private List<IGameInteractionObserver> m_observers = new List<IGameInteractionObserver>(); // �I�u�U�[�o�[�̃��X�g

    private static GameInteractionEventMessenger s_instance;
    /// <summary>
    /// �V���O���g���C���X�^���X���擾
    /// </summary>
    public static GameInteractionEventMessenger GetInstance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new GameInteractionEventMessenger();
            }
            return s_instance;
        }
    }
    private GameInteractionEventMessenger() { }

    public void RegisterObserver(IGameInteractionObserver observer)
    {
        // ���ɓo�^����Ă���I�u�U�[�o�[�͒ǉ����Ȃ�
        if (!m_observers.Contains(observer))
        {
            m_observers.Add(observer);
        }
    }

    /// <summary>
    /// �C�x���g��ʒm���郁�\�b�h
    /// </summary>
    /// <param name="eventMessage">�C�x���g���b�Z�[�W</param>
    public void Notify(InteractionEvent eventMessage)
    {
        foreach (var observer in m_observers)
        {
            observer.OnEvent(eventMessage);
        }
    }
}
