using UnityEngine;

/// <summary>
/// �Q�[�����̃C���^���N�V�����C�x���g���Ď����邽�߂̃C���^�[�t�F�[�X
/// </summary>
public  interface IGameInteractionObserver 
{
    /// <summary>
    /// �Q�[�����̃C���^���N�V�����C�x���g����M���郁�\�b�h
    /// </summary>
    /// <param name="eventID">�C�x���g���b�Z�[�W</param>
    public void OnEvent(InteractionEvent eventID);
}
