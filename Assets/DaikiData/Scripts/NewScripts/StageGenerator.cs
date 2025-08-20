using System.Linq;
using UnityEngine;




/// <summary>
/// �X�e�[�W������
/// </summary>
public class StageGenerator : MonoBehaviour
{
   



    /// <summary>
    /// �M�~�b�N�����f�[�^
    /// </summary>
    [System.Serializable]
    class GenerationGimmickData
    {
        public GridPos gridPos;
        public GameObject prefab;
        public GenerationType blockType; // �u���b�N�̎��
        public Vector3 rotate;          // ��]
        public EmotionCurrent.Type emotionType; // ����^�C�v
    }

    enum GenerationType
    {
        FELT_BLOCK,             // �t�F���g�u���b�N
        FLUFF_BALL,             // �ю���
        FELT_BLOCK_NO_MOVEMENT, // �����Ȃ��t�F���g�u���b�N
        FLOOR,                  // ��
        TERMINUS,               // �I�_
        CURTAIN,                // �J�[�e��
        SATIN_FLOOR,            // �T�e����
        PAIR_BADGE,             // �y�A�o�b�W
        FRAGMENT,               // �z���̒f��
        NONE,                 // �Ȃ�
    }




    /// <summary>
    /// �����f�[�^
    /// </summary>
    [System.Serializable]
    struct Generator
    {
        public GameObject generator;    // �����@
        public float posY;              // �������WY
    }


    [Header("==== �����f�[�^ ==== ")]
    [Header("�M�~�b�N�����f�[�^")]
    [SerializeField] private GenerationGimmickData[] m_gimmickData ;  // �M�~�b�N�̐����@
    [Header("�n�_�j�����f�[�^")]
    [SerializeField] private GenerationGimmickData[] m_floorData ;  // �M�~�b�N�̐����@
    [Header("�I�_�j�����f�[�^")]
    [SerializeField] private GenerationGimmickData[] m_terminusData;  // �I�_�̐����@

    [Header("==== �����@ ==== ")]
    
    [SerializeField] private Generator m_amidaTubeGenerator;  // ���݂��`���[�u�̐����@

    [SerializeField] private Generator m_topFloorBlockGenerator;  // �g�b�v�w�̏��u���b�N�̐����@

    
   
    private void Awake()
    {
       
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
      
    }

    public void Generate(AmidaManager amidaManager, Transform amidaParent, Transform floorParent, Transform gimmickParent, ClearConditionChecker clearConditionChecker)
    {

        MaterialLibrary.GetInstance.GetMaterial(MaterialLibrary.MaterialGroup.YARN, EmotionCurrent.Type.FAITHFULNESS); // �}�e���A�����C�u�����̏�����

        var map = MapData.GetInstance;
        map.Initialize(); // �}�b�v�f�[�^�̏�����
        // �w�i�̐���
        Instantiate(map.GetStagePrefab());

        // ���㕔���W�̐ݒ�
        float topFloorTopPartPosY = m_topFloorBlockGenerator.posY;
        float amidaPosY = m_amidaTubeGenerator.posY;



        // �g�b�v�w�̐���
        if (m_topFloorBlockGenerator.generator)
            m_topFloorBlockGenerator.generator.GetComponent<FloorBlockGenerator>().
            GenerateFloor(false, floorParent);

        // ���݂��`���[�u�̐���
        if (m_amidaTubeGenerator.generator)

            m_amidaTubeGenerator.generator.GetComponent<AmidaTubeGenerator>().GenerateAmida(amidaParent);



        var stageObjectFactory = StageObjectFactory.GetInstance(); 
        // �M�~�b�N�̐���
        foreach (var generation in m_gimmickData)
        {
            if (generation.blockType == GenerationType.NONE) continue; // �u���b�N�̎�ނ�NONE�̏ꍇ�̓X�L�b�v

            GridPos fixedGridPos = new GridPos(generation.gridPos.x - 1, generation.gridPos.y - 1);
            GameObject generationObject = null;           

            switch (generation.blockType)
            {
                case GenerationType.FLUFF_BALL:
                    // �t���t�{�[���̐���
                    generationObject = stageObjectFactory.GenerateFluffBall(gimmickParent, fixedGridPos);
                    // �������ꂽ�I�u�W�F�N�g�̈ʒu��ݒ�
                    map.GetStageGridData().TryPlaceTileObject(fixedGridPos, generationObject);
                    break;
                case GenerationType.FELT_BLOCK:
                    // �t�F���g�u���b�N�̐���
                    generationObject = stageObjectFactory.GenerateFeltBlock(gimmickParent, fixedGridPos, generation.emotionType);
                    // �������ꂽ�I�u�W�F�N�g�̈ʒu��ݒ�
                    map.GetStageGridData().TryPlaceTileObject(fixedGridPos, generationObject);
                    break;
                case GenerationType.FELT_BLOCK_NO_MOVEMENT:
                    // �����Ȃ��t�F���g�u���b�N�̐���
                    generationObject = stageObjectFactory.GenerateNoMovementFeltBlock(gimmickParent, fixedGridPos);
                    // �������ꂽ�I�u�W�F�N�g�̈ʒu��ݒ�
                    map.GetStageGridData().TryPlaceTileObject(fixedGridPos, generationObject);
                    break;
                case GenerationType.CURTAIN:
                    // �J�[�e���̐���
                    generationObject = stageObjectFactory.GenerateCurtain(gimmickParent, generation.rotate.y, fixedGridPos, generation.emotionType);
                    // �������ꂽ�I�u�W�F�N�g�̈ʒu��ݒ�
                    map.GetStageGridData().TryPlaceTileObject(fixedGridPos, generationObject);
                    break;
                case GenerationType.SATIN_FLOOR:
                    // �T�e�����̐���
                    generationObject = stageObjectFactory.GenerateSatinFloor(gimmickParent, fixedGridPos);
                    // �������ꂽ�I�u�W�F�N�g�̈ʒu��ݒ�
                    map.GetStageGridData().TryRePlaceFloorObject(fixedGridPos, generationObject);
                    break;
                case GenerationType.PAIR_BADGE:
                    // �y�A�o�b�W�̐���

                    // ������ނ̍��W��S�Ď擾
                    var generationBlock = m_gimmickData.Where(data => data.blockType == GenerationType.PAIR_BADGE && data.emotionType == generation.emotionType).ToList();
                    // ���W
                    var generationPosList = generationBlock.Select(data => new GridPos(data.gridPos.x - 1, data.gridPos.y - 1)).ToList();

                    // �y�A�o�b�W�̐���
                    generationObject = stageObjectFactory.GeneratePairBadge(gimmickParent, generationPosList, generation.emotionType);

                    // �������ꂽ�I�u�W�F�N�g�̈ʒu��ݒ�
                    
                    foreach (var feltBlock in generationObject.GetComponent<PairBadge>().GetFeltBlocks())
                    {
                        map.GetStageGridData().TryPlaceTileObject(feltBlock.stageBlock.GetGridPos(), feltBlock.gameObject);
                    }

                    foreach (var data in generationBlock)
                    {
                        data.blockType = GenerationType.NONE; // ���������y�A�o�b�W�̍��W��NONE�ɐݒ�
                    }

                    break;
                case GenerationType.FRAGMENT:
                    // �z���̒f�Ђ̐���
                    generationObject = stageObjectFactory.GenerateFragment(gimmickParent, fixedGridPos, generation.emotionType);
                    // �������ꂽ�I�u�W�F�N�g�̈ʒu��ݒ�
                    map.GetStageGridData().TryPlaceTileObject(fixedGridPos, generationObject);
                    break;

            }

        
        }

        // ���̐����@�ʏ폰�͎����Ő�������Ă���
        foreach (var generation in m_floorData)
        {

            GridPos fixedGridPos = new GridPos(generation.gridPos.x - 1, generation.gridPos.y - 1);


            // ���̐���
            GameObject instanceObj = Instantiate(generation.prefab, gimmickParent);

            //Vector3 pos = map.ConvertGridToWorldPos(fixedGridPos.x, fixedGridPos.y);
            //instanceObj.transform.position = new Vector3(pos.x, instanceObj.transform.position.y, pos.z);

            // ���X�̏��I�u�W�F�N�g�𖳌���
            map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].floor.SetActive(false);

            // �V�������I�u�W�F�N�g��ݒ�
            map.GetStageGridData().TryPlaceTileObject(fixedGridPos, instanceObj);
            map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].tileObject.gameObject = instanceObj;

            StageBlock stageBlock = instanceObj.GetComponent<StageBlock>();
            stageBlock.SetBlockType(StageBlock.BlockType.FEELING_SLOT);
            stageBlock.Initialize(fixedGridPos);


            if (instanceObj.GetComponent<FeelingSlot>() != null)
            {
                // FeelingSlot�̏�����
                FeelingSlot feelingSlot = instanceObj.GetComponent<FeelingSlot>();
                amidaManager.AddFeelingSlot(feelingSlot);
            }


        }

        // �I�_�̐���
        foreach (var generation in m_terminusData)
        {
            GridPos fixedGridPos = new GridPos(generation.gridPos.x - 1, generation.gridPos.y - 1);
            GameObject instanceObj = Instantiate(generation.prefab, gimmickParent);

            // �V�������I�u�W�F�N�g��ݒ�
            map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].floor = instanceObj;

            // �V�������I�u�W�F�N�g��ݒ�
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
