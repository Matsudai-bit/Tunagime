using UnityEngine;

public class WalkStatePlayer : PlayerState
{
    public WalkStatePlayer(Player owner) : base(owner)
    {

    }
    /// <summary>
    /// ���s��Ԃ̊J�n���Ɉ�x�����Ă΂��
    /// </summary>
    public override void OnStartState()
    {
        // ���s��Ԃ̊J�n���ɃA�j���[�V������ݒ�
        owner.GetAnimator().SetBool("Walk", true);
    }

    /// <summary>
    /// ���s��Ԓ���Update�Ŗ��t���[���Ă΂��
    /// </summary>
    public override void OnUpdateState()
    {
        // �Ȗу{�[�����E�����������݂�
        owner.TryPickUp();
        // �Ȗу{�[����u�����������݂�
        owner.TryPickDown();

        if (owner.GetFluffBall() != null)
        {
            // �҂ޏ��������݂�
            owner.TryKnot();
        }

    }
    /// <summary>
    /// ���s��Ԓ���FixedUpdate�ŕ������Z�t���[�����ƂɌĂ΂��
    /// </summary>
    public override void OnFixedUpdateState()
    {
     
        // �v���C���[�̈ړ�����
        if (owner.IsMoving())
        {
            // �ړ�����
            owner.Move();
        }
        else
        {
            // ���s��Ԃ���ҋ@��ԂɑJ��
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);
        }
    }

    /// <summary>
    /// ���s��Ԃ̏I�����Ɉ�x�����Ă΂��
    /// </summary>
    public override void OnFinishState()
    {
        // ���s��Ԃ̏I�����ɃA�j���[�V���������Z�b�g
        owner.GetAnimator().SetBool("Walk", false);
    }


}
