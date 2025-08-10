using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header( "====== ステージ生成器(何ステージ)の設定 ======")]
    [SerializeField]
    private GameObject m_stageGenerator;

    [Header("====== 各親の設定 ======")]
    [SerializeField]

    [Header("あみだチューブ")]
    private Transform m_amidaParent;

    [Header("床")]
    [SerializeField]
    private Transform m_floorBlockParent;

    [Header("ギミック関連")]
    [SerializeField]
    private Transform m_gimmickParent;

    [Header("======ゲームディレクターの設定 ======")]
    [SerializeField]　private GameDirector m_gameDirector; // ゲームディレクターのインスタンス

    [Header("====== クリア状態のチェッカーの設定 ======")]
    [SerializeField] private ClearConditionChecker m_clearConditionChecker; // クリア状態のチェッカー

    [Header("====== あみだマネージャーの設定 ======")]
    [SerializeField] private AmidaManager m_amidaManager; // あみだマネージャーのインスタンス


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ステージ生成器のインスタンスを生成
        if (m_stageGenerator != null)
        {
            m_stageGenerator = Instantiate(m_stageGenerator, Vector3.zero, Quaternion.identity);
            m_stageGenerator.GetComponent<StageGenerator>().Generate(
                m_amidaManager,
                m_amidaParent,
                m_floorBlockParent,
                m_gimmickParent,
                m_clearConditionChecker
            );
        }
        else
        {
            Debug.LogError("ステージ生成器が設定されていません。");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
