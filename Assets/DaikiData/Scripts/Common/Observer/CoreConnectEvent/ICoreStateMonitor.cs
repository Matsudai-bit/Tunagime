using UnityEngine;

/// <summary>
/// 核の状態を監視するためのインターフェース
/// </summary>
public interface ICoreStateMonitor
{
    /// <summary>
    /// ゲーム内のインタラクションイベントを受信するメソッド
    /// </summary>
    /// <param name="emotionCurrent">想いの種類</param>
    public void OnConnectEvent(EmotionCurrent emotionCurrent);
}
