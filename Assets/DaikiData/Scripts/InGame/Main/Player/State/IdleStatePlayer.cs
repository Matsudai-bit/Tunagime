using UnityEngine;

public class IdleStatePlayer : PlayerState
{

    public IdleStatePlayer(Player owner) : base(owner)
    {
       
    }


    public override void OnFixedUpdateState()
    {
        // ��]����
        owner.Rotate();

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

    }
    public override void OnFinishState()
    {

    }


   

}
