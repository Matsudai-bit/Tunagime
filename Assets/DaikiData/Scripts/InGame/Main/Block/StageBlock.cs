using UnityEngine;

/// <summary>
/// �X�e�[�W�u���b�N�Ɋւ���N���X
/// </summary>
public class StageBlock : MonoBehaviour
{
    [SerializeField] private GridPos m_gridPos;

    [SerializeField] private BlockType m_blockType = BlockType.NONE; // �u���b�N�̎��

    [SerializeField] private bool m_canInteract = false; // �C���^���N�g�\���ǂ���
    public enum BlockType
    {
        NONE = 0, // �����Ȃ�

        FELT_BLOCK, // �t�F���g�u���b�N
        FLUFF_BALL, // �ю���
        FEELING_SLOT, // �G���[�V�����X���b�g
        AMIDA_TUBE, // ���݂��`���[�u
        CURTAIN, // �J�[�e��
        SATIN_FLOOR, // �T�e����
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
    /// ����������
    /// </summary>
    /// <param name="map"></param>
    public void Initialize(GridPos gridPos)
    {
        MapData map = MapData.GetInstance;

        // �O���b�h���W�̐ݒ�
        m_gridPos = gridPos;


       // �O���b�h���W����ϊ�
        transform.position = map.ConvertGridToWorldPos(gridPos.x, gridPos.y);
        transform.position += new Vector3(0.0f, MapData.GetInstance.GetCommonData().BaseTilePosY );




    }

    /// <summary>
    /// �O���b�h�ʒu��ݒ肷��
    /// </summary>
    /// <param name="gridPos">�ݒ肷��O���b�h�ʒu</param>
    public void SetGridPos(GridPos gridPos)
    {
        m_gridPos = gridPos;
    }


    /// <summary>
    /// �O���b�h�ʒu���擾����
    /// </summary>
    /// <returns>�O���b�h�ʒu</returns>
    public  GridPos GetGridPos()
    {
        return m_gridPos;
    }



    /// <summary>
    /// ���W�̍X�V
    /// </summary>
    /// <param name="gridPos"></param>
    public void UpdatePosition(GridPos gridPos)
    {
        MapData map = MapData.GetInstance;


        if (map.CheckInnerGridPos(gridPos) == false) return;

        var tileData = MapData.GetInstance.GetStageGridData().GetTileData;

        // �����I�u�W�F�N�g������ꍇ�ȉ��̏������΂�
        if (tileData[gridPos.y, gridPos.x].tileObject.gameObject != null) return;
        

        // �V�����W
        Vector3 newPos = map.ConvertGridToWorldPos(gridPos.x, gridPos.y);

        // ���݂���ꏊ��null�ɂ���
        tileData[m_gridPos.y, m_gridPos.x].tileObject.gameObject = null;

        // �ړ������폜
        MapData.GetInstance.GetStageGridData().RemoveGridDataGameObject(m_gridPos);

        //�O���b�h���W�̍X�V
        m_gridPos = gridPos;

        // ���W�̍X�V
        transform.position = newPos + new Vector3(0.0f, transform.position.y, 0.0f);

        // �ړ���̍��W�Ɉړ�����
        MapData.GetInstance.GetStageGridData().TryPlaceTileObject(m_gridPos, gameObject);


    }

    public void SetActive(bool activeSelf)
    {
        gameObject.SetActive(activeSelf);
    }

    public BlockType GetBlockType()
    {
        return m_blockType;
    }

    public void SetBlockType(BlockType blockType)
    {
        m_blockType = blockType;
    }

    /// <summary>
    /// �C���^���N�g�\���ǂ������擾����
    /// </summary>
    /// <returns></returns>
    public bool CanInteract()
    {
        return m_canInteract;
    }
}