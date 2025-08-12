using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ���̃��b�V�����C�u����
/// </summary>
public class YarnMeshLibrary : MonoBehaviour
{
    public static YarnMeshLibrary Instance { get; private set; }

    [System.Serializable]
    public struct MeshEntry
    {
        public AmidaTube.State meshType;    // �����̃L�[�ƂȂ�񋓌^
        public GameObject meshPrefab;            // �����̒l�ƂȂ�GameObject�̃v���n�u
    }

    // �؂�ւ��\�ȃ��b�V��GameObject�̃v���n�u�����X�g�ŊǗ�
    // �eGameObject�ɂ́AMeshFilter��MeshRenderer���ݒ肳��Ă���
    [SerializeField] private List<MeshEntry> m_meshEntries= new List<MeshEntry>();

    // �����ŗ��p���鎫��
    private Dictionary<AmidaTube.State, GameObject> m_meshPrefabMap;

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
        m_meshPrefabMap = new Dictionary<AmidaTube.State, GameObject>();
        foreach (MeshEntry entry in m_meshEntries)
        {
            if (entry.meshPrefab != null)
            {
                // �d���`�F�b�N (�K�v�ł����)
                if (m_meshPrefabMap.ContainsKey(entry.meshType))
                {
                    Debug.LogWarning($"MeshLibrary: '{entry.meshType}' �͊��ɓo�^����Ă��܂��B�㏑�����܂��B");
                }
                m_meshPrefabMap[entry.meshType] = entry.meshPrefab;
            }
            else
            {
                Debug.LogWarning($"MeshLibrary: '{entry.meshType}' �ɑΉ����郁�b�V���v���n�u���ݒ肳��Ă��܂���B");
            }
        }
        Debug.Log("MeshLibrary������������܂����B�o�^���ꂽ���b�V����: " + m_meshPrefabMap.Count);
    }

    /// <summary>
    /// �w�肳�ꂽAmidaTube.ShapeType�ɑΉ����郁�b�V��GameObject�̃v���n�u���擾���܂��B
    /// </summary>
    /// <param name="type">�擾���������b�V����AmidaTube.State</param>
    /// <returns>�Ή����郁�b�V��GameObject�̃v���n�u�A������Ȃ����null</returns>
    public GameObject GetMeshPrefab(AmidaTube.State type)
    {
        if (m_meshPrefabMap.TryGetValue(type, out GameObject meshPrefab))
        {
            return meshPrefab;
        }
        Debug.LogWarning($"MeshLibrary: '{type}' �Ƃ���AmidaTube.ShapeType�̃��b�V���͌�����܂���ł����B");
        return null;
    }
}