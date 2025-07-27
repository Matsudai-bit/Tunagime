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
    /// ��Ԃ̎��
    /// </summary>

    [System.Serializable]

    public enum State
    {
        NONE,       // �����Ȃ��i������Ԃ�ʉߕs�Ȃǁj
        NORMAL,     // �����̂�
        KNOT_UP,    // �㕔�ɕ��򂪂��錋�і�
        KNOT_DOWN,  // �����ɕ��򂪂��錋�і�
        BRIDGE      // �c���̂݁i���j
    }

    /// <summary>
    /// �ʉߕ����̎��
    /// </summary>
    public enum Direction
    {
        UP,         // ��
        DOWN,       // ��
        RIGHT,      // �E
        LEFT,       // ��
        CENTER      // �����i���݂��̒��S�j
    }

    ///// <summary>
    ///// �ʉߕ��� (���̃`���[�u���̂��ǂ̕����ɒʉ߉\��) 
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

    //    // ��������ւ̒ʉ߂��\���`�F�b�N����w���p�[���\�b�h
    //    public bool CanPass(Vector3Int directionVector)
    //    {
    //        if (directionVector == Vector3Int.up) return up;
    //        if (directionVector == Vector3Int.down) return down;
    //        if (directionVector == Vector3Int.right) return right;
    //        if (directionVector == Vector3Int.left) return left; // Vector3Int.left��(-1,0,0)
    //        return false;
    //    }
    //}


    // === �אڂ���AmidaTube�ւ̎Q�� (�d�v�I) ===
    // �����̎Q�Ƃ̓C���X�y�N�^�[�Őݒ肷�邩�A�}�b�v�������Ɏ����Ŋ��蓖�Ă�
    [System.Serializable]
    public struct NeighborAmidaTube
    {
        public AmidaTube up;
        public AmidaTube down;
        public AmidaTube right;
        public AmidaTube left;
    }

    [Header("�אڂ���AmidaTube")]
    private NeighborAmidaTube m_neighborAmida;

    [Space]
    [Header("==== AmidaTube�ݒ� ====")]
    [Header("���b�V���ύX�X�N���v�g")]
    [SerializeField] private YarnMeshChanger m_meshChanger; // ���b�V���`�F���W���[
//    [Header("�ʉ߉\����")]
    //public DirectionPassage m_directionPassage;             // ���̃`���[�u���ʉ߂ł������
    [Header("�������")]
    private State m_currentShapeType = State.NORMAL;        // ���݂̏��
    private State m_requestChangeShape = State.NONE;        // ��Ԃ̕ύX�v��

    [Header("��}�e���A��")]
    [SerializeField] private Material m_standardMaterial;    // ��}�e���A��


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
        // ��ԕύX�̗v�������������ǂ���
        if (m_requestChangeShape != State.NONE)
        {
            // �Ⴄ��Ԃ̏ꍇ
            if (m_requestChangeShape != m_currentShapeType)
            {
                // ��Ԃ�ύX
                ChangeState();
            }

            m_requestChangeShape = State.NONE; // ��ԕύX�v�������Z�b�g

        }
    }

    private void ChangeState()
    {

        // ���݂̏�Ԃ��X�V
        m_currentShapeType = m_requestChangeShape;
        // ���b�V���̕ύX
        m_meshChanger.SetMesh(m_currentShapeType);
        // �ʉߕ����̕ύX
        UpdateNeighborAmida();

        MapData.GetInstance.GetStageGridData().SetAmidaDataChanged(); // ���݂��̏�Ԃ��ύX���ꂽ���Ƃ�ʒm


    }

    /// <summary>
    /// �w�肳�ꂽYarnMaterialGetter.MaterialType�ɑΉ�����}�e���A�����擾
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Material GetMaterial(YarnMaterialGetter.MaterialType type)
    {
        // YarnMaterialGetter����}�e���A�����擾
        return m_meshChanger.GetMaterial(type);
    }

    /// <summary>
    /// ���݂̏�ԂɊ�Â��ă��b�V���̃}�e���A����ύX
    /// </summary>
    public void ChangeMaterial(AmidaTube.Direction followDir)
    {
        
        // ���݂̏�ԂɊ�Â��ėאڂ��邠�݂��̃}�e���A����ύX

        if (m_currentShapeType == State.NONE)
        {
            // �����Ȃ���Ԃł̓}�e���A����ύX���Ȃ�
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
    /// ��ԕύX�v��
    /// </summary>
    /// <param name="state"></param>
    public void RequestChangedState(State state)
    {
        m_requestChangeShape = state;
    }


    /// <summary>
    /// ��Ԃ���אڂ��邠�݂��̕ύX
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
                // �ʏ��Ԃł́A�אڂ��邠�݂��̍��E�݂̂�ݒ�
                {
                    AmidaTube left  = gridData.GetAmidaTube(gridPos.x - 1, gridPos.y);
                    AmidaTube right = gridData.GetAmidaTube(gridPos.x + 1, gridPos.y);
                    SetNeighbor(null, null, left, right);
                }
                break;
            case State.KNOT_UP:
                {
                    // �㕔�ɕ��򂪂��錋�іڂł́A��ƍ��E��ݒ�
                    AmidaTube up = gridData.GetAmidaTube(gridPos.x, gridPos.y - 1);

                    // �אڂ��邠�݂��̍��E��ݒ�
                    AmidaTube left = gridData.GetAmidaTube(gridPos.x - 1, gridPos.y);
                    AmidaTube right = gridData.GetAmidaTube(gridPos.x + 1, gridPos.y);



                    SetNeighbor(up, null, left, right);

                    break;
                }
            case State.KNOT_DOWN:
                {
                    // �����ɕ��򂪂��錋�іڂł́A���ƍ��E��ݒ�
                    AmidaTube down = gridData.GetAmidaTube(gridPos.x, gridPos.y + 1); ;

                    // �אڂ��邠�݂��̍��E��ݒ�
                    AmidaTube left = gridData.GetAmidaTube(gridPos.x - 1, gridPos.y);
                    AmidaTube right = gridData.GetAmidaTube(gridPos.x + 1, gridPos.y);

                    SetNeighbor(null, down, left, right);
                    break;
                }
            case State.BRIDGE:
                {
                    // �c���݂̂̏�Ԃł́A�㉺�ƍ��E��ݒ�
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
    ///// �ʉߕ������擾����
    ///// </summary>
    ///// <returns>�ʉߕ���</returns>
    //public DirectionPassage GetDirectionPassage()
    //{
    //    return m_directionPassage;
    //}

    /// <summary>
    /// ���݂��u���b�N�̐ݒ�
    /// </summary>
    /// <param name="dir">�ݒ肷�����</param>
    /// <param name="setAmidaBlock">�ݒ肷��u���b�N</param>
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
    ///// ���݂��u���b�N�̎擾
    ///// </summary>
    ///// <param name="dir">�ݒ肷�����</param>
    ///// <param name="setAmidaBlock">�ݒ肷��u���b�N</param>
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
    ///// �u���b�N�̐F�̕ύX
    ///// </summary>
    ///// <param name="color">�ύX�F</param>
    ///// <param name="directionA">����A</param>
    ///// <param name="directionB">����B</param>
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
    /// �d�C�𗬂�
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
    /// �S�Ẵu���b�N�̐F�̕ύX
    /// </summary>
    /// <param name="color">�ύX�F</param>
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
    ///// ��]����
    ///// </summary>
    //public void RotateClock()
    //{
    //    // ���݂̒ʉߕ������擾
    //    DirectionPassage currentPassage = GetDirectionPassage();

    //    // 90�x��]�������V�����ʉߕ�����ݒ�
    //    DirectionPassage newPassage = new DirectionPassage
    //    {
    //        up = currentPassage.left,
    //        down = currentPassage.right,
    //        right = currentPassage.up,
    //        left = currentPassage.down
    //    };

    //    // �V�����ʉߕ�����ݒ�
    //    SetDirectionPassage(newPassage);

    //}

    /// <summary>
    /// ���Z�b�g����
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
    /// ���Z�b�g����
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
    /// �אڂ��邠�݂��̎擾
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
    /// �אڂ��邠�݂��̎擾
    /// </summary>
    public NeighborAmidaTube GetNeighbor()
    {
        return m_neighborAmida;
    }

    /// <summary>
    /// �אڂ��邠�݂��̐ݒ�
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
    /// ���݂̏�Ԃ��擾
    /// </summary>
    /// <returns></returns>
    public State GetState()
    {
        return m_currentShapeType;
    }


    public Direction GetFollowDirection()
    {
        // ���݂̏�ԂɊ�Â��Ēʉߕ���������
        switch (m_currentShapeType)
        {
            case State.NORMAL:
                return Direction.RIGHT; // �ʏ��Ԃł͉E����
            case State.KNOT_UP:
                return Direction.UP; // �㕔�ɕ��򂪂��錋�іڂł͏����
            case State.KNOT_DOWN:
                return Direction.DOWN; // �����ɕ��򂪂��錋�іڂł͉�����
            case State.BRIDGE:
                return Direction.CENTER; // �c���݂̂̏�Ԃł͒���
            default:
                return Direction.CENTER; // ���̑��̏ꍇ�͒���
        }
    }

}