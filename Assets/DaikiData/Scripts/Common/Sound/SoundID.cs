using UnityEngine;

public enum SoundID
{
    // ===================================== BGM =====================================

    // **** タイトル ****
    BGM_TITLE,
    BGM_SELECT,

    // **** インゲーム ****
    BGM_W_1,
    BGM_W_2, 
    BGM_W_3,
    BGM_W_4,
    BGM_W_5,

    BGM_STORY,  // ストーリーモードBGM

    // ===================================== SE =====================================

    // **** UI ****
    SE_UI_BUTTON_BACK, // 戻るボタン
    SE_UI_BUTTON_PUSH, // 決定ボタン
    SE_UI_BUTTON_MOVE, // カーソル移動
    SE_UI_BUTTON_GAMESTART_PUSH, // ゲームスタートボタンを押したときの音

    // **** プレイヤー ****
    SE_PLAYER_WALK,     // プレイヤー歩行音
    SE_PLAYER_KNIT,     // 編み音
    SE_PLAYER_UNKNIT,  // 解き音
    SE_PLAYER_PUSHBLOCK,     // 押す音
    SE_PLAYER_SLIDE,     // 滑る音/

    // **** ポーズ画面 ****
    SE_PAUSE_OPEN,      // ポーズ画面オープン音
    SE_PAUSE_CLOSE,     // ポーズ画面クローズ音

    // **** インゲーム ****
    SE_INGAME_CONNECT_REJECTION_SLOT,   // 拒絶の核接続音
    SE_INGAME_STARTING_GAME,            // ゲームスタート音

    SE_INGAME_CURTAIN_OPEN,          // カーテンオープン音
    SE_INGAME_CURTAIN_CLOSE,         // カーテンクローズ音

    SE_INGAME_CLOUD_MERGE,           // 想いの断片合成音
    SE_INGAME_CLOUD_MERGE_ALL,        // 想いの断片全合成音

    SE_INGAME_FEELING_PIECE_SPAWN,   // 想いの断片生成音
    SE_INGAME_FEELING_PIECE_COLLECT, // 想いの断片回収音
    SE_INGAME_FEELING_CORE_SETTING_SLOT,

    SE_INGAME_CONNECT_SLOT_1,      // 核接続音1
    SE_INGAME_CONNECT_SLOT_2,      // 核接続音2
    SE_INGAME_CONNECT_SLOT_3,      // 核接続音3
    SE_INGAME_CONNECT_SLOT_4,      // 核接続音4
    SE_INGAME_CONNECT_SLOT_5,      // 核接続音5
    SE_INGAME_CONNECT_SLOT_6,      // 核接続音6

    // **** ゲームリセット ****
    SE_GAMERESET_WINDOW_OPEN,    // ゲームリセットウィンドウオープン音
    SE_GAMERESET_APPLY,        // ゲームリセット適用音


    SE_INGAME_CLEAR,               // ゴール音

    SE_RESULT_TIME_COUNTING,    // リザルトタイムカウント音

}