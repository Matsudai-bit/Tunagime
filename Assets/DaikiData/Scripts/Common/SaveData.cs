using System;
using System.Collections.Generic;

/// <summary>
/// セーブデータ用ScriptableObject
/// </summary>
public class SaveData 
{
    /// <summary>
    /// JSON保存用構造体
    /// </summary>
    public struct JsonSaver
    {
        public List<WorldData> worldDataList;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SaveData()
    {
        Initialize();
    }

    /// <summary>
    /// ステージクリアデータ
    /// </summary>
    [System.Serializable]
    public struct StageData
    {
        public StageID      stageID; // ステージID
        public StageStatus  stageStatus; // ステージステータス
    }

    /// <summary>
    /// ステージステータス
    /// </summary>
    [System.Serializable]
    public struct StageStatus
    {
        public bool isClear;   // クリアしたかどうか
        public bool locked;  // アンロックされているかどうか
        public float clearTime; // クリアタイム
    }

    /// <summary>
    /// ワールドデータ
    /// </summary>
    [System.Serializable]
    public struct WorldData
    {
        public WorldID worldID;                 // ワールドID
        public List<StageData> stageDataList; // JSON保存用リスト
    }

    // ワールドデータ辞書
    public Dictionary<WorldID, WorldData> worldDataDict = new Dictionary<WorldID, WorldData>();


    /// <summary>
    /// JSON保存用構造体に変換
    /// </summary>
    /// <returns></returns>
    public JsonSaver ConvertJsonSaver()
    {
        JsonSaver jsonSaver = new();

        jsonSaver.worldDataList = new List<WorldData>();

        jsonSaver.worldDataList.Clear();
        foreach (var worldData in worldDataDict)
        {
            jsonSaver.worldDataList.Add(worldData.Value);
        }
        return jsonSaver;
    }

    /// <summary>
    /// JSON保存用構造体から読み込み
    /// </summary>
    /// <param name="jsonSaver"></param>
    public void LoadFromJsonSaver(JsonSaver jsonSaver)
    {
        foreach (var worldData in jsonSaver.worldDataList)
        {
            worldDataDict[worldData.worldID] = worldData;
        }
    }


    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        // ワールドデータとステージクリアデータの初期化
        for (int i = 0; i < Enum.GetValues(typeof(WorldID)).Length; i++)
        {
            // ワールドデータの初期化
            WorldID worldID = (WorldID)i;
            WorldData worldData = new WorldData();
            worldData.worldID = worldID;
            worldData.stageDataList = new ();

            // ステージクリアデータの初期化
            for (int j = 0; j < Enum.GetValues(typeof(StageID)).Length; j++)
            {
                StageID stageID = (StageID)j;
                StageData stageClearData = new StageData();
                stageClearData.stageID = stageID;
                stageClearData.stageStatus = new StageStatus();
                stageClearData.stageStatus.isClear = false;
                stageClearData.stageStatus.locked = (j != 0) ? true : false; // 最初のステージだけアンロック
                stageClearData.stageStatus.clearTime = 0.0f;
                worldData.stageDataList.Add(stageClearData);
            }
            worldDataDict[worldID] = worldData;
        }
    }
}
