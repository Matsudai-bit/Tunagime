using UnityEngine;
using static FloorManager;

public class AmidaTubeBlock : MonoBehaviour
{
    //private MapData m_map;                  // �}�b�v�f�[�^

    //private StageBlock m_stageBlock;        // �X�e�[�W�u���b�N

    //private AmidaManager m_amidaManager;    // ���݂��Ǘ�

    //private FloorConverter m_floorConverter;

    //bool m_onAmidaFloor = false;

    //public AmidaTube.DirectionPassage m_standardDirectionPassage;  // �����ʉߕ���

    //[SerializeField] private GameObject m_amidaTube;
    //[SerializeField] private GameObject m_floorBlock;


    //void Awake()
    //{
    //    m_stageBlock = GetComponent<StageBlock>();

    //}

    ///// <summary>
    ///// ����������
    ///// </summary>
    ///// <param name="map"></param>
    //public void Initialize(GridPos gridPos, MapData map, AmidaManager amidaManager, FloorConverter floorConverter, FloorManager.FloorType startFloorType, AmidaTube.DirectionPassage directionPassage)
    //{
    //    m_map = map;

    //    m_amidaManager = amidaManager;

    //    m_floorConverter = floorConverter;

    //    // �M�~�b�N�̋��ʏ�����
    //    m_stageBlock.Initialize(gridPos, startFloorType, map);

 
    //    m_standardDirectionPassage = directionPassage;

    //    m_amidaTube.GetComponent<AmidaTube>().SetDirectionPassage(directionPassage);

    //    if (startFloorType == FloorManager.FloorType.AMIDA)
    //        floorConverter.ConvertTopToAmidaFloor(gameObject);
    //}

    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Return))
    //    {
    //        //m_amidaManager.RequestAddGridAmidaTube(m_stageBlock.GetGridPos(), m_amidaTube);
    //        if (m_stageBlock.GetFloorType() == FloorType.TOP)
    //            m_onAmidaFloor = m_floorConverter.ConvertTopToAmidaFloor(gameObject);
    //        else
    //        {
    //            m_onAmidaFloor = !m_floorConverter.ConvertAmidaFloorToTopFloor(gameObject);
    //            m_amidaTube.GetComponent<AmidaTube>().Reset();
    //            m_amidaTube.GetComponent<AmidaTube>().SetDirectionPassage(m_standardDirectionPassage);
    //        }

    //    }
    //}

    ///// <summary>
    ///// ���݂��`���[�u�̎擾
    ///// </summary>
    ///// <returns>���݂��`���[�u</returns>
    //public GameObject GetAmidaTube()
    //{
    //    return m_amidaTube;
    //}

    ///// <summary>
    ///// ���u���b�N�̎擾
    ///// </summary>
    ///// <returns>���݂��`���[�u</returns>
    //public GameObject GetFloorBlock()
    //{
    //    return m_floorBlock;
    //}

    ///// <summary>
    ///// ��Ƃ��Ă��邠�݂��̒ʉߕ������擾����
    ///// </summary>
    ///// <returns>��Ƃ��Ă��邠�݂��̒ʉߕ���</returns>
    //public AmidaTube.DirectionPassage GetStandartDirectionPassage()
    //{
    //    return m_standardDirectionPassage;
    //}

    ///// <summary>
    ///// ��Ƃ��Ă��邠�݂��̒ʉߕ�����ݒ肷��
    ///// </summary>
    //public void SetStandartDirectionPassage(AmidaTube.DirectionPassage setDirection)
    //{
    //    m_standardDirectionPassage = setDirection;
    //}
}
