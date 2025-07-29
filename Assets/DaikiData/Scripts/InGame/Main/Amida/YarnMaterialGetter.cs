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
    public struct YarnMaterialData
    {
       public MeshRenderer renderer;
       public EmotionCurrent.Type emotionType; // EmotionCurrent.Type���g�p���ă}�e���A���̎�ނ��w��
       public MaterialType key;
    }

    [Header("���̐ݒ�f�[�^")]
    [SerializeField]
    private List< YarnMaterialData> m_yarnMaterialData = new List<YarnMaterialData>();

    private Dictionary<MaterialType, MeshRenderer> m_materials = new Dictionary<MaterialType, MeshRenderer>();

    // �X�^�[�g
    void Awake()
    {
        // �f�[�^�ɏd�����Ȃ����m�F
        HashSet<MaterialType> uniqueKeys = new HashSet<MaterialType>();
        foreach (var data in m_yarnMaterialData)
        {
            if (!uniqueKeys.Add(data.key))
            {
                Debug.LogWarning($"Duplicate key found: {data.key}. Please ensure all keys are unique.");
            }
        }


        // �}�e���A���̏�����
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
    /// �w�肳�ꂽ�}�e���A���^�C�v��MeshRenderer���擾���܂��B
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public MeshRenderer GetMeshRenderer(MaterialType type)
    {
        // �w�肳�ꂽ�^�C�v�̃}�e���A�������݂��邩�m�F (�z����g�p)
        var findData = m_yarnMaterialData.Find(data => data.key == type);
        if (findData.renderer != null)
        {
            return findData.renderer;
        }
        else
        {
            Debug.LogWarning($"Material of type {type} not found.");
            return null;
        }


    }

    /// <summary>
    /// �w�肳�ꂽ�}�e���A���^�C�v�ɑΉ�����EmotionCurrent.Type���擾���܂��B
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public EmotionCurrent.Type GetEmotionType(MaterialType type)
    {
        // �w�肳�ꂽ�^�C�v��EmotionCurrent.Type���擾
        var findData = m_yarnMaterialData.Find(data => data.key == type);
        if (findData.emotionType != EmotionCurrent.Type.NONE)
        {
            return findData.emotionType;
        }
        else
        {
            Debug.LogWarning($"Emotion type for material {type} not found.");
            return EmotionCurrent.Type.NONE;
        }
    }

    /// <summary>
    /// �w�肳�ꂽ�}�e���A���^�C�v�ɑ΂���EmotionCurrent.Type��ݒ肵�܂��B
    /// </summary>
    /// <param name="type"></param>
    /// <param name="emotionType"></param>
    public void SetEmotionType(MaterialType type, EmotionCurrent.Type emotionType)
    {
        // �w�肳�ꂽ�^�C�v��EmotionCurrent.Type��ݒ�
        var findData = m_yarnMaterialData.Find(data => data.key == type);
        if (findData.renderer != null)
        {
            findData.emotionType = emotionType;
            Debug.Log($"Emotion type for material {type} set to {emotionType}.");
        }
        else
        {
            Debug.LogWarning($"Renderer for material {type} not found. Cannot set emotion type.");
        }
    }

}
