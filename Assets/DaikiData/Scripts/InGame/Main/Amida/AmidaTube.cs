using System;
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

    private StageBlock m_stageBlock; // ���̂��݂��`���[�u��������X�e�[�W�u���b�N


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
        m_stageBlock = GetComponentInParent<StageBlock>();

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }


    public void ResetEmotionCurrentType()
    {
        // EmotionCurrent.Type�����Z�b�g
        m_meshChanger.ResetEmotionType();
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


        }
    }

    public void SetEmotionCurrentType(YarnMaterialGetter.MaterialType materialType, EmotionCurrent.Type emotionTyp)
    {
        // EmotionCurrent.Type��ݒ�
        m_meshChanger.ChangeEmotionType(emotionTyp, materialType);
    }

    

    /// <summary>
    /// ��Ԃ�ύX���郁�\�b�h
    /// </summary>
    public void ChangeState()
    {

        // ���݂̏�Ԃ��X�V
        m_currentShapeType = m_requestChangeShape;
        // ���b�V���̕ύX
        m_meshChanger.SetMesh(m_currentShapeType);
        // �ʉߕ����̕ύX
        UpdateNeighborAmida();

        // ���݂��̏�Ԃ��ύX���ꂽ���Ƃ�ʒm
        GameInteractionEventMessenger.GetInstance.Notify(InteractionEvent.CHANGED_AMIDAKUJI);

        m_requestChangeShape = State.NONE; // ��ԕύX�v�������Z�b�g


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


    public EmotionCurrent.Type GetEmotionType(YarnMaterialGetter.MaterialType type)
    {
        // YarnMaterialGetter����G���[�V�����^�C�v���擾
        return m_meshChanger.GetEmotionType(type);
    }

    /// <summary>
    /// �}�e���A����K�p����
    /// </summary>
    public void ApplyMaterial()
    {
        // YarnMeshChanger���g�p���ă}�e���A����K�p
        m_meshChanger.ApplyMaterial();
    }

    public void ApplyRejectionMaterial()
    {
        m_meshChanger.SetAllEmotionType(EmotionCurrent.Type.REJECTION);
        // ���ׂẴ}�e���A���ɑ΂���EmotionCurrent.Type��REJECTION�ɐݒ�
        m_meshChanger.ApplyMaterial();
    }

    /// <summary>
    /// ���݂̏�ԂɊ�Â��ėאڂ��邠�݂��̃}�e���A�����X�V
    /// </summary>
    /// <param name="followDir"></param>
    public void UpdateMeshMaterialsBasedOnAmidaState(AmidaTube.Direction followDir)
    {
        
        // ���݂̏�ԂɊ�Â��ėאڂ��邠�݂��̃}�e���A����ύX
        if (m_currentShapeType == State.NONE)
        {
            // �����Ȃ���Ԃł̓}�e���A����ύX���Ȃ�
            return;
        }

        // �אڂ��邠�݂��̃}�e���A����ύX
        if (m_currentShapeType == State.NORMAL)
        {
            if (m_neighborAmida.left == null)
            {
                return;
            }
            EmotionCurrent.Type emotionTypeLeft     = m_neighborAmida.left.GetEmotionType(YarnMaterialGetter.MaterialType.OUTPUT);
            m_meshChanger.ChangeEmotionType( emotionTypeLeft, YarnMaterialGetter.MaterialType.INPUT);
            m_meshChanger.ChangeEmotionType( emotionTypeLeft, YarnMaterialGetter.MaterialType.OUTPUT);
        }
        else if (m_currentShapeType == State.KNOT_UP || m_currentShapeType == State.KNOT_DOWN)
        {
            if (m_neighborAmida.left == null)
            {
                return;
            }

            EmotionCurrent.Type emotionTypeLeft = m_neighborAmida.left.GetEmotionType(YarnMaterialGetter.MaterialType.OUTPUT);
            m_meshChanger.ChangeEmotionType(emotionTypeLeft, YarnMaterialGetter.MaterialType.INPUT);

            if (m_neighborAmida.right == null)
            {
                return;
            }

            if (m_currentShapeType == State.KNOT_UP &&
                followDir == Direction.DOWN)
            {
                EmotionCurrent.Type emotionTypeUp = m_neighborAmida.up.GetEmotionType(YarnMaterialGetter.MaterialType.BRIDGE_DOWN);
                m_meshChanger.ChangeEmotionType(emotionTypeUp, YarnMaterialGetter.MaterialType.OUTPUT);
            }
            else if (m_currentShapeType == State.KNOT_DOWN &&
                followDir == Direction.UP)
            {
                Material materialDown               = m_neighborAmida.down.GetMaterial(YarnMaterialGetter.MaterialType.BRIDGE_UP);
                EmotionCurrent.Type emotionTypeDown = m_neighborAmida.down.GetEmotionType(YarnMaterialGetter.MaterialType.BRIDGE_UP);
                m_meshChanger.ChangeEmotionType(emotionTypeDown, YarnMaterialGetter.MaterialType.OUTPUT);
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
                Material materialUp                 = m_neighborAmida.up.GetMaterial(YarnMaterialGetter.MaterialType.INPUT);
                EmotionCurrent.Type emotionTypeUp   = m_neighborAmida.up.GetEmotionType(YarnMaterialGetter.MaterialType.INPUT);
                m_meshChanger.ChangeEmotionType(emotionTypeUp, YarnMaterialGetter.MaterialType.BRIDGE_DOWN);
            }


            if (followDir == Direction.UP)
            {
                Material materialDown               = m_neighborAmida.down.GetMaterial(YarnMaterialGetter.MaterialType.INPUT);
                EmotionCurrent.Type emotionTypeDown = m_neighborAmida.down.GetEmotionType(YarnMaterialGetter.MaterialType.INPUT);
                m_meshChanger.ChangeEmotionType(emotionTypeDown, YarnMaterialGetter.MaterialType.BRIDGE_UP);
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

    public GridPos GetGridPos()
    {
        return m_stageBlock.GetGridPos();
    }

}