using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���f���̊K�w�ɕt����
/// </summary>
public class YarnMeshChanger : MonoBehaviour
{
    private GameObject m_currentMeshInstance; // ���݂̃��b�V��

    private Dictionary<AmidaTube.State, GameObject> m_usedMeshes = new Dictionary<AmidaTube.State, GameObject>(); // �g�p�������b�V����ۊǂ��Ă���(�������������悭���邽��)

    // �������b�V���Ƃ��Đݒ肵����AmidaTube.State (�C���X�y�N�^�[�Ńh���b�v�_�E������I��)
    [SerializeField] private AmidaTube.State m_initialShapeType = AmidaTube.State.NONE; // �f�t�H���g��None�ɂ���

    private void Start()
    {
        if (AmidaTube.State.NONE != m_initialShapeType)
            SetMesh(m_initialShapeType);   
    }

    /// <summary>
    /// �w�肳�ꂽAmidaTube.ShapeType�̃��b�V���ɐ؂�ւ��܂��B
    /// MeshLibrary����v���n�u���擾���A���݂̃I�u�W�F�N�g�̎q�Ƃ��ăC���X�^���X�����܂��B
    /// </summary>
    /// <param name="type">�؂�ւ��������b�V����AmidaTube.State</param>
    public void SetMesh(AmidaTube.State type)
    {
        if (m_currentMeshInstance != null)
        {
            m_currentMeshInstance.SetActive(false);
            
        }

        GameObject meshPrefab = YarnMeshLibrary.Instance.GetMeshPrefab(type); // AmidaTube.ShapeType�Ŏ擾

        if (meshPrefab != null)
        {
            // �����ɓo�^����Ă��Ȃ��ꍇ
            if (m_usedMeshes.ContainsKey(type) == false)
            {
                m_usedMeshes[type] = Instantiate(meshPrefab, transform);
            }

            m_currentMeshInstance = m_usedMeshes[type];

            m_currentMeshInstance.name = type.ToString(); // enum�̖��O��GameObject���ɂ���
            m_currentMeshInstance.transform.localPosition = Vector3.zero;
            m_currentMeshInstance.transform.localRotation = Quaternion.identity;
            m_currentMeshInstance.transform.localScale = Vector3.one;

            Debug.Log($"'{gameObject.name}' �̃��b�V���� '{type}' �ɐ؂�ւ��܂����B");
        }
        else
        {
            Debug.LogWarning($"'{gameObject.name}' �̃��b�V����؂�ւ��ł��܂���ł���: '{type}' ��������܂���B");
        }
    }
}
