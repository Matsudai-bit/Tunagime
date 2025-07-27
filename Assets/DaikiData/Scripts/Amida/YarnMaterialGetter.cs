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
       public MaterialType type;
    }

    [Header("���̃}�e���A���f�[�^")]
    [SerializeField]
    private List< YarnMaterialData> m_yarnMaterialData = new List<YarnMaterialData>();

    private Dictionary<MaterialType, MeshRenderer> m_materials = new Dictionary<MaterialType, MeshRenderer>();

    // �X�^�[�g
    void Awake()
    {
        // �}�e���A���̏�����
        foreach (var data in m_yarnMaterialData)
        {
            // 
            if (data.renderer != null/* && data.renderer.sharedMaterial != null*/)
            {
                if (!m_materials.ContainsKey(data.type))
                {
                    m_materials.Add(data.type, data.renderer);
                }
                else
                {
                    Debug.LogWarning($"Material type {data.type} already exists. Skipping duplicate.");
                }
            }
            else
            {
                Debug.LogWarning("Renderer or Material is null for one of the YarnMaterialData entries.");
            }
        }

    }

    /// <summary>
    /// �}�e���A���f�[�^���擾
    /// </summary>
    /// <returns></returns>
    public Dictionary<MaterialType, MeshRenderer> GetMaterials()
    {
        return m_materials;
    }
}
