using UnityEngine;
/// <summary>
/// 拒絶の核の接続状態を監視し、状態が変化した際にイベントを発行するクラス
/// </summary>

public class TerminusFeelingSlotRefection : MonoBehaviour
{
    private TerminusFeelingSlot m_terminusFeelingSlot; // 終点の想いの型

    private bool m_isConnected = false; // 接続状態を示すフラグ

    private void Awake()
    {
        m_terminusFeelingSlot = GetComponent<TerminusFeelingSlot>();
        if (m_terminusFeelingSlot == null)
        {
            Debug.LogError("TerminusFeelingSlot component is missing on the GameObject.");
        }
    }

    private void Update()
    {
        if (m_terminusFeelingSlot.IsConnected())
        {
            // 接続状態が変わった場合にのみ通知
            if (m_isConnected == false)
                GameInteractionEventMessenger.GetInstance.Notify(InteractionEvent.CONNECTED_REJECTION_SLOT);

            m_isConnected = true;
        }
        else
        {
            // 接続状態が変わった場合にのみ通知
            if (m_isConnected == true)
                GameInteractionEventMessenger.GetInstance.Notify(InteractionEvent.DISCONNECTED_REJECTION_SLOT);
            m_isConnected = false;
        }
    }

}
