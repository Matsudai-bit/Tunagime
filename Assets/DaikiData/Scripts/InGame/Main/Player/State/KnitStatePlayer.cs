using UnityEngine;

/// <summary>
/// ������Ԃ̃v���C���[
/// </summary>
public class KnitStatePlayer : PlayerState
{

    private AnimationEventHandler m_animationEventHandler; // �A�j���[�V�����C�x���g�n���h���[

    public KnitStatePlayer(Player owner) : base(owner)
    {
        // �A�j���[�V�����C�x���g�n���h���[��������
        m_animationEventHandler = new AnimationEventHandler(owner.GetAnimator());
    }
    /// <summary>
    /// ���s��Ԃ̊J�n���Ɉ�x�����Ă΂��
    /// </summary>
    public override void OnStartState()
    {
        // ���C���[�̕ύX���t���O�����Z�b�g
        m_animationEventHandler.PlayAnimationTrigger("Knit", "Carry", "Knit"); // �u���A�j���[�V�������Đ�

        // ���C���[�̃E�F�C�g��ύX���邽�߂̃R�[���o�b�N��ݒ�
        // Carry���C���[�̃E�F�C�g��0�ɐݒ�
        m_animationEventHandler.SetTargetTimeAction(0.9f, () => { owner.RequestTransitionLayerWeight("Carry", 0, 0.8f); });
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

            if (owner.CanKnit())
                // �A�j���[�V�������I��������҂ޏ��������s
                FinishKnit();
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

    /// <summary>
    /// �҂�
    /// </summary>
    private void FinishKnit()
    {
        // �u���ʒu
        GridPos knottingPos = owner.GetForwardGridPos(); // �O���̃O���b�h�ʒu
        // �A�~�_���̐���
        var generateAmida = AmidaTubeGenerator.GetInstance.GenerateAmidaBridge(knottingPos);


        owner.DropCarryingObject();


    }




}
