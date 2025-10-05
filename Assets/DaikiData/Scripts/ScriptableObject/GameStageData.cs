using System;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// ゲームステージのデータを管理するScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "GameStageData", menuName = "DaikiData/GameStageData", order = 0)]
public class GameStageData : ScriptableObject
{
    [Serializable]
    public struct InspectorData
    {
       public string label;
       public WorldID worldID;
       public WorldStageData WorldStageData;
    }


    [Header("インスペクター用データ")]
    public InspectorData[] inspectorDatas;


    [Header("ワールドステージデータ(直接いじらないこと)")]
    public WorldStageData[] worldStageDatas;

    private void Reset()
    {
        inspectorDatas = new InspectorData[Enum.GetValues(typeof(WorldID)).Length];

        for (int i = 0; i < inspectorDatas.Length; i++)
        {
            inspectorDatas[i].label = ((WorldID)i).ToString();
            inspectorDatas[i].worldID = (WorldID)i;
        }
    }

    private void OnValidate()
    {
        worldStageDatas = new WorldStageData[inspectorDatas.Length];
        for (int i = 0; i < inspectorDatas.Length; i++)
        {
            if (inspectorDatas[i].WorldStageData != null && inspectorDatas[i].WorldStageData.worldID == inspectorDatas[i].worldID)
            {
                worldStageDatas[i] = inspectorDatas[i].WorldStageData;
            }
            else
            {
                worldStageDatas[i] = null;
            }
        }
    }

    /// <summary>
    /// 指定されたワールドIDとステージIDに対応するStageSettingを取得する
    /// </summary>
    /// <param name="worldID"></param>
    /// <param name="stageID"></param>
    /// <returns></returns>
    public StageSetting GetStageSetting(WorldID worldID, StageID stageID)
    {
        foreach (var worldStageData in worldStageDatas)
        {
            if (worldStageData != null && worldStageData.worldID == worldID)
            {
                for (int i = 0; i < worldStageData.stageSettings.Count; i++)
                {
                    if (i == (int)stageID)
                    {
                        return worldStageData.stageSettings[i];
                    }
                }

            }
        }
        return null;
    }
}
