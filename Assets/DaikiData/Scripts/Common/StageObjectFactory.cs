using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ステージオブジェクトを生成するためのファクトリークラス　シングルトン
/// </summary>
public class StageObjectFactory : MonoBehaviour
{
    private static StageObjectFactory s_instance;   // シングルトンインスタンス

    [Header("====== ステージオブジェクトのプレハブ設定 ======")]
    [SerializeField] private GameObject m_feelingSlotPrefab;            // 想いの型のプレハブ
    [SerializeField] private GameObject m_terminusFeelingSlotPrefab;    // 終点想いの型のプレハブ
    [SerializeField] private GameObject m_fluffBallPrefab;              // 毛糸玉のプレハブ
    [SerializeField] private GameObject m_feltBlockPrefab;              // フェルトブロックのプレハブ
    [SerializeField] private GameObject m_noMovementFeltBlockPrefab;    // 不動フェルトブロックのプレハブ
    [SerializeField] private GameObject m_curtainPrefab;                // カーテンプレファブ
    [SerializeField] private GameObject m_satinFloorPrefab;             // サテン床のプレハブ
    [SerializeField] private GameObject m_pairBadgePrefab;             // ペアバッジのプレハブ
    [SerializeField] private GameObject m_feltBlock_PairBadgePrefab;   // ペアバッジのフェルトブロックのプレハブ
    [SerializeField] private GameObject m_fragmentPrefab;              // 想いの断片のプレハブ
    [SerializeField] private GameObject m_carriableCorePrefab;         // 持ち運び可能なコアのプレハブ



    // オブジェクトプール
    List<GameObject> m_feelingSlotPool = new List<GameObject>(); // 想いの型のオブジェクトプール
    List<GameObject> m_terminusFeelingSlotPool = new List<GameObject>(); // 終点想いの型のオブジェクトプール
    List<GameObject> m_fluffballPool            = new List<GameObject>(); // 毛糸玉のオブジェクトプール
    List<GameObject> m_feltBlockPool            = new List<GameObject>(); // フェルトブロックのオブジェクトプール
    List<GameObject> m_noMovementFeltBlockPool  = new List<GameObject>(); // 不動フェルトブロックのオブジェクトプール
    List<GameObject> m_curtainPool              = new List<GameObject>(); // カーテンのオブジェクトプール
    List<GameObject> m_satinFloorPool           = new List<GameObject>(); // サテン床のオブジェクトプール
    List<GameObject> m_pairBadgePool            = new List<GameObject>(); // ペアバッジのオブジェクトプール
    List<GameObject> m_feltBlock_PairBadgePool  = new List<GameObject>(); // ペアバッジのフェルトブロックのオブジェクトプール
    List<GameObject> m_fragmentPool             = new List<GameObject>(); // 想いの断片のオブジェクトプール
    List<GameObject> m_carriableCorePool        = new List<GameObject>(); // 持ち運び可能なコアのオブジェクトプール

    private void Awake()
    {
        // シングルトンインスタンスが存在しない場合、現在のオブジェクトをインスタンスとして設定
        if (s_instance == null)
        {
            s_instance = this;
        }
        else if (s_instance != this)
        {
            Destroy(gameObject); // 既に存在する場合は新しいインスタンスを破棄
        }
    }

    /// <summary>
    /// シングルトンインスタンスを取得するメソッド
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
    // ===== ステージオブジェクト生成メソッド ==================================================

    /// <summary>
    /// 想いの型を生成するメソッド
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="gridPos"></param>
    /// <param name="emotionType"></param>
    /// <returns></returns>
    public GameObject GenerateFeelingSlot(Transform parent, GridPos gridPos, EmotionCurrent.Type emotionType)
    {

        // 生成するオブジェクトの取得
        GameObject generationObject = GetFeelingSlotFromPool();
        // 親の設定
        if (parent != null)
            generationObject.transform.SetParent(parent, true);

        // 種類の設定
        var feelingCore = generationObject?.GetComponent<FeelingSlot>().GetFeelingCore();
        if (feelingCore)
        {
            feelingCore.SetEmotionType(emotionType);


        }

        // ステージブロックの設定
        StageBlock stageBlock = generationObject.GetComponent<StageBlock>();
        stageBlock.SetBlockType(StageBlock.BlockType.FEELING_SLOT);
        // 初期化
        stageBlock.Initialize(gridPos);

        return generationObject;
    }

    /// <summary>
    /// 終点想いの型を生成するメソッド
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="gridPos"></param>
    /// <param name="emotionType"></param>
    /// <returns></returns>
    public GameObject GenerateTerminusFeelingSlot(Transform parent, GridPos gridPos, EmotionCurrent.Type emotionType)
    {
        // 生成するオブジェクトの取得
        GameObject generationObject = GetTerminusFeelingSlotFromPool();
        // 親の設定
        if (parent != null)
            generationObject.transform.SetParent(parent, true);
        // 種類の設定
        var feelingCore = generationObject?.GetComponent<FeelingSlot>().GetFeelingCore();
        if (feelingCore)
        {
            feelingCore.SetEmotionType(emotionType);


            if (feelingCore.GetEmotionType() == EmotionCurrent.Type.REJECTION)
            {
                generationObject.AddComponent<TerminusFeelingSlotRefection>();
            }
        }
        // ステージブロックの設定
        StageBlock stageBlock = generationObject.GetComponent<StageBlock>();
        stageBlock.SetBlockType(StageBlock.BlockType.FEELING_SLOT);
        // 初期化
        stageBlock.Initialize(gridPos);
        return generationObject;
    }

    /// <summary>
    /// 毛糸玉を生成するメソッド
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="gridPos"></param>
    /// <returns></returns>
    public GameObject GenerateFluffBall(Transform parent, GridPos gridPos)
    {
        // 生成するオブジェクトの取得
        GameObject generationObject = GetFluffBallFromPool(); 

        // 親の設定
        if (parent != null)
            generationObject.transform.SetParent(parent, true);

        // ステージブロックの設定
        StageBlock stageBlock = generationObject.GetComponent<StageBlock>();
        stageBlock.SetBlockType(StageBlock.BlockType.FLUFF_BALL);
        // 初期化
        stageBlock.Initialize(gridPos);

        generationObject.SetActive(true);

        return generationObject;
    }

    /// <summary>
    /// フェルトブロックを生成するメソッド
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="gridPos"></param>
    /// <param name="material"></param>
    /// <returns></returns>
    public GameObject GenerateFeltBlock(Transform parent, GridPos gridPos, EmotionCurrent.Type emotionType)
    {
        // 生成するオブジェクトの取得
        GameObject generationObject = GetFeltBlockFromPool();
        // 親の設定
        if (parent != null)
            generationObject.transform.SetParent(parent, true);
        // マテリアルの設定
        MeshRenderer meshRenderer = generationObject?.GetComponent<FeltBlock>().meshRenderer;
        if (meshRenderer != null )
        {
            meshRenderer.material = MaterialLibrary.GetInstance.GetMaterial(MaterialLibrary.MaterialGroup.FELT_BLOCK, emotionType);
        }
        
        // ステージブロックの設定
        StageBlock stageBlock = generationObject.GetComponent<StageBlock>();
        stageBlock.SetBlockType(StageBlock.BlockType.FELT_BLOCK);
        // 初期化
        stageBlock.Initialize(gridPos);
        return generationObject;
    }

    /// <summary>
    /// 不動フェルトブロックを生成するメソッド
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="gridPos"></param>
    /// <returns></returns>
    public GameObject GenerateNoMovementFeltBlock(Transform parent, GridPos gridPos)
    {
        // 生成するオブジェクトの取得
        GameObject generationObject = GetNoMovementFeltBlockFromPool();
        // 親の設定
        if (parent != null)
            generationObject.transform.SetParent(parent, true);
        // マテリアルの設定
        MeshRenderer meshRenderer = generationObject?.GetComponent<FeltBlock>().meshRenderer;
        if (meshRenderer != null )
        {
            meshRenderer.material = MaterialLibrary.GetInstance.GetMaterial(MaterialLibrary.MaterialGroup.FELT_BLOCK, EmotionCurrent.Type.REJECTION);
        }
        // ステージブロックの設定
        StageBlock stageBlock = generationObject.GetComponent<StageBlock>();
        stageBlock.SetBlockType(StageBlock.BlockType.FELT_BLOCK);

        // 初期化
        stageBlock.Initialize(gridPos);
        return generationObject;
    }


    public GameObject GenerateCurtain(Transform parent, float localRotateY, GridPos gridPos, EmotionCurrent.Type emotionType)
    {
        // 生成するオブジェクトの取得
        GameObject generationObject = GetCurtainFromPool();
        // 親の設定
        if (parent != null)
            generationObject.transform.SetParent(parent, true);

        // マテリアルの設定
        var curtain = generationObject?.GetComponent<Curtain>();
        if (curtain)
        {
            curtain.SetMaterial(MaterialLibrary.GetInstance.GetMaterial(MaterialLibrary.MaterialGroup.CURTAIN, emotionType));
        }
        // 種類の設定
        var emotionCurrent = generationObject?.GetComponent<EmotionCurrent>();
        if (emotionCurrent)
        {
            emotionCurrent.CurrentType = emotionType;
        }

        // ローカル回転の設定
        generationObject.transform.localRotation = Quaternion.Euler(0.0f, localRotateY, 0.0f);

        // ステージブロックの設定
        StageBlock stageBlock = generationObject.GetComponent<StageBlock>();
        stageBlock.SetBlockType(StageBlock.BlockType.CURTAIN);
        // 初期化
        stageBlock.Initialize(gridPos);
        return generationObject;
    }

    public GameObject GenerateSatinFloor(Transform parent, GridPos gridPos)
    {
        // 生成するオブジェクトの取得
        GameObject generationObject = GetSatinFloorFromPool();
        // 親の設定
        if (parent != null)
            generationObject.transform.SetParent(parent, true);
        // ステージブロックの設定
        StageBlock stageBlock = generationObject.GetComponent<StageBlock>();
        stageBlock.SetBlockType(StageBlock.BlockType.SATIN_FLOOR);
        // 初期化
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

        // 生成するオブジェクトの取得
        GameObject generationObject = GetPairBadgeFromPool();
        var pairBadge = generationObject?.GetComponent<PairBadge>();
        if (pairBadge == null) return null;

        // ブロックの生成と登録
        List<FeltBlock> feltBlocks = new List<FeltBlock>();
        for (int i = 0; i < generationBlockPositionList.Count; i++)
        {
            FeltBlock feltBlock = GetFeltBlock_PairBadgeFromPool().GetComponent<FeltBlock>();
            feltBlock.stageBlock.Initialize(generationBlockPositionList[i]);

            feltBlock.SetModel(PairBadgeMeshLibrary.Instance.GetMeshPrefab(emotionType));

            // メッシュの設定
  

            // 追加
            feltBlocks.Add(feltBlock);
        }

        // ペアバッジの初期化
        pairBadge.Initialize(feltBlocks);


        // 親の設定
        if (parent != null)
            generationObject.transform.SetParent(parent, true);
     

        return generationObject;
    }

    public GameObject GenerateFragment(Transform parent, GridPos gridPos, EmotionCurrent.Type emotionType)
    {
        // 生成するオブジェクトの取得
        GameObject generationObject = GetFragmentFromPool();
        // 親の設定
        if (parent != null)
            generationObject.transform.SetParent(parent, true);
        // マテリアルの設定
        var meshRenderer = generationObject?.GetComponent<Fragment>().MeshRenderer;
        if (meshRenderer != null)
        {
            meshRenderer.material = MaterialLibrary.GetInstance.GetMaterial(MaterialLibrary.MaterialGroup.FRAGMENT, emotionType);
        }

        // 種類の設定
        var emotionCurrent = generationObject?.GetComponent<EmotionCurrent>();
        if (emotionCurrent)
        {
            emotionCurrent.CurrentType = emotionType;
        }

        // ステージブロックの設定
        StageBlock stageBlock = generationObject.GetComponent<StageBlock>();
        stageBlock.SetBlockType(StageBlock.BlockType.FRAGMENT);
        // 初期化
        stageBlock.Initialize(gridPos);
        return generationObject;
    }

    public GameObject GenerateCarriableCore(Transform parent, GridPos gridPos, EmotionCurrent.Type emotionType)
    {
        // 生成するオブジェクトの取得
        GameObject generationObject = GetCarriableCoreFromPool();
        // 親の設定
        if (parent != null)
            generationObject.transform.SetParent(parent, true);
        // マテリアルの設定
        var meshRenderer = generationObject?.GetComponent<FeelingCore>().MeshRenderer;
        if (meshRenderer != null)
        {
            meshRenderer.material = MaterialLibrary.GetInstance.GetMaterial(MaterialLibrary.MaterialGroup.CORE, emotionType);
        }

        // 種類の設定
        var emotionCurrent = generationObject?.GetComponent<EmotionCurrent>();
        if (emotionCurrent)
        {
            emotionCurrent.CurrentType = emotionType;
        }

        // ステージブロックの設定
        StageBlock stageBlock = generationObject.GetComponent<StageBlock>();
        stageBlock.SetBlockType(StageBlock.BlockType.CARRIABLE_CORE);
        // 初期化
        stageBlock.Initialize(gridPos);
        return generationObject;
    }


    // ========================================================================================
    // ===== オブジェクトプールからの取得メソッド ==============================================

    /// <summary>
    /// 想いの型をオブジェクトプールから取得するメソッド
    /// </summary>
    /// <returns></returns>
    private GameObject GetFeelingSlotFromPool()
    {
        // オブジェクトプールから活動していない想いの型の取得
        for (int i = 0; i < m_feelingSlotPool.Count; i++)
        {
            if (m_feelingSlotPool[i] != null && m_feelingSlotPool[i].activeSelf == false)
            {
                return m_feelingSlotPool[i];
            }
        }
        // すべての想いの型が活動中の場合、新しい想いの型を生成してプールに追加
        GameObject newFeelingSlot = Instantiate(m_feelingSlotPrefab);
        m_feelingSlotPool.Add(newFeelingSlot);
        return newFeelingSlot;
    }

    /// <summary>
    /// 終点想いの型をオブジェクトプールから取得するメソッド
    /// </summary>
    /// <returns></returns>
    private GameObject GetTerminusFeelingSlotFromPool()
    {
        // オブジェクトプールから活動していない終点想いの型の取得
        for (int i = 0; i < m_terminusFeelingSlotPool.Count; i++)
        {
            if (m_terminusFeelingSlotPool[i] != null && m_terminusFeelingSlotPool[i].activeSelf == false)
            {
                return m_terminusFeelingSlotPool[i];
            }
        }
        // すべての終点想いの型が活動中の場合、新しい終点想いの型を生成してプールに追加
        GameObject newTerminusFeelingSlot = Instantiate(m_terminusFeelingSlotPrefab);
        m_terminusFeelingSlotPool.Add(newTerminusFeelingSlot);
        return newTerminusFeelingSlot;
    }


    /// <summary>
    /// 毛糸玉をオブジェクトプールから取得するメソッド
    /// </summary>
    /// <returns></returns>
    private GameObject GetFluffBallFromPool()
    {
        // オブジェクトプールから活動していない毛糸玉の取得
        for (int i = 0; i < m_fluffballPool.Count; i++)
        {
            if (m_fluffballPool[i] != null && m_fluffballPool[i].activeSelf == false)
            {
                return m_fluffballPool[i];
            }
        }
        // すべての毛糸玉が活動中の場合、新しい毛糸玉を生成してプールに追加
        GameObject newFluffBall = Instantiate(m_fluffBallPrefab);
        m_fluffballPool.Add(newFluffBall);
        return newFluffBall;
    }

    /// <summary>
    /// フェルトブロックをプールから取得するメソッド
    /// </summary>
    /// <returns></returns>
    private GameObject GetFeltBlockFromPool()
    {
        // オブジェクトプールから活動していない毛糸玉の取得
        for (int i = 0; i < m_feltBlockPool.Count; i++)
        {
            if (m_feltBlockPool[i] != null && m_feltBlockPool[i].activeSelf == false)
            {
                return m_feltBlockPool[i];
            }
        }
        // すべてのフェルトブロックが活動中の場合、新しいフェルトブロックを生成してプールに追加
        GameObject newFeltBlock = Instantiate(m_feltBlockPrefab);
        m_feltBlockPool.Add(newFeltBlock);
        return newFeltBlock;
    }

    /// <summary>
    /// 不動フェルトブロックをプールから取得するメソッド
    /// </summary>
    /// <returns></returns>
    private GameObject GetNoMovementFeltBlockFromPool()
    {
        // オブジェクトプールから活動していない不動フェルトブロックの取得
        for (int i = 0; i < m_noMovementFeltBlockPool.Count; i++)
        {
            if (m_noMovementFeltBlockPool[i] != null && m_noMovementFeltBlockPool[i].activeSelf == false)
            {
                return m_noMovementFeltBlockPool[i];
            }
        }
        // すべての不動フェルトブロックが活動中の場合、新しい不動フェルトブロックを生成してプールに追加
        GameObject newNoMovementFeltBlock = Instantiate(m_noMovementFeltBlockPrefab);
        m_noMovementFeltBlockPool.Add(newNoMovementFeltBlock);
        return newNoMovementFeltBlock;
    }

    /// <summary>
    /// カーテンをプールから取得するメソッド
    /// </summary>
    /// <returns></returns>
    private GameObject GetCurtainFromPool()
    {
        // オブジェクトプールから活動していないカーテンの取得
        for (int i = 0; i < m_curtainPool.Count; i++)
        {
            if (m_curtainPool[i] != null && m_curtainPool[i].activeSelf == false)
            {
                return m_curtainPool[i];
            }
        }
        // すべてのカーテンが活動中の場合、新しいカーテンを生成してプールに追加
        GameObject newCurtain = Instantiate(m_curtainPrefab);
        m_curtainPool.Add(newCurtain);
        return newCurtain;
    }

    /// <summary>
    /// サテン床をプールから取得するメソッド
    /// </summary>
    /// <returns></returns>
    private GameObject GetSatinFloorFromPool()
    {
        // オブジェクトプールから活動していないサテン床の取得
        for (int i = 0; i < m_satinFloorPool.Count; i++)
        {
            if (m_satinFloorPool[i] != null && m_satinFloorPool[i].activeSelf == false)
            {
                return m_satinFloorPool[i];
            }
        }
        // すべてのサテン床が活動中の場合、新しいサテン床を生成してプールに追加
        GameObject newSatinFloor = Instantiate(m_satinFloorPrefab);
        m_satinFloorPool.Add(newSatinFloor);
        return newSatinFloor;
    }


    /// <summary>
    /// ペアバッジをプールから取得するメソッド
    /// </summary>
    /// <returns></returns>
    private GameObject GetPairBadgeFromPool()
    {
        // オブジェクトプールから活動していないペアバッジの取得
        for (int i = 0; i < m_pairBadgePool.Count; i++)
        {
            if (m_pairBadgePool[i] != null && m_pairBadgePool[i].activeSelf == false)
            {
                return m_pairBadgePool[i];
            }
        }
        // すべてのペアバッジが活動中の場合、新しいペアバッジを生成してプールに追加
        GameObject newPairBadge = Instantiate(m_pairBadgePrefab);

        m_pairBadgePool.Add(newPairBadge);
        return newPairBadge;
    }

    /// <summary>
    /// ペアバッジのフェルトブロックをプールから取得するメソッド
    /// </summary>
    /// <returns></returns>
    private GameObject GetFeltBlock_PairBadgeFromPool()
    {
        // オブジェクトプールから活動していないペアバッジのフェルトブロックの取得
        for (int i = 0; i < m_feltBlock_PairBadgePool.Count; i++)
        {
            if (m_feltBlock_PairBadgePool[i] != null && m_feltBlock_PairBadgePool[i].activeSelf == false)
            {
                return m_feltBlock_PairBadgePool[i];
            }
        }
        // すべてのペアバッジのフェルトブロックが活動中の場合、新しいペアバッジのフェルトブロックを生成してプールに追加
        GameObject newFeltBlockPairBadge = Instantiate(m_feltBlock_PairBadgePrefab);
        m_feltBlock_PairBadgePool.Add(newFeltBlockPairBadge);
        return newFeltBlockPairBadge;
    }

    private GameObject GetFragmentFromPool()
    {
        // オブジェクトプールから活動していない想いの断片の取得
        for (int i = 0; i < m_fragmentPool.Count; i++)
        {
            if (m_fragmentPool[i] != null && m_fragmentPool[i].activeSelf == false)
            {
                return m_fragmentPool[i];
            }
        }
        // すべての想いの断片が活動中の場合、新しい想いの断片を生成してプールに追加
        GameObject newFragment = Instantiate(m_fragmentPrefab);
        m_fragmentPool.Add(newFragment);
        return newFragment;
    }

    /// <summary>
    /// 持ち運び可能なコアをプールから取得するメソッド
    /// </summary>
    /// <returns></returns>
    private GameObject GetCarriableCoreFromPool()
    {
        // オブジェクトプールから活動していない持ち運び可能なコアの取得
        for (int i = 0; i < m_carriableCorePool.Count; i++)
        {
            if (m_carriableCorePool[i] != null && m_carriableCorePool[i].activeSelf == false)
            {
                return m_carriableCorePool[i];
            }
        }
        // すべての持ち運び可能なコアが活動中の場合、新しい持ち運び可能なコアを生成してプールに追加
        GameObject newCarriableCore = Instantiate(m_carriableCorePrefab);
        m_carriableCorePool.Add(newCarriableCore);
        return newCarriableCore;
    }


}
