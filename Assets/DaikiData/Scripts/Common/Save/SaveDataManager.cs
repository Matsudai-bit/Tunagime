using System.IO;
using UnityEngine;


/// <summary>
/// セーブデータの管理を行うクラス
/// </summary>
public class SaveDataManager : MonoBehaviour
{
    /// <summary>
    /// セーブデータの読み込み
    /// </summary>
    /// <param name="data"></param>
    public void Save(SaveData data, string fileFullPath)
    {
        // JSON形式で保存
        string json = JsonUtility.ToJson(data.ConvertJsonSaver(), true);
        // ファイル書き込み指定
        StreamWriter writer = new StreamWriter(fileFullPath, false);
        // Json変換した情報をファイルに書き込み
        writer.Write(json);
        // ファイルを閉じる
        writer.Close();
    }

    public SaveData Load(string fileFullPath)
    {
        SaveData.JsonSaver data = new SaveData.JsonSaver();
        // ファイルが存在するか確認
        if (!File.Exists(fileFullPath))
        {
            // ファイルが存在しない場合は新規作成
            Debug.LogWarning($"セーブデータファイルが存在しません。新規作成します。パス:{fileFullPath}");
            SaveData newSaveData = new SaveData();
            Save(newSaveData, fileFullPath);
        }

        // ファイル読み込み指定
        StreamReader reader = new StreamReader(fileFullPath);
        // ファイルの内容をすべて読み込む
        string json = reader.ReadToEnd();
        // Json形式のデータをオブジェクトに変換
        data = JsonUtility.FromJson<SaveData.JsonSaver>(json);
        // ファイルを閉じる
        reader.Close();

        SaveData saveData = new SaveData();
        saveData.LoadFromJsonSaver(data);
        return saveData;
    }

}
