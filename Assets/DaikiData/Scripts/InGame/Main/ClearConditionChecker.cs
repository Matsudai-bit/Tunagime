using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// クリア条件をチェックするクラス
/// </summary>
public class ClearConditionChecker : MonoBehaviour , IGameInteractionObserver
{
    [Header("クリア条件の設定")]
    [SerializeField]
    private List<TerminusFeelingSlot> terminusFeelingSlots = new List<TerminusFeelingSlot>(); // 終点にある型のリスト

    private bool m_isConnectionRejectionSlot = false; // 終点の拒絶の核が接続されているかどうか

    
    private bool m_isDelayClearCheck = false;// クリア判定を遅延させるかどうか

    bool m_isClear = false; // クリアしたかどうか

    private GameDirector m_gameDirector;

    private void Awake()
    {
        // GameDirectorのインスタンスを取得
        m_gameDirector = GetComponent<GameDirector>();
        if (m_gameDirector == null)
        {
            Debug.LogError("GameDirector instance not found in the scene.");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_isClear = false;
        // オブザーバーとして登録
        GameInteractionEventMessenger.GetInstance.RegisterObserver(this);

        m_isConnectionRejectionSlot = false; // 初期状態では接続されていない
        m_isDelayClearCheck = false;
    }

    void DelayedClearCheck()
    {
        // クリア条件のチェックを遅延させる場合は何もしない
        m_isDelayClearCheck = false;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void FixedUpdate()
    {

        // クリア条件のチェック
        if (CheckClearCondition())
        {
            Debug.Log("クリア条件を達成しました！");

            // クリア処理をここに追加
            if (m_gameDirector != null && !m_isClear)
            {
                InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.GAME_CLEAR); // ゲームクリアのイベントを通知
            }
            m_isClear = true;

        }

    }

    private bool CheckClearCondition()
    {
        // 終点の拒絶の核が接続されている場合はクリア条件を満たさない
        if (m_isConnectionRejectionSlot)
            return false;

        // クリア条件を満たしているかどうかをチェック
        foreach (var slot in terminusFeelingSlots)
        {
            if (!slot.IsConnected())
            {
                return false; // 1つでも繋がっていない場合はクリア条件未達成
            }
        }
        return true; // 全ての終点が繋がっている場合はクリア条件達成
    }

    /// <summary>
    /// 終点にある型を追加するメソッド
    /// </summary>
    /// <param name="slot"></param>
    public void AddTerminusFeelingSlot(TerminusFeelingSlot slot) // いつかインターフェースを貰う形にしたい
    {
        if (!terminusFeelingSlots.Contains(slot))
        {
            terminusFeelingSlots.Add(slot);
        }
    }

    public void OnEvent(InteractionEvent eventID)
    {
        // イベントIDに応じて処理を分岐
        switch (eventID)
        {
            case InteractionEvent.CONNECTED_REJECTION_SLOT:
                m_isConnectionRejectionSlot = true; // 終点の拒絶の核が接続された
                break;
            case InteractionEvent.DISCONNECTED_REJECTION_SLOT:
                m_isConnectionRejectionSlot = false; // 終点の拒絶の核が切断された
                break;
            case InteractionEvent.FLOWWING_AMIDAKUJI:
                m_isDelayClearCheck = true; // あみだを辿るイベントが発生した場合、クリア判定を遅延させる
                // 0.5秒後にクリア条件のチェックを行う
                Invoke("DelayedClearCheck", 0.5f);
                break;
            default:
                break;
        }
    }
    // 削除時
    private void OnDestroy()
    {
        // ゲームインタラクションイベントのオブザーバーを解除
        GameInteractionEventMessenger.GetInstance.RemoveObserver(this);
    }

    public int GetConnectedTerminusFeelingSlotCount()
    {
        int count = 0;
        foreach (var slot in terminusFeelingSlots)
        {
            if (slot.IsConnected())
            {
                count++;
            }
        }
        return count;
    }

    // 残りの未接続の終点の型の数を取得
    public int GetRemainingUnconnectedTerminusFeelingSlotCount()
    {
        int count = 0;
        foreach (var slot in terminusFeelingSlots)
        {
            if (!slot.IsConnected() && !(slot.emotionType == EmotionCurrent.Type.REJECTION))
            {
                count++;
            }
        }
        return count;
    }

}

