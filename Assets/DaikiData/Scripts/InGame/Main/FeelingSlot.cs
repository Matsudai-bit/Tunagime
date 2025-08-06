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
}
