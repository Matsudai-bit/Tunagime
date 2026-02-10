using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム内のフローイベントを管理するクラス シングルトン
/// </summary>
public class UndoTransactionMessenger
{
    private List<IUndoTransactionObserver> m_observers = new List<IUndoTransactionObserver>(); // オブザーバーのリスト

    private static UndoTransactionMessenger s_instance; // シングルトンインスタンス

    
    private int m_currentTransactionID = 0; // 現在のトランザクションID
    private int m_nextTransactionID = 1; // 次のトランザクションID

    public int CurrentTransactionID
    {
        get { return m_currentTransactionID; }
    }


    /// <summary>
    /// 履歴
    /// </summary>
    struct TransactionData
    {
        int instanceID;
    }

    public UndoTransactionMessenger()
    {
        m_currentTransactionID = 0;
        m_nextTransactionID = 1;
    }
       

    /// <summary>
    /// シングルトンインスタンスを取得
    /// </summary>
    public static UndoTransactionMessenger GetInstance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new UndoTransactionMessenger();
            }
            return s_instance;
        }
    }

    /// <summary>
    /// オブザーバーを登録するメソッド
    /// </summary>
    /// <param name="observer"></param>
    public void RegisterObserver(IUndoTransactionObserver observer)
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
    public void NotifyUndo()
    {
        if (m_nextTransactionID > 0)
        {
            int messageID = Mathf.Max(0, --m_nextTransactionID);
            foreach (var observer in m_observers)
            {
                observer.OnEvent(messageID);
            }
        }

    }

    /// <summary>
    /// オブザーバーを登録解除するメソッド
    /// </summary>
    /// <param name="observer"></param>
    public void RemoveObserver(IUndoTransactionObserver observer)
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

    public void BeginTransaction()
    {
        m_currentTransactionID = m_nextTransactionID;
        m_nextTransactionID++;
    }

    public void EndTransaction()
    {
        m_currentTransactionID = -1;
    }


}
