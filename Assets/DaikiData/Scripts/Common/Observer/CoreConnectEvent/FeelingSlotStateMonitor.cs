using System.Collections.Generic;

/// <summary>
/// 核の状態を監視しているクラス シングルトン
/// </summary>
public class FeelingSlotStateMonitor
{

    private List<TerminusFeelingSlot> m_monitorObjects = new List<TerminusFeelingSlot>(); // 観察者リスト

    private static FeelingSlotStateMonitor s_instance;
    /// <summary>
    /// シングルトンインスタンスを取得
    /// </summary>
    public static FeelingSlotStateMonitor GetInstance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new FeelingSlotStateMonitor();
            }
            return s_instance;
        }
    }
  

    public void RegisterMonitorObject(TerminusFeelingSlot monitor)
    {
        // 既に登録されているオブザーバーは追加しない
        if (!m_monitorObjects.Contains(monitor))
        {
            m_monitorObjects.Add(monitor);
        }
    }

    public bool IsConnected(EmotionCurrent.Type coreType)
    {
        foreach (var monitor in m_monitorObjects)
        {
            if (monitor.emotionType == coreType)
            {
                if (monitor.IsConnected()) return true;// オブザーバーが感情タイプと一致する場合は接続されている
            }
        }

        return false; // 一致するオブザーバーがいない場合は接続されていない
    }

    /// <summary>
    /// オブザーバーを登録解除するメソッド
    /// </summary>
    /// <param name="monitor"></param>
    public void RemoveMonitor(TerminusFeelingSlot monitor)
    {
        // オブザーバーリストから削除
        if (m_monitorObjects.Contains(monitor))
        {
            m_monitorObjects.Remove(monitor);
        }
    }
}
