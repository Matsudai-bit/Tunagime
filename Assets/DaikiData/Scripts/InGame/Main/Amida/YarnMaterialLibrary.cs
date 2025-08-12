using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 糸のメッシュライブラリ
/// </summary>
public class YarnMaterialLibrary : MonoBehaviour
{
    /// <summary>
    /// 糸のマテリアルタイプを定義する列挙型
    /// </summary>
    

    public static YarnMaterialLibrary Instance { get; private set; }

    [System.Serializable]
    public struct MaterialEntry
    {
        public EmotionCurrent.Type emotionType;    // 辞書のキーとなる列挙型
        public Material material;            // 辞書の値となるGameObjectのプレハブ
    }

    // 切り替え可能なメッシュGameObjectのプレハブをリストで管理
    // 各GameObjectには、MeshFilterとMeshRendererが設定されている
    [SerializeField] private List<MaterialEntry> m_materialEntries = new List<MaterialEntry>();

    // 内部で利用する辞書
    private Dictionary<EmotionCurrent.Type, Material> m_materialMap;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeLibrary();
            DontDestroyOnLoad(gameObject); // シーンをまたいで存続させる

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeLibrary()
    {
        m_materialMap = new Dictionary<EmotionCurrent.Type, Material>();
        foreach (MaterialEntry entry in m_materialEntries)
        {
            if (entry.material != null)
            {
                // 重複チェック (必要であれば)
                if (m_materialMap.ContainsKey(entry.emotionType))
                {
                    Debug.LogWarning($"MeshLibrary: '{entry.emotionType}' は既に登録されています。上書きします。");
                }
                m_materialMap[entry.emotionType] = entry.material;
            }
            else
            {
                Debug.LogWarning($"MeshLibrary: '{entry.emotionType}' に対応するメッシュプレハブが設定されていません。");
            }
        }
        Debug.Log("MeshLibraryが初期化されました。登録されたメッシュ数: " + m_materialMap.Count);
    }

    /// <summary>
    /// 指定されたマテリアルタイプに対応するメッシュプレハブを取得します。
    /// </summary>
    /// <param name="type">  </param>
    /// <returns></returns>
    public Material GetMaterial(EmotionCurrent.Type type)
    {
        if (m_materialMap.TryGetValue(type, out Material material))
        {
            return material;
        }
        Debug.LogWarning($"MeshLibrary: '{type}' というAmidaTube.ShapeTypeのメッシュは見つかりませんでした。");
        return null;
    }
}