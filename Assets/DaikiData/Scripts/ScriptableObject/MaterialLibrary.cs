using System;
using System.Collections.Generic;
using System.Linq; // ToDictionaryを使うために必要
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialLibrary", menuName = "ScriptableObjects/MaterialLibrary")]
public class MaterialLibrary : ScriptableObject
{
    // マテリアルのグループを定義
    public enum MaterialGroup
    {
        YARN,
        FELT_BLOCK,
        CURTAIN,
        CORE,
        FRAGMENT, // 追加: 想いの断片
        FEELING_SLOT // 
    }

    [Serializable]
    public struct MaterialEntry
    {
        public EmotionCurrent.Type emotionType;
        public Material material;

        public MaterialEntry(EmotionCurrent.Type type, Material value) : this()
        {
            this.emotionType = type;
            this.material= value;
        }
    }

    // マテリアルグループとそのエントリのペア
    [Serializable]
    public struct MaterialGroupEntry
    {
        public MaterialGroup materialGroup;
        public List<MaterialEntry> materialEntries;
    }

    // Inspectorで設定するためのリスト
    [SerializeField]
    private List<MaterialGroupEntry> m_materialGroups = new List<MaterialGroupEntry>();

    // ランタイムで利用する辞書
    private Dictionary<MaterialGroup, Dictionary<EmotionCurrent.Type, Material>> m_materialMap;

    private static MaterialLibrary s_instance;
    public static MaterialLibrary GetInstance
    {
        get
        {
            if (s_instance == null)
            {
                // リソースフォルダからスクリプタブルオブジェクトをロード
                s_instance = Resources.Load<MaterialLibrary>("MaterialLibrary");
                if (s_instance == null)
                {
                    Debug.LogError("MaterialLibraryスクリプタブルオブジェクトが'Resources'フォルダに見つかりません。");
                }
                else
                {
                    s_instance.InitializeLibrary();
                }
            }
            return s_instance;
        }
    }

    /// <summary>
    /// ライブラリの初期化処理。Inspectorで設定したリストを辞書に変換します。
    /// </summary>
    private void InitializeLibrary()
    {
        m_materialMap = new Dictionary<MaterialGroup, Dictionary<EmotionCurrent.Type, Material>>();
        foreach (var groupEntry in m_materialGroups)
        {
            // 各グループのエントリを辞書に変換し、m_materialMapに追加
            m_materialMap[groupEntry.materialGroup] = groupEntry.materialEntries
                .ToDictionary(entry => entry.emotionType, entry => entry.material);
        }
        Debug.Log("[MaterialLibrary] Library initialized.");
    }

    /// <summary>
    /// 指定されたマテリアルグループと感情タイプに対応するマテリアルを取得します。
    /// </summary>
    /// <param name="materialGroup">マテリアルの種類</param>
    /// <param name="emotionType">取得したいマテリアルの感情タイプ</param>
    /// <returns>対応するマテリアル。見つからない場合はnullを返します。</returns>
    public Material GetMaterial(MaterialGroup materialGroup, EmotionCurrent.Type emotionType)
    {
        // まずマテリアルグループが存在するか確認
        if (m_materialMap.TryGetValue(materialGroup, out var innerDictionary))
        {
            // 次に感情タイプに対応するマテリアルが存在するか確認
            if (innerDictionary.TryGetValue(emotionType, out Material material))
            {
                return material;
            }
        }

        Debug.LogWarning($"[MaterialLibrary] マテリアル '{materialGroup}' の感情タイプ '{emotionType}' が見つかりませんでした。");
        return null;
    }

    public void Reset()
    {
        m_materialGroups.Add(new MaterialGroupEntry  {   materialGroup = MaterialGroup.YARN,   materialEntries = new List<MaterialEntry>()});
        m_materialGroups.Add(new MaterialGroupEntry { materialGroup = MaterialGroup.FELT_BLOCK, materialEntries = new List<MaterialEntry>() });
        m_materialGroups.Add(new MaterialGroupEntry { materialGroup = MaterialGroup.CURTAIN, materialEntries = new List<MaterialEntry>() });
        m_materialGroups.Add(new MaterialGroupEntry { materialGroup = MaterialGroup.CORE, materialEntries = new List<MaterialEntry>() });
        m_materialGroups.Add(new MaterialGroupEntry { materialGroup = MaterialGroup.FEELING_SLOT, materialEntries = new List<MaterialEntry>() });
        
        foreach (var group in m_materialGroups)
        {
            group.materialEntries.Add( new MaterialEntry( EmotionCurrent.Type.REJECTION, null));
            group.materialEntries.Add(new MaterialEntry(EmotionCurrent.Type.LONGING, null));
            group.materialEntries.Add(new MaterialEntry(EmotionCurrent.Type.COURAGE, null));
            group.materialEntries.Add(new MaterialEntry(EmotionCurrent.Type.KINDNESS, null));
            group.materialEntries.Add(new MaterialEntry(EmotionCurrent.Type.LOVE, null));
            group.materialEntries.Add(new MaterialEntry(EmotionCurrent.Type.FAITHFULNESS, null));
            group.materialEntries.Add(new MaterialEntry(EmotionCurrent.Type.NONE, null));
        }



    }
}