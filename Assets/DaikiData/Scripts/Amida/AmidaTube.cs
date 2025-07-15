using System;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using static AmidaTube;


[Serializable]
public class AmidaTubeData
{
    public DirectionPassage directionPassage;     // 通過できる方向

}

public class AmidaTube : MonoBehaviour, ISerializableComponent
{
    /// <summary>
    /// 状態の種類
    /// </summary>

    [System.Serializable]

    public enum State
    {
        NONE,
        NORMAL,     // 通常状態
        KNOT_UP,    // 上部の結び目
        KNOT_DOWN,  // 下部の結び目
        BRIDGE      // 橋
    }

    /// <summary>
    /// 通過方向
    /// </summary>
    [System.Serializable]
    public struct DirectionPassage
    {
        public bool up;
        public bool down;
        public bool right;
        public bool left;

        public DirectionPassage(bool up, bool down, bool right, bool left)
        {
            this.up = up;
            this.down = down;
            this.right = right;
            this.left = left;
        }
    }


    [SerializeField] private bool m_startInstance = false;  // すぐに生成するかどうか
    [SerializeField] private YarnMeshChanger m_meshChanger; // メッシュチェンジャー


    public GameObject m_amidaTubeBlockPrefab;       // あみだチューブの構成ブロック

    public DirectionPassage m_directionPassage;     // 通過できる方向

    private State m_currentShapeType = State.NORMAL; // 現在の状態
    private State m_requestChangeShape = State.NONE; // 状態の変更要求

    //private GameObject[] m_amidaBlocks ;

    [SerializeField] private Material m_standardMaterial;    // 基準マテリアル


    private void Awake()
    {

        //m_amidaBlocks = new GameObject[5];



    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {



        if (m_startInstance)
            CreateAmidaTubeBlock();

    }

    // Update is called once per frame
    void Update()
    {
        // 状態変更の要求があったかどうか
        if (m_requestChangeShape != State.NONE)
        {
            // 違う状態の場合
            if (m_requestChangeShape != m_currentShapeType)
            {
                m_meshChanger.SetMesh(m_requestChangeShape);
            }

            m_currentShapeType = m_requestChangeShape;
        }
    }

    /// <summary>
    /// 状態変更要求
    /// </summary>
    /// <param name="state"></param>
    public void RequestChangedState(State state)
    {
        m_requestChangeShape = state;
    }

    /// <summary>
    /// 通過方向を設定する
    /// </summary>
    /// <param name="directionPassage">設定する通過方向</param>
    public void SetDirectionPassage(DirectionPassage directionPassage)
    {
        // 通過方向を設定
        m_directionPassage = directionPassage;

        // 生成する
        CreateAmidaTubeBlock();
    }

    /// <summary>
    /// あみだブロックを生成する
    /// </summary>
    public void CreateAmidaTubeBlock()
    {
        // 一旦リセットする
        //   Reset();

        // あみだチューブブロックのスケールを取得
        float scale = m_amidaTubeBlockPrefab.transform.localScale.x;
        // 現在のオブジェクトの位置を基準にブロックの初期位置を計算
        Vector3 pos = transform.position + new Vector3(0.0f, scale / 2.0f, 0.0f);

        // 中央のブロックを設定
        //SetAmidaBlock(Direction.CENTER, Instantiate(m_amidaTubeBlockPrefab, pos, Quaternion.identity, transform));

        //// 各方向にブロックを配置
        //// 生成されてい泣ければ生成
        //if (m_directionPassage.up)
        //{
        //    if (GetAmidaBlock(Direction.UP))
        //        GetAmidaBlock(Direction.UP).SetActive(true);
        //    else
        //        SetAmidaBlock(Direction.UP, Instantiate(m_amidaTubeBlockPrefab, pos + new Vector3(0.0f, 0.0f, scale), Quaternion.identity, transform));
        //}
        //if (m_directionPassage.down)
        //{
        //    if (GetAmidaBlock(Direction.DOWN))
        //        GetAmidaBlock(Direction.DOWN).SetActive(true);
        //    else
        //        SetAmidaBlock(Direction.DOWN, Instantiate(m_amidaTubeBlockPrefab, pos + new Vector3(0.0f, 0.0f, -scale), Quaternion.identity, transform));
        //}
        //if (m_directionPassage.left)
        //{
        //    if (GetAmidaBlock(Direction.LEFT))
        //        GetAmidaBlock(Direction.LEFT).SetActive(true);
        //    else
        //        SetAmidaBlock(Direction.LEFT, Instantiate(m_amidaTubeBlockPrefab, pos + new Vector3(-scale, 0.0f, 0.0f), Quaternion.identity, transform));
        //}
        //if (m_directionPassage.right)
        //{
        //    if (GetAmidaBlock(Direction.RIGHT))
        //        GetAmidaBlock(Direction.RIGHT).SetActive(true);
        //    else
        //        SetAmidaBlock(Direction.RIGHT, Instantiate(m_amidaTubeBlockPrefab, pos + new Vector3(scale, 0.0f, 0.0f), Quaternion.identity, transform));
        //}
    }

    /// <summary>
    /// 通過方向を取得する
    /// </summary>
    /// <returns>通過方向</returns>
    public DirectionPassage GetDirectionPassage()
    {
        return m_directionPassage;
    }

    /// <summary>
    /// あみだブロックの設定
    /// </summary>
    /// <param name="dir">設定する方向</param>
    /// <param name="setAmidaBlock">設定するブロック</param>
    private void SetAmidaBlock(Direction dir, GameObject setAmidaBlock)
    {
        //switch(dir)
        //{

        //    case Direction.UP:
        //        m_amidaBlocks[0] = setAmidaBlock;
        //        break;
        //    case Direction.DOWN:
        //        m_amidaBlocks[1] = setAmidaBlock;
        //        break;
        //    case Direction.LEFT:
        //        m_amidaBlocks[2] = setAmidaBlock;
        //        break;
        //    case Direction.RIGHT:
        //        m_amidaBlocks[3] = setAmidaBlock;
        //        break;
        //    case Direction.CENTER:
        //        m_amidaBlocks[4] = setAmidaBlock;
        //        break;
        //}
    }

    ///// <summary>
    ///// あみだブロックの取得
    ///// </summary>
    ///// <param name="dir">設定する方向</param>
    ///// <param name="setAmidaBlock">設定するブロック</param>
    //public GameObject GetAmidaBlock(Direction dir)
    //{
    //    //switch (dir)
    //    //{

    //    //    case Direction.UP:
    //    //        return m_amidaBlocks[0] ;
    //    //    case Direction.DOWN:
    //    //        return m_amidaBlocks[1] ;
    //    //    case Direction.LEFT:
    //    //        return m_amidaBlocks[2] ;
    //    //    case Direction.RIGHT:
    //    //        return m_amidaBlocks[3] ;
    //    //    case Direction.CENTER:
    //    //        return m_amidaBlocks[4] ;
    //    //}
    //    //return null;
    //}

    ///// <summary>
    ///// ブロックの色の変更
    ///// </summary>
    ///// <param name="color">変更色</param>
    ///// <param name="directionA">方向A</param>
    ///// <param name="directionB">方向B</param>
    //private void ChangeBlockColor(Color32 color, Direction direction)
    //{


    //    GameObject block = GetAmidaBlock(direction);

    //    GameObject blockCenter = GetAmidaBlock(Direction.CENTER);

    //    if (block)
    //        block.GetComponent<MeshRenderer>().material.color = color;

    //    if (blockCenter)
    //        blockCenter.GetComponent<MeshRenderer>().material.color = color;
    //}


    /// <summary>
    /// 電気を流す
    /// </summary>
    /// <param name="color"></param>
    /// <param name="direction"></param>
    /// <param name="electricFlowType"></param>
    public void ConductElectricity(/*Color32 color, Direction direction, Electric.ElectricFlowType electricFlowType,*/ Texture mainTex)
    {
        GetComponent<MeshRenderer>().material.mainTexture = mainTex;

        //GetAmidaBlock(Direction.CENTER).GetComponent<Electric>().SetElectricFlowType(electricFlowType);
        //GetAmidaBlock(direction).GetComponent<Electric>().SetElectricFlowType(electricFlowType);
        //ChangeBlockColor(color, direction);
    }

    /// <summary>
    /// 全てのブロックの色の変更
    /// </summary>
    /// <param name="color">変更色</param>
    public void ChangeAllBlockColor(Texture mainTex)
    {
        GetComponent<MeshRenderer>().material.mainTexture = mainTex;

        //foreach (var obj in m_amidaBlocks)
        //{
        //    if (obj)
        //        obj.GetComponent<MeshRenderer>().material.color = color;
        //}
    }

    ///// <summary>
    ///// 回転する
    ///// </summary>
    //public void RotateClock()
    //{
    //    // 現在の通過方向を取得
    //    DirectionPassage currentPassage = GetDirectionPassage();

    //    // 90度回転させた新しい通過方向を設定
    //    DirectionPassage newPassage = new DirectionPassage
    //    {
    //        up = currentPassage.left,
    //        down = currentPassage.right,
    //        right = currentPassage.up,
    //        left = currentPassage.down
    //    };

    //    // 新しい通過方向を設定
    //    SetDirectionPassage(newPassage);

    //}

    /// <summary>
    /// リセットする
    /// </summary>
    public void Reset()
    {
        //foreach (var obj in m_amidaBlocks)
        //{
        //    if (obj)
        //    {
        //        obj.SetActive(false);
        //    }
        //    ResetState();
        //}

    }

    /// <summary>
    /// 状態をリセットする
    /// </summary>
    public void ResetState()
    {
        //foreach (var obj in m_amidaBlocks)
        //{
        //    if (obj)
        //    {
        //        obj.GetComponent<MeshRenderer>().material = m_standardMaterial;
        //        obj.GetComponent<Electric>().SetElectricFlowType(Electric.ElectricFlowType.NO_FLOW);
        //    }
        //}

    }

    public object CaptureData()
    {
        return new AmidaTubeData
        {
            //directionPassage = this.m_directionPassage,
            //amidaBlocks = this.m_amidaBlocks
        };
    }

    public void ApplyData(object data)
    {
        if (data is AmidaTubeData d)
        {
            //m_directionPassage = d.directionPassage;
            //m_amidaBlocks = d.amidaBlocks;
        }
    }

    public void SetActive(bool activeSelf)
    {
        gameObject.SetActive(activeSelf);
    }


    public Transform GetTransform()
    {
        return gameObject.transform;
    }
}