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
        // �ړ����~
        owner.StopMove();

    }

    public override void OnUpdateState() 
    {

        // �����^�ԃI�u�W�F�N�g���E�����������݂�
        owner.TryPickUp();
        // �����^�ԃI�u�W�F�N�g��u�����������݂�
        owner.TryPutDown();

        if (owner.GetCarryingObject() != null)
        {
            // �҂ޏ��������݂�
            owner.TryKnit();
            // test
            owner.TryForwardFloorSetting();
        }
        else
        {

            // test
            owner.TryForwardObjectSetting();
            owner.TryUnknit();
            owner.TryPushBlock();


        }

    }
    public override void OnFinishState()
    {
        owner.ResetTargetObject();
    }


   

}
