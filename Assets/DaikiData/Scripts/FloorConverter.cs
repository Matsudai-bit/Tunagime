//using UnityEngine;

///// <summary>
///// 層変換
///// </summary>
//public class FloorConverter : MonoBehaviour
//{
//    [SerializeField] private AmidaManager m_amidaManager;

//    [SerializeField] private MapData m_map;

//    [SerializeField] private FloorBlockGenerator m_amidaFloorBlockGenerator;

//    /// <summary>
//    /// トップ層からあみだ層への変換
//    /// </summary>
//    /// <param name="amidaBlock"></param>
//    /// <returns></returns>
//    public bool ConvertTopToAmidaFloor(GameObject amidaBlockObj)
//    {
//        AmidaTubeBlock amidaTubeBlock = amidaBlockObj.GetComponent<AmidaTubeBlock>();

//        GridPos gridPos = amidaBlockObj.GetComponent<StageBlock>().GetGridPos();
//        if (m_map.CheckInnerGridPos(gridPos) == false) return false;

//        // あみだ層に追加要求
//        if (m_amidaManager.RequestAddGridAmidaTube(gridPos, amidaTubeBlock.GetAmidaTube()) == false)
//            return false;
//        // あみだ床の取得
//        GameObject[,] amidaFloorGrid = m_map.GetStageGridData().GetAmidaFloorBlockGrid();

//        if (amidaFloorGrid[gridPos.y, gridPos.x] == null) return false;

//        // 現在の床を活動停止させる
//        amidaFloorGrid[gridPos.y, gridPos.x].SetActive(false);

//        // 床グリッドの登録
//        amidaFloorGrid[gridPos.y, gridPos.x] = amidaTubeBlock.GetFloorBlock();

//        // 新しく登録
//        m_map.GetStageGridData().SetAmidaFloorBlockGrid(amidaFloorGrid);

//        // 座標の設定
//        Vector3 amidaFloorPos = m_map.ConvertGridToWorldPos(gridPos.x, gridPos.y);

//        // サイズをコライダー基準
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
//    /// あみだ層からトップ層への変換
//    /// </summary>
//    /// <param name="amidaBlock"></param>
//    /// <returns></returns>
//    public bool ConvertAmidaFloorToTopFloor(GameObject amidaBlockObj)
//    {
//        AmidaTubeBlock amidaTubeBlock = amidaBlockObj.GetComponent<AmidaTubeBlock>();

//        GridPos gridPos = amidaBlockObj.GetComponent<StageBlock>().GetGridPos();
//        if (m_map.CheckInnerGridPos(gridPos) == false) return false;

//        // あみだグリッドから削除する
//        m_amidaManager.RequestRemoveGridAmidaTube(gridPos);

//        // トップ層にする
//        Vector3 topFloorPos = m_map.ConvertGridToWorldPos(gridPos.x, gridPos.y);
//        topFloorPos += new Vector3(
//            0.0f,
//            m_map.GetFloorManager().GetFloorTopPartPosY(FloorManager.FloorType.TOP) + amidaTubeBlock.GetFloorBlock().GetComponent<Collider>().bounds.size.y / 2.0f,
//            0.0f);

//        // 座標の設定
//        amidaTubeBlock.transform.position = topFloorPos;

//        // あみだ床の取得
//        GameObject[,] amidaFloorGrid = m_map.GetStageGridData().GetAmidaFloorBlockGrid();


//        amidaFloorGrid[gridPos.y, gridPos.x] = m_amidaFloorBlockGenerator.GenerateFloor(
//            gridPos,
//            m_map,
//            m_map.GetFloorManager().GetFloorTopPartPosY(FloorManager.FloorType.AMIDA) - amidaTubeBlock.GetFloorBlock().transform.localScale.y / 2.0f);

//        // 新しく登録
//        m_map.GetStageGridData().SetAmidaFloorBlockGrid(amidaFloorGrid);

//        amidaBlockObj.GetComponent<StageBlock>().SetFloorType(FloorManager.FloorType.TOP);


//        return true;
//    }
//}
