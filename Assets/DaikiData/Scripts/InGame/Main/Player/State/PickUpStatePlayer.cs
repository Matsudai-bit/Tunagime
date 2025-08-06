using UnityEngine;

public class PickUpStatePlayer : PlayerState
{
    private int m_pickUpAnimationHash;

    public PickUpStatePlayer(Player owner) : base(owner)
    {

    }
    /// <summary>
    /// ���s��Ԃ̊J�n���Ɉ�x�����Ă΂��
    /// </summary>
    public override void OnStartState()
    {
        owner.GetAnimator().SetTrigger("PickUp"); // �����グ��I�u�W�F�N�g���E���A�j���[�V�������g���K�[

        m_pickUpAnimationHash = Animator.StringToHash("Normal.PickUp"); // �A�j���[�V�����̃n�b�V�����擾

    }

    /// <summary>
    /// ���s��Ԓ���Update�Ŗ��t���[���Ă΂��
    /// </summary>
    public override void OnUpdateState()
    {
        // ���݂̃A�j���[�V�����̃n�b�V�����擾
        int currentHash = owner.GetAnimator().GetCurrentAnimatorStateInfo(0).fullPathHash;

        // �A�j���[�V�����̏I����҂�
        if (currentHash == m_pickUpAnimationHash &&
            owner.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            // �A�j���[�V�������I�������玝���グ��I�u�W�F�N�g���E�����������s
            PickUp();
            // �ҋ@��ԂɑJ��
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);

            // �����グ��I�u�W�F�N�g�������Ă���ꍇ�A�A�j���[�V�������C���[��L����
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
        var carryingObject = gameObj.GetComponent<Carryable>(); // �����グ��I�u�W�F�N�g���擾
        if (carryingObject == null)
        {
            Debug.LogError("Carryable component not found on the object.");
            return; // �����グ��I�u�W�F�N�g��������Ȃ��ꍇ�͏����𒆒f
        }
        // �����グ��I�u�W�F�N�g�̃O���b�h�ʒu���擾
        var stageBlock = carryingObject.stageBlock;

        // �O���b�h���玝���グ��I�u�W�F�N�g���폜
        map.GetStageGridData().RemoveGridData(stageBlock.GetGridPos()); 

        // �f�o�b�O���O�Ɏ����グ��I�u�W�F�N�g�̖��O���o��
        Debug.Log(gameObj.name); 

        // �����グ��I�u�W�F�N�g���A�N�e�B�u�ɂ���
        stageBlock.SetActive(false);

        // �����グ��
        carryingObject.OnPickUp();

        // �v���C���[�Ɏ����グ��I�u�W�F�N�g���Z�b�g
        owner.SetCarryingObject(carryingObject); 
    }


}
