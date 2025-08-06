using UnityEngine;

/// <summary>
/// �I�_�ɂ���^
/// </summary>
public class TerminusFeelingSlot : MonoBehaviour
{
    private StageBlock m_stageBlock;    // �X�e�[�W�u���b�N

    private FeelingSlot m_feelingSlot; // �z���̌^


    private void Awake()
    {
        // �X�e�[�W�u���b�N���擾
        m_stageBlock = GetComponent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("TerminusFeelingSlot requires a StageBlock component.");
        }

        // �z���̌^���擾
        m_feelingSlot = GetComponent<FeelingSlot>();
        if (m_feelingSlot == null)
        {
            Debug.LogError("TerminusFeelingSlot requires a FeelingSlot component.");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �q�����Ă��邩�ǂ���
    /// </summary>
    /// <returns></returns>
    public bool IsConnected()
    {
        //�@�O���b�h�f�[�^�̎擾
        StageGridData gridData = MapData.GetInstance.GetStageGridData();

        //�@�O���b�h�ʒu�̎擾
        GridPos gridPos = m_stageBlock.GetGridPos();

        GridPos checkGridPos = gridPos + new GridPos(-1, 0); // ���ׂ̃O���b�h���`�F�b�N

        var amidaTube =  gridData.GetAmidaTube(checkGridPos);

        if (amidaTube.GetEmotionType(YarnMaterialGetter.MaterialType.OUTPUT) == m_feelingSlot.GetEmotionType())
        {
            // �I�_�̊���ƈ�v���Ă���ꍇ�͌q�����Ă���
            return true;
        }


        return false;
    }
}
