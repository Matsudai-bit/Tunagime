using UnityEngine;

public class PushBlockStatePlayer : PlayerState
{
    public PushBlockStatePlayer(Player owner) : base(owner)
    {

    }
    /// <summary>
    /// ���s��Ԃ̊J�n���Ɉ�x�����Ă΂��
    /// </summary>
    public override void OnStartState()
    {
   
    }

    /// <summary>
    /// ���s��Ԓ���Update�Ŗ��t���[���Ă΂��
    /// </summary>
    public override void OnUpdateState()
    {
       

    }
    /// <summary>
    /// ���s��Ԓ���FixedUpdate�ŕ������Z�t���[�����ƂɌĂ΂��
    /// </summary>
    public override void OnFixedUpdateState()
    {

        // �u���b�N����������
        Push(); 

        // ���s��Ԃ���ҋ@��ԂɑJ��
        owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);
        
    }

    /// <summary>
    /// ���s��Ԃ̏I�����Ɉ�x�����Ă΂��
    /// </summary>
    public override void OnFinishState()
    {
        // ���s��Ԃ̏I�����ɃA�j���[�V���������Z�b�g
        owner.GetAnimator().SetBool("Walk", false);
    }

    /// <summary>
    /// �u���b�N����������
    /// </summary>
    private void Push()
    {
        // �}�b�v�̎擾
        var map = MapData.GetInstance;
        // ���C�̔�΂�������ݒ�
        float rayDistance = (float)(map.GetCommonData().width) / 2.0f;

        // ���C�L���X�g���g�p���āA�v���C���[�̑O���ɂ���I�u�W�F�N�g�����o
        RaycastHit hit;
        if (Physics.Raycast(new Ray(owner.transform.position, owner.transform.forward), out hit, rayDistance))
        {
            // ���C�����������I�u�W�F�N�g���X�e�[�W�u���b�N�ł��邩�`�F�b�N
            if (hit.collider.gameObject?.GetComponent<FeltBlock>() != null)
            {
                var feltBlock = hit.collider.gameObject.GetComponent<FeltBlock>();

                // �u���b�N����������
                feltBlock.Move(owner.GetForwardDirection());
            }
        }
    }

}
