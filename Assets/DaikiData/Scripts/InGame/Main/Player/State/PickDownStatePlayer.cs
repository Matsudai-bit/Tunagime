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
        //owner.GetAnimator().SetTrigger("PickDown"); // �Ȗу{�[�����E���A�j���[�V�������g���K�[


        
    }

    /// <summary>
    /// ���s��Ԓ���Update�Ŗ��t���[���Ă΂��
    /// </summary>
    public override void OnUpdateState()
    {
        // �A�j���[�V�����̏I����҂�
        //if (owner.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        //{
            // �A�j���[�V�������I��������Ȗу{�[�����E�����������s
            PickDown();
            // �ҋ@��ԂɑJ��
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);


            // �Ȗу{�[���������Ă���ꍇ�A�A�j���[�V�������C���[��L����
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

        // �ł��߂��O���b�h�ʒu�̎擾
        var closestPos = map.GetClosestGridPos(owner.transform.position);
        owner.GetFluffBall().UpdatePosition(closestPos); // �Ȗу{�[���̈ʒu���X�V

        owner.GetFluffBall().SetActive(true); // �Ȗу{�[�����A�N�e�B�u�ɂ���

        // �O���b�h�f�[�^�ɖȖу{�[����z�u
        map.GetStageGridData().TryPlaceTileObject(closestPos, owner.GetFluffBall().GetComponent<GameObject>());
        owner.SetFluffBall(null); // �Ȗу{�[�������
    }


}
