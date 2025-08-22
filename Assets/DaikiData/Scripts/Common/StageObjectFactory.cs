using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// �X�e�[�W�I�u�W�F�N�g�𐶐����邽�߂̃t�@�N�g���[�N���X�@�V���O���g��
/// </summary>
public class StageObjectFactory : MonoBehaviour
{
    private static StageObjectFactory s_instance;   // �V���O���g���C���X�^���X

    [Header("====== �X�e�[�W�I�u�W�F�N�g�̃v���n�u�ݒ� ======")]
    [SerializeField] private GameObject m_feelingSlotPrefab;            // �z���̌^�̃v���n�u
    [SerializeField] private GameObject m_terminusFeelingSlotPrefab;    // �I�_�z���̌^�̃v���n�u
    [SerializeField] private GameObject m_fluffBallPrefab;              // �ю��ʂ̃v���n�u
    [SerializeField] private GameObject m_feltBlockPrefab;              // �t�F���g�u���b�N�̃v���n�u
    [SerializeField] private GameObject m_noMovementFeltBlockPrefab;    // �s���t�F���g�u���b�N�̃v���n�u
    [SerializeField] private GameObject m_curtainPrefab;                // �J�[�e���v���t�@�u
    [SerializeField] private GameObject m_satinFloorPrefab;             // �T�e�����̃v���n�u
    [SerializeField] private GameObject m_pairBadgePrefab;             // �y�A�o�b�W�̃v���n�u
    [SerializeField] private GameObject m_feltBlock_PairBadgePrefab;   // �y�A�o�b�W�̃t�F���g�u���b�N�̃v���n�u
    [SerializeField] private GameObject m_fragmentPrefab;              // �z���̒f�Ђ̃v���n�u
    [SerializeField] private GameObject m_carriableCorePrefab;         // �����^�щ\�ȃR�A�̃v���n�u



    // �I�u�W�F�N�g�v�[��
    List<GameObject> m_feelingSlotPool = new List<GameObject>(); // �z���̌^�̃I�u�W�F�N�g�v�[��
    List<GameObject> m_terminusFeelingSlotPool = new List<GameObject>(); // �I�_�z���̌^�̃I�u�W�F�N�g�v�[��
    List<GameObject> m_fluffballPool            = new List<GameObject>(); // �ю��ʂ̃I�u�W�F�N�g�v�[��
    List<GameObject> m_feltBlockPool            = new List<GameObject>(); // �t�F���g�u���b�N�̃I�u�W�F�N�g�v�[��
    List<GameObject> m_noMovementFeltBlockPool  = new List<GameObject>(); // �s���t�F���g�u���b�N�̃I�u�W�F�N�g�v�[��
    List<GameObject> m_curtainPool              = new List<GameObject>(); // �J�[�e���̃I�u�W�F�N�g�v�[��
    List<GameObject> m_satinFloorPool           = new List<GameObject>(); // �T�e�����̃I�u�W�F�N�g�v�[��
    List<GameObject> m_pairBadgePool            = new List<GameObject>(); // �y�A�o�b�W�̃I�u�W�F�N�g�v�[��
    List<GameObject> m_feltBlock_PairBadgePool  = new List<GameObject>(); // �y�A�o�b�W�̃t�F���g�u���b�N�̃I�u�W�F�N�g�v�[��
    List<GameObject> m_fragmentPool             = new List<GameObject>(); // �z���̒f�Ђ̃I�u�W�F�N�g�v�[��
    List<GameObject> m_carriableCorePool        = new List<GameObject>(); // �����^�щ\�ȃR�A�̃I�u�W�F�N�g�v�[��

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
    /// �z���̌^�𐶐����郁�\�b�h
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="gridPos"></param>
    /// <param name="emotionType"></param>
    /// <returns></returns>
    public GameObject GenerateFeelingSlot(Transform parent, GridPos gridPos, EmotionCurrent.Type emotionType)
    {

        // ��������I�u�W�F�N�g�̎擾
        GameObject generationObject = GetFeelingSlotFromPool();
        // �e�̐ݒ�
        if (parent != null)
            generationObject.transform.SetParent(parent, true);

        // ��ނ̐ݒ�
        var feelingCore = generationObject?.GetComponent<FeelingSlot>().GetFeelingCore();
        if (feelingCore)
        {
            feelingCore.SetEmotionType(emotionType);
        }

        // �X�e�[�W�u���b�N�̐ݒ�
        StageBlock stageBlock = generationObject.GetComponent<StageBlock>();
        stageBlock.SetBlockType(StageBlock.BlockType.FEELING_SLOT);
        // ������
        stageBlock.Initialize(gridPos);

        return generationObject;
    }

    /// <summary>
    /// �I�_�z���̌^�𐶐����郁�\�b�h
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="gridPos"></param>
    /// <param name="emotionType"></param>
    /// <returns></returns>
    public GameObject GenerateTerminusFeelingSlot(Transform parent, GridPos gridPos, EmotionCurrent.Type emotionType)
    {
        // ��������I�u�W�F�N�g�̎擾
        GameObject generationObject = GetTerminusFeelingSlotFromPool();
        // �e�̐ݒ�
        if (parent != null)
            generationObject.transform.SetParent(parent, true);
        // ��ނ̐ݒ�
        var feelingCore = generationObject?.GetComponent<FeelingSlot>().GetFeelingCore();
        if (feelingCore)
        {
            feelingCore.SetEmotionType(emotionType);
        }
        // �X�e�[�W�u���b�N�̐ݒ�
        StageBlock stageBlock = generationObject.GetComponent<StageBlock>();
        stageBlock.SetBlockType(StageBlock.BlockType.FEELING_SLOT);
        // ������
        stageBlock.Initialize(gridPos);
        return generationObject;
    }

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
            generationObject.transform.SetParent(parent, true);

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
            generationObject.transform.SetParent(parent, true);
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
            generationObject.transform.SetParent(parent, true);
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
            generationObject.transform.SetParent(parent, true);

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

    public GameObject GenerateSatinFloor(Transform parent, GridPos gridPos)
    {
        // ��������I�u�W�F�N�g�̎擾
        GameObject generationObject = GetSatinFloorFromPool();
        // �e�̐ݒ�
        if (parent != null)
            generationObject.transform.SetParent(parent, true);
        // �X�e�[�W�u���b�N�̐ݒ�
        StageBlock stageBlock = generationObject.GetComponent<StageBlock>();
        stageBlock.SetBlockType(StageBlock.BlockType.SATIN_FLOOR);
        // ������
        stageBlock.Initialize(gridPos);
        return generationObject;
    }

    public GameObject GeneratePairBadge(Transform parent, List<GridPos> generationBlockPositionList, EmotionCurrent.Type emotionType)
    {
        if (generationBlockPositionList == null || generationBlockPositionList.Count <= 0)
        {
            Debug.LogError("PairBadge generation requires at least two block positions.");
            return null;
        }

        // ��������I�u�W�F�N�g�̎擾
        GameObject generationObject = GetPairBadgeFromPool();
        var pairBadge = generationObject?.GetComponent<PairBadge>();
        if (pairBadge == null) return null;

        // �u���b�N�̐����Ɠo�^
        List<FeltBlock> feltBlocks = new List<FeltBlock>();
        for (int i = 0; i < generationBlockPositionList.Count; i++)
        {
            FeltBlock feltBlock = GetFeltBlock_PairBadgeFromPool().GetComponent<FeltBlock>();
            feltBlock.stageBlock.Initialize(generationBlockPositionList[i]);

            // �ǉ�
            feltBlocks.Add(feltBlock);
        }

        // �y�A�o�b�W�̏�����
        pairBadge.Initialize(feltBlocks);


        // �e�̐ݒ�
        if (parent != null)
            generationObject.transform.SetParent(parent, true);
        // �}�e���A���̐ݒ�
        var meshRenderer = generationObject?.GetComponent<MeshRenderer>();

        return generationObject;
    }

    public GameObject GenerateFragment(Transform parent, GridPos gridPos, EmotionCurrent.Type emotionType)
    {
        // ��������I�u�W�F�N�g�̎擾
        GameObject generationObject = GetFragmentFromPool();
        // �e�̐ݒ�
        if (parent != null)
            generationObject.transform.SetParent(parent, true);
        // �}�e���A���̐ݒ�
        var meshRenderer = generationObject?.GetComponent<Fragment>().MeshRenderer;
        if (meshRenderer != null)
        {
            meshRenderer.material = MaterialLibrary.GetInstance.GetMaterial(MaterialLibrary.MaterialGroup.FRAGMENT, emotionType);
        }

        // ��ނ̐ݒ�
        var emotionCurrent = generationObject?.GetComponent<EmotionCurrent>();
        if (emotionCurrent)
        {
            emotionCurrent.CurrentType = emotionType;
        }

        // �X�e�[�W�u���b�N�̐ݒ�
        StageBlock stageBlock = generationObject.GetComponent<StageBlock>();
        stageBlock.SetBlockType(StageBlock.BlockType.FRAGMENT);
        // ������
        stageBlock.Initialize(gridPos);
        return generationObject;
    }

    public GameObject GenerateCarriableCore(Transform parent, GridPos gridPos, EmotionCurrent.Type emotionType)
    {
        // ��������I�u�W�F�N�g�̎擾
        GameObject generationObject = GetCarriableCoreFromPool();
        // �e�̐ݒ�
        if (parent != null)
            generationObject.transform.SetParent(parent, true);
        // �}�e���A���̐ݒ�
        var meshRenderer = generationObject?.GetComponent<FeelingCore>().MeshRenderer;
        if (meshRenderer != null)
        {
            meshRenderer.material = MaterialLibrary.GetInstance.GetMaterial(MaterialLibrary.MaterialGroup.CORE, emotionType);
        }

        // ��ނ̐ݒ�
        var emotionCurrent = generationObject?.GetComponent<EmotionCurrent>();
        if (emotionCurrent)
        {
            emotionCurrent.CurrentType = emotionType;
        }

        // �X�e�[�W�u���b�N�̐ݒ�
        StageBlock stageBlock = generationObject.GetComponent<StageBlock>();
        stageBlock.SetBlockType(StageBlock.BlockType.CARRIABLE_CORE);
        // ������
        stageBlock.Initialize(gridPos);
        return generationObject;
    }


    // ========================================================================================
    // ===== �I�u�W�F�N�g�v�[������̎擾���\�b�h ==============================================

    /// <summary>
    /// �z���̌^���I�u�W�F�N�g�v�[������擾���郁�\�b�h
    /// </summary>
    /// <returns></returns>
    private GameObject GetFeelingSlotFromPool()
    {
        // �I�u�W�F�N�g�v�[�����犈�����Ă��Ȃ��z���̌^�̎擾
        for (int i = 0; i < m_feelingSlotPool.Count; i++)
        {
            if (m_feelingSlotPool[i] != null && m_feelingSlotPool[i].activeSelf == false)
            {
                return m_feelingSlotPool[i];
            }
        }
        // ���ׂĂ̑z���̌^���������̏ꍇ�A�V�����z���̌^�𐶐����ăv�[���ɒǉ�
        GameObject newFeelingSlot = Instantiate(m_feelingSlotPrefab);
        m_feelingSlotPool.Add(newFeelingSlot);
        return newFeelingSlot;
    }

    /// <summary>
    /// �I�_�z���̌^���I�u�W�F�N�g�v�[������擾���郁�\�b�h
    /// </summary>
    /// <returns></returns>
    private GameObject GetTerminusFeelingSlotFromPool()
    {
        // �I�u�W�F�N�g�v�[�����犈�����Ă��Ȃ��I�_�z���̌^�̎擾
        for (int i = 0; i < m_terminusFeelingSlotPool.Count; i++)
        {
            if (m_terminusFeelingSlotPool[i] != null && m_terminusFeelingSlotPool[i].activeSelf == false)
            {
                return m_terminusFeelingSlotPool[i];
            }
        }
        // ���ׂĂ̏I�_�z���̌^���������̏ꍇ�A�V�����I�_�z���̌^�𐶐����ăv�[���ɒǉ�
        GameObject newTerminusFeelingSlot = Instantiate(m_terminusFeelingSlotPrefab);
        m_terminusFeelingSlotPool.Add(newTerminusFeelingSlot);
        return newTerminusFeelingSlot;
    }


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
        for (int i = 0; i < m_feltBlockPool.Count; i++)
        {
            if (m_feltBlockPool[i] != null && m_feltBlockPool[i].activeSelf == false)
            {
                return m_feltBlockPool[i];
            }
        }
        // ���ׂẴt�F���g�u���b�N���������̏ꍇ�A�V�����t�F���g�u���b�N�𐶐����ăv�[���ɒǉ�
        GameObject newFeltBlock = Instantiate(m_feltBlockPrefab);
        m_feltBlockPool.Add(newFeltBlock);
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

    /// <summary>
    /// �T�e�������v�[������擾���郁�\�b�h
    /// </summary>
    /// <returns></returns>
    private GameObject GetSatinFloorFromPool()
    {
        // �I�u�W�F�N�g�v�[�����犈�����Ă��Ȃ��T�e�����̎擾
        for (int i = 0; i < m_satinFloorPool.Count; i++)
        {
            if (m_satinFloorPool[i] != null && m_satinFloorPool[i].activeSelf == false)
            {
                return m_satinFloorPool[i];
            }
        }
        // ���ׂẴT�e�������������̏ꍇ�A�V�����T�e�����𐶐����ăv�[���ɒǉ�
        GameObject newSatinFloor = Instantiate(m_satinFloorPrefab);
        m_satinFloorPool.Add(newSatinFloor);
        return newSatinFloor;
    }


    /// <summary>
    /// �y�A�o�b�W���v�[������擾���郁�\�b�h
    /// </summary>
    /// <returns></returns>
    private GameObject GetPairBadgeFromPool()
    {
        // �I�u�W�F�N�g�v�[�����犈�����Ă��Ȃ��y�A�o�b�W�̎擾
        for (int i = 0; i < m_pairBadgePool.Count; i++)
        {
            if (m_pairBadgePool[i] != null && m_pairBadgePool[i].activeSelf == false)
            {
                return m_pairBadgePool[i];
            }
        }
        // ���ׂẴy�A�o�b�W���������̏ꍇ�A�V�����y�A�o�b�W�𐶐����ăv�[���ɒǉ�
        GameObject newPairBadge = Instantiate(m_pairBadgePrefab);

        m_pairBadgePool.Add(newPairBadge);
        return newPairBadge;
    }

    /// <summary>
    /// �y�A�o�b�W�̃t�F���g�u���b�N���v�[������擾���郁�\�b�h
    /// </summary>
    /// <returns></returns>
    private GameObject GetFeltBlock_PairBadgeFromPool()
    {
        // �I�u�W�F�N�g�v�[�����犈�����Ă��Ȃ��y�A�o�b�W�̃t�F���g�u���b�N�̎擾
        for (int i = 0; i < m_feltBlock_PairBadgePool.Count; i++)
        {
            if (m_feltBlock_PairBadgePool[i] != null && m_feltBlock_PairBadgePool[i].activeSelf == false)
            {
                return m_feltBlock_PairBadgePool[i];
            }
        }
        // ���ׂẴy�A�o�b�W�̃t�F���g�u���b�N���������̏ꍇ�A�V�����y�A�o�b�W�̃t�F���g�u���b�N�𐶐����ăv�[���ɒǉ�
        GameObject newFeltBlockPairBadge = Instantiate(m_feltBlock_PairBadgePrefab);
        m_feltBlock_PairBadgePool.Add(newFeltBlockPairBadge);
        return newFeltBlockPairBadge;
    }

    private GameObject GetFragmentFromPool()
    {
        // �I�u�W�F�N�g�v�[�����犈�����Ă��Ȃ��z���̒f�Ђ̎擾
        for (int i = 0; i < m_fragmentPool.Count; i++)
        {
            if (m_fragmentPool[i] != null && m_fragmentPool[i].activeSelf == false)
            {
                return m_fragmentPool[i];
            }
        }
        // ���ׂĂ̑z���̒f�Ђ��������̏ꍇ�A�V�����z���̒f�Ђ𐶐����ăv�[���ɒǉ�
        GameObject newFragment = Instantiate(m_fragmentPrefab);
        m_fragmentPool.Add(newFragment);
        return newFragment;
    }

    /// <summary>
    /// �����^�щ\�ȃR�A���v�[������擾���郁�\�b�h
    /// </summary>
    /// <returns></returns>
    private GameObject GetCarriableCoreFromPool()
    {
        // �I�u�W�F�N�g�v�[�����犈�����Ă��Ȃ������^�щ\�ȃR�A�̎擾
        for (int i = 0; i < m_carriableCorePool.Count; i++)
        {
            if (m_carriableCorePool[i] != null && m_carriableCorePool[i].activeSelf == false)
            {
                return m_carriableCorePool[i];
            }
        }
        // ���ׂĂ̎����^�щ\�ȃR�A���������̏ꍇ�A�V���������^�щ\�ȃR�A�𐶐����ăv�[���ɒǉ�
        GameObject newCarriableCore = Instantiate(m_carriableCorePrefab);
        m_carriableCorePool.Add(newCarriableCore);
        return newCarriableCore;
    }


}
