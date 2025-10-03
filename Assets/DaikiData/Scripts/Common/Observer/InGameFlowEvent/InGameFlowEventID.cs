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

    GAME_START_EFFECT_START,// ゲーム開始エフェクトが始まった
    GAME_START_EFFECT_END,  // ゲーム開始エフェクトが終わった

    GAME_PLAYING_START,     // ゲームプレイが開始された

    GAME_CLEAR,             // ゲームがクリアされた

    GAME_CLEAR_EFFECT_START, // ゲームクリアエフェクトが始まった
    GAME_CLEAR_EFFECT_END,   // ゲームクリアエフェクトが終わった

    GOING_GET_FEELING_PIECE_START, // 感情ピース取得シーケンスが始まった
    GOING_GET_FEELING_PIECE_END,   // 感情ピース取得シーケンスが終わった

    GAME_PLAYING_END,       // ゲームプレイが終了した

    GAME_PAUSE,             // ゲームが一時停止された
    GAME_RESUME,            // ゲームが再開された

}

