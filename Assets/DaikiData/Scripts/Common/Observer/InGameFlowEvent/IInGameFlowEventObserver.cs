using UnityEngine;

/// <summary>
/// ゲーム内のフローイベントのインターフェース
/// </summary>
public interface IInGameFlowEventObserver
{
    /// <summary>
    /// ゲーム内のインタラクションイベントを受信するメソッド
    /// </summary>
    /// <param name="eventID">イベントメッセージ</param>
    public void OnEvent(InGameFlowEventID eventID);
}
