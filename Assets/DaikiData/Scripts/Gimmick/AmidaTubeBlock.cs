using UnityEngine;
using static FloorManager;

public class AmidaTubeBlock : MonoBehaviour
{
    //private MapData m_map;                  // マップデータ

    //private StageBlock m_stageBlock;        // ステージブロック

    //private AmidaManager m_amidaManager;    // あみだ管理

    //private FloorConverter m_floorConverter;

    //bool m_onAmidaFloor = false;

    //public AmidaTube.DirectionPassage m_standardDirectionPassage;  // 初期通過方向

    //[SerializeField] private GameObject m_amidaTube;
    //[SerializeField] private GameObject m_floorBlock;


    //void Awake()
    //{
    //    m_stageBlock = GetComponent<StageBlock>();

    //}

    ///// <summary>
    ///// 初期化処理
    ///// </summary>
    ///// <param name="map"></param>
    //public void Initialize(GridPos gridPos, MapData map, AmidaManager amidaManager, FloorConverter floorConverter, FloorManager.FloorType startFloorType, AmidaTube.DirectionPassage directionPassage)
    //{
    //    m_map = map;

    //    m_amidaManager = amidaManager;

    //    m_floorConverter = floorConverter;

    //    // ギミックの共通初期化
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
    ///// あみだチューブの取得
    ///// </summary>
    ///// <returns>あみだチューブ</returns>
    //public GameObject GetAmidaTube()
    //{
    //    return m_amidaTube;
    //}

    ///// <summary>
    ///// 床ブロックの取得
    ///// </summary>
    ///// <returns>あみだチューブ</returns>
    //public GameObject GetFloorBlock()
    //{
    //    return m_floorBlock;
    //}

    ///// <summary>
    ///// 基準としているあみだの通過方向を取得する
    ///// </summary>
    ///// <returns>基準としているあみだの通過方向</returns>
    //public AmidaTube.DirectionPassage GetStandartDirectionPassage()
    //{
    //    return m_standardDirectionPassage;
    //}

    ///// <summary>
    ///// 基準としているあみだの通過方向を設定する
    ///// </summary>
    //public void SetStandartDirectionPassage(AmidaTube.DirectionPassage setDirection)
    //{
    //    m_standardDirectionPassage = setDirection;
    //}
}
