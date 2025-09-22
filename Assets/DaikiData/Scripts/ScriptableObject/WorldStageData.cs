using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ワールドステージデータ
/// </summary>
[CreateAssetMenu(fileName = "WorldStageData", menuName = "WorldStageData")]
public class WorldStageData : ScriptableObject
{
    public List<StageSetting> stageSettings; // ステージ設定のリスト
}
