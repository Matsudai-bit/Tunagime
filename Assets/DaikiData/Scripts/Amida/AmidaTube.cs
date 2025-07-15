using System;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using static AmidaTube;


[Serializable]
public class AmidaTubeData
{
    public DirectionPassage directionPassage;     // �ʉ߂ł������

}

public class AmidaTube : MonoBehaviour, ISerializableComponent
{
    /// <summary>
    /// ��Ԃ̎��
    /// </summary>

    [System.Serializable]

    public enum State
    {
        NONE,
        NORMAL,     // �ʏ���
        KNOT_UP,    // �㕔�̌��і�
        KNOT_DOWN,  // �����̌��і�
        BRIDGE      // ��
    }

    /// <summary>
    /// �ʉߕ���
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


    [SerializeField] private bool m_startInstance = false;  // �����ɐ������邩�ǂ���
    [SerializeField] private YarnMeshChanger m_meshChanger; // ���b�V���`�F���W���[


    public GameObject m_amidaTubeBlockPrefab;       // ���݂��`���[�u�̍\���u���b�N

    public DirectionPassage m_directionPassage;     // �ʉ߂ł������

    private State m_currentShapeType = State.NORMAL; // ���݂̏��
    private State m_requestChangeShape = State.NONE; // ��Ԃ̕ύX�v��

    //private GameObject[] m_amidaBlocks ;

    [SerializeField] private Material m_standardMaterial;    // ��}�e���A��


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
        // ��ԕύX�̗v�������������ǂ���
        if (m_requestChangeShape != State.NONE)
        {
            // �Ⴄ��Ԃ̏ꍇ
            if (m_requestChangeShape != m_currentShapeType)
            {
                m_meshChanger.SetMesh(m_requestChangeShape);
            }

            m_currentShapeType = m_requestChangeShape;
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
    /// �ʉߕ�����ݒ肷��
    /// </summary>
    /// <param name="directionPassage">�ݒ肷��ʉߕ���</param>
    public void SetDirectionPassage(DirectionPassage directionPassage)
    {
        // �ʉߕ�����ݒ�
        m_directionPassage = directionPassage;

        // ��������
        CreateAmidaTubeBlock();
    }

    /// <summary>
    /// ���݂��u���b�N�𐶐�����
    /// </summary>
    public void CreateAmidaTubeBlock()
    {
        // ��U���Z�b�g����
        //   Reset();

        // ���݂��`���[�u�u���b�N�̃X�P�[�����擾
        float scale = m_amidaTubeBlockPrefab.transform.localScale.x;
        // ���݂̃I�u�W�F�N�g�̈ʒu����Ƀu���b�N�̏����ʒu���v�Z
        Vector3 pos = transform.position + new Vector3(0.0f, scale / 2.0f, 0.0f);

        // �����̃u���b�N��ݒ�
        //SetAmidaBlock(Direction.CENTER, Instantiate(m_amidaTubeBlockPrefab, pos, Quaternion.identity, transform));

        //// �e�����Ƀu���b�N��z�u
        //// ��������Ă�������ΐ���
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
    /// �ʉߕ������擾����
    /// </summary>
    /// <returns>�ʉߕ���</returns>
    public DirectionPassage GetDirectionPassage()
    {
        return m_directionPassage;
    }

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
    /// ��Ԃ����Z�b�g����
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