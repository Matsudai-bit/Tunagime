using UnityEngine;
using System.Collections.Generic;
using UnityEditor;



/// <summary>
/// ステージ生成器
/// </summary>
public class StageGeneratorNew : MonoBehaviour
{
    
    public MapData m_map; // ステージデータ

    public AmidaManager m_amidaManager;

    [SerializeField] private GameObject m_player;                   //　プレイヤー



    /// <summary>
    /// ギミック生成データ
    /// </summary>
    [System.Serializable]
    struct GenerationGimmickData
    {
        public GridPos gridPos;
        public GameObject prefab;
    }




    /// <summary>
    /// 生成データ
    /// </summary>
    [System.Serializable]
    struct Generator
    {
        public GameObject generator;    // 生成機
        public float posY;              // 生成座標Y
    }


    [SerializeField] private GenerationGimmickData[] m_gimmickData ;  // ギミックの生成機
    [SerializeField] private GenerationGimmickData[] m_floorData ;  // ギミックの生成機

    [SerializeField] private Generator m_amidaTubeGenerator;  // あみだチューブの生成機

    [SerializeField] private Generator m_topFloorBlockGenerator;  // トップ層の床ブロックの生成機

    


    private void Awake()
    {
       
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        // 床上部座標の設定
        float topFloorTopPartPosY = m_topFloorBlockGenerator.posY;
        float amidaPosY = m_amidaTubeGenerator.posY;

        var map = MapData.GetInstance;

        
        // トップ層の生成
        if (m_topFloorBlockGenerator.generator)
                m_topFloorBlockGenerator.generator.GetComponent<FloorBlockGenerator>().
                GenerateFloor(
                    false
            );

        // あみだチューブの生成
        if (m_amidaTubeGenerator.generator)

            m_amidaTubeGenerator.generator.GetComponent<AmidaTubeGenerator>().GenerateAmida();




        // ギミックの生成
        foreach (var generation in m_gimmickData)
        {
            GridPos fixedGridPos = new GridPos ( generation.gridPos.x - 1, generation.gridPos.y - 1 );

            GameObject instanceObj = Instantiate(generation.prefab);
            map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].tileObject.gameObject = instanceObj;

            StageBlock stageBlock = instanceObj.GetComponent<StageBlock>();

            stageBlock.Initialize(fixedGridPos);
        }

        // 床の生成　通常床は自動で生成されている
        foreach (var generation in m_floorData)
        {
            
            GridPos fixedGridPos = new GridPos(generation.gridPos.x, generation.gridPos.y);


            // 床の生成
            GameObject instanceObj = Instantiate(generation.prefab);

            //Vector3 pos = map.ConvertGridToWorldPos(fixedGridPos.x, fixedGridPos.y);
            //instanceObj.transform.position = new Vector3(pos.x, instanceObj.transform.position.y, pos.z);

            // 元々の床オブジェクトを無効化
            map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].floor.SetActive(false);
            // 新しい床オブジェクトを設定
            map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].floor = instanceObj;

            StageBlock stageBlock = instanceObj.GetComponent<StageBlock>();
            stageBlock.Initialize(fixedGridPos);

            if (instanceObj.GetComponent<FeelingSlot>() != null)
            {
                // FeelingSlotの初期化
                FeelingSlot feelingSlot = instanceObj.GetComponent<FeelingSlot>();
                m_amidaManager.AddFeelingSlot(feelingSlot);
            }


        }





    }




}
