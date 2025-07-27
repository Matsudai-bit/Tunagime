using System;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using static AmidaTube;


[Serializable]
public class AmidaTubeData
{

}

public class AmidaTube : MonoBehaviour, ISerializableComponent
{
    /// <summary>
    /// 状態の種類
    /// </summary>

    [System.Serializable]

    public enum State
    {
        NONE,       // 何もなし（初期状態や通過不可など）
        NORMAL,     // 横線のみ
        KNOT_UP,    // 上部に分岐がある結び目
        KNOT_DOWN,  // 下部に分岐がある結び目
        BRIDGE      // 縦線のみ（橋）
    }

    /// <summary>
    /// 通過方向の種類
    /// </summary>
    public enum Direction
    {
        UP,         // 上
        DOWN,       // 下
        RIGHT,      // 右
        LEFT,       // 左
        CENTER      // 中央（あみだの中心）
    }

    ///// <summary>
    ///// 通過方向 (このチューブ自体がどの方向に通過可能か) 
    ///// </summary>
    //[System.Serializable]
    //public struct DirectionPassage
    //{
    //    public bool up;
    //    public bool down;
    //    public bool right;
    //    public bool left;

    //    public DirectionPassage(bool up, bool down, bool right, bool left)
    //    {
    //        this.up = up;
    //        this.down = down;
    //        this.right = right;
    //        this.left = left;
    //    }

    //    // ある方向への通過が可能かチェックするヘルパーメソッド
    //    public bool CanPass(Vector3Int directionVector)
    //    {
    //        if (directionVector == Vector3Int.up) return up;
    //        if (directionVector == Vector3Int.down) return down;
    //        if (directionVector == Vector3Int.right) return right;
    //        if (directionVector == Vector3Int.left) return left; // Vector3Int.leftは(-1,0,0)
    //        return false;
    //    }
    //}


    // === 隣接するAmidaTubeへの参照 (重要！) ===
    // これらの参照はインスペクターで設定するか、マップ生成時に自動で割り当てる
    [System.Serializable]
    public struct NeighborAmidaTube
    {
        public AmidaTube up;
        public AmidaTube down;
        public AmidaTube right;
        public AmidaTube left;
    }

    [Header("隣接するAmidaTube")]
    private NeighborAmidaTube m_neighborAmida;

    [Space]
    [Header("==== AmidaTube設定 ====")]
    [Header("メッシュ変更スクリプト")]
    [SerializeField] private YarnMeshChanger m_meshChanger; // メッシュチェンジャー
//    [Header("通過可能方向")]
    //public DirectionPassage m_directionPassage;             // このチューブが通過できる方向
    [Header("初期状態")]
    private State m_currentShapeType = State.NORMAL;        // 現在の状態
    private State m_requestChangeShape = State.NONE;        // 状態の変更要求

    [Header("基準マテリアル")]
    [SerializeField] private Material m_standardMaterial;    // 基準マテリアル


    private void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
                // 状態を変更
                ChangeState();
            }

            m_requestChangeShape = State.NONE; // 状態変更要求をリセット

        }
    }

    private void ChangeState()
    {

        // 現在の状態を更新
        m_currentShapeType = m_requestChangeShape;
        // メッシュの変更
        m_meshChanger.SetMesh(m_currentShapeType);
        // 通過方向の変更
        UpdateNeighborAmida();

        MapData.GetInstance.GetStageGridData().SetAmidaDataChanged(); // あみだの状態が変更されたことを通知


    }

    /// <summary>
    /// 指定されたYarnMaterialGetter.MaterialTypeに対応するマテリアルを取得
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Material GetMaterial(YarnMaterialGetter.MaterialType type)
    {
        // YarnMaterialGetterからマテリアルを取得
        return m_meshChanger.GetMaterial(type);
    }

    /// <summary>
    /// 現在の状態に基づいてメッシュのマテリアルを変更
    /// </summary>
    public void ChangeMaterial(AmidaTube.Direction followDir)
    {
        
        // 現在の状態に基づいて隣接するあみだのマテリアルを変更

        if (m_currentShapeType == State.NONE)
        {
            // 何もない状態ではマテリアルを変更しない
            return;
        }

        if (m_currentShapeType == State.NORMAL)
        {
            if (m_neighborAmida.left == null)
            {
                return;
            }
            Material material = m_neighborAmida.left.GetMaterial(YarnMaterialGetter.MaterialType.OUTPUT);
            m_meshChanger.ChangeMaterial(material, YarnMaterialGetter.MaterialType.OUTPUT);
        }
        else if (m_currentShapeType == State.KNOT_UP || m_currentShapeType == State.KNOT_DOWN)
        {
            if (m_neighborAmida.left == null)
            {
                return;
            }

            Material materialLeft= m_neighborAmida.left.GetMaterial(YarnMaterialGetter.MaterialType.OUTPUT);
            m_meshChanger.ChangeMaterial(materialLeft, YarnMaterialGetter.MaterialType.INPUT);

            if (m_neighborAmida.right == null)
            {
                return;
            }

            if (m_currentShapeType == State.KNOT_UP &&
                followDir == Direction.DOWN)
            {
                Material materialUp = m_neighborAmida.up.GetMaterial(YarnMaterialGetter.MaterialType.BRIDGE_DOWN);
                m_meshChanger.ChangeMaterial(materialUp, YarnMaterialGetter.MaterialType.OUTPUT);
            }
            else if (m_currentShapeType == State.KNOT_DOWN &&
                followDir == Direction.UP)
            {
                Material materialDown = m_neighborAmida.down.GetMaterial(YarnMaterialGetter.MaterialType.BRIDGE_UP);
                m_meshChanger.ChangeMaterial(materialDown, YarnMaterialGetter.MaterialType.OUTPUT);
            }

         
        }

        else if (m_currentShapeType == State.BRIDGE)
        {
            if (m_neighborAmida.up == null || m_neighborAmida.down == null)
            {
                return;
            }

            if (followDir == Direction.DOWN)
            {
                Material materialUp = m_neighborAmida.up.GetMaterial(YarnMaterialGetter.MaterialType.INPUT);
                m_meshChanger.ChangeMaterial(materialUp, YarnMaterialGetter.MaterialType.BRIDGE_DOWN);
            }


            if (followDir == Direction.UP)
            {
                Material materialDown = m_neighborAmida.down.GetMaterial(YarnMaterialGetter.MaterialType.INPUT);
                m_meshChanger.ChangeMaterial(materialDown, YarnMaterialGetter.MaterialType.BRIDGE_UP);
            }



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
    /// 状態から隣接するあみだの変更
    /// </summary>
    /// <param name="state"></param>
    
    public void UpdateNeighborAmida ()
    {
        var map = MapData.GetInstance;
        var gridData = MapData.GetInstance.GetStageGridData();
        
        GridPos gridPos = map.GetClosestGridPos(transform.position);

        switch (m_currentShapeType)
        {
            case State.NORMAL:
                // 通常状態では、隣接するあみだの左右のみを設定
                {
                    AmidaTube left  = gridData.GetAmidaTube(gridPos.x - 1, gridPos.y);
                    AmidaTube right = gridData.GetAmidaTube(gridPos.x + 1, gridPos.y);
                    SetNeighbor(null, null, left, right);
                }
                break;
            case State.KNOT_UP:
                {
                    // 上部に分岐がある結び目では、上と左右を設定
                    AmidaTube up = gridData.GetAmidaTube(gridPos.x, gridPos.y - 1);

                    // 隣接するあみだの左右を設定
                    AmidaTube left = gridData.GetAmidaTube(gridPos.x - 1, gridPos.y);
                    AmidaTube right = gridData.GetAmidaTube(gridPos.x + 1, gridPos.y);



                    SetNeighbor(up, null, left, right);

                    break;
                }
            case State.KNOT_DOWN:
                {
                    // 下部に分岐がある結び目では、下と左右を設定
                    AmidaTube down = gridData.GetAmidaTube(gridPos.x, gridPos.y + 1); ;

                    // 隣接するあみだの左右を設定
                    AmidaTube left = gridData.GetAmidaTube(gridPos.x - 1, gridPos.y);
                    AmidaTube right = gridData.GetAmidaTube(gridPos.x + 1, gridPos.y);

                    SetNeighbor(null, down, left, right);
                    break;
                }
            case State.BRIDGE:
                {
                    // 縦線のみの状態では、上下と左右を設定
                    AmidaTube up = gridData.GetAmidaTube(gridPos.x, gridPos.y - 1);
                    AmidaTube down = gridData.GetAmidaTube(gridPos.x, gridPos.y + 1); ;

                    SetNeighbor(up, down, null, null);
                }
                break;
            default:
                Debug.LogWarning("Unknown state for UpdateNeighborAmida: " + m_currentShapeType);
                break;
        }
    }


    ///// <summary>
    ///// 通過方向を取得する
    ///// </summary>
    ///// <returns>通過方向</returns>
    //public DirectionPassage GetDirectionPassage()
    //{
    //    return m_directionPassage;
    //}

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
    /// リセットする
    /// </summary>
    public void ResetState()
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



    /// <summary>
    /// 隣接するあみだの取得
    /// </summary>
    public AmidaTube GetNeighbor(Direction direction)
    {
        switch (direction)
        {
            case Direction.UP:
                return m_neighborAmida.up;
            case Direction.DOWN:
                return m_neighborAmida.down;
            case Direction.RIGHT:
                return m_neighborAmida.right;
            case Direction.LEFT:
                return m_neighborAmida.left;
            default:
                Debug.LogWarning("Invalid direction specified for GetNeighbor");
                return null;
        }
    }

    /// <summary>
    /// 隣接するあみだの取得
    /// </summary>
    public NeighborAmidaTube GetNeighbor()
    {
        return m_neighborAmida;
    }

    /// <summary>
    /// 隣接するあみだの設定
    /// </summary>
    /// <param name="up"></param>
    /// <param name="down"></param>
    /// <param name="right"></param>
    /// <param name="left"></param>
    public void SetNeighbor(AmidaTube up, AmidaTube down, AmidaTube left, AmidaTube right)
    {
        m_neighborAmida = new NeighborAmidaTube
        {
            up = up,
            down = down,
            right = right,
            left = left
        };
    }

    /// <summary>
    /// 現在の状態を取得
    /// </summary>
    /// <returns></returns>
    public State GetState()
    {
        return m_currentShapeType;
    }


    public Direction GetFollowDirection()
    {
        // 現在の状態に基づいて通過方向を決定
        switch (m_currentShapeType)
        {
            case State.NORMAL:
                return Direction.RIGHT; // 通常状態では右方向
            case State.KNOT_UP:
                return Direction.UP; // 上部に分岐がある結び目では上方向
            case State.KNOT_DOWN:
                return Direction.DOWN; // 下部に分岐がある結び目では下方向
            case State.BRIDGE:
                return Direction.CENTER; // 縦線のみの状態では中央
            default:
                return Direction.CENTER; // その他の場合は中央
        }
    }

}