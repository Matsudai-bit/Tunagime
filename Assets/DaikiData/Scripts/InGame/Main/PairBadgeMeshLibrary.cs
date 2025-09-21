using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 糸のメッシュライブラリ
/// </summary>
public class PairBadgeMeshLibrary : MonoBehaviour
{
    public static PairBadgeMeshLibrary Instance { get; private set; }

    [System.Serializable]
    public struct MeshEntry
    {
        public EmotionCurrent.Type meshType;    // 辞書のキーとなる列挙型
        public GameObject meshPrefab;            // 辞書の値となるGameObjectのプレハブ
    }

    // 切り替え可能なメッシュGameObjectのプレハブをリストで管理
    [SerializeField] private List<MeshEntry> m_meshEntries = new List<MeshEntry>();

    // 内部で利用する辞書
    private Dictionary<EmotionCurrent.Type, GameObject> m_meshPrefabMap;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeLibrary();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeLibrary()
    {
        m_meshPrefabMap = new Dictionary<EmotionCurrent.Type, GameObject>();
        foreach (MeshEntry entry in m_meshEntries)
        {
            if (entry.meshPrefab != null)
            {
                // 重複チェック (必要であれば)
                if (m_meshPrefabMap.ContainsKey(entry.meshType))
                {
                    Debug.LogWarning($"MeshLibrary: '{entry.meshType}' は既に登録されています。上書きします。");
                }
                m_meshPrefabMap[entry.meshType] = entry.meshPrefab;
            }
            else
            {
                Debug.LogWarning($"MeshLibrary: '{entry.meshType}' に対応するメッシュプレハブが設定されていません。");
            }
        }
        Debug.Log("MeshLibraryが初期化されました。登録されたメッシュ数: " + m_meshPrefabMap.Count);
    }

    /// <summary>
    /// 指定された種類に対応するメッシュGameObjectのプレハブを取得します。
    /// </summary>
    /// <param name="type">取得したいメッシュのEmotionCurrent.Type</param>
    /// <returns>対応するメッシュGameObjectのプレハブ、見つからなければnull</returns>
    public GameObject GetMeshPrefab(EmotionCurrent.Type type)
    {
        if (m_meshPrefabMap.TryGetValue(type, out GameObject meshPrefab))
        {
            return meshPrefab;
        }
        Debug.LogWarning($"MeshLibrary: '{type}' というメッシュは見つかりませんでした。");
        return m_meshPrefabMap[EmotionCurrent.Type.NONE];
      
    }
}