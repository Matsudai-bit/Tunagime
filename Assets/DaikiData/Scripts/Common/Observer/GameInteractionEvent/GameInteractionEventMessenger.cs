using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ゲーム内のインタラクションイベントを管理するクラス シングルトン
/// </summary>
public class GameInteractionEventMessenger 
{
    
    private List<IGameInteractionObserver> m_observers = new List<IGameInteractionObserver>(); // オブザーバーのリスト

    private static GameInteractionEventMessenger s_instance;
    /// <summary>
    /// シングルトンインスタンスを取得
    /// </summary>
    public static GameInteractionEventMessenger GetInstance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new GameInteractionEventMessenger();
            }
            return s_instance;
        }
    }
    private GameInteractionEventMessenger() { }

    public void RegisterObserver(IGameInteractionObserver observer)
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
    public void Notify(InteractionEvent eventMessage)
    {
        foreach (var observer in m_observers)
        {
            observer.OnEvent(eventMessage);
        }
    }
}
