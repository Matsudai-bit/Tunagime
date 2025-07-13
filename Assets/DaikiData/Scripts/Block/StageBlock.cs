using UnityEngine;

/// <summary>
/// �X�e�[�W�u���b�N�Ɋւ���N���X
/// </summary>
public class StageBlock : MonoBehaviour
{
    [SerializeField] private GridPos m_gridPos;


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
        transform.position += new Vector3(0.0f, MapData.GetInstance.GetCommonData().BaseTilePosY + transform.localScale.y / 2.0f, 0.0f);




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

        var tileData = MapData.GetInstance.GetStageGridData().GetTileData();

        // �����I�u�W�F�N�g������ꍇ�ȉ��̏������΂�
        if (tileData[gridPos.y, gridPos.x].tileObject.gameObject != null) return;
        

        // �V�����W
        Vector3 newPos = map.ConvertGridToWorldPos(gridPos.x, gridPos.y);

        // ���݂���ꏊ��null�ɂ���
        tileData[m_gridPos.y, m_gridPos.x].tileObject.gameObject = null;

        //�O���b�h���W�̍X�V
        m_gridPos = gridPos;

        // ���W�̍X�V
        transform.position = newPos + new Vector3(0.0f, transform.position.y, 0.0f);

        // �ړ���̍��W�Ɉړ�����
        tileData[m_gridPos.y, m_gridPos.x].tileObject.gameObject = gameObject;


    }
}