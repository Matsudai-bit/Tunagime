using UnityEngine;

/// <summary>
///  �z���̊j
/// </summary>
public class FeelingCore : MonoBehaviour
{

    private YarnMaterialLibrary.MaterialType m_materialType; // �}�e���A���̃^�C�v

    /// <summary>
    /// Awake���\�b�h
    /// </summary>
    private void Awake()
    {
    }

    /// <summary>
    /// �}�e���A�����擾����
    /// </summary>
    /// <returns></returns>
    public Material GetMaterial()
    {
        return YarnMaterialLibrary.Instance.GetMaterial(m_materialType); // �}�e���A�����C�u��������}�e���A�����擾
    }

    /// <summary>
    /// �}�e���A����ݒ肷��
    /// </summary>
    public void SetMaterial(YarnMaterialLibrary.MaterialType materialType)
    {
        m_materialType = materialType; // �}�e���A����ݒ�
    }

    /// <summary>
    /// �Q�[���I�u�W�F�N�g�̃A�N�e�B�u��Ԃ�ݒ肷��
    /// </summary>
    /// <param name="isActive"></param>
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive); // �Q�[���I�u�W�F�N�g�̃A�N�e�B�u��Ԃ�ݒ�
    }


}
