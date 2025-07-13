//using UnityEngine;

///// <summary>
///// �w�ϊ�
///// </summary>
//public class FloorConverter : MonoBehaviour
//{
//    [SerializeField] private AmidaManager m_amidaManager;

//    [SerializeField] private MapData m_map;

//    [SerializeField] private FloorBlockGenerator m_amidaFloorBlockGenerator;

//    /// <summary>
//    /// �g�b�v�w���炠�݂��w�ւ̕ϊ�
//    /// </summary>
//    /// <param name="amidaBlock"></param>
//    /// <returns></returns>
//    public bool ConvertTopToAmidaFloor(GameObject amidaBlockObj)
//    {
//        AmidaTubeBlock amidaTubeBlock = amidaBlockObj.GetComponent<AmidaTubeBlock>();

//        GridPos gridPos = amidaBlockObj.GetComponent<StageBlock>().GetGridPos();
//        if (m_map.CheckInnerGridPos(gridPos) == false) return false;

//        // ���݂��w�ɒǉ��v��
//        if (m_amidaManager.RequestAddGridAmidaTube(gridPos, amidaTubeBlock.GetAmidaTube()) == false)
//            return false;
//        // ���݂����̎擾
//        GameObject[,] amidaFloorGrid = m_map.GetStageGridData().GetAmidaFloorBlockGrid();

//        if (amidaFloorGrid[gridPos.y, gridPos.x] == null) return false;

//        // ���݂̏���������~������
//        amidaFloorGrid[gridPos.y, gridPos.x].SetActive(false);

//        // ���O���b�h�̓o�^
//        amidaFloorGrid[gridPos.y, gridPos.x] = amidaTubeBlock.GetFloorBlock();

//        // �V�����o�^
//        m_map.GetStageGridData().SetAmidaFloorBlockGrid(amidaFloorGrid);

//        // ���W�̐ݒ�
//        Vector3 amidaFloorPos = m_map.ConvertGridToWorldPos(gridPos.x, gridPos.y);

//        // �T�C�Y���R���C�_�[�
//        float scaleY = amidaTubeBlock.GetFloorBlock().GetComponent<Collider>().bounds.size.y;

//        amidaFloorPos += new Vector3(
//            0.0f, 
//            m_map.GetFloorManager().GetFloorTopPartPosY(FloorManager.FloorType.AMIDA) - scaleY / 2.0f, 
//            0.0f);

//        amidaTubeBlock.transform.position = amidaFloorPos;

//        amidaBlockObj.GetComponent<StageBlock>().SetFloorType(FloorManager.FloorType.AMIDA);

//        return true;
//    }

//    /// <summary>
//    /// ���݂��w����g�b�v�w�ւ̕ϊ�
//    /// </summary>
//    /// <param name="amidaBlock"></param>
//    /// <returns></returns>
//    public bool ConvertAmidaFloorToTopFloor(GameObject amidaBlockObj)
//    {
//        AmidaTubeBlock amidaTubeBlock = amidaBlockObj.GetComponent<AmidaTubeBlock>();

//        GridPos gridPos = amidaBlockObj.GetComponent<StageBlock>().GetGridPos();
//        if (m_map.CheckInnerGridPos(gridPos) == false) return false;

//        // ���݂��O���b�h����폜����
//        m_amidaManager.RequestRemoveGridAmidaTube(gridPos);

//        // �g�b�v�w�ɂ���
//        Vector3 topFloorPos = m_map.ConvertGridToWorldPos(gridPos.x, gridPos.y);
//        topFloorPos += new Vector3(
//            0.0f,
//            m_map.GetFloorManager().GetFloorTopPartPosY(FloorManager.FloorType.TOP) + amidaTubeBlock.GetFloorBlock().GetComponent<Collider>().bounds.size.y / 2.0f,
//            0.0f);

//        // ���W�̐ݒ�
//        amidaTubeBlock.transform.position = topFloorPos;

//        // ���݂����̎擾
//        GameObject[,] amidaFloorGrid = m_map.GetStageGridData().GetAmidaFloorBlockGrid();


//        amidaFloorGrid[gridPos.y, gridPos.x] = m_amidaFloorBlockGenerator.GenerateFloor(
//            gridPos,
//            m_map,
//            m_map.GetFloorManager().GetFloorTopPartPosY(FloorManager.FloorType.AMIDA) - amidaTubeBlock.GetFloorBlock().transform.localScale.y / 2.0f);

//        // �V�����o�^
//        m_map.GetStageGridData().SetAmidaFloorBlockGrid(amidaFloorGrid);

//        amidaBlockObj.GetComponent<StageBlock>().SetFloorType(FloorManager.FloorType.TOP);


//        return true;
//    }
//}
