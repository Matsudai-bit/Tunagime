using UnityEngine;

/// <summary>
/// �j�̏�Ԃ��Ď����邽�߂̃C���^�[�t�F�[�X
/// </summary>
public interface ICoreStateMonitor
{
    /// <summary>
    /// �Q�[�����̃C���^���N�V�����C�x���g����M���郁�\�b�h
    /// </summary>
    /// <param name="emotionCurrent">�z���̎��</param>
    public void OnConnectEvent(EmotionCurrent emotionCurrent);
}
