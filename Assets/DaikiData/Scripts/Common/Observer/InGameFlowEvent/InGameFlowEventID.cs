using UnityEngine;

/// <summary>
/// ゲーム内のフローイベントを表す列挙型
/// </summary>
public enum InGameFlowEventID
{
    ZOOM_OUT_PLAYER_START,  // ゲームが開始される前のズームアウトが始まった
    ZOOM_OUT_PLAYER_END,    // ゲームが開始される前のズームアウトが終わった

    INTRO_SEQUENCE_START,   // ゲームのイントロシーケンスが開始された
    INTRO_SEQUENCE_END,     // ゲームのイントロシーケンスが終了した

    GAME_START,             // ゲームが開始された
    GAME_END,               // ゲームが終了された

    GAME_PAUSE,             // ゲームが一時停止された
    GAME_RESUME,            // ゲームが再開された

}

