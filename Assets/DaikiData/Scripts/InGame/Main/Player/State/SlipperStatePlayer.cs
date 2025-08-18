using UnityEngine;

/// <summary>
/// ������
/// </summary>
public class SlipperStatePlayer : PlayerState
{
    public GridPos m_directionBaseGrid; // �O���b�h��̃X���C�h���������

    public SlipperStatePlayer(Player owner) : base(owner)
    {

    }
    /// <summary>
    /// �����Ԃ̊J�n���Ɉ�x�����Ă΂��
    /// </summary>
    public override void OnStartState()
    {
        // �����Ԃ̊J�n���ɃA�j���[�V������ݒ�
        owner.GetAnimator().SetBool("Slide", true);

        // �v���C���[�̈ړ������̎擾
        Vector3 velocityNormal = owner.GetRigidbody().linearVelocity.normalized;

        // �v���C���[�̈ړ��������O���b�h��ɕϊ�
        m_directionBaseGrid = new GridPos(
            Mathf.RoundToInt(velocityNormal.x),
            Mathf.RoundToInt(-velocityNormal.z)
        );
    }

    /// <summary>
    /// �����Ԓ���Update�Ŗ��t���[���Ă΂��
    /// </summary>
    public override void OnUpdateState()
    {
        //// �����^�ԃI�u�W�F�N�g���E�����������݂�
        //owner.TryPickUp();
        //// �����^�ԃI�u�W�F�N�g��u�����������݂�
        //owner.TryPutDown();
      
        //if (owner.GetCarryingObject() != null)
        //{
        //    // test
        //    owner.TryForwardFloorSetting();
        //    // �҂ޏ��������݂�
        //    owner.TryKnit();
        //}
        //else
        //{
        //    // test
        //    owner.TryForwardObjectSetting();

        //    owner.TryPushBlock();

        //    owner.TryUnknit();
        //}

    }
    /// <summary>
    /// �����Ԓ���FixedUpdate�ŕ������Z�t���[�����ƂɌĂ΂��
    /// </summary>
    public override void OnFixedUpdateState()
    {
        Slide();

        //// �v���C���[�̈ړ�����
        //if (owner.IsMoving())
        //{
        //    // �ړ�����
        //    owner.Move();
        //}
        //else
        //{
        //    // �����Ԃ���ҋ@��ԂɑJ��
        //    owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);
        //    // �ړ����~
        //    owner.StopMove();
        //}
    }

    /// <summary>
    /// �����Ԃ̏I�����Ɉ�x�����Ă΂��
    /// </summary>
    public override void OnFinishState()
    {
        // �����Ԃ̏I�����ɃA�j���[�V���������Z�b�g
        owner.GetAnimator().SetBool("Slide", false);

        // �ړ����~
        owner.StopMove();
    }


    /// <summary>
    /// �v���C���[���w�肵�������ɃX���C�h������B
    /// </summary>
    /// <param name="direction">  </param>
    public void Slide()
    {
        Vector3 direction = new Vector3(m_directionBaseGrid.x, 0, -m_directionBaseGrid.y);

        // �v���C���[�̈ʒu���X�V
        owner.transform.position += direction * 2.0f * Time.deltaTime;
    }


}
