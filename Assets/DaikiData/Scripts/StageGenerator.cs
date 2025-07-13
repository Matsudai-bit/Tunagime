//using UnityEngine;
//using System.Collections.Generic;


///// <summary>
///// ステージ生成器
///// </summary>
//public class StageGenerator : MonoBehaviour
//{
    
//    public MapData m_map; // ステージデータ

//    public AmidaManager m_amidaManager;

//    [SerializeField] private GameObject m_powerSource;              // 動力源
//    [SerializeField] private GameObject m_player;                   //　プレイヤー
//    [SerializeField] private GameObject m_movableAmidaTubeBlock;    //　移動ギミック
//    [SerializeField] private GameObject m_amidaTubeBlock;           //　ギミック
//    [SerializeField] private GameObject m_rotableAmidaTubeBlock;    //　回転ギミック
//    [SerializeField] private GameObject m_yellowSwitch;             //　黄色スイッチ
//    [SerializeField] private GameObject m_purpleSwitch;             //　紫色スイッチ
//    [SerializeField] private GameObject m_switchWall;               //　スイッチで動作する壁
//    [SerializeField] private GameObject m_doubleSwitchWall;         //　二つのスイッチで動作する壁
//    [SerializeField] private GameObject m_jammingArea;              //　妨害領域

//    [SerializeField] private GameObject m_clearItemA;              //　クリアアイテムA
//    [SerializeField] private GameObject m_clearItemB;              //　クリアアイテムB

//    [SerializeField] private GameObject m_connector;        //　コネクター
//    [SerializeField] private GameObject m_connectSwitch;    //　コネクターのスイッチ

//    [SerializeField] private FloorConverter m_floorConverter;  //　層変換
//    [SerializeField] private SwitchManager m_switchManager;     // スイッチ管理
//    /// <summary>
//    /// 生成データ
//    /// </summary>
//    [System.Serializable]
//    struct FloorGenerationData
//    {
//        public GameObject generator;    // 生成機
//        public float posY;              // 生成座標Y
//    }


//    [SerializeField] private FloorGenerationData m_amidaFloorBlockGenerator;  // あみだ層の床ブロックの生成機
//    [SerializeField] private FloorGenerationData m_amidaTubeGenerator;  // あみだチューブの生成機

//    [SerializeField] private FloorGenerationData m_topFloorBlockGenerator;  // トップ層の床ブロックの生成機
//    [SerializeField] private FloorGenerationData m_wallGenerator;           // 壁の生成機

    


//    private void Awake()
//    {
       
//    }

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    public void Start()
//    {
//        // 床上部座標の設定
//        float amidaFloorTopPartPosY = m_amidaFloorBlockGenerator.posY;
//        float topFloorTopPartPosY = m_topFloorBlockGenerator.posY;
        
//        // あみだ層の床の生成
//        if (m_amidaFloorBlockGenerator.generator)
//            m_map.GetStageGridData().SetAmidaFloorBlockGrid(
//                m_amidaFloorBlockGenerator.generator.GetComponent<FloorBlockGenerator>().
//                GenerateFloor(
//                    m_amidaFloorBlockGenerator.posY,
//                    m_map,
//                    out amidaFloorTopPartPosY,
//                    false)
//            );

//        // トップ層の生成
//        if (m_topFloorBlockGenerator.generator)
//            m_map.GetStageGridData().SetTopFloorBlockGrid(
//                m_topFloorBlockGenerator.generator.GetComponent<FloorBlockGenerator>().
//                GenerateFloor(
//                    m_topFloorBlockGenerator.posY,
//                    m_map,
//                    out topFloorTopPartPosY,
//                    true)
//            );
//        // 上部座標の設定
//        m_map.GetFloorManager().SetFloorTopPartPosY(FloorManager.FloorType.AMIDA, amidaFloorTopPartPosY);
//        m_map.GetFloorManager().SetFloorTopPartPosY(FloorManager.FloorType.TOP, topFloorTopPartPosY);

//        // あみだチューブの生成
//        if (m_amidaTubeGenerator.generator)
//            m_map.GetStageGridData().SetAmidaTubeGrid(
//                m_amidaTubeGenerator.generator.GetComponent<AmidaTubeGenerator>().
//                    GenerateAmida(
//                        amidaFloorTopPartPosY,
//                        m_map)
//            );

//        // 壁の生成
//        if (m_wallGenerator.generator)
//            m_map.GetStageGridData().SetWallGrid(
//                m_wallGenerator.generator.GetComponent<WallGenerator>().
//                    GenerateWall(m_map)
//            );


//        // ギミックブロックグリッドの取得
//        GameObject[,] gimmickBlockGrid = new GameObject[m_map.GetCommonData().height, m_map.GetCommonData().width];

//        // あみだのギミックグリッドの取得
//        GameObject[,] amidaGimmickGrid = m_map.GetFloorManager().GetGimmickFloor(FloorManager.FloorType.AMIDA);

//        // 壁グリッド
//        GameObject[,] wallGrid = m_map.GetStageGridData().GetWallGrid();

//        // ステージグリッドデータにギミックブロックグリッドを設定
//        m_map.GetStageGridData().SetTopGimmickBlockGrid(gimmickBlockGrid);

//        // フロアマネージャーにあみだギミックグリッドを設定
//        m_map.GetFloorManager().SetGimmickFloor(FloorManager.FloorType.AMIDA, amidaGimmickGrid);

//        // 電力源の作成
//        GameObject powerSource = Instantiate(m_powerSource);
//        powerSource.GetComponent<PowerSource>().Initialize(new GridPos(0, 1), m_map, m_amidaManager);

//        // プレイヤーの取得
//        //GameObject player =  Instantiate(m_player);
//        m_player.GetComponent<Player>().Initialize(new GridPos(1, 1), m_map);

//        // 移動可能あみだブロックの作成
//        GameObject movableAmidaTubeBlock = Instantiate(m_movableAmidaTubeBlock);
        
//        movableAmidaTubeBlock.GetComponent<AmidaTubeBlock>().Initialize(
//            new GridPos(3, 6), m_map, m_amidaManager, m_floorConverter, FloorManager.FloorType.TOP, new AmidaTube.DirectionPassage(false, true, true, false));


//        // 移動可能なあみだチューブブロックを生成し、初期化
//        GameObject movableAmidaTubeBlockA = Instantiate(m_movableAmidaTubeBlock);
//        movableAmidaTubeBlockA.GetComponent<AmidaTubeBlock>().Initialize(
//            new GridPos(4, 8), m_map, m_amidaManager, m_floorConverter, FloorManager.FloorType.TOP, new AmidaTube.DirectionPassage(true, true, false, false));

//        // 移動可能なあみだチューブブロックを生成し、初期化
//        GameObject movableAmidaTubeBlockB = Instantiate(m_movableAmidaTubeBlock);
//        movableAmidaTubeBlockB.GetComponent<AmidaTubeBlock>().Initialize(
//            new GridPos(6, 8), m_map, m_amidaManager, m_floorConverter, FloorManager.FloorType.TOP, new AmidaTube.DirectionPassage(false, true, true, false));

//        // 移動可能なあみだチューブブロックを生成し、初期化
//        GameObject movableAmidaTubeBlockC = Instantiate(m_movableAmidaTubeBlock);
//        movableAmidaTubeBlockC.GetComponent<AmidaTubeBlock>().Initialize(
//            new GridPos(6, 2), m_map, m_amidaManager, m_floorConverter, FloorManager.FloorType.AMIDA, new AmidaTube.DirectionPassage(true,true, false, false));

//        // 移動可能なあみだチューブブロックを生成し、初期化
//        GameObject movableAmidaTubeBlockD = Instantiate(m_movableAmidaTubeBlock);
//        movableAmidaTubeBlockD.GetComponent<AmidaTubeBlock>().Initialize(
//            new GridPos(15, 2), m_map, m_amidaManager, m_floorConverter, FloorManager.FloorType.TOP, new AmidaTube.DirectionPassage(true, true, false, false));


//        // あみだチューブブロックを生成し、初期化
//        GameObject amidaTubeBlock = Instantiate(m_amidaTubeBlock);
//        amidaTubeBlock.GetComponent<AmidaTubeBlock>().Initialize(
//            new GridPos(5, 6), m_map, m_amidaManager, m_floorConverter, FloorManager.FloorType.TOP, new AmidaTube.DirectionPassage(true , true, false, false));

//        // 回転可能なあみだチューブブロックを生成し、初期化
//        GameObject rotableAmidaTubeBlock = Instantiate(m_rotableAmidaTubeBlock);
//        rotableAmidaTubeBlock.GetComponent<AmidaTubeBlock>().Initialize(
//            new GridPos(7, 4), m_map, m_amidaManager, m_floorConverter, FloorManager.FloorType.TOP, new AmidaTube.DirectionPassage(false, true, true, false));

//        GameObject rotableAmidaTubeBlock2 = Instantiate(m_rotableAmidaTubeBlock);
//        rotableAmidaTubeBlock2.GetComponent<AmidaTubeBlock>().Initialize(
//            new GridPos(14, 8), m_map, m_amidaManager, m_floorConverter, FloorManager.FloorType.TOP, new AmidaTube.DirectionPassage(false, true, true, false));

//        // あみだのギミックグリッドの取得
//        amidaGimmickGrid = m_map.GetFloorManager().GetGimmickFloor(FloorManager.FloorType.AMIDA);

//        //gimmickBlockGrid[9, 9] = movableAmidaTubeBlockA;
//        gimmickBlockGrid[6, 3] = movableAmidaTubeBlock;
//        gimmickBlockGrid[8, 4] = movableAmidaTubeBlockA;
//        gimmickBlockGrid[8, 6] = movableAmidaTubeBlockB;
//        gimmickBlockGrid[2, 6] = movableAmidaTubeBlockC;
//        gimmickBlockGrid[2, 16] = movableAmidaTubeBlockD;
//        gimmickBlockGrid[6, 5] = amidaTubeBlock;
//        gimmickBlockGrid[4, 7] = rotableAmidaTubeBlock;
//        gimmickBlockGrid[8, 14] = rotableAmidaTubeBlock2;

//        // 壁グリッド
//        wallGrid = m_map.GetStageGridData().GetWallGrid();

//        // 黄色スイッチを生成し、初期化
//        GameObject yellowSwitch = Instantiate(m_yellowSwitch);
//        yellowSwitch.GetComponent<AmidaSwitch>().Initialize(new GridPos(3, 2), m_map);
//        yellowSwitch.GetComponent<Switch>().SetSwitchType(SwitchType.YELLOW);

//        // 黄色スイッチを生成し、初期化
//        GameObject yellowSwitch2 = Instantiate(m_yellowSwitch);
//        yellowSwitch2.GetComponent<AmidaSwitch>().Initialize(new GridPos(15, 8), m_map);
//        yellowSwitch2.GetComponent<Switch>().SetSwitchType(SwitchType.YELLOW);
//        // 紫色スイッチを生成し、初期化
//        GameObject purpleSwitch = Instantiate(m_purpleSwitch);
//        purpleSwitch.GetComponent<AmidaSwitch>().Initialize(new GridPos(20, 3), m_map);
//        purpleSwitch.GetComponent<Switch>().SetSwitchType(SwitchType.PURPLE);

//        // コネクトスイッチAを生成し、初期化
//        GameObject connectSwitchA = Instantiate(m_connectSwitch);
//        connectSwitchA.GetComponent<AmidaSwitch>().Initialize(new GridPos(6, 4), m_map);

//        // コネクトスイッチBを生成し、初期化
//        GameObject connectSwitchB = Instantiate(m_connectSwitch);
//        connectSwitchB.GetComponent<AmidaSwitch>().Initialize(new GridPos(11, 8), m_map);

//        GameObject connectSwitchC = Instantiate(m_connectSwitch);
//        connectSwitchC.GetComponent<AmidaSwitch>().Initialize(new GridPos(20,1), m_map);

//        // コネクターを生成し、初期化
//        GameObject connector = Instantiate(m_connector);
//        connector.GetComponent<Connector>().Initialize(m_amidaManager);
//        connector.GetComponent<Connector>().AddSwitch(connectSwitchA);
//        connector.GetComponent<Connector>().AddSwitch(connectSwitchB);
//        connector.GetComponent<Connector>().AddSwitch(connectSwitchC);
//        connector.GetComponent<Switch>().SetSwitchType(SwitchType.RED);

//        // スイッチマネージャーにスイッチを追加
//        m_switchManager.AddSwitch(yellowSwitch.GetComponent<Switch>());
//        m_switchManager.AddSwitch(yellowSwitch2.GetComponent<Switch>());
//        m_switchManager.AddSwitch(purpleSwitch.GetComponent<Switch>());
//        m_switchManager.AddSwitch(connector.GetComponent<Switch>());

//        // スイッチタイプのリストを作成
//        List<SwitchType> yellowSet = new List<SwitchType>();
//        yellowSet.Add(SwitchType.YELLOW);

//        List<SwitchType> yellowRedSet = new List<SwitchType>();
//        yellowRedSet.Add(SwitchType.RED);
//        yellowRedSet.Add(SwitchType.YELLOW);

//        // スイッチタイプのリストを作
//        List<SwitchType> redSet = new List<SwitchType>();
//        redSet.Add(SwitchType.RED);


//        // 黄色スイッチで動作する壁を生成し、初期化
//        GameObject yellowSwitchWall = WallGenerator.CreateWall(WallGenerator.ConvertStageGridToLeftWallGrid(new GridPos(7, 9)), m_map, m_switchWall, null);
//        yellowSwitchWall.gameObject.GetComponentInChildren<MeshRenderer>().material = yellowSwitch.GetComponent<MeshRenderer>().material;
//        yellowSwitchWall.GetComponent<SwitchTypeController>().SetSwitchType(yellowSet);



//        // 二つのスイッチで動作する壁を生成し、初期化
//        GameObject doubleSwitchWall = WallGenerator.CreateWall(WallGenerator.ConvertStageGridToLeftWallGrid(new GridPos(14, 4)), m_map, m_doubleSwitchWall, null);
//        doubleSwitchWall.GetComponent<SwitchTypeController>().SetSwitchType(yellowRedSet);


//        // ジャミング領域の生成
//        GameObject jammingArea = Instantiate(m_jammingArea);
//        jammingArea.GetComponent<JammingArea>().Initialize(new GridPos(14, 1), 9, 6, m_map);
//        jammingArea.GetComponent<SwitchTypeController>().SetSwitchType(redSet);


//        // 黄色スイッチを生成し、初期化
//        //GameObject clearItemA = Instantiate(m_clearItemA);
//        m_clearItemA.GetComponent<StageBlock>().Initialize(new GridPos(11, 2), FloorManager.FloorType.TOP, m_map);

//        //GameObject clearItemB = Instantiate(m_clearItemB);
//        m_clearItemB.GetComponent<StageBlock>().Initialize(new GridPos(18, 9), FloorManager.FloorType.TOP, m_map);



//        // スイッチマネージャーにスイッチウォールを追加
//        m_switchManager.AddSwitchTypeController(yellowSwitchWall.GetComponent<SwitchTypeController>());
//        m_switchManager.AddSwitchTypeController(doubleSwitchWall.GetComponent<SwitchTypeController>());
//        m_switchManager.AddSwitchTypeController(jammingArea.GetComponent<SwitchTypeController>());

//        // ギミックブロックグリッドにオブジェクトを配置
//        gimmickBlockGrid[0, 1] = powerSource;
   

//        // あみだギミックグリッドにオブジェクトを配置
//        amidaGimmickGrid[2, 3] = yellowSwitch;
//        amidaGimmickGrid[8, 15] = yellowSwitch2;
//        amidaGimmickGrid[3, 20] = purpleSwitch;
//        amidaGimmickGrid[4, 6] = connectSwitchA;
//        amidaGimmickGrid[8, 11] = connectSwitchB;
//        amidaGimmickGrid[1, 20] = connectSwitchB;

//        // 壁グリッドにオブジェクトを配置
//        wallGrid[9, 7] = yellowSwitchWall;
//        wallGrid[4, 14] = doubleSwitchWall;

//        // ステージグリッドデータにギミックブロックグリッドを設定
//        m_map.GetStageGridData().SetTopGimmickBlockGrid(gimmickBlockGrid);

//        // フロアマネージャーにあみだギミックグリッドを設定
//        m_map.GetFloorManager().SetGimmickFloor(FloorManager.FloorType.AMIDA, amidaGimmickGrid);

//    }


//}
