using UnityEngine;





public class PlayerStateMachine 
{

    // ���݂̃v���C���[�̏��
    private PlayerState m_currentState;
    // ���̃v���C���[�̏�Ԃ̗v��
    private PlayerStateID m_requestedStateID;

    // ���L��
    private Player m_owner;


    // �R���X�g���N�^
    public PlayerStateMachine(Player owner)
    {
        m_owner = owner;
        m_requestedStateID = PlayerStateID.NONE; // ������Ԃ͂Ȃ�
        m_currentState = null; // ������Ԃ�null
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateState()
    {
        // ��Ԃ̕ύX�v��������ꍇ
        if (m_requestedStateID != PlayerStateID.NONE )
        {
           // ��Ԃ�ύX����
           if (m_currentState != null)
            m_currentState.OnFinishState();

            // �ύX�v�����ꂽ��ԂɑJ��
            ChangeState(m_requestedStateID);
            m_requestedStateID = PlayerStateID.NONE; // ���Z�b�g

            // �V������Ԃ̊J�n�������Ăяo��
            m_currentState.OnStartState(); 
        }

        // ���݂̏�Ԃ�null�łȂ��ꍇ�AUpdateState���Ăяo��
        if (m_currentState != null)
        {
            // ���݂̏�Ԃ�UpdateState���Ăяo��
            m_currentState.OnUpdateState();
        }

    }

    public void FixedUpdateState()
    {
        // ���݂̏�Ԃ�null�łȂ��ꍇ�AFixedUpdateState���Ăяo��
        if (m_currentState != null)
        {
            // ���݂̏�Ԃ�FixedUpdateState���Ăяo��
            m_currentState.OnFixedUpdateState();
        }
    }

    /// <summary>
    /// �v���C���[�̏�Ԃ�ύX����v����ݒ�
    /// </summary>
    /// <param name="newStateID"></param>
    public void RequestStateChange(PlayerStateID newStateID)
    {
        // �V������Ԃ̗v����ݒ�
        m_requestedStateID = newStateID;
    }

    /// <summary>
    /// ��Ԃ̕ύX
    /// </summary>
    /// <param name="newStateID"></param>
    private void ChangeState(PlayerStateID newStateID)
    {
        // ���݂̏�Ԃ����݂���ꍇ�A�I���������Ăяo��
        if (m_currentState != null)
        {
            m_currentState.OnFinishState();
        }
        // �V������Ԃ��擾
        m_currentState = GetState(newStateID);

        // �V������Ԃ����݂���ꍇ�A�J�n�������Ăяo��
        if (m_currentState != null)
        {
            m_currentState.OnStartState();
        }
    }


    /// <summary>
    /// �v���C���[�̏�Ԃ��擾����
    /// </summary>
    /// <param name="stateID"></param>
    /// <returns></returns>
    private PlayerState GetState(PlayerStateID stateID)
    {
        switch (stateID)
        {
            case PlayerStateID.IDLE:
                return new IdleStatePlayer(m_owner);
            case PlayerStateID.WALK:
                return new WalkStatePlayer(m_owner);
            case PlayerStateID.PICK_UP:
                return new PickUpStatePlayer(m_owner);
            case PlayerStateID.PICK_DOWN:
                return new PickDownStatePlayer(m_owner);
            case PlayerStateID.KNIT:
                return new KnotStatePlayer(m_owner);
            case PlayerStateID.PUSH_BLOCK:
                return new PushBlockStatePlayer(m_owner);
            default:
                Debug.LogError("Unknown state ID: " + stateID);
                return null;
        }
    }
}
