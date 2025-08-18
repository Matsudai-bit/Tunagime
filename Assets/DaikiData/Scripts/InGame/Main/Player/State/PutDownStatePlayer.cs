using UnityEngine;

public class PutDownStatePlayer : PlayerState
{

    private AnimationEventHandler m_animationEventHandler; // �A�j���[�V�����C�x���g�n���h���[

    public PutDownStatePlayer(Player owner) : base(owner)
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
        StartPutDown();

      

    }

    /// <summary>
    /// ���s��Ԓ���Update�Ŗ��t���[���Ă΂��
    /// </summary>
    public override void OnUpdateState()
    {
    
        m_animationEventHandler.OnUpdate(); // �A�j���[�V�����C�x���g�n���h���[�̍X�V


        // �A�j���[�V�����̏I����҂�
        if ( m_animationEventHandler.HasAnimationPlayed() && m_animationEventHandler.IsPlaying() == false)
        {
            // �A�j���[�V�������I�������玝���^�ԃI�u�W�F�N�g���E�����������s
            ApplyPutDown();
            // �ҋ@��ԂɑJ��
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);


         
        }


    }


    /// <summary>
    /// �����^�ԃI�u�W�F�N�g��u���������J�n����
    /// </summary>
    private void StartPutDown()
    {
        // ���C���[�̕ύX���t���O�����Z�b�g
        m_animationEventHandler.PlayAnimationTrigger("PutDown", "Carry", "PutDown"); // �u���A�j���[�V�������Đ�

        // ���C���[�̃E�F�C�g��ύX���邽�߂̃R�[���o�b�N��ݒ�
        m_animationEventHandler.SetTargetTimeAction(0.7f, () => { owner.RequestTransitionLayerWeight("Carry", 0, 0.4f); }); // Carry���C���[�̃E�F�C�g��0�ɐݒ�
    }

    /// �����^�ԃI�u�W�F�N�g��u�����������s����
    /// </summary>
    private void ApplyPutDown()
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



}
