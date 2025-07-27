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
    
    [SerializeField]
    public enum MaterialType
    {
        NONE,
        GREEN,
        BLUE,
    }

    public static YarnMaterialLibrary Instance { get; private set; }

    [System.Serializable]
    public struct MaterialEntry
    {
        public MaterialType meshType;    // 辞書のキーとなる列挙型
        public Material material;            // 辞書の値となるGameObjectのプレハブ
    }

    // 切り替え可能なメッシュGameObjectのプレハブをリストで管理
    // 各GameObjectには、MeshFilterとMeshRendererが設定されている
    [SerializeField] private List<MaterialEntry> m_meshEntries = new List<MaterialEntry>();

    // 内部で利用する辞書
    private Dictionary<MaterialType, Material> m_materialMap;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeLibrary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeLibrary()
    {
        m_materialMap = new Dictionary<MaterialType, Material>();
        foreach (MaterialEntry entry in m_meshEntries)
        {
            if (entry.material != null)
            {
                // 重複チェック (必要であれば)
                if (m_materialMap.ContainsKey(entry.meshType))
                {
                    Debug.LogWarning($"MeshLibrary: '{entry.meshType}' は既に登録されています。上書きします。");
                }
                m_materialMap[entry.meshType] = entry.material;
            }
            else
            {
                Debug.LogWarning($"MeshLibrary: '{entry.meshType}' に対応するメッシュプレハブが設定されていません。");
            }
        }
        Debug.Log("MeshLibraryが初期化されました。登録されたメッシュ数: " + m_materialMap.Count);
    }

    /// <summary>
    /// 指定されたマテリアルタイプに対応するメッシュプレハブを取得します。
    /// </summary>
    /// <param name="type">  </param>
    /// <returns></returns>
    public Material GetMaterial(MaterialType type)
    {
        if (m_materialMap.TryGetValue(type, out Material material))
        {
            return material;
        }
        Debug.LogWarning($"MeshLibrary: '{type}' というAmidaTube.ShapeTypeのメッシュは見つかりませんでした。");
        return null;
    }
}