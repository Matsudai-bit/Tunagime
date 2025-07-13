using UnityEngine;

/// <summary>
/// フロア(層)管理
/// </summary>
public class FloorManager : MonoBehaviour
{

    ///// <summary>
    ///// フロアの名称
    ///// </summary>
    //public enum FloorType
    //{
    //    AMIDA,  // あみだ層
    //    TOP     // トップ層
    //}

    ///// <summary>
    ///// 床の上部のY座標
    ///// </summary>
    //private struct FloorTopPartPosYData
    //{
    //    public float topFloor;
    //    public float amidaFloor;
    //}

    //[SerializeField]　private StageGridData m_stageGridData;

    //FloorTopPartPosYData m_floorTopPartPosYData;


    ///// <summary>
    ///// 指定した層の上部のY座標の取得
    ///// </summary>
    ///// <param name="floorType"></param>
    ///// <returns></returns>
    //public float GetFloorTopPartPosY(FloorType floorType)
    //{
    //    switch (floorType)
    //    {
    //        case FloorType.AMIDA:
    //            return m_floorTopPartPosYData.amidaFloor;
    //        case FloorType.TOP:
    //            return m_floorTopPartPosYData.topFloor;

    //        default:
    //            return 0.0f;
    //    }
    //}

    ///// <summary>
    ///// 指定した層の上部のY座標の設定
    ///// </summary>
    ///// <param name="floorType"></param>
    ///// <returns></returns>
    //public void SetFloorTopPartPosY(FloorType floorType, float topPartPosY)
    //{
    //    switch (floorType)
    //    {
    //        case FloorType.AMIDA:
    //            m_floorTopPartPosYData.amidaFloor = topPartPosY;
    //            break;
    //        case FloorType.TOP:
    //            m_floorTopPartPosYData.topFloor = topPartPosY;
    //            break;
    //    }
    //}


    ///// <summary>
    ///// 指定したフロアのグリッドデータの取得
    ///// </summary>
    ///// <param name="floorType"></param>
    ///// <returns></returns>
    //public GameObject[,] GetFloor(FloorType floorType)
    //{
    //    switch (floorType)
    //    {
    //        case FloorType.AMIDA:
    //            return m_stageGridData.GetAmidaFloorBlockGrid();
    //        case FloorType.TOP:
    //            return m_stageGridData.GetTopFloorBlockGrid();

    //        default:
    //            return null;
    //    }
    //}

    ///// <summary>
    ///// 指定したフロアのグリッドデータの設定
    ///// </summary>
    ///// <param name="floorType"></param>
    ///// <param name="grid"></param>
    //public void SetFloor(FloorType floorType, GameObject[,] grid)
    //{
    //    switch (floorType)
    //    {
    //        case FloorType.AMIDA:
    //            m_stageGridData.SetAmidaFloorBlockGrid(grid);
    //            break;
    //        case FloorType.TOP:
    //            m_stageGridData.SetTopFloorBlockGrid(grid);
    //            break;
    //    }
    //}

    ///// <summary>
    ///// 指定したフロアのギミックグリッドデータの取得
    ///// </summary>
    ///// <param name="floorType"></param>
    ///// <returns></returns>
    //public GameObject[,] GetGimmickFloor(FloorType floorType)
    //{
    //    switch (floorType)
    //    {
    //        case FloorType.AMIDA:
    //            return m_stageGridData.GetAmidaTubeGrid();
    //        case FloorType.TOP:
    //            return m_stageGridData.GetTopGimmickBlockGrid();

    //        default:
    //            return null;
    //    }
    //}

    ///// <summary>
    ///// 指定したフロアのギミックグリッドデータの設定
    ///// </summary>
    ///// <param name="floorType"></param>
    ///// <param name="grid"></param>
    //public void SetGimmickFloor(FloorType floorType, GameObject[,] grid)
    //{
    //    switch (floorType)
    //    {
    //        case FloorType.AMIDA:
    //            m_stageGridData.SetAmidaTubeGrid(grid);
    //            break;
    //        case FloorType.TOP:
    //            m_stageGridData.SetTopGimmickBlockGrid(grid);
    //            break;
    //    }
    //}
}
