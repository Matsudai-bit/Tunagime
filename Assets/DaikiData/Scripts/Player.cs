using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float m_speed = 5.0f; // 移動速度
    private Rigidbody m_rb;           // Rigidbodyコンポーネント

    private StageBlock m_stageBlock;

    [SerializeField] private GameDirector m_gameDirector;
    [SerializeField] private AmidaTubeGenerator m_amidaGenerator;

    private StageBlock m_fluffBall;


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
        var map = MapData.GetInstance;

        // リロード
        if (Input.GetKeyDown(KeyCode.Q)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (m_fluffBall)
            {        // 近いグリッド座標の取得
                var clossesPos = map.GetClosestGridPos(transform.position);
                var generateAmida = m_amidaGenerator.GenerateAmidaBridge(clossesPos);
                

                m_fluffBall = null;
            }

        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (m_fluffBall)
            {
                // 近いグリッド座標の取得
                var clossesPos = map.GetClosestGridPos(transform.position);
                m_fluffBall.UpdatePosition(clossesPos);

                m_fluffBall.SetActive(true);

                map.GetStageGridData().TryPlaceTileObject(clossesPos, m_fluffBall.GetComponent<GameObject>());
                m_fluffBall = null;

            }
            else 
            {

                // 近いグリッド座標の取得
                var clossesPos = map.GetClosestGridPos(transform.position);

                // 処理用変数 : 正面方向ベクトル
                Vector3 forward = transform.forward;

                GridPos forward2D;
                // 正面グリッド方向の取得

                // 正面ベクトルの成分を比較して大きい方を正面ベクトルとして選択
                // 注意 : めちゃくちゃ綺麗な斜めの場合バグる可能性あり Roundを使用しているため致命的ではないと判断
                forward2D = (Mathf.Abs(forward.x) > Mathf.Abs(forward.z))
                    ? new GridPos((int)Mathf.Round(forward.x), 0)
                    : new GridPos(0, -(int)Mathf.Round(forward.z));
                GridPos checkPos = clossesPos + forward2D;

                // でてきたグリッド座標がグリッド範囲内かチェック
                if (map.CheckInnerGridPos(checkPos))
                {
                    TileObject tileObject = map.GetStageGridData().GetTileData[checkPos.y, checkPos.x].tileObject;

                    // 毛糸だった時の処理
                    if (tileObject.type == TileType.FLUFF_BALL && tileObject.gameObject)
                    {
                        GameObject gameObj = tileObject.gameObject;
                        m_fluffBall = gameObj.GetComponent<StageBlock>();
                        map.GetStageGridData().RemoveGridData(checkPos);
                        Debug.Log(gameObj.name);

                        m_fluffBall.SetActive(false);
                    }
                }
            }

          
        }

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
