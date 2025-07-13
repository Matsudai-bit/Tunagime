using UnityEngine;

/// <summary>
/// ステージのグリッド管理
/// </summary>
public class StageGridData : MonoBehaviour
{
    //private GameObject[,] m_amidaFloorBlockGrid;   // あみだ層のブロックグリッド
    //private GameObject[,] m_topFloorBlockGrid;     // トップ層のブロックグリッド
    //private GameObject[,] m_amidaTubeGrid;         // あみだチューブのグリッド
    //private GameObject[,] m_topGimmickBlockGrid;   // ギミックブロックのグリッド

    //private GameObject[,] m_wallGrid;               // 壁のグリッド

    private TileData[,] m_tileData = new TileData[,] { }; //　タイルデータ
                                    //　
    public TileData[,]  GetTileData()
    {
        return m_tileData;
    }

    public void SetData(TileData[,] tileData)
    {
        m_tileData = tileData;
    }

    public void  Initialize(int gridWidth, int gridHeight)
    {
        m_tileData = new TileData[gridHeight, gridWidth];
    }

    ///// <summary>
    ///// あみだ層のブロックグリッドを設定する
    ///// </summary>
    ///// <param name="grid">設定するグリッド</param>
    //public void SetAmidaFloorBlockGrid(GameObject[,] grid)
    //{
    //    m_amidaFloorBlockGrid = grid;
    //}

    ///// <summary>
    ///// あみだ層のブロックグリッドを取得する
    ///// </summary>
    ///// <returns>あみだ層のブロックグリッド</returns>
    //public GameObject[,] GetAmidaFloorBlockGrid()
    //{
    //    return m_amidaFloorBlockGrid;
    //}

    ///// <summary>
    ///// トップ層のブロックグリッドを設定する
    ///// </summary>
    ///// <param name="grid">設定するグリッド</param>
    //public void SetTopFloorBlockGrid(GameObject[,] grid)
    //{
    //    m_topFloorBlockGrid = grid;
    //}

    ///// <summary>
    ///// トップ層のブロックグリッドを取得する
    ///// </summary>
    ///// <returns>トップ層のブロックグリッド</returns>
    //public GameObject[,] GetTopFloorBlockGrid()
    //{
    //    return m_topFloorBlockGrid;
    //}

    ///// <summary>
    ///// あみだチューブのグリッドを設定する
    ///// </summary>
    ///// <param name="grid">設定するグリッド</param>
    //public void SetAmidaTubeGrid(GameObject[,] grid)
    //{
    //    m_amidaTubeGrid = grid;
    //}

    ///// <summary>
    ///// あみだチューブのグリッドを取得する
    ///// </summary>
    ///// <returns>あみだチューブのグリッド</returns>
    //public GameObject[,] GetAmidaTubeGrid()
    //{
    //    return m_amidaTubeGrid;
    //}

    ///// <summary>
    ///// ギミックブロックのグリッドを設定する
    ///// </summary>
    ///// <param name="grid">設定するグリッド</param>
    //public void SetTopGimmickBlockGrid(GameObject[,] grid)
    //{
    //    m_topGimmickBlockGrid = grid;
    //}

    ///// <summary>
    ///// ギミックブロックのグリッドを取得する
    ///// </summary>
    ///// <returns>ギミックブロックのグリッド</returns>
    //public GameObject[,] GetTopGimmickBlockGrid()
    //{
    //    return m_topGimmickBlockGrid;
    //}

    ///// <summary>
    ///// 壁のグリッドを設定する
    ///// </summary>
    ///// <param name="grid">設定するグリッド</param>
    //public void SetWallGrid(GameObject[,] grid)
    //{
    //    m_wallGrid = grid;
    //}

    ///// <summary>
    ///// 壁のグリッドを取得する
    ///// </summary>
    ///// <returns>壁のグリッド</returns>
    //public GameObject[,] GetWallGrid()
    //{
    //    return m_wallGrid;
    //}
}