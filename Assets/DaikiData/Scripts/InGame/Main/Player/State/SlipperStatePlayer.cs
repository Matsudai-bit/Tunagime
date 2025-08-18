using UnityEngine;

/// <summary>
/// ������
/// </summary>
public class SlipperStatePlayer : PlayerState
{
    public GridPos m_directionBaseGrid; // �O���b�h��̃X���C�h���������

    private readonly float SLIDE_SPEED = 2.0f; // �X���C�h���x 1�b�ɓ�����

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
        Vector3 velocityNormal = owner.GetPreviousMoveVelocity();
        velocityNormal.Normalize();
        m_directionBaseGrid = (Mathf.Abs(velocityNormal.x) > Mathf.Abs(velocityNormal.z))
          ? new GridPos((int)Mathf.Round(velocityNormal.x), 0)
          : new GridPos(0, -(int)Mathf.Round(velocityNormal.z));

        owner.StopMove(); // �����ړ����~

        Debug.Log(m_directionBaseGrid.x + "," + m_directionBaseGrid.y);
    }

    /// <summary>
    /// �����Ԓ���Update�Ŗ��t���[���Ă΂��
    /// </summary>
    public override void OnUpdateState()
    {
        if (CanFinish())
        {
            // �����Ԃ���ҋ@��ԂɑJ��
            owner.GetStateMachine().RequestStateChange(PlayerStateID.IDLE);
         
        }
    }
    /// �����Ԓ���FixedUpdate�ŕ������Z�t���[�����ƂɌĂ΂��
    /// </summary>
    public override void OnFixedUpdateState()
    {
        Slide();
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
        owner.transform.position += direction * SLIDE_SPEED * Time.deltaTime;
    }

    /// <summary>
    /// �����Ԃ��I���ł��邩�ǂ������`�F�b�N����B
    /// </summary>
    /// <returns></returns>
    public bool CanFinish()
    {
        // **** ���̎�ނŃ`�F�b�N ****
        var gridPos = owner.GetGridPosition();

        var stageGrid = MapData.GetInstance.GetStageGridData();

        var currentTileFloor = stageGrid.GetFloorObject(gridPos);

        var satainFloorOfCurrentTile = currentTileFloor?.GetComponent<SatinFloor>();
        if (!satainFloorOfCurrentTile)
        {
            return true;
        }

        // **** �O��3�����̃��C�ɓ��������I�u�W�F�N�g�Ń`�F�b�N ****
        RaycastHit[] hits = new RaycastHit[3];
        // �O��3�����Ƀ��C���΂��āA���������I�u�W�F�N�g���擾
        for (int i = 0; i < 3; i++)

        {
            int offset = i - 1; // -1, 0, 1 �̃I�t�Z�b�g���g�p


            Vector3 direction = new Vector3(m_directionBaseGrid.x, 0, -m_directionBaseGrid.y);
            direction.Normalize();

            direction = Quaternion.Euler(0, offset * 45, 0) * direction; // 90�x����]

            Debug.DrawRay(owner.transform.position, direction, Color.red, 0.5f); // �f�o�b�O�p�̃��C�\��

            if (Physics.Raycast(owner.transform.position, direction, out hits[i], 0.5f))
            {
                // ���������I�u�W�F�N�g�̃^�O���`�F�b�N
                if (hits[i].collider?.gameObject.GetComponent<StageBlock>())
                {
                    return true; // �����Ԃ��I���ł���
                }
            }
        }


        return false;
    }


}
