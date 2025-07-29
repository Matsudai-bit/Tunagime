using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���f���̊K�w�ɕt����
/// </summary>
public class YarnMeshChanger : MonoBehaviour
{
    private GameObject m_currentMeshInstance; // ���݂̃��b�V��
    private AmidaTube.State m_currentMeshType = AmidaTube.State.NONE; // ���݂̃��b�V���̃^�C�v

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

        // ���ɓ����^�C�v�̃��b�V�����ݒ肳��Ă���ꍇ�͉������Ȃ�
        if (m_currentMeshType == type)
        {
            return; 
        }

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

            m_currentMeshType = type; // ���݂̃��b�V���^�C�v���X�V

            Debug.Log($"'{gameObject.name}' �̃��b�V���� '{type}' �ɐ؂�ւ��܂����B");
        }
        else
        {
            Debug.LogWarning($"'{gameObject.name}' �̃��b�V����؂�ւ��ł��܂���ł���: '{type}' ��������܂���B");
        }
    }

    /// <summary>
    /// �w�肳�ꂽ�}�e���A���^�C�v�̃}�e���A�����擾���܂��B
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Material GetMaterial(YarnMaterialGetter.MaterialType type)
    {
        YarnMaterialGetter materialGetter = m_currentMeshInstance.GetComponent<YarnMaterialGetter>();
        if (materialGetter != null)
        {
            var meshRenderer = materialGetter.GetMeshRenderer(type);
            if (meshRenderer)
            {
                return meshRenderer.material;
            }
            else
            {
                Debug.LogWarning($"'{gameObject.name}' �̃}�e���A���^�C�v '{type}' ��������܂���B");
            }
        }
        else
        {
            Debug.LogWarning($"'{gameObject.name}' ��YarnMaterialGetter��������܂���B");
        }
        return null;
    }

    public EmotionCurrent.Type GetEmotionType(YarnMaterialGetter.MaterialType type)
    {
        YarnMaterialGetter materialGetter = m_currentMeshInstance.GetComponent<YarnMaterialGetter>();
        if (materialGetter != null)
        {
            return materialGetter.GetEmotionType(type);
        }
        else
        {
            Debug.LogWarning($"'{gameObject.name}' ��YarnMaterialGetter��������܂���B");
        }
        return EmotionCurrent.Type.NONE;
    }

    /// <summary>
    /// �w�肳�ꂽ�}�e���A���Ƀ��b�V���̃}�e���A����ύX���܂��B
    /// </summary>
    /// <param name="material"></param>
    /// <param name="type"></param>
    public void ChangeMaterial(Material material, EmotionCurrent.Type changeEmotionType, YarnMaterialGetter.MaterialType type)
    {
        if (m_currentMeshInstance == null)
        {
            Debug.LogWarning("���݂̃��b�V���C���X�^���X������܂���B���b�V����ݒ肵�Ă��������B");
            return;
        }
        YarnMaterialGetter materialGetter = m_currentMeshInstance.GetComponent<YarnMaterialGetter>();
        if (materialGetter != null)
        {
            // �}�e���A����ύX
            MeshRenderer meshRenderer = materialGetter.GetMeshRenderer(type);
            if (meshRenderer == null)
            {
                Debug.LogWarning($"'{gameObject.name}' �̃}�e���A���^�C�v '{type}' ��MeshRenderer��������܂���B");
                return;
            }
            meshRenderer.material = material;

            // �z���̎�ނ�ݒ�
             materialGetter.SetEmotionType(type, changeEmotionType) ;
          



            Debug.Log($"'{gameObject.name}' �̃��b�V���̃}�e���A���� '{type}' �ɕύX���܂����B");
        }
        else
        {
            Debug.LogWarning($"'{gameObject.name}' ��YarnMaterialGetter��������܂���B���b�V���̃}�e���A����ύX�ł��܂���B");
        }
    }
}
