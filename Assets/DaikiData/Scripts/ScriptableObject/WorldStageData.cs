using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ワールドステージデータ
/// </summary>
[CreateAssetMenu(fileName = "WorldStageData", menuName = "WorldStageData")]
public class WorldStageData : ScriptableObject
{
    [Header("ワールドID")]
    public WorldID worldID; // ワールドID

    [Header("ステージ設定のリスト")]
    public List<StageSetting> stageSettings; // ステージ設定のリスト
}
