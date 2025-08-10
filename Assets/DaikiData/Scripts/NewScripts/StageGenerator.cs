using UnityEngine;
using System.Collections.Generic;
using UnityEditor;



/// <summary>
/// �X�e�[�W������
/// </summary>
public class StageGenerator : MonoBehaviour
{
   



    /// <summary>
    /// �M�~�b�N�����f�[�^
    /// </summary>
    [System.Serializable]
    struct GenerationGimmickData
    {
        public GridPos gridPos;
        public GameObject prefab;
        public StageBlock.BlockType blockType; // �u���b�N�̎��
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
    [Header("�������f�[�^")]
    [SerializeField] private GenerationGimmickData[] m_floorData ;  // �M�~�b�N�̐����@
    [Header("�I�_�����f�[�^")]
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
        // ���㕔���W�̐ݒ�
        float topFloorTopPartPosY = m_topFloorBlockGenerator.posY;
        float amidaPosY = m_amidaTubeGenerator.posY;

        var map = MapData.GetInstance;


        // �g�b�v�w�̐���
        if (m_topFloorBlockGenerator.generator)
            m_topFloorBlockGenerator.generator.GetComponent<FloorBlockGenerator>().
            GenerateFloor(false, floorParent);

        // ���݂��`���[�u�̐���
        if (m_amidaTubeGenerator.generator)

            m_amidaTubeGenerator.generator.GetComponent<AmidaTubeGenerator>().GenerateAmida(amidaParent);




        // �M�~�b�N�̐���
        foreach (var generation in m_gimmickData)
        {
            GridPos fixedGridPos = new GridPos(generation.gridPos.x - 1, generation.gridPos.y - 1);

            GameObject instanceObj = Instantiate(generation.prefab, gimmickParent);
            map.GetStageGridData().TryPlaceTileObject(fixedGridPos, instanceObj);

            StageBlock stageBlock = instanceObj.GetComponent<StageBlock>();
            stageBlock.SetBlockType(generation.blockType);


            stageBlock.Initialize(fixedGridPos);
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
            if (terminusFeelingSlot != null)
            {
                clearConditionChecker.AddTerminusFeelingSlot(terminusFeelingSlot);
            }
        }





    }

  

}
