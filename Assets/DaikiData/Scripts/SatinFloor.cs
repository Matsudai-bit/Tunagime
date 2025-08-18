using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// �T�e����
/// </summary>
public class SatinFloor : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> m_satinModels = new List<GameObject>(); // �T�e���̃��f���̃��X�g

    [SerializeField]
    private GameObject m_currentModel; // ���݂̃T�e�����f��

    private void Awake()
    {
        // ����������
        if (m_satinModels == null || m_satinModels.Count == 0)
        {
            Debug.LogError("Satin models list is empty or not assigned.");
        }

        // �����_���Ń��f����I��
        int randomIndex = Random.Range(0, m_satinModels.Count);
        GameObject selectedModel = m_satinModels[randomIndex];
        // �I���������f�����C���X�^���X��
        GameObject instance = Instantiate(selectedModel,  transform.position, transform.rotation, transform);

        // ���݂̃��f����ݒ�
        m_currentModel = instance;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
