using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

/// <summary>
/// �Q�[�����Ŏ����^�щ\�ȃI�u�W�F�N�g�B
/// </summary>
public class Carryable : MonoBehaviour
{
    
    private StageBlock m_stageBlock; // ����Carryable��������StageBlock

    private void Awake()
    {
        // StageBlock�R���|�[�l���g���擾
        m_stageBlock = GetComponent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("Carryable must be attached to a GameObject with a StageBlock component.");
        }
    }

    public StageBlock stageBlock
    {
        get { return m_stageBlock; }
    }

    /// <summary>
    /// ����Carryable�I�u�W�F�N�g���E�������B
    /// </summary>
    public void OnPickUp()
    {
        gameObject.SetActive(false); // �I�u�W�F�N�g���\���ɂ���
    }

    /// <summary>
    /// ����Carryable�I�u�W�F�N�g��u�������B
    /// </summary>
    public void OnDrop( GridPos  placementPos)
    {
        gameObject.SetActive(true); // �I�u�W�F�N�g��\������
        stageBlock.UpdatePosition(placementPos); // StageBlock�̈ʒu���X�V

        // �O���b�h�f�[�^�ɖȖу{�[����z�u
        MapData.GetInstance.GetStageGridData().TryPlaceTileObject(placementPos, gameObject);
    }
}
