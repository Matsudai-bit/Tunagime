using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float m_speed = 5.0f; // 移動速度
    private Rigidbody m_rb;           // Rigidbodyコンポーネント

    private StageBlock m_stageBlock;

    [SerializeField] private GameDirector m_gameDirector;


    void Awake()
    {
        m_stageBlock = GetComponent<StageBlock>();

        Application.targetFrameRate = 60;
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="map"></param>
    public void Initialize(GridPos gridPos)
    {
 
        // ギミックの共通初期化
        m_stageBlock.Initialize(gridPos);
    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Rigidbodyコンポーネントの取得
        m_rb = GetComponent<Rigidbody>();

        

    }

    // Update is called once per frame
    void Update()
    {
        // リロード
        if (Input.GetKeyDown(KeyCode.Q)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    private void FixedUpdate()
    {
        // 動く
        Move();
    }

    /// <summary>
    /// 移動ベクトルの取得
    /// </summary>
    /// <returns>移動ベクトル</returns>
    private Vector3 GetMovementVec()
    {
        // キー入力で移動
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 入力がなければ
        if (Mathf.Approximately(x, 0.0f) && Mathf.Approximately(z, 0.0f))
        {
            // 移動停止
            m_rb.linearVelocity = new Vector3(0.0f, m_rb.linearVelocity.y, 0.0f);
       
            return Vector3.zero;
        }

        // 移動方向の計算
        Vector3 moveVec = new Vector3(x, 0.0f, z);

 

        return moveVec;


    }

    /// <summary>
    /// 動く
    /// </summary>
    private void Move()
    {
        // 移動ベクトルの取得
        Vector3 movementVec = GetMovementVec();

        if (movementVec.magnitude > 0.0f)
        {

            // 単位ベクトルではない場合正規化
            if (movementVec.magnitude > 1.0f)
            {
                movementVec.Normalize();
            }





        }

        // 加速度
        m_rb.linearVelocity = movementVec * m_speed;



    }

    void HaveItem()
    {
        m_gameDirector.AddHasItemNum();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ClearItem"))
        {
            // 削除(クラスを分けるべきかも)
            collision.gameObject.SetActive(false);
            // アイテムを獲得数
            HaveItem();

        }
    }
}
