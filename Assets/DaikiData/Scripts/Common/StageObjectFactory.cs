using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// �X�e�[�W�I�u�W�F�N�g�𐶐����邽�߂̃t�@�N�g���[�N���X�@�V���O���g��
/// </summary>
public class StageObjectFactory : MonoBehaviour
{
    private static StageObjectFactory s_instance;   // �V���O���g���C���X�^���X

    [Header("====== �X�e�[�W�I�u�W�F�N�g�̃v���n�u�ݒ� ======")]
    [SerializeField] private GameObject m_fluffBallPrefab;              // �ю��ʂ̃v���n�u
    [SerializeField] private GameObject m_feltBlockPrefab;              // �t�F���g�u���b�N�̃v���n�u
    [SerializeField] private GameObject m_noMovementFeltBlockPrefab;    // �s���t�F���g�u���b�N�̃v���n�u
    [SerializeField] private GameObject m_curtainPrefab;                // �J�[�e���v���t�@�u


    // �I�u�W�F�N�g�v�[��
    List<GameObject> m_fluffballPool            = new List<GameObject>(); // �ю��ʂ̃I�u�W�F�N�g�v�[��
    List<GameObject> m_feltBlcokPool            = new List<GameObject>(); // �t�F���g�u���b�N�̃I�u�W�F�N�g�v�[��
    List<GameObject> m_noMovementFeltBlockPool  = new List<GameObject>(); // �s���t�F���g�u���b�N�̃I�u�W�F�N�g�v�[��
    List<GameObject> m_curtainPool              = new List<GameObject>(); // �J�[�e���̃I�u�W�F�N�g�v�[��

    private void Awake()
    {
        // �V���O���g���C���X�^���X�����݂��Ȃ��ꍇ�A���݂̃I�u�W�F�N�g���C���X�^���X�Ƃ��Đݒ�
        if (s_instance == null)
        {
            s_instance = this;
        }
        else if (s_instance != this)
        {
            Destroy(gameObject); // ���ɑ��݂���ꍇ�͐V�����C���X�^���X��j��
        }
    }

    /// <summary>
    /// �V���O���g���C���X�^���X���擾���郁�\�b�h
    /// </summary>
    /// <returns></returns>
    static public StageObjectFactory GetInstance()
    {
  
            if (s_instance == null)
            {
                GameObject obj = new GameObject("StageObjectFactory");
                s_instance = obj.AddComponent<StageObjectFactory>();
            }
        
        return s_instance;
    }

    // ========================================================================================
    // ===== �X�e�[�W�I�u�W�F�N�g�������\�b�h ==================================================


    /// <summary>
    /// �ю��ʂ𐶐����郁�\�b�h
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="gridPos"></param>
    /// <returns></returns>
    public GameObject GenerateFluffBall(Transform parent, GridPos gridPos)
    {
        // ��������I�u�W�F�N�g�̎擾
        GameObject generationObject = GetFluffBallFromPool(); 

        // �e�̐ݒ�
        if (parent != null)
            generationObject.transform.SetParent(parent, false);

        // �X�e�[�W�u���b�N�̐ݒ�
        StageBlock stageBlock = generationObject.GetComponent<StageBlock>();
        stageBlock.SetBlockType(StageBlock.BlockType.FLUFF_BALL);
        // ������
        stageBlock.Initialize(gridPos);

        return generationObject;
    }

    /// <summary>
    /// �t�F���g�u���b�N�𐶐����郁�\�b�h
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="gridPos"></param>
    /// <param name="material"></param>
    /// <returns></returns>
    public GameObject GenerateFeltBlock(Transform parent, GridPos gridPos, EmotionCurrent.Type emotionType)
    {
        // ��������I�u�W�F�N�g�̎擾
        GameObject generationObject = GetFeltBlockFromPool();
        // �e�̐ݒ�
        if (parent != null)
            generationObject.transform.SetParent(parent, false);
        // �}�e���A���̐ݒ�
        MeshRenderer meshRenderer = generationObject?.GetComponent<FeltBlock>().meshRenderer;
        if (meshRenderer != null )
        {
            meshRenderer.material = MaterialLibrary.GetInstance.GetMaterial(MaterialLibrary.MaterialGroup.FELT_BLOCK, emotionType);
        }
        
        // �X�e�[�W�u���b�N�̐ݒ�
        StageBlock stageBlock = generationObject.GetComponent<StageBlock>();
        stageBlock.SetBlockType(StageBlock.BlockType.FELT_BLOCK);
        // ������
        stageBlock.Initialize(gridPos);
        return generationObject;
    }

    /// <summary>
    /// �s���t�F���g�u���b�N�𐶐����郁�\�b�h
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="gridPos"></param>
    /// <returns></returns>
    public GameObject GenerateNoMovementFeltBlock(Transform parent, GridPos gridPos)
    {
        // ��������I�u�W�F�N�g�̎擾
        GameObject generationObject = GetNoMovementFeltBlockFromPool();
        // �e�̐ݒ�
        if (parent != null)
            generationObject.transform.SetParent(parent, false);
        // �}�e���A���̐ݒ�
        MeshRenderer meshRenderer = generationObject?.GetComponent<FeltBlock>().meshRenderer;
        if (meshRenderer != null )
        {
            meshRenderer.material = MaterialLibrary.GetInstance.GetMaterial(MaterialLibrary.MaterialGroup.FELT_BLOCK, EmotionCurrent.Type.REJECTION);
        }
        // �X�e�[�W�u���b�N�̐ݒ�
        StageBlock stageBlock = generationObject.GetComponent<StageBlock>();
        stageBlock.SetBlockType(StageBlock.BlockType.FELT_BLOCK);

        // ������
        stageBlock.Initialize(gridPos);
        return generationObject;
    }


    public GameObject GenerateCurtain(Transform parent, float localRotateY, GridPos gridPos, EmotionCurrent.Type emotionType)
    {
        // ��������I�u�W�F�N�g�̎擾
        GameObject generationObject = GetCurtainFromPool();
        // �e�̐ݒ�
        if (parent != null)
            generationObject.transform.SetParent(parent, false);

        // �}�e���A���̐ݒ�
        var curtain = generationObject?.GetComponent<Curtain>();
        if (curtain)
        {
            curtain.SetMaterial(MaterialLibrary.GetInstance.GetMaterial(MaterialLibrary.MaterialGroup.CURTAIN, emotionType));
        }
        // ��ނ̐ݒ�
        var emotionCurrent = generationObject?.GetComponent<EmotionCurrent>();
        if (emotionCurrent)
        {
            emotionCurrent.CurrentType = emotionType;
        }

        // ���[�J����]�̐ݒ�
        generationObject.transform.localRotation = Quaternion.Euler(0.0f, localRotateY, 0.0f);

        // �X�e�[�W�u���b�N�̐ݒ�
        StageBlock stageBlock = generationObject.GetComponent<StageBlock>();
        stageBlock.SetBlockType(StageBlock.BlockType.CURTAIN);
        // ������
        stageBlock.Initialize(gridPos);
        return generationObject;
    }

    // ========================================================================================
    // ===== �I�u�W�F�N�g�v�[������̎擾���\�b�h ==============================================


    /// <summary>
    /// �ю��ʂ��I�u�W�F�N�g�v�[������擾���郁�\�b�h
    /// </summary>
    /// <returns></returns>
    private GameObject GetFluffBallFromPool()
    {
        // �I�u�W�F�N�g�v�[�����犈�����Ă��Ȃ��ю��ʂ̎擾
        for (int i = 0; i < m_fluffballPool.Count; i++)
        {
            if (m_fluffballPool[i] != null && m_fluffballPool[i].activeSelf == false)
            {
                return m_fluffballPool[i];
            }
        }
        // ���ׂĂ̖ю��ʂ��������̏ꍇ�A�V�����ю��ʂ𐶐����ăv�[���ɒǉ�
        GameObject newFluffBall = Instantiate(m_fluffBallPrefab);
        m_fluffballPool.Add(newFluffBall);
        return newFluffBall;
    }

    /// <summary>
    /// �t�F���g�u���b�N���v�[������擾���郁�\�b�h
    /// </summary>
    /// <returns></returns>
    private GameObject GetFeltBlockFromPool()
    {
        // �I�u�W�F�N�g�v�[�����犈�����Ă��Ȃ��ю��ʂ̎擾
        for (int i = 0; i < m_feltBlcokPool.Count; i++)
        {
            if (m_feltBlcokPool[i] != null && m_feltBlcokPool[i].activeSelf == false)
            {
                return m_feltBlcokPool[i];
            }
        }
        // ���ׂẴt�F���g�u���b�N���������̏ꍇ�A�V�����t�F���g�u���b�N�𐶐����ăv�[���ɒǉ�
        GameObject newFeltBlock = Instantiate(m_feltBlockPrefab);
        m_feltBlcokPool.Add(newFeltBlock);
        return newFeltBlock;
    }

    /// <summary>
    /// �s���t�F���g�u���b�N���v�[������擾���郁�\�b�h
    /// </summary>
    /// <returns></returns>
    private GameObject GetNoMovementFeltBlockFromPool()
    {
        // �I�u�W�F�N�g�v�[�����犈�����Ă��Ȃ��s���t�F���g�u���b�N�̎擾
        for (int i = 0; i < m_noMovementFeltBlockPool.Count; i++)
        {
            if (m_noMovementFeltBlockPool[i] != null && m_noMovementFeltBlockPool[i].activeSelf == false)
            {
                return m_noMovementFeltBlockPool[i];
            }
        }
        // ���ׂĂ̕s���t�F���g�u���b�N���������̏ꍇ�A�V�����s���t�F���g�u���b�N�𐶐����ăv�[���ɒǉ�
        GameObject newNoMovementFeltBlock = Instantiate(m_noMovementFeltBlockPrefab);
        m_noMovementFeltBlockPool.Add(newNoMovementFeltBlock);
        return newNoMovementFeltBlock;
    }

    /// <summary>
    /// �J�[�e�����v�[������擾���郁�\�b�h
    /// </summary>
    /// <returns></returns>
    private GameObject GetCurtainFromPool()
    {
        // �I�u�W�F�N�g�v�[�����犈�����Ă��Ȃ��J�[�e���̎擾
        for (int i = 0; i < m_curtainPool.Count; i++)
        {
            if (m_curtainPool[i] != null && m_curtainPool[i].activeSelf == false)
            {
                return m_curtainPool[i];
            }
        }
        // ���ׂẴJ�[�e�����������̏ꍇ�A�V�����J�[�e���𐶐����ăv�[���ɒǉ�
        GameObject newCurtain = Instantiate(m_curtainPrefab);
        m_curtainPool.Add(newCurtain);
        return newCurtain;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
