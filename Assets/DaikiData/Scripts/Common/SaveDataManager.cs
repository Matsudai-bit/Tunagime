using System.IO;
using UnityEngine;


/// <summary>
/// セーブデータの管理を行うクラス
/// </summary>
public class SaveDataManager : MonoBehaviour
{
    [SerializeField]
    private SaveDataSetting saveDataSetting;

    private void Awake()
    {
        // セーブデータの初期化や読み込み処理をここに追加
        Debug.Log("SaveDataManager Awake: " + saveDataSetting.fileFullPath);

        // セーブデータのディレクトリが存在しない場合、作成する

        if (!File.Exists(saveDataSetting.fileFullPath))
        {
            Save(new SaveData());
        }
 
    }

    /// <summary>
    /// セーブデータの読み込み
    /// </summary>
    /// <param name="data"></param>
    public void Save(SaveData data)
    {
        // JSON形式で保存
        string json = JsonUtility.ToJson(data.ConvertJsonSaver(), true);
        // ファイル書き込み指定
        StreamWriter writer = new StreamWriter(saveDataSetting.fileFullPath, false);
        // Json変換した情報をファイルに書き込み
        writer.Write(json);
        // ファイルを閉じる
        writer.Close();
    }

    public SaveData Load()
    {
        SaveData.JsonSaver data = new SaveData.JsonSaver();
        // ファイルが存在するか確認
        if (File.Exists(saveDataSetting.fileFullPath))
        {
            // ファイル読み込み指定
            StreamReader reader = new StreamReader(saveDataSetting.fileFullPath);
            // ファイルの内容をすべて読み込む
            string json = reader.ReadToEnd();
            // Json形式のデータをオブジェクトに変換
            data = JsonUtility.FromJson<SaveData.JsonSaver>(json);
            // ファイルを閉じる
            reader.Close();
        }
        else
        {
            Debug.LogWarning("Save file not found: " + saveDataSetting.fileFullPath);
        }
        SaveData saveData = new SaveData();
        saveData.LoadFromJsonSaver(data);
        return saveData;
    }

}
