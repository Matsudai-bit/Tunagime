using UnityEngine;

/// <summary>
/// 履歴から元に戻すインターフェース
/// </summary>
public interface IUndoTransactionObserver
{
    /// <summary>
    /// ゲーム内のインタラクションイベントを受信するメソッド
    /// </summary>
    /// <param name="eventID">イベントメッセージ</param>
    public void OnEvent(int transactionID);
}
