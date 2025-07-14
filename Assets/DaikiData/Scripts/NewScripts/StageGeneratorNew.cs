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
    struct FloorGenerationData
    {
        public GameObject generator;    // 生成機
        public float posY;              // 生成座標Y
    }


    [SerializeField] private GenerationGimmickData[] m_gimmickData ;  // ギミックの生成機

    [SerializeField] private FloorGenerationData m_amidaTubeGenerator;  // あみだチューブの生成機

    [SerializeField] private FloorGenerationData m_topFloorBlockGenerator;  // トップ層の床ブロックの生成機

    


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
            


        // ギミックブロックグリッドの取得
        GameObject[,] gimmickBlockGrid = new GameObject[m_map.GetCommonData().height, m_map.GetCommonData().width];

        foreach (var generation in m_gimmickData)
        {
            GridPos fixedGridPos = new GridPos ( generation.gridPos.x - 1, generation.gridPos.y - 1 );

            GameObject instanceObj = Instantiate(generation.prefab);
            map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].tileObject.gameObject = instanceObj;

            StageBlock stageBlock = instanceObj.GetComponent<StageBlock>();

            stageBlock.Initialize(fixedGridPos);
        }


        // ステージグリッドデータにギミックブロックグリッドを設定
        //m_map.GetStageGridData().SetTopGimmickBlockGrid(gimmickBlockGrid);



    }


   

}
