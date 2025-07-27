using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ���̃��b�V�����C�u����
/// </summary>
public class YarnMaterialLibrary : MonoBehaviour
{
    /// <summary>
    /// ���̃}�e���A���^�C�v���`����񋓌^
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
        public MaterialType meshType;    // �����̃L�[�ƂȂ�񋓌^
        public Material material;            // �����̒l�ƂȂ�GameObject�̃v���n�u
    }

    // �؂�ւ��\�ȃ��b�V��GameObject�̃v���n�u�����X�g�ŊǗ�
    // �eGameObject�ɂ́AMeshFilter��MeshRenderer���ݒ肳��Ă���
    [SerializeField] private List<MaterialEntry> m_meshEntries = new List<MaterialEntry>();

    // �����ŗ��p���鎫��
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
                // �d���`�F�b�N (�K�v�ł����)
                if (m_materialMap.ContainsKey(entry.meshType))
                {
                    Debug.LogWarning($"MeshLibrary: '{entry.meshType}' �͊��ɓo�^����Ă��܂��B�㏑�����܂��B");
                }
                m_materialMap[entry.meshType] = entry.material;
            }
            else
            {
                Debug.LogWarning($"MeshLibrary: '{entry.meshType}' �ɑΉ����郁�b�V���v���n�u���ݒ肳��Ă��܂���B");
            }
        }
        Debug.Log("MeshLibrary������������܂����B�o�^���ꂽ���b�V����: " + m_materialMap.Count);
    }

    /// <summary>
    /// �w�肳�ꂽ�}�e���A���^�C�v�ɑΉ����郁�b�V���v���n�u���擾���܂��B
    /// </summary>
    /// <param name="type">  </param>
    /// <returns></returns>
    public Material GetMaterial(MaterialType type)
    {
        if (m_materialMap.TryGetValue(type, out Material material))
        {
            return material;
        }
        Debug.LogWarning($"MeshLibrary: '{type}' �Ƃ���AmidaTube.ShapeType�̃��b�V���͌�����܂���ł����B");
        return null;
    }
}