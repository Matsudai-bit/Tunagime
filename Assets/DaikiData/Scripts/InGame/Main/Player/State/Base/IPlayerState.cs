using UnityEngine;

public enum PlayerStateID
{
    NONE,     // 無効な状態
    IDLE,       // 待機
    WALK,       // 歩行
    PICK_UP,    // 拾う
    PUT_DOWN,  // 下ろす
    KNIT,       // 編む
    UNKNIT,     // 解く
    PUSH_BLOCK, // ブロックを押す

}


/// <summary>
/// プレイヤーの状態を表す抽象クラス
/// </summary>
public abstract class PlayerState 
{
    // プレイヤー
    private Player m_owner; // 所有者のプレイヤー

    public abstract void OnStartState();    // ステート開始時に一度だけ呼ばれる
    public abstract void OnUpdateState();     // ステート中のUpdateで毎フレーム呼ばれる
    public abstract void OnFixedUpdateState(); // ステート中のFixedUpdateで物理演算フレームごとに呼ばれる
    public abstract void OnFinishState();   // ステート終了時に一度だけ呼ばれる


    /// <summary>
    /// プレイヤーの状態を初期化するコンストラクタ
    /// </summary>
    /// <param name="owner"></param>
    public PlayerState(Player owner)
    {
        m_owner = owner;

    }

    public Player owner
    {
        get { return m_owner; }
    }

}