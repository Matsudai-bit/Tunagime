using UnityEngine;
using System.Collections.Generic;

public class YarnMaterialGetter : MonoBehaviour
{
    public enum MaterialType
    {
        INPUT,
        OUTPUT,
        BRIDGE_DOWN,
        BRIDGE_UP
    }

    [System.Serializable]
    public class YarnMaterialData
    {
       public MeshRenderer renderer;
       public EmotionCurrent.Type emotionType; // EmotionCurrent.Typeを使用してマテリアルの種類を指定
       public MaterialType key;
    }

    [Header("糸の設定データ")]
    [SerializeField]
    private List< YarnMaterialData> m_yarnMaterialData = new List<YarnMaterialData>();

    private Dictionary<MaterialType, MeshRenderer> m_materials = new Dictionary<MaterialType, MeshRenderer>();

    // スタート
    void Awake()
    {
        // データに重複がないか確認
        HashSet<MaterialType> uniqueKeys = new HashSet<MaterialType>();
        foreach (var data in m_yarnMaterialData)
        {
            if (!uniqueKeys.Add(data.key))
            {
                Debug.LogWarning($"Duplicate key found: {data.key}. Please ensure all keys are unique.");
            }
        }


        // マテリアルの初期化
        foreach (var data in m_yarnMaterialData)
        {
            // 
            if (data.renderer != null/* && data.renderer.sharedMaterial != null*/)
            {
                if (!m_materials.ContainsKey(data.key))
                {
                    m_materials.Add(data.key, data.renderer);
                }
                else
                {
                    Debug.LogWarning($"Material key {data.key} already exists. Skipping duplicate.");
                }
            }
            else
            {
                Debug.LogWarning("Renderer or Material is null for one of the YarnMaterialData entries.");
            }
        }

    }

    /// <summary>
    /// 指定されたマテリアルタイプのMeshRendererを取得します。
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public MeshRenderer GetMeshRenderer(MaterialType type)
    {
        // 指定されたタイプのマテリアルが存在するか確認 (配列を使用)
        int index = m_yarnMaterialData.FindIndex(data => data.key == type);
        if (index != -1)
        {
            return m_yarnMaterialData[index].renderer;
        }
        else
        {
            Debug.LogWarning($"Material of type {type} not found.");
            return null;
        }


    }

    /// <summary>
    /// 指定されたマテリアルタイプに対応するEmotionCurrent.Typeを取得します。
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public EmotionCurrent.Type GetEmotionType(MaterialType type)
    {
        // 指定されたタイプのEmotionCurrent.Typeを取得
        int index = m_yarnMaterialData.FindIndex(data => data.key == type);
        if (index != -1)
        {
            return m_yarnMaterialData[index].emotionType;
        }
        else
        {
            Debug.LogWarning($"Emotion type for material {type} not found.");
            return EmotionCurrent.Type.NONE;
        }
    }

    /// <summary>
    /// 指定されたマテリアルタイプに対してEmotionCurrent.Typeを設定します。
    /// </summary>
    /// <param name="type"></param>
    /// <param name="emotionType"></param>
    public void SetEmotionType(MaterialType type, EmotionCurrent.Type emotionType)
    {
        // 指定されたタイプのEmotionCurrent.Typeを設定
        int index = m_yarnMaterialData.FindIndex(data => data.key == type);
        if (index != -1)
        {
            m_yarnMaterialData[index].emotionType = emotionType;
         //   Debug.Log($"Emotion type for material {type} set to {emotionType}.");
        }
        else
        {
            Debug.LogWarning($"Renderer for material {type} not found. Cannot set emotion type.");
        }
    }

    /// <summary>
    ///  想いの種類に応じてマテリアルを設定する
    /// </summary>
    public void ApplyMaterialForEmotion()
    {
        // 辞書のキーをループして、各MeshRendererに対応するEmotionCurrent.Typeを設定
        foreach (var data in m_yarnMaterialData)
        {
            if (data.renderer != null)
            {
                // EmotionCurrent.Typeを設定
                data.renderer.material = MaterialLibrary.GetInstance.GetMaterial(MaterialLibrary.MaterialGroup.YARN, data.emotionType);
              //  data.renderer.material =  YarnMaterialLibrary.Instance.GetMaterial(data.emotionType);
                // Debug.Log($"Applied material for {data.key} with emotion type {data.emotionType}.");
            }
            else
            {
                Debug.LogWarning($"Renderer for material {data.key} is null. Cannot apply material.");
            }
        }

    }

    /// <summary>
    /// 全てのマテリアルのEmotionCurrent.TypeをNONEにリセットします。
    /// </summary>
    public void ResetEmotionType()
    {
        // 全てのマテリアルのEmotionCurrent.TypeをNONEにリセット
        foreach (var data in m_yarnMaterialData)
        {
            data.emotionType = EmotionCurrent.Type.NONE;

        }
    }

    /// <summary>
    /// 全てのマテリアルのEmotionCurrent.Typeを指定された値に設定します。
    /// </summary>
    /// <param name="emotionType"></param>
    public void SetAllEmotionType(EmotionCurrent.Type emotionType)
    {
        // 全てのマテリアルのEmotionCurrent.Typeを指定された値に設定
        foreach (var data in m_yarnMaterialData)
        {
            data.emotionType = emotionType;
        }
    }
}
