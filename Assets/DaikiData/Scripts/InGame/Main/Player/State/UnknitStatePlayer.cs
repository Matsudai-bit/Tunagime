using UnityEngine;

public class UnknitStatePlayer : PlayerState
{

    private AnimationEventHandler m_animationEventHandler; // �A�j���[�V�����C�x���g�n���h���[

    public UnknitStatePlayer(Player owner) : base(owner)
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
        // ���C���[�̕ύX���t���O�����Z�b�g
        m_animationEventHandler.PlayAnimationTrigger("Unknit", "Normal", "Unknit"); // �����A�j���[�V�������Đ�
                                                                                    // ���C���[�̃E�F�C�g��ύX���邽�߂̃R�[���o�b�N��ݒ�
        m_animationEventHandler.SetTargetTimeAction(0.7f, () => { owner.RequestTransitionLayerWeight("Carry", 1, 0.6f); }); // Carry���C���[�̃E�F�C�g��0�ɐݒ�


    }

    /// <summary>
    /// ���s��Ԓ���Update�Ŗ��t���[���Ă΂��
    /// </summary>
    public override void OnUpdateState()
    {

        // �A�j���[�V�����C�x���g�n���h���[�̍X�V
        m_animationEventHandler.OnUpdate();

        if (m_animationEventHandler.HasAnimationPlayed() && m_animationEventHandler.IsPlaying() == false)
        {

            // �A�j���[�V�������I��������҂ޏ��������s
            FinishUnknit();
       

            // �ю����܂𐶐�
            var stageObjectFactory = StageObjectFactory.GetInstance();
            // �O���̂��݂��Ɏ擾
            var frontAmidaPos = owner.GetForwardGridPos(); ;

            var fluffBallObj = stageObjectFactory.GenerateFluffBall(null, frontAmidaPos);


            var carryingObject = fluffBallObj.GetComponent<Carryable>(); // �����グ��I�u�W�F�N�g���擾
            if (carryingObject == null)
            {
                Debug.LogError("Carryable component not found on the object.");
                return; // �����グ��I�u�W�F�N�g��������Ȃ��ꍇ�͏����𒆒f
            }

            // �����グ��I�u�W�F�N�g���A�N�e�B�u�ɂ���
            fluffBallObj.SetActive(false);

            // �����グ��
            carryingObject.OnPickUp();

            // �v���C���[�Ɏ����グ��I�u�W�F�N�g���Z�b�g
            owner.SetCarryingObject(carryingObject);

            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE); // �����グ���ԂɑJ��

            // �ύX��ʒm
            GameInteractionEventMessenger.GetInstance.Notify(InteractionEvent.CHANGED_AMIDAKUJI);
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

    /// <summary>
    /// �҂�
    /// </summary>
    private void FinishUnknit()
    {
        var stageGridData = MapData.GetInstance.GetStageGridData();

        // �O���̂��݂��Ɏ擾
        var frontAmidaPos = owner.GetForwardGridPos(); ;

        stageGridData.RemoveAmidaTube(frontAmidaPos); // �O���̂��݂����폜

        // �㉺�̂��݂��̏�Ԃ��Đݒ�
        var upAmidaPos = frontAmidaPos + GridPos.UP;
        var downAmidaPos = frontAmidaPos + GridPos.DOWN;

        stageGridData.GetAmidaTube(upAmidaPos).RequestChangedState(AmidaTube.State.NORMAL);
        stageGridData.GetAmidaTube(downAmidaPos).RequestChangedState(AmidaTube.State.NORMAL);

        
    }




}
