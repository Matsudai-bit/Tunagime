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
    public override void OnFinishState()
    {

    }


   

}
