using UnityEngine;

public class PickUpStatePlayer : PlayerState
{

    private AnimationEventHandler m_animationEventHandler; // �A�j���[�V�����C�x���g�n���h���[


    public PickUpStatePlayer(Player owner) : base(owner)
    {
        // �A�j���[�V�����C�x���g�n���h���[��������
        m_animationEventHandler = new AnimationEventHandler(owner.GetAnimator());
    }
    /// <summary>
    /// ���s��Ԃ̊J�n���Ɉ�x�����Ă΂��
    /// </summary>
    public override void OnStartState()
    {
        // �ړ����~
        owner.StopMove();
        // �����グ�鏈�����J�n
        StartPickUp(); 

    }

    /// <summary>
    /// ���s��Ԓ���Update�Ŗ��t���[���Ă΂��
    /// </summary>
    public override void OnUpdateState()
    {
        // ���݂̃A�j���[�V�����̃n�b�V�����擾
        m_animationEventHandler.OnUpdate(); // �A�j���[�V�����C�x���g�n���h���[�̍X�V


        // �A�j���[�V�����̏I����҂�
        if (m_animationEventHandler.HasAnimationPlayed() && m_animationEventHandler.IsPlaying() == false)
        {
            // �A�j���[�V�������I�������玝���グ��I�u�W�F�N�g���E�����������s
            PickUp();
            // �ҋ@��ԂɑJ��
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);

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
        map.GetStageGridData().RemoveGridDataGameObject(stageBlock.GetGridPos()); 

        // �f�o�b�O���O�Ɏ����グ��I�u�W�F�N�g�̖��O���o��
        Debug.Log(gameObj.name); 

        // �����グ��I�u�W�F�N�g���A�N�e�B�u�ɂ���
        stageBlock.SetActive(false);

        // �����グ��
        carryingObject.OnPickUp();

        // �v���C���[�Ɏ����グ��I�u�W�F�N�g���Z�b�g
        owner.SetCarryingObject(carryingObject); 
    }


    /// <summary>
    /// �����グ�鏈�����J�n����
    /// </summary>
    private void StartPickUp()
    {
        // ���C���[�̕ύX���t���O�����Z�b�g
        m_animationEventHandler.PlayAnimationTrigger("PickUp", "Normal", "PickUp"); // �u���A�j���[�V�������Đ�

        // ���C���[�̃E�F�C�g��ύX���邽�߂̃R�[���o�b�N��ݒ�
        m_animationEventHandler.SetTargetTimeAction(0.5f, () => { owner.RequestTransitionLayerWeight("Carry", 1, 0.4f); }); // Carry���C���[�̃E�F�C�g��0�ɐݒ�
    }
}
