using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム内のフローイベントを管理するクラス シングルトン
/// </summary>
public class InGameFlowEventMessenger 
{
    private List<IInGameFlowEventObserver> m_observers = new List<IInGameFlowEventObserver>(); // オブザーバーのリスト

    private static InGameFlowEventMessenger s_instance; // シングルトンインスタンス

    /// <summary>
    /// シングルトンインスタンスを取得
    /// </summary>
    public static InGameFlowEventMessenger GetInstance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new InGameFlowEventMessenger();
            }
            return s_instance;
        }
    }

    /// <summary>
    /// オブザーバーを登録するメソッド
    /// </summary>
    /// <param name="observer"></param>
    public void RegisterObserver(IInGameFlowEventObserver observer)
    {
        // 既に登録されているオブザーバーは追加しない
        if (!m_observers.Contains(observer))
        {
            m_observers.Add(observer);
        }
    }

    /// <summary>
    /// イベントを通知するメソッド
    /// </summary>
    /// <param name="eventMessage">イベントメッセージ</param>
    public void Notify(InGameFlowEventID eventMessage)
    {
        foreach (var observer in m_observers)
        {
            observer.OnEvent(eventMessage);
        }
    }

    /// <summary>
    /// オブザーバーを登録解除するメソッド
    /// </summary>
    /// <param name="observer"></param>
    public void RemoveObserver(IInGameFlowEventObserver observer)
    {
        // オブザーバーリストから削除
        if (m_observers.Contains(observer))
        {
            m_observers.Remove(observer);
        }
    }

    /// <summary>
    /// 全てのオブザーバーを登録解除するメソッド
    /// </summary>
    public void RemoveAllObserver()
    {
        m_observers.Clear();
    }
}
