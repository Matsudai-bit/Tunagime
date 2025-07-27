using UnityEngine;

/// <summary>
/// �z���̌^
/// </summary>
public class FeelingSlot : MonoBehaviour
{
    [SerializeField] private FeelingCore m_feelingCore; // �z���̊j

    [Header("�}�e���A����ނ̐ݒ�")]
    [SerializeField] private YarnMaterialLibrary.MaterialType m_materialType; // �}�e���A���^�C�v

    /// <summary>
    /// �X�^�[�g���\�b�h
    /// </summary>

    private void Start()
    {
        if (m_materialType != YarnMaterialLibrary.MaterialType.NONE)
        {
            m_feelingCore.SetMaterial(m_materialType); // �����}�e���A����ݒ�
        }
    }


    /// <summary>
    /// �z���̊j�̃}�e���A�����擾����
    /// </summary>
    /// <returns></returns>
    public Material GetCoreMaterial()
    {
        return m_feelingCore.GetMaterial(); // �z���̊j�̃}�e���A�����擾
    }

    /// <summary>
    /// �z���̊j�̃}�e���A����ݒ肷��
    /// </summary>
    /// <param name="material"></param>
    public void SetCoreMaterial(YarnMaterialLibrary.MaterialType materialType)
    {
        m_feelingCore.SetMaterial(materialType); // �z���̊j�̃}�e���A����ݒ�
    }

    /// <summary>
    /// �X�e�[�W�u���b�N�̎擾
    /// </summary>
    public StageBlock StageBlock
    {
        get
        {
            return GetComponent<StageBlock>(); // StageBlock�R���|�[�l���g���擾
        }
    }
}
