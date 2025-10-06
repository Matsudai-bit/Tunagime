using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static StageLayoutData;




/// <summary>
/// ステージ生成器
/// </summary>
public class StageGenerator : MonoBehaviour
{

    [Header("==== 新しいシステムを使うかどうか ==== ")]
    public bool m_usingNewSystem = false; // 新しいシステムを使うかどうか
    [Header("==== ギミック生成データ ==== ")]
    public StageLayoutData m_stageLayoutData; // ステージレイアウトデータ <- New

    /// <summary>
    /// ギミック生成データ
    /// </summary>
    [System.Serializable]
    public class GenerationGimmickData
    {
        public GridPos gridPos;
        public GenerationType blockType; // ブロックの種類
        public EmotionCurrent.Type emotionType; // 感情タイプ
        public Vector3 rotate;          // 回転

        GenerationGimmickData()
        {
            gridPos = new GridPos();
            blockType = GenerationType.NONE;
            rotate = Vector3.zero;
            emotionType = EmotionCurrent.Type.NONE;
        }
    }

    public enum GenerationType
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
        TERMINUS_SLOT_EMPTY,    // 終点の想いの核（空）
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
    [SerializeField] private GenerationGimmickData[] m_gimmickData;  // ギミックの生成機
    [Header("床系生成データ")]
    [SerializeField] private GenerationGimmickData[] m_floorData;  // ギミックの生成機
    [Header("始点核生成データ")]
    [SerializeField] private GenerationGimmickData[] m_startSlotData;  // ギミックの生成機
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

    public void Generate(
        AmidaManager amidaManager,
        Transform amidaParent,
        Transform floorParent,
        Transform gimmickParent,
        Transform feelingSlotParent,
        ClearConditionChecker clearConditionChecker)
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



        if (m_usingNewSystem)
        {
            GenerateNewSystem(
                amidaManager,
                amidaParent,
                floorParent,
                gimmickParent,
                feelingSlotParent,
                clearConditionChecker);
        }
        else
        {

            // あみだチューブの生成
            if (m_amidaTubeGenerator.generator)
                m_amidaTubeGenerator.generator.GetComponent<AmidaTubeGenerator>().GenerateAmida(amidaParent);

            GenerateOldSystem(
                amidaManager,
                amidaParent,
                floorParent,
                gimmickParent,
                feelingSlotParent,
                clearConditionChecker);
        }


    }

    void GenerateOldSystem(
        AmidaManager amidaManager,
        Transform amidaParent,
        Transform floorParent,
        Transform gimmickParent,
        Transform feelingSlotParent,
        ClearConditionChecker clearConditionChecker)
    {
        var map = MapData.GetInstance;

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
                    generationObject = stageObjectFactory.GenerateFragment(gimmickParent, fixedGridPos, generation.emotionType, (int)generation.rotate.x);
                    // 生成されたオブジェクトの位置を設定
                    map.GetStageGridData().TryPlaceTileObject(fixedGridPos, generationObject);
                    break;

            }


        }

        // 床の生成 通常床は自動で生成されている
        foreach (var generation in m_floorData)
        {
            if (generation.blockType == GenerationType.NONE) continue; // ブロックの種類がNONEの場合はスキップ

            GridPos fixedGridPos = new GridPos(generation.gridPos.x - 1, generation.gridPos.y - 1);
            GameObject generationObject = null;

            switch (generation.blockType)
            {
                case GenerationType.SATIN_FLOOR:
                    // サテン床の生成
                    generationObject = stageObjectFactory.GenerateSatinFloor(floorParent, fixedGridPos);
                    // 生成されたオブジェクトの位置を設定
                    map.GetStageGridData().TryRePlaceFloorObject(fixedGridPos, generationObject);
                    break;
            }
        }

        // 開始スロットの生成　
        foreach (var generation in m_startSlotData)
        {

            GridPos fixedGridPos = new GridPos(generation.gridPos.x - 1, generation.gridPos.y - 1);

            // 床の生成
            GameObject instanceObj = stageObjectFactory.GenerateFeelingSlot(feelingSlotParent, fixedGridPos, generation.emotionType);


            // 新しい床オブジェクトを設定
            map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].floor = instanceObj;

            // 新しい床オブジェクトを設定
            map.GetStageGridData().TryPlaceTileObject(fixedGridPos, instanceObj);
            map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].tileObject.gameObject = instanceObj;

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

            // 型の生成
            GameObject instanceCoreObj = null;

            if (generation.blockType == GenerationType.TERMINUS_SLOT_EMPTY)
            {
                // 終点の想いの型（空）の生成
                instanceCoreObj = stageObjectFactory.GenerateTerminusFeelingSlot(feelingSlotParent, fixedGridPos, generation.emotionType, true);
            }
            else
            {
                // 終点の生成
                instanceCoreObj = stageObjectFactory.GenerateTerminusFeelingSlot(feelingSlotParent, fixedGridPos, generation.emotionType, false);
            }


            // 新しい床オブジェクトを設定
            map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].floor = instanceCoreObj;

            // 新しい床オブジェクトを設定
            map.GetStageGridData().TryPlaceTileObject(fixedGridPos, instanceCoreObj);
            map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].tileObject.gameObject = instanceCoreObj;

            TerminusFeelingSlot terminusFeelingSlot = instanceCoreObj.GetComponent<TerminusFeelingSlot>();
            if (terminusFeelingSlot != null && instanceCoreObj?.GetComponent<TerminusFeelingSlotRefection>() == null)
            {
                clearConditionChecker.AddTerminusFeelingSlot(terminusFeelingSlot);
            }
        }
    }



    void GenerateNewSystem(
        AmidaManager amidaManager,
        Transform amidaParent,
        Transform floorParent,
        Transform gimmickParent,
        Transform feelingSlotParent,
        ClearConditionChecker clearConditionChecker)
    {
        List<RootLayout> rootLayoutList = m_stageLayoutData.rootLayoutList
        .Select(layout => layout.DeepCopy())
        .ToList();

        var map = MapData.GetInstance;
        var stageObjectFactory = StageObjectFactory.GetInstance();
        // あみだチューブの生成器
        var amidaGenerator = m_amidaTubeGenerator.generator.GetComponent<AmidaTubeGenerator>();

        amidaGenerator.m_addAmidaPos.Clear();
        amidaGenerator.m_horizonalAmidaPosY.Clear();


        // ギミックの生成
        foreach (var generation in rootLayoutList)
        {
            if (generation.createHorizonalAmidaLine)
            {
                // 横あみだラインの生成
                amidaGenerator.m_horizonalAmidaPosY.Add(generation.gridDataList.First().gimmickData.gridPos.y + 1);
            }


            foreach (var gridData in generation.gridDataList)
            {
                var gimmickData = gridData.gimmickData;

                GridPos fixedGridPos = new GridPos(gimmickData.gridPos.x, gimmickData.gridPos.y);

                // あみだチューブの設置座標の追加
                if (gimmickData.placeAmidaTube)
                {
                    amidaGenerator.m_addAmidaPos.Add(new GridPos(fixedGridPos.x + 1, fixedGridPos.y + 1));
                }


                if (gimmickData.changeToSatinFloor)
                {
                    // サテン床の生成
                    GameObject satinObject = stageObjectFactory.GenerateSatinFloor(floorParent, fixedGridPos);
                    // 生成されたオブジェクトの位置を設定
                    map.GetStageGridData().TryRePlaceFloorObject(fixedGridPos, satinObject);
                }

                if (gimmickData.blockType == StageLayoutData.GenerationType.NONE) continue; // ブロックの種類がNONEの場合はスキップ
                GameObject gimmickDataObject = null;



                switch (gimmickData.blockType)
                {
                    case StageLayoutData.GenerationType.FLUFF_BALL:
                        // フラフボールの生成
                        gimmickDataObject = stageObjectFactory.GenerateFluffBall(gimmickParent, fixedGridPos);
                        // 生成されたオブジェクトの位置を設定
                        map.GetStageGridData().TryPlaceTileObject(fixedGridPos, gimmickDataObject);
                        break;
                    case StageLayoutData.GenerationType.FELT_BLOCK:
                        // フェルトブロックの生成
                        gimmickDataObject = stageObjectFactory.GenerateFeltBlock(gimmickParent, fixedGridPos, gimmickData.emotionType);
                        // 生成されたオブジェクトの位置を設定
                        map.GetStageGridData().TryPlaceTileObject(fixedGridPos, gimmickDataObject);
                        break;
                    case StageLayoutData.GenerationType.FELT_BLOCK_NO_MOVEMENT:
                        // 動かないフェルトブロックの生成
                        gimmickDataObject = stageObjectFactory.GenerateNoMovementFeltBlock(gimmickParent, fixedGridPos);
                        // 生成されたオブジェクトの位置を設定
                        map.GetStageGridData().TryPlaceTileObject(fixedGridPos, gimmickDataObject);
                        break;
                    case StageLayoutData.GenerationType.CURTAIN:
                        // カーテンの生成
                        gimmickDataObject = stageObjectFactory.GenerateCurtain(gimmickParent, gimmickData.option, fixedGridPos, gimmickData.emotionType);
                        // 生成されたオブジェクトの位置を設定
                        map.GetStageGridData().TryPlaceTileObject(fixedGridPos, gimmickDataObject);
                        break;

                    case StageLayoutData.GenerationType.PAIR_BADGE:
                        // ペアバッジの生成

                        // 同じ種類の座標を全て取得
                        List<GridData> gimmickDataBlock = new List<GridData>();

                       
                        for (int i = 0; i < rootLayoutList.Count; i++)
                        {
                            var otherLayout = rootLayoutList[i];

                            foreach (var data in otherLayout.gridDataList)
                            {
                                if (data.gimmickData.blockType == StageLayoutData.GenerationType.PAIR_BADGE
                                    && data.gimmickData.emotionType == gimmickData.emotionType
                                    && !gimmickDataBlock.Contains(data))
                                {
                                    gimmickDataBlock.Add(data);
                                }
                            }
                        }

                        // 座標
                        var gimmickDataPosList = gimmickDataBlock.Select(data => new GridPos(data.gimmickData.gridPos.x, data.gimmickData.gridPos.y)).ToList();

                        // ペアバッジの生成
                        gimmickDataObject = stageObjectFactory.GeneratePairBadge(gimmickParent, gimmickDataPosList, gimmickData.emotionType);

                        // 生成されたオブジェクトの位置を設定
                        foreach (var feltBlock in gimmickDataObject.GetComponent<PairBadge>().GetFeltBlocks())
                        {
                            map.GetStageGridData().TryPlaceTileObject(feltBlock.stageBlock.GetGridPos(), feltBlock.gameObject);
                        }

                        foreach (var data in gimmickDataBlock)
                        {
                            data.gimmickData.blockType = StageLayoutData.GenerationType.NONE; // 生成したペアバッジの座標はNONEに設定
                        }

                        break;
                    case StageLayoutData.GenerationType.FRAGMENT:
                        // 想いの断片の生成
                        gimmickDataObject = stageObjectFactory.GenerateFragment(gimmickParent, fixedGridPos, gimmickData.emotionType, (int)gimmickData.option + 1);
                        // 生成されたオブジェクトの位置を設定
                        map.GetStageGridData().TryPlaceTileObject(fixedGridPos, gimmickDataObject);
                        break;


                }


            }

        }

        // あみだチューブの生成
        amidaGenerator.GenerateAmida(amidaParent);

        // 開始スロットの生成　
        foreach (var generation in m_stageLayoutData.SlotPlaceDataList)
        {
            // 始点スロットデータ
            if (generation.slotPlaceData[0] != null && generation.slotPlaceData[0].slotType != StageLayoutData.GenerationSlotType.NONE)
            {
                var slotData = generation.slotPlaceData[0];



                GridPos fixedGridPos = slotData.gridPos;

                // 始点の生成

                GameObject instanceObj = stageObjectFactory.GenerateFeelingSlot(feelingSlotParent, fixedGridPos, slotData.emotionType);


                // 新しい床オブジェクトを設定
                map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].floor = instanceObj;

                // 新しい床オブジェクトを設定
                map.GetStageGridData().TryPlaceTileObject(fixedGridPos, instanceObj);
                map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].tileObject.gameObject = instanceObj;

                if (instanceObj.GetComponent<FeelingSlot>() != null)
                {
                    // FeelingSlotの初期化
                    FeelingSlot feelingSlot = instanceObj.GetComponent<FeelingSlot>();
                    amidaManager.AddFeelingSlot(feelingSlot);
                }

            }

            // 終点スロットデータ
            if (generation.slotPlaceData[1] != null && generation.slotPlaceData[1].slotType != StageLayoutData.GenerationSlotType.NONE)
            {
                var slotData = generation.slotPlaceData[1];
                GridPos fixedGridPos = slotData.gridPos;
                // 型の生成
                GameObject instanceCoreObj = null;

                if (slotData.slotType == StageLayoutData.GenerationSlotType.SLOT_EMPTY)
                {
                    // 終点の想いの型（空）の生成
                    instanceCoreObj = stageObjectFactory.GenerateTerminusFeelingSlot(feelingSlotParent, fixedGridPos, slotData.emotionType, true);
                }
                else
                {
                    // 終点の生成
                    instanceCoreObj = stageObjectFactory.GenerateTerminusFeelingSlot(feelingSlotParent, fixedGridPos, slotData.emotionType, false);
                }


                // 新しい床オブジェクトを設定
                map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].floor = instanceCoreObj;

                // 新しい床オブジェクトを設定
                map.GetStageGridData().TryPlaceTileObject(fixedGridPos, instanceCoreObj);
                map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].tileObject.gameObject = instanceCoreObj;

                TerminusFeelingSlot terminusFeelingSlot = instanceCoreObj.GetComponent<TerminusFeelingSlot>();
                if (terminusFeelingSlot != null && instanceCoreObj?.GetComponent<TerminusFeelingSlotRefection>() == null)
                {
                    clearConditionChecker.AddTerminusFeelingSlot(terminusFeelingSlot);
                }

            }
        }





    }



}
