//using NUnit.Framework;
//using UnityEngine;
//using System.Collections.Generic;


//public class WallGenerator : MonoBehaviour
//{
//    /// <summary>
//    /// 壁の生成用データ
//    /// </summary>
//    [System.Serializable]
//    public struct WallGenerationData
//    {
//        public GridPos pos;
//    }

//    List<GridPos> m_wallGenerationDataGrid = new List<GridPos>();

//    [SerializeField] private GameObject m_wallPrefab;    // あみだ
//    [SerializeField] private GameObject m_wallParent;    // あみだの親

//    static private Transform m_baseTrans;

//    private void Awake()
//    {
//        m_baseTrans = m_wallPrefab.transform;

//        // プロトタイプ用データ

//        // 左
//        m_wallGenerationDataGrid.Add(ConvertStageGridToLeftWallGrid(new GridPos(7, 10)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToLeftWallGrid(new GridPos(7, 8)));

//        m_wallGenerationDataGrid.Add(ConvertStageGridToLeftWallGrid(new GridPos(12, 10)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToLeftWallGrid(new GridPos(12, 9)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToLeftWallGrid(new GridPos(12, 8)));

//        // 上
//        m_wallGenerationDataGrid.Add(ConvertStageGridToTopWallGrid(new GridPos(7, 8)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToTopWallGrid(new GridPos(8, 8)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToTopWallGrid(new GridPos(9, 8)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToTopWallGrid(new GridPos(10, 8)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToTopWallGrid(new GridPos(11, 8)));

//        // 左
//        m_wallGenerationDataGrid.Add(ConvertStageGridToLeftWallGrid(new GridPos(14, 1)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToLeftWallGrid(new GridPos(14, 2)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToLeftWallGrid(new GridPos(14, 3)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToLeftWallGrid(new GridPos(14, 5)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToLeftWallGrid(new GridPos(14, 6)));

//        m_wallGenerationDataGrid.Add(ConvertStageGridToLeftWallGrid(new GridPos(20, 1)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToLeftWallGrid(new GridPos(20, 2)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToLeftWallGrid(new GridPos(20, 3)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToLeftWallGrid(new GridPos(20, 4)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToLeftWallGrid(new GridPos(20, 5)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToLeftWallGrid(new GridPos(20, 6)));

//        // 上
//        m_wallGenerationDataGrid.Add(ConvertStageGridToTopWallGrid(new GridPos(14, 1)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToTopWallGrid(new GridPos(15, 1)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToTopWallGrid(new GridPos(16, 1)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToTopWallGrid(new GridPos(17, 1)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToTopWallGrid(new GridPos(18, 1)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToTopWallGrid(new GridPos(19, 1)));

//        m_wallGenerationDataGrid.Add(ConvertStageGridToTopWallGrid(new GridPos(14, 7)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToTopWallGrid(new GridPos(15, 7)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToTopWallGrid(new GridPos(16, 7)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToTopWallGrid(new GridPos(17, 7)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToTopWallGrid(new GridPos(18, 7)));
//        m_wallGenerationDataGrid.Add(ConvertStageGridToTopWallGrid(new GridPos(19, 7)));
//    }

//    public GameObject[,] GenerateWall(MapData map)
//    {
      
      



//        // あみだグリッド
//        GameObject[,] wallGrid = new GameObject[map.GetCommonData().height + 1, map.GetCommonData().width * 2]; // 一つ多くに作る

//        foreach (var wallData in m_wallGenerationDataGrid)
//        {
//            CreateWall(wallData, map, m_wallPrefab, m_wallParent.transform);

//        }

//        return wallGrid;
//    }

//    /// <summary>
//    /// 壁の生成
//    /// </summary>
//    /// <param name="gridPos">グリッド座標　壁のグリッド作りが違うので注意</param>
//    /// <param name="map"></param>
//    /// <param name="wallPrefab"></param>
//    /// <returns></returns>
//   static public GameObject CreateWall(GridPos gridPos, MapData map, GameObject wallPrefab, Transform parent )
//    {
//        if (gridPos.x % 2 == 0)
//            return CreateLeftWall(gridPos, map, wallPrefab, parent);

//        else
//            return CreateTopWall(gridPos, map, wallPrefab, parent);
//    }

//    // |-|-|-|
//    // |-|-|-|

//    static GameObject CreateLeftWall(GridPos gridPos, MapData map, GameObject wallPrefab, Transform parent)
//    {
//        Vector3 generationPos = map.ConvertGridToWorldPos(gridPos.x / 2, gridPos.y);

//        // 左攻めにする
//        generationPos.x -= map.GetCommonData().tileSize / 2.0f;

//        float scaleY = m_baseTrans.localScale.y;

//        generationPos.y = map.GetFloorManager().GetFloorTopPartPosY(FloorManager.FloorType.TOP) + scaleY / 2.0f;

//        Quaternion quaternion = Quaternion.LookRotation(Vector3.right);

//        return Instantiate(wallPrefab, generationPos, quaternion, parent);
//    }
//    static GameObject CreateTopWall(GridPos gridPos, MapData map, GameObject wallPrefab, Transform parent)
//    {
//        Vector3 generationPos = map.ConvertGridToWorldPos(gridPos.x / 2, gridPos.y);

//        generationPos.z += map.GetCommonData().tileSize / 2.0f;

//        float scaleY = m_baseTrans.localScale.y;


//        generationPos.y = map.GetFloorManager().GetFloorTopPartPosY(FloorManager.FloorType.TOP) + scaleY / 2.0f;


//        // 上攻め
//        return Instantiate(wallPrefab, generationPos, Quaternion.identity, parent);
//    }

//    public static GridPos ConvertStageGridToLeftWallGrid(GridPos gridPos)
//    {
//        return new(gridPos.x * 2 , gridPos.y);
//    }

//    public static GridPos ConvertStageGridToTopWallGrid(GridPos gridPos)
//    {
//        return new (gridPos.x * 2 + 1, gridPos.y);
//    }

//}
