using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ステージオブジェクトを生成するためのファクトリークラス　シングルトン
/// </summary>
public class StageObjectFactory : MonoBehaviour
{
    private static StageObjectFactory s_instance;   // シングルトンインスタンス

    [Header("====== ステージオブジェクトのプレハブ設定 ======")]
    [SerializeField] private GameObject m_fluffBallPrefab;              // 毛糸玉のプレハブ
    [SerializeField] private GameObject m_feltBlockPrefab;              // フェルトブロックのプレハブ
    [SerializeField] private GameObject m_noMovementFeltBlockPrefab;    // 不動フェルトブロックのプレハブ
    [SerializeField] private GameObject m_curtainPrefab;                // カーテンプレファブ


    // オブジェクトプール
    List<GameObject> m_fluffballPool            = new List<GameObject>(); // 毛糸玉のオブジェクトプール
    List<GameObject> m_feltBlcokPool            = new List<GameObject>(); // フェルトブロックのオブジェクトプール
    List<GameObject> m_noMovementFeltBlockPool  = new List<GameObject>(); // 不動フェルトブロックのオブジェクトプール
    List<GameObject> m_curtainPool              = new List<GameObject>(); // カーテンのオブジェクトプール

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
            generationObject.transform.SetParent(parent, false);

        // ステージブロックの設定
        StageBlock stageBlock = generationObject.GetComponent<StageBlock>();
        stageBlock.SetBlockType(StageBlock.BlockType.FLUFF_BALL);
        // 初期化
        stageBlock.Initialize(gridPos);

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
            generationObject.transform.SetParent(parent, false);
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
            generationObject.transform.SetParent(parent, false);
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
            generationObject.transform.SetParent(parent, false);

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

    // ========================================================================================
    // ===== オブジェクトプールからの取得メソッド ==============================================


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
        for (int i = 0; i < m_feltBlcokPool.Count; i++)
        {
            if (m_feltBlcokPool[i] != null && m_feltBlcokPool[i].activeSelf == false)
            {
                return m_feltBlcokPool[i];
            }
        }
        // すべてのフェルトブロックが活動中の場合、新しいフェルトブロックを生成してプールに追加
        GameObject newFeltBlock = Instantiate(m_feltBlockPrefab);
        m_feltBlcokPool.Add(newFeltBlock);
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
