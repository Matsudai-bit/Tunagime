// ファイルの読み書きに使用
using System.IO;

// UnityEditor の拡張用名前空間（EditorWindow 等）
using UnityEditor;

// Unity の基本機能（Transformなど）
using UnityEngine;

// JSON シリアライズ用（Newtonsoft.JsonはUnity公式より高機能）
using Newtonsoft.Json;

/// <summary>
/// ステージの階層構造をエディタ上から保存・読み込みできるカスタムウィンドウ
/// </summary>
public class StageLayoutEditorWindow : EditorWindow
{
    // ユーザーが選択する対象の Transform（ルートオブジェクト）
    Transform target;

    // JSONファイル名（※現状では使用されていない）
    string jsonName;

    /// <summary>
    /// メニューにウィンドウを追加
    /// </summary>
    [MenuItem("Tools/Hierarchy Saver")]
    static void ShowWindow() => GetWindow<StageLayoutEditorWindow>("Hierarchy Saver");

    /// <summary>
    /// カスタムウィンドウのGUI描画処理
    /// </summary>
    void OnGUI()
    {
        // JSONの循環参照を無視する設定（例：親→子→親の参照など）
        var settings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

        // エディタ上でTransformを指定できるようにする（任意の階層のルートにできる）
        target = EditorGUILayout.ObjectField("Target Object", target, typeof(Transform), true) as Transform;

        // ▼ 「保存」ボタン
        if (GUILayout.Button("Save Hierarchy"))
        {
            // 対象Transformから階層構造をシリアライズ
            var data = StageLayoutSerializer.SerializeStage(target);

            // JSON文字列として整形して保存（インデントあり）
            string json = JsonConvert.SerializeObject(data, Formatting.Indented, settings);

            // ファイルに書き出し（Assets配下に保存）
            File.WriteAllText("Assets/DaikiData/SaveData/Test.json", json);

            Debug.Log("Hierarchy saved.");
        }

        // ▼ 「読み込み」ボタン
        if (GUILayout.Button("Load Hierarchy"))
        {
            // ステージ内のPrefabの参照情報を持つ ScriptableObject をロード
            StagePrefabDatabase prefabDatabase = Resources.Load<StagePrefabDatabase>("StagePrefabDatabase");

            // JSONファイルを読み込み
            string json = File.ReadAllText("Assets/DaikiData/SaveData/Test.json");

            // JSONから階層データに変換（デシリアライズ）
            var data = JsonConvert.DeserializeObject<StageSerializableChildData>(json, settings);

            // プレハブ情報を使って階層構造を復元
            StageLayoutSerializer.DeserializeStage(data, prefabDatabase);

            Debug.Log("Hierarchy loaded.");
        }
    }
}
