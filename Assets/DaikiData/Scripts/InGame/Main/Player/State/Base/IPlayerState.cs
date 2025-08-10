using UnityEngine;

public enum PlayerStateID
{
    NONE,     // �����ȏ��
    IDLE,       // �ҋ@
    WALK,       // ���s
    PICK_UP,    // �E��
    PUT_DOWN,  // ���낷
    KNIT,       // �҂�
    UNKNIT,     // ����
    PUSH_BLOCK, // �u���b�N������

}


/// <summary>
/// �v���C���[�̏�Ԃ�\�����ۃN���X
/// </summary>
public abstract class PlayerState 
{
    // �v���C���[
    private Player m_owner; // ���L�҂̃v���C���[

    public abstract void OnStartState();    // �X�e�[�g�J�n���Ɉ�x�����Ă΂��
    public abstract void OnUpdateState();     // �X�e�[�g����Update�Ŗ��t���[���Ă΂��
    public abstract void OnFixedUpdateState(); // �X�e�[�g����FixedUpdate�ŕ������Z�t���[�����ƂɌĂ΂��
    public abstract void OnFinishState();   // �X�e�[�g�I�����Ɉ�x�����Ă΂��


    /// <summary>
    /// �v���C���[�̏�Ԃ�����������R���X�g���N�^
    /// </summary>
    /// <param name="owner"></param>
    public PlayerState(Player owner)
    {
        m_owner = owner;

    }

    public Player owner
    {
        get { return m_owner; }
    }

}