using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StageSelectButtonData", menuName = "StageSelectButtonData")]
public class StageSelectButtonDataForWorld : ScriptableObject
{
    /// <summary>
    /// ステージセレクトボタンの情報（ワールドごと）
    /// </summary>
    [Serializable]
    public struct StageSelectButtonInfoForWorld
    {
        public string stageName;        // ステージ名
        public WorldID worldID;         // ステージID
        public GameObject buttonPrefab; // ボタンのプレハブ
    }

    [Header("ステージセレクトボタン情報")]
    public List<StageSelectButtonInfoForWorld> stageSelectButtonData = new(); // ステージセレクトボタンの情報配列


    private void Reset()
    {
        for (int i = 0 ; i < Enum.GetValues(typeof(WorldID)).Length; i++)
        {
            stageSelectButtonData.Add(new StageSelectButtonInfoForWorld
            {
                stageName = $"World {i + 1}",
                worldID = (WorldID)i,
                buttonPrefab = null
            });
        }
    }


}
