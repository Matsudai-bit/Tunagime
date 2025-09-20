using UnityEngine;





public class PlayerStateMachine 
{

    // 現在のプレイヤーの状態
    private PlayerState m_currentState;
    // 次のプレイヤーの状態の要求
    private PlayerStateID m_requestedStateID;

    // 所有者
    private Player m_owner;


    // コンストラクタ
    public PlayerStateMachine(Player owner)
    {
        m_owner = owner;
        m_requestedStateID = PlayerStateID.NONE; // 初期状態はなし
        m_currentState = null; // 初期状態はnull
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateState()
    {
        // 状態の変更要求がある場合
        if (m_requestedStateID != PlayerStateID.NONE )
        {

            // 変更要求された状態に遷移
            ChangeState(m_requestedStateID);
            m_requestedStateID = PlayerStateID.NONE; // リセット

  
        }

        // 現在の状態がnullでない場合、UpdateStateを呼び出す
        if (m_currentState != null)
        {
            // 現在の状態のUpdateStateを呼び出す
            m_currentState.OnUpdateState();
        }

    }

    public void FixedUpdateState()
    {
        // 現在の状態がnullでない場合、FixedUpdateStateを呼び出す
        if (m_currentState != null)
        {
            // 現在の状態のFixedUpdateStateを呼び出す
            m_currentState.OnFixedUpdateState();
        }
    }

    /// <summary>
    /// プレイヤーの状態を変更する要求を設定
    /// </summary>
    /// <param name="newStateID"></param>
    public void RequestStateChange(PlayerStateID newStateID)
    {
        // 新しい状態の要求を設定
        m_requestedStateID = newStateID;
    }

    /// <summary>
    /// 状態の変更
    /// </summary>
    /// <param name="newStateID"></param>
    private void ChangeState(PlayerStateID newStateID)
    {
        // 現在の状態が存在する場合、終了処理を呼び出す
        if (m_currentState != null)
        {
            m_currentState.OnFinishState();
        }
        // 新しい状態を取得
        m_currentState = GetState(newStateID);

        // 新しい状態が存在する場合、開始処理を呼び出す
        if (m_currentState != null)
        {
            m_currentState.OnStartState();
        }
    }


    /// <summary>
    /// プレイヤーの状態を取得する
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
            case PlayerStateID.PUT_DOWN:
                return new PutDownStatePlayer(m_owner);
            case PlayerStateID.KNIT:
                return new KnitStatePlayer(m_owner);
            case PlayerStateID.UNKNIT:
                return new UnknitStatePlayer(m_owner);
            case PlayerStateID.PUSH_BLOCK:
                return new PushBlockStatePlayer(m_owner);
            case PlayerStateID.SLIPPER:
                return new SlipperStatePlayer(m_owner);
            default:
                Debug.LogError("Unknown state ID: " + stateID);
                return null;
        }
    }

    public PlayerStateID GetStateID()
    {
        if (m_currentState is IdleStatePlayer) return PlayerStateID.IDLE;
        if (m_currentState is WalkStatePlayer) return PlayerStateID.WALK;
        if (m_currentState is PickUpStatePlayer) return PlayerStateID.PICK_UP;
        if (m_currentState is PutDownStatePlayer) return PlayerStateID.PUT_DOWN;
        if (m_currentState is KnitStatePlayer) return PlayerStateID.KNIT;
        if (m_currentState is UnknitStatePlayer) return PlayerStateID.UNKNIT;
        if (m_currentState is PushBlockStatePlayer) return PlayerStateID.PUSH_BLOCK;
        if (m_currentState is SlipperStatePlayer) return PlayerStateID.SLIPPER;
        return PlayerStateID.NONE;
    }
}
