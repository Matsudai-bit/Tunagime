using UnityEngine;

public class IdleStatePlayer : PlayerState
{

    private int m_idleAnimationHash;

    public IdleStatePlayer(Player owner) : base(owner)
    {
       
    }


    public override void OnFixedUpdateState()
    {

        // �v���C���[�̈ړ�����
        if (owner.IsMoving())
        {
            // ���s��ԂɑJ��
            owner.GetStateMachine().RequestStateChange(PlayerStateID.WALK);
        }
    }

    public override void  OnStartState() 
    {

    
    }

    public override void OnUpdateState() 
    {
        // �����^�ԃI�u�W�F�N�g���E�����������݂�
        owner.TryPickUp();
        // �����^�ԃI�u�W�F�N�g��u�����������݂�
        owner.TryPickDown();

        if (owner.GetCarryingObject() != null)
        {
            // �҂ޏ��������݂�
            owner.TryKnit();
        }

    }
    public override void OnFinishState()
    {

    }


   

}
