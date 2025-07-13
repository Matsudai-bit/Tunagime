using UnityEngine;

/// <summary>
/// 電気に関するクラス
/// </summary>
public class Electric : MonoBehaviour
{
    /// <summary>
    /// 流れている電気の種類
    /// </summary>
    [System.Serializable]
    public enum ElectricFlowType
    {
        NO_FLOW,
        NORMAL,
        RED
        
    }

    ElectricFlowType m_electricFlowType;    // 電気の種類

    /// <summary>
    /// 電気の流れている種類の取得
    /// </summary>
    /// <returns>電気の種類</returns>
    public ElectricFlowType  GetElectricFlowType()
    {
        return m_electricFlowType;
    }

    /// <summary>
    /// 電気の流れている種類の設定
    /// </summary>
    /// <returns>電気の種類</returns>
    public void SetElectricFlowType(ElectricFlowType type)
    {
       m_electricFlowType = type;
    }

}
