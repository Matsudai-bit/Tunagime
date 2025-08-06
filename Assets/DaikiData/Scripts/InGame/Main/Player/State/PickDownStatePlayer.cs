using UnityEngine;

public class PickDownStatePlayer : PlayerState
{
    public PickDownStatePlayer(Player owner) : base(owner)
    {

    }
    /// <summary>
    /// ���s��Ԃ̊J�n���Ɉ�x�����Ă΂��
    /// </summary>
    public override void OnStartState()
    {
        //owner.GetAnimator().SetTrigger("PickDown"); // �����^�ԃI�u�W�F�N�g���E���A�j���[�V�������g���K�[


        
    }

    /// <summary>
    /// ���s��Ԓ���Update�Ŗ��t���[���Ă΂��
    /// </summary>
    public override void OnUpdateState()
    {
        // �A�j���[�V�����̏I����҂�
        //if (owner.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        //{
            // �A�j���[�V�������I�������玝���^�ԃI�u�W�F�N�g���E�����������s
            PickDown();
            // �ҋ@��ԂɑJ��
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);


            // �����^�ԃI�u�W�F�N�g�������Ă���ꍇ�A�A�j���[�V�������C���[��L����
            owner.GetAnimator().SetLayerWeight(owner.GetAnimator().GetLayerIndex("TakeFluffBall"), 0);
        //}

     
    }
    /// <summary>
    /// ���s��Ԓ���FixedUpdate�ŕ������Z�t���[�����ƂɌĂ΂��
    /// </summary>
    public override void OnFixedUpdateState()
    {
     

    }

    /// <summary>
    /// ���s��Ԃ̏I�����Ɉ�x�����Ă΂��
    /// </summary>
    public override void OnFinishState()
    {

    }

    private void PickDown()
    {

        var map = MapData.GetInstance; // �}�b�v���擾

        // �u���ʒu
        GridPos placementPos = owner.GetForwardGridPos(); // �O���̃O���b�h�ʒu

        // �^��ł���I�u�W�F�N�g���擾
        var carryingObject = owner.GetCarryingObject(); 

        // �����^�ԃI�u�W�F�N�g��u�����������s
        carryingObject.OnDrop(placementPos); 

       
        owner.SetCarryingObject(null); // �����^�ԃI�u�W�F�N�g�����
    }


}
