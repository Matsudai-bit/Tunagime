using UnityEngine;

/// <summary>
/// ゲームのコンテキストを管理するクラス
/// </summary>
public class GameContext : MonoBehaviour
{
    #region シングルトンの実装
    private static GameContext m_instance;
    /// <summary>
    /// GameContextのインスタンスを取得する
    /// </summary>
    public static GameContext GetInstance
    {
        get
        {
            if (m_instance == null)
            {
                var obj = new GameObject("GameContext");
                m_instance = obj.AddComponent<GameContext>();
            }
            return m_instance;
        }
    }

    #endregion

    /// <summary>
    /// Awakeメソッドでシングルトンインスタンスを設定する
    /// </summary>
    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);

            // 初期化
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }

    }


    [Header("ステージセーブデータ")]
    [SerializeField]
    private SaveDataSetting saveDataSetting;

    #region デバック用設定


    [System.Serializable]
    class DebugSettings
    {
        public bool debugMode = false;          // デバッグモードの有効化
        public bool unlockAllStages;    // 全てのステージをアンロック
        public SaveDataSetting debugSaveData;  // デバッグ用セーブデータ設定

    }
    #endregion

    [Header("デバッグ設定")]
    [SerializeField]
    private DebugSettings m_debugSettings = new(); // デバッグ用設定

    [Header("セーブデータ")]
    [SerializeField]


    private SaveData m_saveData;
    private SaveDataManager m_saveDataManager;


    public void Initialize()
    {

        // セーブデータ管理の作成
        GameObject gameObject = new GameObject("SaveDataManager");
        gameObject.name = "SaveDataManager";

        gameObject.transform.parent = transform;
        m_saveDataManager = gameObject.AddComponent<SaveDataManager>();



        // デバッグモードの適用
        if (m_debugSettings.debugMode)
        {
            // デバッグ用セーブデータの読み込み
            m_saveData = m_saveDataManager.Load(m_debugSettings.debugSaveData.GetFileFullPath());

            // 全てのステージをアンロック
            if (m_debugSettings.unlockAllStages)
            {
                UnlockAllStage(m_saveData);

            }
        }
        else
        {
            // 通常セーブデータの読み込み 
            m_saveData = m_saveDataManager.Load(saveDataSetting.GetFileFullPath());
         

        }

        // ファイルが無い場合作成セーブデータの読み込み
        SaveGame();


    }

    public SaveData GetSaveData()
    {
        return m_saveData;



    }

    /// <summary>
    /// ゲームのセーブを行う
    /// </summary>
    public void SaveGame()
    {
        m_saveDataManager.Save(m_saveData, saveDataSetting.GetFileFullPath());
    }

    /// <summary>
    /// ゲームのリセットを行う
    /// </summary>
    public void ResetGame()
    {
        m_saveData = new SaveData();
        SaveGame();
    }

    /// <summary>
    /// デバッグモードを適用する
    /// </summary>
    static public void UnlockAllStage(SaveData saveData)
    {
        // 全てのワールドとステージをアンロック
      
        int worldCount = 5;
        int stageCount = 5;
        for (int w = 0; w < worldCount; w++)
        {
            WorldID worldID = (WorldID)w;
            if (saveData.worldDataDict.ContainsKey(worldID))
            {
                SaveData.WorldData worldData = saveData.worldDataDict[worldID];
                worldData.isLocked = false;
                for (int s = 0; s < stageCount; s++)
                {
                    StageID stageID = (StageID)s;
                    SaveData.StageData stageData = worldData.stageDataList.Find(sd => sd.stageID == stageID);
                    if (stageData != null)
                    {
                        stageData.stageStatus.isLocked = false;
                    }
                }
            }
        }
        

    }

    /// <summary>
    /// 次のステージをアンロックする
    /// </summary>
    public void UnlockNextStage()
    {
        // 次のステージをアンロックする処理をここに実装
        int worldCount = 5;
        int stageCount = 5;
        // ワールドごとにステージをチェックして現在最後に空いたステージを見つける
        for (int w = 0; w < worldCount; w++)
        {
            WorldID worldID = (WorldID)w;
            if (m_saveData.worldDataDict.ContainsKey(worldID))
            {
                SaveData.WorldData worldData = m_saveData.worldDataDict[worldID];
                worldData.isLocked = false;
                for (int s = 0; s < stageCount; s++)
                {
                    StageID stageID = (StageID)s;
                    SaveData.StageData stageData = worldData.stageDataList.Find(sd => sd.stageID == stageID);
                    if (stageData != null && !stageData.stageStatus.isLocked)
                    {
                        // 次のステージをアンロック
                        int nextStageIndex = s + 1;
                        if (nextStageIndex < stageCount)
                        {
                            StageID nextStageID = (StageID)nextStageIndex;
                            SaveData.StageData nextStageData = worldData.stageDataList.Find(sd => sd.stageID == nextStageID);
                            if (nextStageData != null)
                            {
                                nextStageData.stageStatus.isLocked = false;
                                return; // 一つアンロックしたら終了
                            }
                        }
                    }
                }
            }
        }
    }

    public void UnlockNextStage(WorldID worldID, StageID stageID)
    {
        // 指定されたステージの次のステージをアンロックする処理をここに実装
        if (m_saveData.worldDataDict.ContainsKey(worldID))
        {
            SaveData.WorldData worldData = m_saveData.worldDataDict[worldID];
            int nextStageIndex = (int)stageID + 1;

            // 同じワールド内の次のステージをアンロック
            if (nextStageIndex < 5) // ステージ数が5の場合
            {
                StageID nextStageID = (StageID)nextStageIndex;
                SaveData.StageData nextStageData = worldData.stageDataList.Find(sd => sd.stageID == nextStageID);
                if (nextStageData != null)
                {
                    nextStageData.stageStatus.isLocked = false;
                }
            }
            // 次のワールドの最初のステージをアンロック
            else
            {
                int nextWorldIndex = (int)worldID + 1;
                if (nextWorldIndex < 5) // ワールド数が5の場合
                {
                    WorldID nextWorldID = (WorldID)nextWorldIndex;

                    if (m_saveData.worldDataDict.ContainsKey(nextWorldID))
                    {
                        SaveData.WorldData nextWorldData = m_saveData.worldDataDict[nextWorldID];
                        nextWorldData.isLocked = false;
                        SaveData.StageData firstStageData = nextWorldData.stageDataList.Find(sd => sd.stageID == StageID.STAGE_1);
                        if (firstStageData != null)
                        {
                            firstStageData.stageStatus.isLocked = false;
                        }
                    }
                }
            }
        }
    }

}
