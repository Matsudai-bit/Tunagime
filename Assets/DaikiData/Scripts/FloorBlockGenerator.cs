using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 床生成
/// </summary>
public class FloorBlockGenerator : MonoBehaviour
{
    /// <summary>
    /// 床のマテリアルデータ
    /// </summary>
    [Serializable]
    struct FloorMaterialData
    {
        public string label;
        public Material material;
        public WorldID worldID;
    }

    [Header("====== 床のマテリアルデータ ======")]
    [SerializeField] private FloorMaterialData[] m_floorMaterialDatas; // 床のマテリアルデータ配列

    private Dictionary<WorldID, Material> m_floorMaterialDataDic = new Dictionary<WorldID, Material>(); // 床のマテリアルデータ辞書


    [SerializeField] private GameObject m_floorBlockPrefab;    // ステージブロック
    [SerializeField] private Transform m_floorBlockParent;    // ステージブロックの親   


    public GameObject line; // ライン表示用オブジェクト

    private void Awake()
    {
        // 床のマテリアルデータ辞書の初期化
        if (m_floorMaterialDatas != null)
        {
            foreach (var data in m_floorMaterialDatas)
            {
                if (data.material != null && !m_floorMaterialDataDic.ContainsKey(data.worldID))
                {
                    m_floorMaterialDataDic.Add(data.worldID, data.material);
                }
            }
        }
        else
        {
            Debug.LogWarning("床のマテリアルデータが設定されていません");
        }

        var map = MapData.GetInstance;
        var gameProgressData = GameProgressManager.Instance.GameProgressData;
        m_floorBlockPrefab.GetComponent<MeshRenderer>().material = m_floorMaterialDataDic[gameProgressData.worldID];
    }

    private void Start()
    {

    }


    /// <summary>
    /// 活動していない床を捜して送る
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="map"></param>
    /// <param name="generationPosY"></param>
    /// <returns></returns>
    public GameObject GenerateFloor(GridPos gridPos)
    {



        // 子オブジェクトを格納する配列作成
        var children = new Transform[m_floorBlockParent.transform.childCount];

        MapData map = MapData.GetInstance;

        float posX = (float)(gridPos.x) * map.GetCommonData().tileSize;
        float posY = (float)(gridPos.y) * map.GetCommonData().tileSize;

        Vector3 pos = map.ConvertGridToWorldPos(gridPos.x, gridPos.y);

        pos.y = map.GetCommonData().BaseTilePosY;

        // 0～個数-1までの子を順番に配列に格納
        for (int i = 0; i < m_floorBlockParent.transform.childCount; i++)
        {
            // 子要素の取得
            var child = m_floorBlockParent.transform.GetChild(i);

            if (child.gameObject != null && child.gameObject.activeSelf == false)
            {
               child.gameObject.SetActive(true);
                child.transform.position = pos;
                return child.gameObject;
            }
        }


      

        // 生成
        return Instantiate(m_floorBlockPrefab, pos, Quaternion.identity, m_floorBlockParent.transform);
    }



    /// <summary>
    /// 床の生成
    /// </summary>
    /// <param name="generationPosY"></param>
    /// <returns>生成したグリッドデータ</returns>
    public GameObject[,] GenerateFloor(  bool grid, Transform parent)
    {
        GameObject[,] floorBlockGrid;

        MapData map = MapData.GetInstance;


        // 親オブジェクトの設定
        m_floorBlockParent = parent;


        // ステージグリッドの生成
        floorBlockGrid = new GameObject[map.GetCommonData().height, map.GetCommonData().width];

        for (int cy = 0; cy < map.GetCommonData().height; cy++)
        {
            for (int cx = 0; cx < map.GetCommonData().width; cx++)
            {
                float posX = (float)(cx) * map.GetCommonData().tileSize;
                float posY = (float)(cy) * map.GetCommonData().tileSize;

                Vector3 pos = map.ConvertGridToWorldPos(cx, cy);

                pos.y = map.GetCommonData().BaseTilePosY;

                // 生成
                floorBlockGrid[cy, cx] = Instantiate(m_floorBlockPrefab, pos, Quaternion.identity, m_floorBlockParent.transform);


                map.GetStageGridData().GetTileData[cy, cx].floor = floorBlockGrid[cy, cx];
            }

        }


        if (grid)
        // グリッドの線の描画
        CreateGridEffects(map, map.GetCommonData().BaseTilePosY);

        // 上部の座標の算出
        //       topPartPosY = generationPosY + (m_floorBlockPrefab.transform.localScale.y / 2.0f);

        var boxCollider = m_floorBlockParent.GetComponent<BoxCollider>();
        if (boxCollider)
        {
            float sizeX = (float)map.GetCommonData().width * map.GetCommonData().tileSize;
            float sizeY = 0.01f;
            float sizeZ = (float)map.GetCommonData().height * map.GetCommonData().tileSize;
            boxCollider.size =  new Vector3(sizeX, sizeY, sizeZ);

            
        }
        else
            Debug.LogWarning(m_floorBlockParent.name + "のBoxColliderが見つかりません");


            // 生成したグリッドデータを返す
            return floorBlockGrid;
    }


    /// <summary>
    /// グリッドラインの生成
    /// </summary>
    private void CreateGridEffects(MapData map, float generationPosY)
    {
        for (int x = 1; x < map.GetCommonData().width; x++)
        {
            GameObject obj = Instantiate(line, m_floorBlockParent.transform.GetChild(0));

            float worldX = map.ConvertGridToWorldPosX(x);

            obj.transform.position = new Vector3(
                worldX - map.GetCommonData().tileSize / 2,
                generationPosY + (map.GetCommonData().tileSize / 2 + 0.01f),
                -((float)(map.GetCommonData().height) / 2.0f) * map.GetCommonData().tileSize + map.GetCommonData().tileSize / 2.0f);

            obj.transform.localScale = new Vector3(1, 1, map.GetCommonData().height * map.GetCommonData().tileSize);    // どれだけ伸ばすかはGetCommonData().heightを見る
        }
        for (int y = 1; y < map.GetCommonData().height; y++)
        {
            GameObject obj = Instantiate(line, m_floorBlockParent.transform.GetChild(0));

            float worldZ = map.ConvertGridToWorldPosZ(y);

            obj.transform.position = new Vector3(
                -((float)(map.GetCommonData().width) / 2.0f) * map.GetCommonData().tileSize - map.GetCommonData().tileSize / 2.0f,
                generationPosY + (map.GetCommonData().tileSize / 2 + 0.01f),
                worldZ + map.GetCommonData().tileSize / 2);

            obj.transform.rotation = Quaternion.Euler(0, 90, 0);
            obj.transform.localScale = new Vector3(1, 1, map.GetCommonData().width * map.GetCommonData().tileSize); // どれだけ伸ばすかはGetWidth()を見る
        }
    }

    private void Reset()
    {
        m_floorMaterialDatas = new FloorMaterialData[Enum.GetValues(typeof(WorldID)).Length];

        for (int i = 0; i < m_floorMaterialDatas.Length; i++)
        {
            m_floorMaterialDatas[i].label = "World_" + (i + 1);
            m_floorMaterialDatas[i].worldID = (WorldID)i;
            m_floorMaterialDatas[i].material = null;
        }


    }
}
