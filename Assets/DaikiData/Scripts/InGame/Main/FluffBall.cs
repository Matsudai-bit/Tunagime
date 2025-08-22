using UnityEngine;

public class FluffBall : MonoBehaviour
{
    Carryable m_carriable; // Carriable�R���|�[�l���g���Q�Ƃ��邽�߂̕ϐ�
    StageBlock m_stageBlock;

    private void Awake()
    {
        // Carryable�R���|�[�l���g���擾
        m_carriable = GetComponent<Carryable>();
        if (m_carriable == null)
        {
            Debug.LogError("FluffBall must be attached to a GameObject with a Carryable component.");
        }

        // StageBlock�R���|�[�l���g���擾
        m_stageBlock = GetComponent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("FluffBall must be attached to a GameObject with a StageBlock component.");
        }

        m_carriable.SetOnDropAction(OnDrop); // �u���Ƃ��̏�����ݒ�
    }

    private void OnDrop(GridPos placementPos)
    {
        gameObject.SetActive(true); // �I�u�W�F�N�g��\������
        m_stageBlock.UpdatePosition(placementPos); // StageBlock�̈ʒu���X�V

        // �O���b�h�f�[�^�ɖȖу{�[����z�u
        MapData.GetInstance.GetStageGridData().TryPlaceTileObject(placementPos, gameObject);

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
