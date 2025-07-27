using UnityEngine;
using System.Collections.Generic;
using UnityEditor;



/// <summary>
/// �X�e�[�W������
/// </summary>
public class StageGeneratorNew : MonoBehaviour
{
    
    public MapData m_map; // �X�e�[�W�f�[�^

    public AmidaManager m_amidaManager;

    [SerializeField] private GameObject m_player;                   //�@�v���C���[



    /// <summary>
    /// �M�~�b�N�����f�[�^
    /// </summary>
    [System.Serializable]
    struct GenerationGimmickData
    {
        public GridPos gridPos;
        public GameObject prefab;
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


    [SerializeField] private GenerationGimmickData[] m_gimmickData ;  // �M�~�b�N�̐����@
    [SerializeField] private GenerationGimmickData[] m_floorData ;  // �M�~�b�N�̐����@

    [SerializeField] private Generator m_amidaTubeGenerator;  // ���݂��`���[�u�̐����@

    [SerializeField] private Generator m_topFloorBlockGenerator;  // �g�b�v�w�̏��u���b�N�̐����@

    


    private void Awake()
    {
       
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        // ���㕔���W�̐ݒ�
        float topFloorTopPartPosY = m_topFloorBlockGenerator.posY;
        float amidaPosY = m_amidaTubeGenerator.posY;

        var map = MapData.GetInstance;

        
        // �g�b�v�w�̐���
        if (m_topFloorBlockGenerator.generator)
                m_topFloorBlockGenerator.generator.GetComponent<FloorBlockGenerator>().
                GenerateFloor(
                    false
            );

        // ���݂��`���[�u�̐���
        if (m_amidaTubeGenerator.generator)

            m_amidaTubeGenerator.generator.GetComponent<AmidaTubeGenerator>().GenerateAmida();




        // �M�~�b�N�̐���
        foreach (var generation in m_gimmickData)
        {
            GridPos fixedGridPos = new GridPos ( generation.gridPos.x - 1, generation.gridPos.y - 1 );

            GameObject instanceObj = Instantiate(generation.prefab);
            map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].tileObject.gameObject = instanceObj;

            StageBlock stageBlock = instanceObj.GetComponent<StageBlock>();

            stageBlock.Initialize(fixedGridPos);
        }

        // ���̐����@�ʏ폰�͎����Ő�������Ă���
        foreach (var generation in m_floorData)
        {
            
            GridPos fixedGridPos = new GridPos(generation.gridPos.x, generation.gridPos.y);


            // ���̐���
            GameObject instanceObj = Instantiate(generation.prefab);

            //Vector3 pos = map.ConvertGridToWorldPos(fixedGridPos.x, fixedGridPos.y);
            //instanceObj.transform.position = new Vector3(pos.x, instanceObj.transform.position.y, pos.z);

            // ���X�̏��I�u�W�F�N�g�𖳌���
            map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].floor.SetActive(false);
            // �V�������I�u�W�F�N�g��ݒ�
            map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].floor = instanceObj;

            StageBlock stageBlock = instanceObj.GetComponent<StageBlock>();
            stageBlock.Initialize(fixedGridPos);

            if (instanceObj.GetComponent<FeelingSlot>() != null)
            {
                // FeelingSlot�̏�����
                FeelingSlot feelingSlot = instanceObj.GetComponent<FeelingSlot>();
                m_amidaManager.AddFeelingSlot(feelingSlot);
            }


        }





    }




}
