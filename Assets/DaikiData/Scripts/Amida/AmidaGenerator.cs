using UnityEngine;
using static AmidaManager;
using static UnityEditor.PlayerSettings;


/// <summary>
/// あみだくじの生成器
/// </summary>
public class AmidaTubeGenerator : MonoBehaviour
{
    /// <summary>
    /// あみだチューブの生成用データ
    /// </summary>
    /// 

    [System.Serializable]
    public struct DirectionPassage
    {
        public bool up;       // 上に通過可能かどうか
        public bool down;     // 下に通過可能かどうか
        public bool left;     // 左に通過可能かどうか
        public bool right;    // 右に通過可能かどうか
        public bool CanPass(Vector3Int direction)
        {
            if (direction == Vector3Int.up) return up;
            if (direction == Vector3Int.down) return down;
            if (direction == Vector3Int.left) return left;
            if (direction == Vector3Int.right) return right;
            return false;
        }
    }

    [System.Serializable]
    public struct AmidaCellData
    {
        public bool isMake;                         //　作るかどうか
        public DirectionPassage passage;            //　通過可能な方向
    }

    AmidaCellData[,] m_amidaGenerationDataGrid;

    [SerializeField] private GameObject m_amidaKnotPrefab;    // 結び目
    [SerializeField] private GameObject m_amidaBridgePrefab;    // あみだ橋
    [SerializeField] private GameObject m_amidaTubePrefab;    // あみだ
    [SerializeField] private GameObject m_amidaTubeParent;    // あみだの親

    [SerializeField] private Mesh[] m_amidaTubeMeshis;

    /// <summary>
    /// あみだの生成
    /// </summary>
    /// <param name="amidaTopPartPosY">あみだの床の上部座標</param>
    /// <param name="map"></param>
    /// <returns>生成したグリッドデータ</returns>
    public GameObject[,]  GenerateAmida()
    {
        var map = MapData.GetInstance;
        

        // あみだグリッド
        GameObject[,] amidaGrid;

        // あみだグリッドの生成
        amidaGrid = new GameObject[map.GetCommonData().height, map.GetCommonData().width];
        m_amidaGenerationDataGrid = new AmidaCellData[map.GetCommonData().height, map.GetCommonData().width];

        AmidaCellData amidaA = new AmidaCellData();
        AmidaCellData amidaB = new AmidaCellData();
        AmidaCellData straightLeftRight = new AmidaCellData();
        AmidaCellData straightUpDown = new AmidaCellData();

        // それぞれのパイプの初期化
        amidaA.isMake = true;
        amidaB.isMake = true;
        straightLeftRight.isMake = true;
        straightUpDown.isMake = true;

        amidaA.passage.up = true;
        amidaA.passage.down = true;

        amidaB.passage.up = true;
        amidaB.passage.down = true;

        straightLeftRight.passage.left = true;
        straightLeftRight.passage.right = true;

        straightUpDown.passage.up = true;
        straightUpDown.passage.down = true;

        //// 生成データの設定
        //m_amidaGenerationDataGrid[2, 1] = straightUpDown;
        //m_amidaGenerationDataGrid[2, 5] = straightUpDown;
        //m_amidaGenerationDataGrid[2, 10] = straightUpDown;
        //m_amidaGenerationDataGrid[2, 13] = straightUpDown;
        ////m_amidaGenerationDataGrid[2, 15] = straightUpDown;
        //m_amidaGenerationDataGrid[4, 4] = straightUpDown;
        //m_amidaGenerationDataGrid[4, 18] = straightUpDown;
        //m_amidaGenerationDataGrid[4, 19] = straightUpDown;
        //m_amidaGenerationDataGrid[4, 11] = straightUpDown;
        //m_amidaGenerationDataGrid[8, 3] = straightUpDown;
        //m_amidaGenerationDataGrid[6, 10] = straightUpDown;
        //m_amidaGenerationDataGrid[6, 8] = straightUpDown;
        //m_amidaGenerationDataGrid[8, 9] = straightUpDown;
        //m_amidaGenerationDataGrid[6, 17] = straightUpDown;
        //m_amidaGenerationDataGrid[6, 2] = straightUpDown;

        // 横向きのパイプを配置
        for (int cy = 1; cy < map.GetCommonData().height; cy += 2)
        {
            for (int cx = 0; cx < map.GetCommonData().width; cx++)
            {
                m_amidaGenerationDataGrid[cy, cx] = straightLeftRight;
            }
        }
        //m_amidaGenerationDataGrid[3, 16].isMake = false;



        // あみだパイプの生成
        for (int cy = 0; cy < map.GetCommonData().height; cy++)
        {
            for (int cx = 0; cx < map.GetCommonData().width; cx++)
            {
                if (m_amidaGenerationDataGrid == null) continue;
                if (m_amidaGenerationDataGrid[cy, cx].isMake == false) continue;


                amidaGrid[cy, cx]  = CreateAmidaTube(m_amidaGenerationDataGrid[cy, cx], cx, cy, map, map.GetCommonData().BaseTilePosY);

                var amidaTube = amidaGrid[cy, cx].GetComponent<AmidaTube>();

                map.GetStageGridData().GetTileData[cy, cx].amidaTube = amidaTube;

            }
        }

        // 各あみだに隣接するあみだパイプを設定する
        for (int cy = 0; cy < map.GetCommonData().height; cy++)
        {
            for (int cx = 0; cx < map.GetCommonData().width; cx++)
            {
                if (m_amidaGenerationDataGrid == null) continue;
                if (m_amidaGenerationDataGrid[cy, cx].isMake == false) continue;

                var amidaTube = amidaGrid[cy, cx]?.GetComponent<AmidaTube>();

                if (amidaTube == null)
                {
                    Debug.LogWarning($"あみだチューブが({cx}, {cy}) のコンポーネントに存在しません");
                    continue;
                }

                if (m_amidaGenerationDataGrid[cy, cx].passage.CanPass(Vector3Int.right) || 
                    m_amidaGenerationDataGrid[cy, cx].passage.CanPass(Vector3Int.up)    ||
                    m_amidaGenerationDataGrid[cy, cx].passage.CanPass(Vector3Int.down))
                {
                    // 右に通過可能な場合、右、左のあみだパイプを設定
                    AmidaTube left;
                    AmidaTube right;

                    if (cx - 1 >= 0)
                        left = amidaGrid[cy, cx - 1].GetComponent<AmidaTube>();
                    else
                        left = null;

                    if (cx + 1 < map.GetCommonData().width)
                        right = amidaGrid[cy, cx + 1].GetComponent<AmidaTube>();
                    else
                        right = null;

                    amidaTube.SetNeighbor(null, null, left, right);
                }

                if (m_amidaGenerationDataGrid[cy, cx].passage.CanPass(Vector3Int.up))
                {
                    // 上に通過可能な場合、上のあみだパイプを設定
                    AmidaTube up;
                    if (cy - 1 >= 0)
                        up = amidaGrid[cy - 1, cx].GetComponent<AmidaTube>();
                    else
                        up = null;
       
                    amidaTube.SetNeighbor(up, null, null, null);
                }

                if (m_amidaGenerationDataGrid[cy, cx].passage.CanPass(Vector3Int.down))
                {
                    // 下に通過可能な場合、下のあみだパイプを設定
                    AmidaTube down;
                    if (cy + 1 < map.GetCommonData().height)
                        down = amidaGrid[cy + 1, cx].GetComponent<AmidaTube>();
                    else
                        down = null;
                    amidaTube.SetNeighbor(null, down, null, null);
                }





            }

        }


                // 生成したグリッドデータを返す
                return amidaGrid;
    }

    public GameObject GenerateAmidaBridge(GridPos gridPos)
    {
        Vector3 generatePos = MapData.GetInstance.ConvertGridToWorldPos(gridPos.x, gridPos.y);

        GameObject bridge = Instantiate(m_amidaTubePrefab ,generatePos, Quaternion.identity, m_amidaTubeParent.transform);

        var map = MapData.GetInstance;

        var stageGridData = map.GetStageGridData();
        // あみだチューブの登録
        stageGridData.SetAmidaTube(gridPos, bridge.GetComponent<AmidaTube>());
        


        GridPos knotDownPos =  new GridPos(gridPos.x, gridPos.y - 1);
        GridPos knotUpPos = new GridPos(gridPos.x, gridPos.y + 1);



        
        stageGridData.GetAmidaTube(gridPos).RequestChangedState(AmidaTube.State.BRIDGE);
        stageGridData.GetAmidaTube(knotDownPos).RequestChangedState(AmidaTube.State.KNOT_DOWN);
        stageGridData.GetAmidaTube(knotUpPos).RequestChangedState(AmidaTube.State.KNOT_UP);


        // 方向のあみだの設定






        return bridge;
    }


   /// <summary>
   /// あみだパイプを生成する処理
   /// </summary>
   /// <param name="amidaData"></param>
   /// <param name="cx"></param>
   /// <param name="cy"></param>
   /// <param name="map"></param>
   /// <param name="amidaTopPartPosY"></param>
   /// <param name="creationMesh"></param>
   /// <returns></returns>
    GameObject CreateAmidaTube(AmidaCellData amidaData, int cx, int cy, MapData map, float amidaTopPartPosY)
    {
        // 上下左右に通過可能かどうかを判定
        bool canUpPassThrough = false;
        bool canDownThrough = false;
        bool canRightPassThrough = false;
        bool canLeftPassThrough = false;

        if (0 <= cy - 1)
        {
            canUpPassThrough = m_amidaGenerationDataGrid[cy - 1, cx].passage.down;

            if (canUpPassThrough)
                amidaData.passage.up = canUpPassThrough;
        }
        if (map.GetCommonData().height > cy + 1)
        {
            canDownThrough = m_amidaGenerationDataGrid[cy + 1, cx].passage.up;
            if (canDownThrough)
                amidaData.passage.down = canDownThrough;
        }

        if (0 <= cx - 1)
        {
            canLeftPassThrough = m_amidaGenerationDataGrid[cy, cx - 1].passage.right;
            if (canLeftPassThrough)
                amidaData.passage.left = canLeftPassThrough;
        }
        if (0 > cx + 1)
        {
            canRightPassThrough = m_amidaGenerationDataGrid[cy, cx + 1].passage.right;

            if (canRightPassThrough)
                amidaData.passage.right = canRightPassThrough;
        }


        // 座標の設定
        Vector3 pos = map.ConvertGridToWorldPos(cx, cy);
        pos.y =  amidaTopPartPosY;

        // 生成
        GameObject newAmida = Instantiate(m_amidaTubePrefab, pos, Quaternion.identity, m_amidaTubeParent.transform);



        newAmida.GetComponent<AmidaTube>().RequestChangedState(AmidaTube.State.NORMAL);

  
        // あみだチューブの登録
        return newAmida;
    }


}
