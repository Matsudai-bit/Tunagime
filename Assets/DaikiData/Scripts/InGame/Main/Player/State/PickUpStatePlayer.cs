using UnityEngine;

public class PickUpStatePlayer : PlayerState
{
    public PickUpStatePlayer(Player owner) : base(owner)
    {

    }
    /// <summary>
    /// ���s��Ԃ̊J�n���Ɉ�x�����Ă΂��
    /// </summary>
    public override void OnStartState()
    {
        owner.GetAnimator().SetTrigger("PickUp"); // �Ȗу{�[�����E���A�j���[�V�������g���K�[


        
    }

    /// <summary>
    /// ���s��Ԓ���Update�Ŗ��t���[���Ă΂��
    /// </summary>
    public override void OnUpdateState()
    {
        // �A�j���[�V�����̏I����҂�
        if (owner.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            // �A�j���[�V�������I��������Ȗу{�[�����E�����������s
            PickUp();
            // �ҋ@��ԂɑJ��
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);

            // �Ȗу{�[���������Ă���ꍇ�A�A�j���[�V�������C���[��L����
            owner.GetAnimator().SetLayerWeight(owner.GetAnimator().GetLayerIndex("TakeFluffBall"), 1);
        }

     
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

    private void PickUp()
    {

        var map = MapData.GetInstance; // �}�b�v���擾

        var checkPos = owner.GetForwardGridPos(); // �`�F�b�N����O���b�h�ʒu

        TileObject tileObject = map.GetStageGridData().GetTileData[checkPos.y, checkPos.x].tileObject;


        GameObject gameObj = tileObject.gameObject;
        var fluffBall = gameObj.GetComponent<StageBlock>(); // �Ȗу{�[�����擾
        map.GetStageGridData().RemoveGridData(fluffBall.GetGridPos()); // �O���b�h����Ȗу{�[�����폜
        Debug.Log(gameObj.name); // �f�o�b�O���O�ɖȖу{�[���̖��O���o��

        fluffBall.SetActive(false); // �Ȗу{�[�����A�N�e�B�u�ɂ���

        owner.SetFluffBall(fluffBall); // �v���C���[�ɖȖу{�[�����Z�b�g
    }


}
