using UnityEngine;

/// <summary>
/// ゲーム内のインタラクションイベントを監視するためのインターフェース
/// </summary>
public  interface IGameInteractionObserver 
{
    /// <summary>
    /// ゲーム内のインタラクションイベントを受信するメソッド
    /// </summary>
    /// <param name="eventID">イベントメッセージ</param>
    public void OnEvent(InteractionEvent eventID);
}
