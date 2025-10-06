using UnityEngine;

/// <summary>
/// セーブデータの設定を管理するScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "SaveDataSetting", menuName = "ScriptableObject/SaveDataSetting")]
public class SaveDataSetting : ScriptableObject
{
    [Header("セーブデータの設定 =============================-")]

    [Header("ファイルフルパス(表示用)")]
    public string fileFullPath;

    [Header("ファイルパス・ファイル名")]
    public string filePath = ""; // セーブデータの保存パス
    public string fileName = ""; // セーブデータのファイル名

    private void Reset()
    {
        
    }

    private void OnValidate()
    {
        // パス取得
        fileFullPath = Application.dataPath + "/" + filePath + "/" + fileName;

        //if (!filePath.EndsWith("/"))
        //{
        //    filePath += "/";
        //}
    }

}
