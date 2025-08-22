using UnityEngine;

/// <summary>
///  �z���̊j
/// </summary>
public class FeelingCore : MonoBehaviour 
{


    private EmotionCurrent m_emotionCurrent; // �z���̊���^�C�v

    [SerializeField] MeshRenderer m_meshRenderer;

    Carryable m_carriable; // Carriable�R���|�[�l���g���Q�Ƃ��邽�߂̕ϐ�


    public MeshRenderer MeshRenderer
    {
        get { return m_meshRenderer; }
    }

    /// <summary>
    /// Awake���\�b�h
    /// </summary>
    private void Awake()
    {
        m_emotionCurrent = GetComponent<EmotionCurrent>(); // EmotionCurrent�R���|�[�l���g���擾
        if (m_emotionCurrent == null)
        {
            Debug.LogError("EmotionCurrent �� null �ł�");
        }

        if (m_meshRenderer == null)
        {
            m_meshRenderer = GetComponent<MeshRenderer>();
        }

        m_carriable = GetComponent<Carryable>(); // Carryable�R���|�[�l���g���擾
                                                 // �o�^
        if (m_carriable)
            m_carriable.SetOnDropAction(OnDrop); // �u���Ƃ��̏�����ݒ�  
    }

    private void Start()
    {
      
    }



    /// <summary>
    /// �Q�[���I�u�W�F�N�g�̃A�N�e�B�u��Ԃ�ݒ肷��
    /// </summary>
    /// <param name="isActive"></param>
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive); // �Q�[���I�u�W�F�N�g�̃A�N�e�B�u��Ԃ�ݒ�
    }

    public EmotionCurrent.Type GetEmotionType()
    {
        return m_emotionCurrent.CurrentType; // ���݂̊���^�C�v���擾
    }

    public void SetEmotionType(EmotionCurrent.Type type)
    {
        m_emotionCurrent.CurrentType = type; // ����^�C�v��ݒ�
    }


    public void OnConnectEvent(EmotionCurrent emotionCurrent)
    {
    }

    /// <summary>
    /// �z���̊j��z�u���鏈��
    /// </summary>
    /// <param name="placementPos"></param>
    public void OnDrop(GridPos placementPos)
    {
        var map = MapData.GetInstance; // MapData�̃C���X�^���X���擾
        var stageGridData = map.GetStageGridData(); // �X�e�[�W�O���b�h�f�[�^���擾

        // �z�u�\��ӏ��̃^�C�����擾
        var tile = stageGridData.GetTileObject(placementPos);

        if (tile.stageBlock != null && tile.stageBlock.GetBlockType() == StageBlock.BlockType.FEELING_SLOT)
        {
            var feelingSlot = tile.gameObject.GetComponent<FeelingSlot>();
            if (feelingSlot != null)
            {
                // �z���̊j��z�u����X���b�g�Ɋ���^�C�v��ݒ�
                feelingSlot.InsertCore(this);
            }
            else
            {
                Debug.LogError("FeelingSlot �R���|�[�l���g��������܂���B");
            }
            return;
        }

        gameObject.SetActive(true); // �I�u�W�F�N�g��\������
        var stageBlock = GetComponent<StageBlock>();
        if (stageBlock == null)
        {
            Debug.LogError("StageBlock �R���|�[�l���g��������܂���B");
            return;
        }
        stageBlock.UpdatePosition(placementPos); // StageBlock�̈ʒu���X�V
        // �O���b�h�f�[�^�ɖȖу{�[����z�u
        MapData.GetInstance.GetStageGridData().TryPlaceTileObject(placementPos, gameObject);
    }



}
