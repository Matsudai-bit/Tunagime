using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// セーブデータの設定を管理するScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "SaveDataSetting", menuName = "ScriptableObject/SaveDataSetting")]
public class SaveDataSetting : ScriptableObject
{
    [Header("セーブデータの設定 =============================-")]

    [Header("ファイルフルパス(表示用)")]
    [SerializeField]
    private string fileFullPath;

    [Header("ファイルパス・ファイル名")]
    public string saveDirectoryPath = ""; // セーブデータフォルダの保存パス
    public string fileName = ""; // セーブデータのファイル名

    private void Reset()
    {
        
    }

    public string GetFileFullPath()
    {
        // 永続的な保存場所のルートパスを取得
        string rootPath = Application.persistentDataPath;
        // SaveDataフォルダのフルパスを作成
        string path = Path.Combine(rootPath, saveDirectoryPath);
        // フォルダが存在しない場合は作成（存在しないと開けないため）
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return Path.Combine(path, fileName);
    }

    private void OnValidate()
    {
        fileFullPath = GetFileFullPath();
    }

    // *************************************************************
    // **************** エディタ専用機能 (ビルドから除外) ****************
    // *************************************************************
    
    #if UNITY_EDITOR

    // ボタンでフォルダを開く
    [ContextMenu("セーブデータのフォルダを開く")]
public void OpenSaveDataFileLocation()
{
    string fullPath = GetFileFullPath();
    string directoryPath = Path.GetDirectoryName(fullPath);

    if (Directory.Exists(directoryPath))
    {
        // Windows/Macでフォルダを開くエディタ機能
        EditorUtility.RevealInFinder(directoryPath);
    }
    else
    {
        Debug.LogWarning("セーブデータのフォルダが存在しません。");
    }
}
    #endif // UNITY_EDITOR

}


