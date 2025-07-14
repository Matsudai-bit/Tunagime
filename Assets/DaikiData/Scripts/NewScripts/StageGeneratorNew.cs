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
    struct FloorGenerationData
    {
        public GameObject generator;    // �����@
        public float posY;              // �������WY
    }


    [SerializeField] private GenerationGimmickData[] m_gimmickData ;  // �M�~�b�N�̐����@

    [SerializeField] private FloorGenerationData m_amidaTubeGenerator;  // ���݂��`���[�u�̐����@

    [SerializeField] private FloorGenerationData m_topFloorBlockGenerator;  // �g�b�v�w�̏��u���b�N�̐����@

    


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
            


        // �M�~�b�N�u���b�N�O���b�h�̎擾
        GameObject[,] gimmickBlockGrid = new GameObject[m_map.GetCommonData().height, m_map.GetCommonData().width];

        foreach (var generation in m_gimmickData)
        {
            GridPos fixedGridPos = new GridPos ( generation.gridPos.x - 1, generation.gridPos.y - 1 );

            GameObject instanceObj = Instantiate(generation.prefab);
            map.GetStageGridData().GetTileData[fixedGridPos.y, fixedGridPos.x].tileObject.gameObject = instanceObj;

            StageBlock stageBlock = instanceObj.GetComponent<StageBlock>();

            stageBlock.Initialize(fixedGridPos);
        }


        // �X�e�[�W�O���b�h�f�[�^�ɃM�~�b�N�u���b�N�O���b�h��ݒ�
        //m_map.GetStageGridData().SetTopGimmickBlockGrid(gimmickBlockGrid);



    }


   

}
