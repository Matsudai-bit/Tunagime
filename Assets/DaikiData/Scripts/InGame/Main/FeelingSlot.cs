using UnityEngine;

/// <summary>
/// �z���̌^
/// </summary>
public class FeelingSlot : MonoBehaviour 
{
    [SerializeField] private FeelingCore m_feelingCore; // �z���̊j

    /// <summary>
    /// �X�^�[�g���\�b�h
    /// </summary>

    private void Start()
    {
        if (m_feelingCore.GetEmotionType() != EmotionCurrent.Type.NONE)
        {
            var coreMaterial = MaterialLibrary.GetInstance.GetMaterial(MaterialLibrary.MaterialGroup.CORE, m_feelingCore.GetEmotionType()); // �}�e���A�����C�u�����̏�����
            m_feelingCore.MeshRenderer.material = coreMaterial; // �z���̊j�̃}�e���A����ݒ�
        }
        else
        {
            m_feelingCore.SetActive(false); // �z���̊j���ݒ肳��Ă��Ȃ��ꍇ�͔�A�N�e�B�u�ɂ���
        }
        
    }



    /// <summary>
    /// ���݂̊���^�C�v���擾����
    /// </summary>
    /// <returns>�z���̎��</returns>
    public EmotionCurrent.Type GetEmotionType()
    {
        return m_feelingCore.GetEmotionType(); // �z���̗���̎�ނ��擾
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

    /// <summary>
    /// �z���̊j��}������
    /// </summary>
    /// <param name="feelingCore"></param>
    public void InsertCore(FeelingCore feelingCore)
    {
        if (m_feelingCore.GetEmotionType() != EmotionCurrent.Type.NONE)
        {
            Debug.LogError("���ɑz���̊j���}������Ă��܂��B");
            return; // ���ɑz���̊j���}������Ă���ꍇ�͏����𒆒f
        }

        m_feelingCore.MeshRenderer.material = feelingCore.MeshRenderer.material;    // �z���̊j�̃}�e���A����ݒ�
        m_feelingCore.SetEmotionType(feelingCore.GetEmotionType());                 // �z���̊j�̊���^�C�v��ݒ�
        m_feelingCore.SetActive(true);                                               // �z���̊j���A�N�e�B�u�ɂ���
    }

    public FeelingCore GetFeelingCore()
    {
        return m_feelingCore; // �z���̊j���擾
    }

}
