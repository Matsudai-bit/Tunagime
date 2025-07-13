// 必要な名前空間をインポート
using Newtonsoft.Json;                 // 高機能な JSON シリアライザ（JsonUtility より柔軟）
using System;
using System.Linq;
using UnityEngine;

// ステージのオブジェクト配置をシリアライズ/デシリアライズするユーティリティクラス
public class StageLayoutSerializer : MonoBehaviour
{
    /// <summary>
    /// 指定された Transform（ルート）から再帰的にステージ情報をシリアライズする
    /// </summary>
    public static StageSerializableChildData SerializeStage(Transform transform)
    {
        // 現在のTransformの基本情報（名前、タグ、位置、回転、スケール）を保存
        var data = new StageSerializableChildData
        {
            name = transform.name,
            tag = transform.tag,
            localPosition = transform.localPosition,
            localRotation = transform.localEulerAngles,
            localScale = transform.localScale
        };

        // この GameObject にアタッチされている MonoBehaviour コンポーネントを取得
        var components = transform.gameObject.GetComponents<MonoBehaviour>();

        // 各コンポーネントの中からシリアライズ可能なものだけを抽出
        foreach (var comp in components)
        {
            if (comp is ISerializableComponent serializable)
            {
                // シリアライズ用データを取得
                var payload = serializable.CaptureData();

                // JSON 文字列に変換（Unity純正のJsonUtilityを使用）
                var json = JsonUtility.ToJson(payload);

                // 保存用の構造体に追加（MonoBehaviourの型とPayloadの型の両方を保持）
                data.components.Add(new SerializedComponentData
                {
                    typeName = comp.GetType().AssemblyQualifiedName,           // 例: AmidaTube
                    payloadTypeName = payload.GetType().AssemblyQualifiedName,        // 例: AmidaTubeData
                    json = json                                            // 実際のデータ（JSON）
                });
            }
        }

        // 子オブジェクトも再帰的に保存
        foreach (Transform child in transform)
        {
            data.children.Add(SerializeStage(child));
        }

        return data;
    }

    /// <summary>
    /// 保存されたステージデータからGameObjectツリーを再構築する
    /// </summary>
    public static GameObject DeserializeStage(StageSerializableChildData data, StagePrefabDatabase stageyPrefab)
    {
        // タグに応じたプレハブを取得（例：Switch、AmidaTubeなど）
        var prefab = stageyPrefab.GetPrefab(data.tag);

        GameObject instance = null;

        if (prefab != null)
        {
            // プレハブからインスタンス生成
            instance = Instantiate(prefab);
            instance.gameObject.name = data.name;
            instance.gameObject.tag = data.tag;
            instance.transform.localPosition = data.localPosition;
            instance.transform.localEulerAngles = data.localRotation;
            instance.transform.localScale = data.localScale;

            // 各シリアライズ済みコンポーネントを復元
            foreach (var compData in data.components)
            {
                // MonoBehaviourの型を取得
                Type type = Type.GetType(compData.typeName);
                var component = instance.GetComponent(type);

                // 該当する ISerializableComponent を持っているか確認
                if (component is ISerializableComponent serializable)
                {
                    // JSON復元対象の「データ型」（AmidaTubeDataなど）を名前から取得
                    string payloadTypeName = compData.payloadTypeName.Replace("+", ".");
                    Type payloadType = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .FirstOrDefault(t => t.AssemblyQualifiedName == payloadTypeName);

                    if (payloadType == null)
                    {
                        Debug.LogWarning($"型が見つかりません: {compData.typeName}");
                        continue;
                    }

                    // JSON → データクラスに変換（デシリアライズ）
                    var payload = JsonConvert.DeserializeObject(compData.json, payloadType);
                    if (payload == null)
                    {
                        Debug.LogWarning($"デシリアライズ失敗: JSON={compData.json}, 型={payloadType.FullName}");
                        continue;
                    }

                    // 復元したデータをコンポーネントへ適用
                    serializable.ApplyData(payload);
                }
            }
        }
        else
        {
            // プレハブが見つからなかった場合は空のGameObjectを生成
            instance = new GameObject(data.name);
            instance.transform.name = data.name;
            instance.transform.localPosition = data.localPosition;
            instance.transform.localEulerAngles = data.localRotation;
            instance.transform.localScale = data.localScale;
        }

        // 子データも再帰的に復元
        foreach (var childData in data.children)
        {
            GameObject child = DeserializeStage(childData, stageyPrefab);

            // ワールド座標を維持せずに親にぶら下げる（ローカルTransform）
            child.transform.SetParent(instance.transform, false);
        }

        return instance;
    }
}
