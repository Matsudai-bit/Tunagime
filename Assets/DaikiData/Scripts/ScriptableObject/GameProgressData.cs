using UnityEngine;

/// <summary>
/// ゲームの進行状況を管理するScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "GameProgressData", menuName = "ScriptableObjects/GameProgressData", order = 1)]
public class GameProgressData : ScriptableObject
{
    [Header("現在のワールドID")]
    public WorldID worldID; // 現在のワールドID
    [Header("現在のステージID")]
    public StageID stageID; // 現在のステージID

    [Header("開始状態")]
    public InGameFlowEventID startState; // 開始状態

    [Header("クリア時間")]
    public float clearTime; // クリア時間

    [Header("前のシーンでゲームがプレイされたか")]
    public bool isPrevSceneGamePlayed = false;
}
