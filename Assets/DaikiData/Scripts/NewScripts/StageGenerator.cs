using System.Linq;
using UnityEngine;




/// <summary>
/// ステージ生成器
/// </summary>
public class StageGenerator : MonoBehaviour
{
   



    /// <summary>
    /// ギミック生成データ
    /// </summary>
    [System.Serializable]
    class GenerationGimmickData
    {
        public GridPos gridPos;
        public GameObject prefab;
        public GenerationType blockType; // ブロックの種類
        public Vector3 rotate;          // 回転
        public EmotionCurrent.Type emotionType; // 感情タイプ
    }

    enum GenerationType
    {
        FELT_BLOCK,             // フェルトブロック
        FLUFF_BALL,             // 毛糸玉
        FELT_BLOCK_NO_MOVEMENT, // 動かないフェルトブロック
        FLOOR,                  // 床
        TERMINUS,               // 終点
        CURTAIN,                // カーテン
        SATIN_FLOOR,            // サテン床
        PAIR_BADGE,             // ペアバッジ
        FRAGMENT,               // 想いの断片
        NONE,                 // なし
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


    [Header("==== 生成データ ==== ")]
    [Header("ギミック生成データ")]
    [SerializeField] private GenerationGimmickData[] m_gimmickData ;  // ギミックの生成機
    [Header("始点核生成データ")]
    [SerializeField] private GenerationGimmickData[] m_floorData ;  // ギミックの生成機
    [Header("終点核生成データ")]
    [SerializeField] private GenerationGimmickData[] m_terminusData;  // 終点の生成機

    [Header("==== 生成機 ==== ")]
    
    [SerializeField] private Generator m_amidaTubeGenerator;  // あみだチューブの生成機

    [SerializeField] private Generator m_topFloorBlockGenerator;  // トップ層の床ブロックの生成機

    
   
    private void Awake()
    {
       
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
      
    }

    public void Generate(AmidaManager amidaManager, Transform amidaParent, Transform floorParent, Transform gimmickParent, ClearConditionChecker clearConditionChecker)
    {

        MaterialLibrary.GetInstance.GetMaterial(MaterialLibrary.MaterialGroup.YARN, EmotionCurrent.Type.FAITHFULNESS); // マテリアルライブラリの初期化

        var map = MapData.GetInstance;
        map.Initialize(); // マップデータの初期化
        // 背景の生成
        Instantiate(map.GetStagePrefab());

        // 床上部座標の設定
        float topFloorTopPartPosY = m_topFloorBlockGenerator.posY;
        float amidaPosY = m_amidaTubeGenerator.posY;



        // トップ層の生成
        if (m_topFloorBlockGenerator.generator)
            m_topFloorBlockGenerator.generator.GetComponent<FloorBlockGenerator>().
            GenerateFloor(false, floorParent);

        // あみだチューブの生成
        if (m_amidaTubeGenerator.generator)

            m_amidaTubeGenerator.generator.GetComponent<AmidaTubeGenerator>().GenerateAmida(amidaParent);



        var stageObjectFactory = StageObjectFactory.GetInstance(); 
        // ギミックの生成
        foreach (var generation in m_gimmickData)
        {
            if (generation.blockType == GenerationType.NONE) continue; // ブロックの種類がNONEの場合はスキップ

            GridPos fixedGridPos = new GridPos(generation.gridPos.x - 1, generation.gridPos.y - 1);
            GameObject generationObject = null;           

            switch (generation.blockType)
            {
                case GenerationType.FLUFF_BALL:
                    // フラフボールの生成
                    generationObject = stageObjectFactory.GenerateFluffBall(gimmickParent, fixedGridPos);
                    // 生成されたオブジェクトの位置を設定
                    map.GetStageGridData().TryPlaceTileObject(fixedGridPos, generationObject);
                    break;
                case GenerationType.FELT_BLOCK:
                    // フェルトブロックの生成
                    generationObject = stageObjectFactory.GenerateFeltBlock(gimmickParent, fixedGridPos, generation.emotionType);
                    // 生成されたオブジェクトの位置を設定
                    map.GetStageGridData().TryPlaceTileObject(fixedGridPos, generationObject);
                    break;
                case GenerationType.FELT_BLOCK_NO_MOVEMENT:
                    // 動かないフェルトブロックの生成
                    generationObject = stageObjectFactory.GenerateNoMovementFeltBlock(gimmickParent, fixedGridPos);
                    // 生成されたオブジェクトの位置を設定
                    map.GetStageGridData().TryPlaceTileObject(fixedGridPos, generationObject);
                    break;
                case GenerationType.CURTAIN:
                    // カーテンの生成
                    generationObject = stageObjectFactory.GenerateCurtain(gimmickParent, generation.rotate.y, fixedGridPos, generation.emotionType);
                    // 生成されたオブジェクトの位置を設定
                    map.GetStageGridData().TryPlaceTileObject(fixedGridPos, generationObject);
                    break;
                case GenerationType.SATIN_FLOOR:
                    // サテン床の生成
                    generationObject = stageObjectFactory.GenerateSatinFloor(gimmickParent, fixedGridPos);
                    // 生成されたオブジェクトの位置を設定
                    map.GetStageGridData().TryRePlaceFloorObject(fixedGridPos, generationObject);
                    break;
                case GenerationType.PAIR_BADGE:
                    // ペアバッジの生成

                    // 同じ種類の座標を全て取得
                    var generationBlock = m_gimmickData.Where(data => data.blockType == GenerationType.PAIR_BADGE && data.emotionType == generation.emotionType).ToList();
                    // 座標
                    var generationPosList = generationBlock.Select(data => new GridPos(data.gridPos.x - 1, data.gridPos.y - 1)).ToList();

                    // ペアバッジの生成
                    generationObject = stageObjectFactory.GeneratePairBadge(gimmickParent, generationPosList, generation.emotionType);

                    // 生成されたオブジェクトの位置を設定
                    
                    foreach (var feltBlock in generationObject.GetComponent<PairBadge>().GetFeltBlocks())
                    {
                        map.GetStageGridData().TryPlaceTileObject(feltBlock.stageBlock.GetGridPos(), feltBlock.gameObject);
                    }

                    foreach (var data in generationBlock)
                    {
                        data.blockType = GenerationType.NONE; // 生成したペアバッジの座標はNONEに設定
                    }

                    break;
                case GenerationType.FRAGMENT:
                    // 想いの断片の生成
                    generationObject = stageObjectFactory.GenerateFragment(gimmickParent, fixedGridPos, generation.emotionType);
                    // 生成されたオブジェクトの位置を設定
                    map.GetStageGridData().TryPlaceTileObject(fixedGridPos, generationObject);
                    break;

            }

        
        }

        // 床の生成　通常床は自動で生成されている
        foreach (var generation in m_floorData)
        {

            GridPos fixedGridPos = new GridPos(generation.gridPos.x - 1, generation.gridPos.y - 1);


            // 床の生成
            GameObject instanceObj = Instantiate(generation.prefab, gimmickParent);

            //Vector3 pos = map.ConvertGridToWorldPos(fixedGridPos.x, fixedGridPos.y);
            //instanceObj.transform.position = new Vector3(pos.x, instanceObj.transform.position.y, pos.z);

            // 元々の床オブジェクトを無効化
            map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].floor.SetActive(false);

            // 新しい床オブジェクトを設定
            map.GetStageGridData().TryPlaceTileObject(fixedGridPos, instanceObj);
            map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].tileObject.gameObject = instanceObj;

            StageBlock stageBlock = instanceObj.GetComponent<StageBlock>();
            stageBlock.SetBlockType(StageBlock.BlockType.FEELING_SLOT);
            stageBlock.Initialize(fixedGridPos);


            if (instanceObj.GetComponent<FeelingSlot>() != null)
            {
                // FeelingSlotの初期化
                FeelingSlot feelingSlot = instanceObj.GetComponent<FeelingSlot>();
                amidaManager.AddFeelingSlot(feelingSlot);
            }


        }

        // 終点の生成
        foreach (var generation in m_terminusData)
        {
            GridPos fixedGridPos = new GridPos(generation.gridPos.x - 1, generation.gridPos.y - 1);
            GameObject instanceObj = Instantiate(generation.prefab, gimmickParent);

            // 新しい床オブジェクトを設定
            map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].floor = instanceObj;

            // 新しい床オブジェクトを設定
            map.GetStageGridData().TryPlaceTileObject(fixedGridPos, instanceObj);
            map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].tileObject.gameObject = instanceObj;

            StageBlock stageBlock = instanceObj.GetComponent<StageBlock>();
            stageBlock.SetBlockType(StageBlock.BlockType.FEELING_SLOT);
            stageBlock.Initialize(fixedGridPos);
            TerminusFeelingSlot terminusFeelingSlot = instanceObj.GetComponent<TerminusFeelingSlot>();
            if (terminusFeelingSlot != null && instanceObj?.GetComponent<TerminusFeelingSlotRefection>() == null)
            {
                clearConditionChecker.AddTerminusFeelingSlot(terminusFeelingSlot);
            }
        }





    }

  

}
