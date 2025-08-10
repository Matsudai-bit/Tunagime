using UnityEngine;

/// <summary>
/// ゲーム内のインタラクションイベントを表す列挙型
/// </summary>
public enum InteractionEvent
{
    // **** プレイヤーの行動に関するイベント ****
    PLAYER_START_KNIT, // プレイヤーが編み物を開始した
    PLAYER_END_KNIT,  // プレイヤーが編み物を終了した

    // **** あみだの状態に関するイベント ****
    CHANGED_AMIDAKUJI, // 編み物の状態が変化した
    FLOWWING_AMIDAKUJI, // 編み物が流れた


    // **** ギミックに関するイベント ****
    CONNECTED_REJECTION_SLOT, // 拒絶の核が接続された
    DISCONNECTED_REJECTION_SLOT, // 拒絶の核が切断された


}
