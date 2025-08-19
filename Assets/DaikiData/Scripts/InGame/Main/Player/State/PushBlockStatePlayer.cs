using UnityEngine;

public class PushBlockStatePlayer : PlayerState
{
    private AnimationEventHandler m_animationEventHandler; // �A�j���[�V�����C�x���g�n���h���[

    private readonly float TARGET_TIME = 1.0f; // �A�j���[�V�����̃^�[�Q�b�g����
    private float currentTime = 0.0f; // ���݂̎���

    private FeltBlock m_feltBlock;      // �����Ώۂ̃u���b�N
    private Vector3 m_endPosition;   // �u���b�N����������̖ڕW�ʒu
    private Vector3 m_startPosition;    // �u���b�N�������O�̊J�n�ʒu
    private float m_lerpValue = 0.0f; // ��Ԓl

    public PushBlockStatePlayer(Player owner) : base(owner)
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

        var map = MapData.GetInstance; // �}�b�v���擾
                                       // test
        owner.TryForwardObjectSetting();

        // �����Ώۂ̃u���b�N���擾
        m_feltBlock = GetPushBlock();
        if (m_feltBlock == null)
        {
            // �����Ώۂ̃u���b�N��������Ȃ��ꍇ�͑ҋ@��ԂɑJ��
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);
            return;
        }



        
        // �u���b�N�������O�̊J�n�ʒu��ݒ�
        var blockPos = m_feltBlock.GetComponent<StageBlock>().GetGridPos(); // �u���b�N�̃O���b�h�ʒu���擾


      

        m_startPosition = map.ConvertGridToWorldPos(blockPos );

        // �u���b�N����������̖ڕW�ʒu��ݒ�
        m_endPosition = map.ConvertGridToWorldPos(owner.GetForwardDirection() + blockPos);

        // ���C���[�̕ύX���t���O�����Z�b�g
        m_animationEventHandler.PlayAnimationBool("Push", "Normal", "Push"); // �u���A�j���[�V�������Đ�


        // �v���C���[�̑O���ɏ������ꂽ�ʒu���v�Z 
        Vector3 dephPos = map.GetCommonData().tileSize / 2.0f * -owner.GetForwardDirectionForGrid();
        // �v���C���[�̃O���b�h�ʒu���v�Z
        GridPos playerGridPos = blockPos - owner.GetForwardDirection();

        // �����̍��W��ݒ� // �u���b�N�̃O���b�h�ʒu�����[���h���W�ɕϊ����āA�v���C���[�̈ʒu��ݒ�
        owner.transform.position = map.ConvertGridToWorldPos(playerGridPos) + dephPos;
        owner.GetComponent<Rigidbody>().linearVelocity = Vector3.zero; // �v���C���[�̑��x�����Z�b�g

        // �t�F���g�u���b�N�̕���������
        owner.transform.LookAt(m_feltBlock.transform); // �u���b�N�̖ڕW�ʒu�������悤�ɐݒ�

        // �q�Ƃ��Đݒ�
        owner.transform.SetParent(m_feltBlock.transform);
    }

    /// <summary>
    /// ���s��Ԓ���Update�Ŗ��t���[���Ă΂��
    /// </summary>
    public override void OnUpdateState()
    {
        // ��������
        Push();

        // �A�j���[�V�����C�x���g�n���h���[�̍X�V
        m_animationEventHandler.OnUpdate();

        if (m_lerpValue >= 1.0f)
        {
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
        // �O�̂���
        m_feltBlock.transform.position = m_endPosition; // �v���C���[�̈ʒu��ڕW�ʒu�ɐݒ�
        owner.transform.SetParent(null); // �u���b�N�̐e������
                                         // �u���b�N����������
        m_feltBlock.RequestMove(owner.GetForwardDirection());

        m_animationEventHandler.StopAnimation(); // �A�j���[�V�������~


        //�t�F���g�u���b�N�����������Ƃ�ʒm
        GameInteractionEventMessenger.GetInstance.Notify(InteractionEvent.PUSH_FELTBLOCK); 
    }

    /// <summary>
    /// �u���b�N����������
    /// </summary>
    private void Push()
    {
        // ��������
        Vector3 newBlockPos = Vector3.Lerp(m_startPosition, m_endPosition, m_lerpValue);
        // ��Ԓl���X�V
        m_lerpValue = currentTime / TARGET_TIME;

        currentTime += Time.deltaTime; // ���݂̎��Ԃ��X�V

        // �u���b�N�̈ʒu���X�V
        m_feltBlock.transform.position = newBlockPos;


    }

    private FeltBlock GetPushBlock()
    {
       return owner.GetTargetObject()?.GetComponent<FeltBlock>();
    }

}
