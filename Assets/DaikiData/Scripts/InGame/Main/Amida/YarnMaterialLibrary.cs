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
    

    public static YarnMaterialLibrary Instance { get; private set; }

    [System.Serializable]
    public struct MaterialEntry
    {
        public EmotionCurrent.Type emotionType;    // �����̃L�[�ƂȂ�񋓌^
        public Material material;            // �����̒l�ƂȂ�GameObject�̃v���n�u
    }

    // �؂�ւ��\�ȃ��b�V��GameObject�̃v���n�u�����X�g�ŊǗ�
    // �eGameObject�ɂ́AMeshFilter��MeshRenderer���ݒ肳��Ă���
    [SerializeField] private List<MaterialEntry> m_materialEntries = new List<MaterialEntry>();

    // �����ŗ��p���鎫��
    private Dictionary<EmotionCurrent.Type, Material> m_materialMap;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeLibrary();
            DontDestroyOnLoad(gameObject); // �V�[�����܂����ő���������

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
                // �d���`�F�b�N (�K�v�ł����)
                if (m_materialMap.ContainsKey(entry.emotionType))
                {
                    Debug.LogWarning($"MeshLibrary: '{entry.emotionType}' �͊��ɓo�^����Ă��܂��B�㏑�����܂��B");
                }
                m_materialMap[entry.emotionType] = entry.material;
            }
            else
            {
                Debug.LogWarning($"MeshLibrary: '{entry.emotionType}' �ɑΉ����郁�b�V���v���n�u���ݒ肳��Ă��܂���B");
            }
        }
        Debug.Log("MeshLibrary������������܂����B�o�^���ꂽ���b�V����: " + m_materialMap.Count);
    }

    /// <summary>
    /// �w�肳�ꂽ�}�e���A���^�C�v�ɑΉ����郁�b�V���v���n�u���擾���܂��B
    /// </summary>
    /// <param name="type">  </param>
    /// <returns></returns>
    public Material GetMaterial(EmotionCurrent.Type type)
    {
        if (m_materialMap.TryGetValue(type, out Material material))
        {
            return material;
        }
        Debug.LogWarning($"MeshLibrary: '{type}' �Ƃ���AmidaTube.ShapeType�̃��b�V���͌�����܂���ł����B");
        return null;
    }
}