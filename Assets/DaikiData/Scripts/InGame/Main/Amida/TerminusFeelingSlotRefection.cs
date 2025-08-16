using UnityEngine;
/// <summary>
/// ���傼���̊j
/// </summary>

public class TerminusFeelingSlotRefection : MonoBehaviour
{
    private TerminusFeelingSlot m_terminusFeelingSlot; // �I�_�̑z���̌^

    private bool m_isConnected = false; // �ڑ���Ԃ������t���O

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
            // �ڑ���Ԃ��ς�����ꍇ�ɂ̂ݒʒm
            if (m_isConnected == false)
                GameInteractionEventMessenger.GetInstance.Notify(InteractionEvent.CONNECTED_REJECTION_SLOT);

            m_isConnected = true;
        }
        else
        {
            // �ڑ���Ԃ��ς�����ꍇ�ɂ̂ݒʒm
            if (m_isConnected == true)
                GameInteractionEventMessenger.GetInstance.Notify(InteractionEvent.DISCONNECTED_REJECTION_SLOT);
            m_isConnected = false;
        }
    }

}
