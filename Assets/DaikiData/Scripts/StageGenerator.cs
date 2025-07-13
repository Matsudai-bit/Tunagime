//using UnityEngine;
//using System.Collections.Generic;


///// <summary>
///// �X�e�[�W������
///// </summary>
//public class StageGenerator : MonoBehaviour
//{
    
//    public MapData m_map; // �X�e�[�W�f�[�^

//    public AmidaManager m_amidaManager;

//    [SerializeField] private GameObject m_powerSource;              // ���͌�
//    [SerializeField] private GameObject m_player;                   //�@�v���C���[
//    [SerializeField] private GameObject m_movableAmidaTubeBlock;    //�@�ړ��M�~�b�N
//    [SerializeField] private GameObject m_amidaTubeBlock;           //�@�M�~�b�N
//    [SerializeField] private GameObject m_rotableAmidaTubeBlock;    //�@��]�M�~�b�N
//    [SerializeField] private GameObject m_yellowSwitch;             //�@���F�X�C�b�`
//    [SerializeField] private GameObject m_purpleSwitch;             //�@���F�X�C�b�`
//    [SerializeField] private GameObject m_switchWall;               //�@�X�C�b�`�œ��삷���
//    [SerializeField] private GameObject m_doubleSwitchWall;         //�@��̃X�C�b�`�œ��삷���
//    [SerializeField] private GameObject m_jammingArea;              //�@�W�Q�̈�

//    [SerializeField] private GameObject m_clearItemA;              //�@�N���A�A�C�e��A
//    [SerializeField] private GameObject m_clearItemB;              //�@�N���A�A�C�e��B

//    [SerializeField] private GameObject m_connector;        //�@�R�l�N�^�[
//    [SerializeField] private GameObject m_connectSwitch;    //�@�R�l�N�^�[�̃X�C�b�`

//    [SerializeField] private FloorConverter m_floorConverter;  //�@�w�ϊ�
//    [SerializeField] private SwitchManager m_switchManager;     // �X�C�b�`�Ǘ�
//    /// <summary>
//    /// �����f�[�^
//    /// </summary>
//    [System.Serializable]
//    struct FloorGenerationData
//    {
//        public GameObject generator;    // �����@
//        public float posY;              // �������WY
//    }


//    [SerializeField] private FloorGenerationData m_amidaFloorBlockGenerator;  // ���݂��w�̏��u���b�N�̐����@
//    [SerializeField] private FloorGenerationData m_amidaTubeGenerator;  // ���݂��`���[�u�̐����@

//    [SerializeField] private FloorGenerationData m_topFloorBlockGenerator;  // �g�b�v�w�̏��u���b�N�̐����@
//    [SerializeField] private FloorGenerationData m_wallGenerator;           // �ǂ̐����@

    


//    private void Awake()
//    {
       
//    }

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    public void Start()
//    {
//        // ���㕔���W�̐ݒ�
//        float amidaFloorTopPartPosY = m_amidaFloorBlockGenerator.posY;
//        float topFloorTopPartPosY = m_topFloorBlockGenerator.posY;
        
//        // ���݂��w�̏��̐���
//        if (m_amidaFloorBlockGenerator.generator)
//            m_map.GetStageGridData().SetAmidaFloorBlockGrid(
//                m_amidaFloorBlockGenerator.generator.GetComponent<FloorBlockGenerator>().
//                GenerateFloor(
//                    m_amidaFloorBlockGenerator.posY,
//                    m_map,
//                    out amidaFloorTopPartPosY,
//                    false)
//            );

//        // �g�b�v�w�̐���
//        if (m_topFloorBlockGenerator.generator)
//            m_map.GetStageGridData().SetTopFloorBlockGrid(
//                m_topFloorBlockGenerator.generator.GetComponent<FloorBlockGenerator>().
//                GenerateFloor(
//                    m_topFloorBlockGenerator.posY,
//                    m_map,
//                    out topFloorTopPartPosY,
//                    true)
//            );
//        // �㕔���W�̐ݒ�
//        m_map.GetFloorManager().SetFloorTopPartPosY(FloorManager.FloorType.AMIDA, amidaFloorTopPartPosY);
//        m_map.GetFloorManager().SetFloorTopPartPosY(FloorManager.FloorType.TOP, topFloorTopPartPosY);

//        // ���݂��`���[�u�̐���
//        if (m_amidaTubeGenerator.generator)
//            m_map.GetStageGridData().SetAmidaTubeGrid(
//                m_amidaTubeGenerator.generator.GetComponent<AmidaTubeGenerator>().
//                    GenerateAmida(
//                        amidaFloorTopPartPosY,
//                        m_map)
//            );

//        // �ǂ̐���
//        if (m_wallGenerator.generator)
//            m_map.GetStageGridData().SetWallGrid(
//                m_wallGenerator.generator.GetComponent<WallGenerator>().
//                    GenerateWall(m_map)
//            );


//        // �M�~�b�N�u���b�N�O���b�h�̎擾
//        GameObject[,] gimmickBlockGrid = new GameObject[m_map.GetCommonData().height, m_map.GetCommonData().width];

//        // ���݂��̃M�~�b�N�O���b�h�̎擾
//        GameObject[,] amidaGimmickGrid = m_map.GetFloorManager().GetGimmickFloor(FloorManager.FloorType.AMIDA);

//        // �ǃO���b�h
//        GameObject[,] wallGrid = m_map.GetStageGridData().GetWallGrid();

//        // �X�e�[�W�O���b�h�f�[�^�ɃM�~�b�N�u���b�N�O���b�h��ݒ�
//        m_map.GetStageGridData().SetTopGimmickBlockGrid(gimmickBlockGrid);

//        // �t���A�}�l�[�W���[�ɂ��݂��M�~�b�N�O���b�h��ݒ�
//        m_map.GetFloorManager().SetGimmickFloor(FloorManager.FloorType.AMIDA, amidaGimmickGrid);

//        // �d�͌��̍쐬
//        GameObject powerSource = Instantiate(m_powerSource);
//        powerSource.GetComponent<PowerSource>().Initialize(new GridPos(0, 1), m_map, m_amidaManager);

//        // �v���C���[�̎擾
//        //GameObject player =  Instantiate(m_player);
//        m_player.GetComponent<Player>().Initialize(new GridPos(1, 1), m_map);

//        // �ړ��\���݂��u���b�N�̍쐬
//        GameObject movableAmidaTubeBlock = Instantiate(m_movableAmidaTubeBlock);
        
//        movableAmidaTubeBlock.GetComponent<AmidaTubeBlock>().Initialize(
//            new GridPos(3, 6), m_map, m_amidaManager, m_floorConverter, FloorManager.FloorType.TOP, new AmidaTube.DirectionPassage(false, true, true, false));


//        // �ړ��\�Ȃ��݂��`���[�u�u���b�N�𐶐����A������
//        GameObject movableAmidaTubeBlockA = Instantiate(m_movableAmidaTubeBlock);
//        movableAmidaTubeBlockA.GetComponent<AmidaTubeBlock>().Initialize(
//            new GridPos(4, 8), m_map, m_amidaManager, m_floorConverter, FloorManager.FloorType.TOP, new AmidaTube.DirectionPassage(true, true, false, false));

//        // �ړ��\�Ȃ��݂��`���[�u�u���b�N�𐶐����A������
//        GameObject movableAmidaTubeBlockB = Instantiate(m_movableAmidaTubeBlock);
//        movableAmidaTubeBlockB.GetComponent<AmidaTubeBlock>().Initialize(
//            new GridPos(6, 8), m_map, m_amidaManager, m_floorConverter, FloorManager.FloorType.TOP, new AmidaTube.DirectionPassage(false, true, true, false));

//        // �ړ��\�Ȃ��݂��`���[�u�u���b�N�𐶐����A������
//        GameObject movableAmidaTubeBlockC = Instantiate(m_movableAmidaTubeBlock);
//        movableAmidaTubeBlockC.GetComponent<AmidaTubeBlock>().Initialize(
//            new GridPos(6, 2), m_map, m_amidaManager, m_floorConverter, FloorManager.FloorType.AMIDA, new AmidaTube.DirectionPassage(true,true, false, false));

//        // �ړ��\�Ȃ��݂��`���[�u�u���b�N�𐶐����A������
//        GameObject movableAmidaTubeBlockD = Instantiate(m_movableAmidaTubeBlock);
//        movableAmidaTubeBlockD.GetComponent<AmidaTubeBlock>().Initialize(
//            new GridPos(15, 2), m_map, m_amidaManager, m_floorConverter, FloorManager.FloorType.TOP, new AmidaTube.DirectionPassage(true, true, false, false));


//        // ���݂��`���[�u�u���b�N�𐶐����A������
//        GameObject amidaTubeBlock = Instantiate(m_amidaTubeBlock);
//        amidaTubeBlock.GetComponent<AmidaTubeBlock>().Initialize(
//            new GridPos(5, 6), m_map, m_amidaManager, m_floorConverter, FloorManager.FloorType.TOP, new AmidaTube.DirectionPassage(true , true, false, false));

//        // ��]�\�Ȃ��݂��`���[�u�u���b�N�𐶐����A������
//        GameObject rotableAmidaTubeBlock = Instantiate(m_rotableAmidaTubeBlock);
//        rotableAmidaTubeBlock.GetComponent<AmidaTubeBlock>().Initialize(
//            new GridPos(7, 4), m_map, m_amidaManager, m_floorConverter, FloorManager.FloorType.TOP, new AmidaTube.DirectionPassage(false, true, true, false));

//        GameObject rotableAmidaTubeBlock2 = Instantiate(m_rotableAmidaTubeBlock);
//        rotableAmidaTubeBlock2.GetComponent<AmidaTubeBlock>().Initialize(
//            new GridPos(14, 8), m_map, m_amidaManager, m_floorConverter, FloorManager.FloorType.TOP, new AmidaTube.DirectionPassage(false, true, true, false));

//        // ���݂��̃M�~�b�N�O���b�h�̎擾
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

//        // �ǃO���b�h
//        wallGrid = m_map.GetStageGridData().GetWallGrid();

//        // ���F�X�C�b�`�𐶐����A������
//        GameObject yellowSwitch = Instantiate(m_yellowSwitch);
//        yellowSwitch.GetComponent<AmidaSwitch>().Initialize(new GridPos(3, 2), m_map);
//        yellowSwitch.GetComponent<Switch>().SetSwitchType(SwitchType.YELLOW);

//        // ���F�X�C�b�`�𐶐����A������
//        GameObject yellowSwitch2 = Instantiate(m_yellowSwitch);
//        yellowSwitch2.GetComponent<AmidaSwitch>().Initialize(new GridPos(15, 8), m_map);
//        yellowSwitch2.GetComponent<Switch>().SetSwitchType(SwitchType.YELLOW);
//        // ���F�X�C�b�`�𐶐����A������
//        GameObject purpleSwitch = Instantiate(m_purpleSwitch);
//        purpleSwitch.GetComponent<AmidaSwitch>().Initialize(new GridPos(20, 3), m_map);
//        purpleSwitch.GetComponent<Switch>().SetSwitchType(SwitchType.PURPLE);

//        // �R�l�N�g�X�C�b�`A�𐶐����A������
//        GameObject connectSwitchA = Instantiate(m_connectSwitch);
//        connectSwitchA.GetComponent<AmidaSwitch>().Initialize(new GridPos(6, 4), m_map);

//        // �R�l�N�g�X�C�b�`B�𐶐����A������
//        GameObject connectSwitchB = Instantiate(m_connectSwitch);
//        connectSwitchB.GetComponent<AmidaSwitch>().Initialize(new GridPos(11, 8), m_map);

//        GameObject connectSwitchC = Instantiate(m_connectSwitch);
//        connectSwitchC.GetComponent<AmidaSwitch>().Initialize(new GridPos(20,1), m_map);

//        // �R�l�N�^�[�𐶐����A������
//        GameObject connector = Instantiate(m_connector);
//        connector.GetComponent<Connector>().Initialize(m_amidaManager);
//        connector.GetComponent<Connector>().AddSwitch(connectSwitchA);
//        connector.GetComponent<Connector>().AddSwitch(connectSwitchB);
//        connector.GetComponent<Connector>().AddSwitch(connectSwitchC);
//        connector.GetComponent<Switch>().SetSwitchType(SwitchType.RED);

//        // �X�C�b�`�}�l�[�W���[�ɃX�C�b�`��ǉ�
//        m_switchManager.AddSwitch(yellowSwitch.GetComponent<Switch>());
//        m_switchManager.AddSwitch(yellowSwitch2.GetComponent<Switch>());
//        m_switchManager.AddSwitch(purpleSwitch.GetComponent<Switch>());
//        m_switchManager.AddSwitch(connector.GetComponent<Switch>());

//        // �X�C�b�`�^�C�v�̃��X�g���쐬
//        List<SwitchType> yellowSet = new List<SwitchType>();
//        yellowSet.Add(SwitchType.YELLOW);

//        List<SwitchType> yellowRedSet = new List<SwitchType>();
//        yellowRedSet.Add(SwitchType.RED);
//        yellowRedSet.Add(SwitchType.YELLOW);

//        // �X�C�b�`�^�C�v�̃��X�g����
//        List<SwitchType> redSet = new List<SwitchType>();
//        redSet.Add(SwitchType.RED);


//        // ���F�X�C�b�`�œ��삷��ǂ𐶐����A������
//        GameObject yellowSwitchWall = WallGenerator.CreateWall(WallGenerator.ConvertStageGridToLeftWallGrid(new GridPos(7, 9)), m_map, m_switchWall, null);
//        yellowSwitchWall.gameObject.GetComponentInChildren<MeshRenderer>().material = yellowSwitch.GetComponent<MeshRenderer>().material;
//        yellowSwitchWall.GetComponent<SwitchTypeController>().SetSwitchType(yellowSet);



//        // ��̃X�C�b�`�œ��삷��ǂ𐶐����A������
//        GameObject doubleSwitchWall = WallGenerator.CreateWall(WallGenerator.ConvertStageGridToLeftWallGrid(new GridPos(14, 4)), m_map, m_doubleSwitchWall, null);
//        doubleSwitchWall.GetComponent<SwitchTypeController>().SetSwitchType(yellowRedSet);


//        // �W���~���O�̈�̐���
//        GameObject jammingArea = Instantiate(m_jammingArea);
//        jammingArea.GetComponent<JammingArea>().Initialize(new GridPos(14, 1), 9, 6, m_map);
//        jammingArea.GetComponent<SwitchTypeController>().SetSwitchType(redSet);


//        // ���F�X�C�b�`�𐶐����A������
//        //GameObject clearItemA = Instantiate(m_clearItemA);
//        m_clearItemA.GetComponent<StageBlock>().Initialize(new GridPos(11, 2), FloorManager.FloorType.TOP, m_map);

//        //GameObject clearItemB = Instantiate(m_clearItemB);
//        m_clearItemB.GetComponent<StageBlock>().Initialize(new GridPos(18, 9), FloorManager.FloorType.TOP, m_map);



//        // �X�C�b�`�}�l�[�W���[�ɃX�C�b�`�E�H�[����ǉ�
//        m_switchManager.AddSwitchTypeController(yellowSwitchWall.GetComponent<SwitchTypeController>());
//        m_switchManager.AddSwitchTypeController(doubleSwitchWall.GetComponent<SwitchTypeController>());
//        m_switchManager.AddSwitchTypeController(jammingArea.GetComponent<SwitchTypeController>());

//        // �M�~�b�N�u���b�N�O���b�h�ɃI�u�W�F�N�g��z�u
//        gimmickBlockGrid[0, 1] = powerSource;
   

//        // ���݂��M�~�b�N�O���b�h�ɃI�u�W�F�N�g��z�u
//        amidaGimmickGrid[2, 3] = yellowSwitch;
//        amidaGimmickGrid[8, 15] = yellowSwitch2;
//        amidaGimmickGrid[3, 20] = purpleSwitch;
//        amidaGimmickGrid[4, 6] = connectSwitchA;
//        amidaGimmickGrid[8, 11] = connectSwitchB;
//        amidaGimmickGrid[1, 20] = connectSwitchB;

//        // �ǃO���b�h�ɃI�u�W�F�N�g��z�u
//        wallGrid[9, 7] = yellowSwitchWall;
//        wallGrid[4, 14] = doubleSwitchWall;

//        // �X�e�[�W�O���b�h�f�[�^�ɃM�~�b�N�u���b�N�O���b�h��ݒ�
//        m_map.GetStageGridData().SetTopGimmickBlockGrid(gimmickBlockGrid);

//        // �t���A�}�l�[�W���[�ɂ��݂��M�~�b�N�O���b�h��ݒ�
//        m_map.GetFloorManager().SetGimmickFloor(FloorManager.FloorType.AMIDA, amidaGimmickGrid);

//    }


//}
