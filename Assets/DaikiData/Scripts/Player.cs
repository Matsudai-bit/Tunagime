using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("プレイヤーの移動速度")]
    public float SPEED = 5.0f; // 移動速度
    [Header("プレイヤーの回転速度")]
    public float ROTATE_SPEED = 3.0f; // 回転速度

    [Header("プレイヤーのアニメーター")]
    [SerializeField] private Animator m_animator;

    [Header("ゲームディレクター")]
    [SerializeField] private GameDirector m_gameDirector;
    [Header("現在のアミダ生成機")]
    [SerializeField] private AmidaTubeGenerator m_amidaGenerator;

    private Rigidbody m_rb;             // Rigidbodyコンポーネント
    private StageBlock m_stageBlock;

    private StageBlock m_fluffBall; // 綿毛ボール (fluff ball) を保持する変数

    void Awake()
    {
        m_stageBlock = GetComponent<StageBlock>();

        // アプリケーションのフレームレートを60に設定
        Application.targetFrameRate = 60;
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="gridPos"></param>
    public void Initialize(GridPos gridPos)
    {
        // ギミックの初期位置を設定
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

        // Xキーを押したときの処理
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (m_fluffBall) // 綿毛ボールを持っている場合
            {
                // 最も近いグリッド位置の取得
                var closestPos = map.GetClosestGridPos(transform.position);

                // アミダ橋の生成
                var generateAmida = m_amidaGenerator.GenerateAmidaBridge(closestPos);

                m_fluffBall = null; // 綿毛ボールを解放
            }
        }

        // Zキーを押したときの処理
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (m_fluffBall) // 綿毛ボールを持っている場合
            {
                // 最も近いグリッド位置の取得
                var closestPos = map.GetClosestGridPos(transform.position);
                m_fluffBall.UpdatePosition(closestPos); // 綿毛ボールの位置を更新

                m_fluffBall.SetActive(true); // 綿毛ボールをアクティブにする

                // グリッドデータに綿毛ボールを配置
                map.GetStageGridData().TryPlaceTileObject(closestPos, m_fluffBall.GetComponent<GameObject>());
                m_fluffBall = null; // 綿毛ボールを解放
            }
            else // 綿毛ボールを持っていない場合
            {
                // 最も近いグリッド位置の取得
                var closestPos = map.GetClosestGridPos(transform.position);

                // 参照用変数 : 現在の向いている方向ベクトル
                Vector3 forward = transform.forward;

                GridPos forward2D;
                // 周囲のグリッドデータの取得

                // 各軸の方向の大きさを比較して大きい方を正規化し、グリッド方向として選択
                // 注意 : 小数点が絡むため、厳密ではない場合があるのでRoundを使用しているため不適切と判断
                forward2D = (Mathf.Abs(forward.x) > Mathf.Abs(forward.z))
                    ? new GridPos((int)Mathf.Round(forward.x), 0)
                    : new GridPos(0, -(int)Mathf.Round(forward.z));
                GridPos checkPos = closestPos + forward2D; // チェックするグリッド位置

                // 拾うグリッド位置がグリッド範囲内かチェック
                if (map.CheckInnerGridPos(checkPos))
                {
                    TileObject tileObject = map.GetStageGridData().GetTileData[checkPos.y, checkPos.x].tileObject;

                    // 拾えるアイテムかどうかの判定 (綿毛ボールの場合)
                    if (tileObject.type == TileType.FLUFF_BALL && tileObject.gameObject)
                    {
                        
                        m_animator.SetTrigger("PickUp"); // 綿毛ボールを拾うアニメーションをトリガー

                        GameObject gameObj = tileObject.gameObject;
                        m_fluffBall = gameObj.GetComponent<StageBlock>(); // 綿毛ボールを取得
                        map.GetStageGridData().RemoveGridData(checkPos); // グリッドから綿毛ボールを削除
                        Debug.Log(gameObj.name); // デバッグログに綿毛ボールの名前を出力

                        m_fluffBall.SetActive(false); // 綿毛ボールを非アクティブにする
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // 回転処理
        Rotate();

        if (IsMoving()) // 移動している場合
        {
            // 歩行アニメーションを有効にする
            m_animator.SetBool("Walk", true);
            // 移動処理
            Move();
        }
        else // 移動していない場合
        {
            // 歩行アニメーションを無効にする
            m_animator.SetBool("Walk", false);
        }
    }

    private void Rotate()
    {
        // 回転入力の取得
        float x = Input.GetAxis("Horizontal");
        // x軸の入力が大きい場合、x軸を中心に回転
        transform.rotation *= Quaternion.Euler(0.0f, x * ROTATE_SPEED, 0.0f);
    }

    /// <summary>
    /// 移動しているかどうかの判定
    /// </summary>
    /// <returns></returns>
    private bool IsMoving()
    {
        // キー入力で移動
        float z = Input.GetAxis("Vertical");
        // 入力がない場合
        if (z <= 0.0f)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void Move()
    {
        // 移動ベクトルを取得
        float z = Input.GetAxis("Vertical");
        Vector3 movementVec = transform.forward * z;

        if (movementVec.magnitude > 0.0f)
        {
            // アニメーションの設定
            m_animator.SetBool("IsWalk", true);

            // スケールベクトルではない場合を考慮
            if (movementVec.magnitude > 1.0f)
            {
                movementVec.Normalize(); // 正規化
            }
        }

        // 移動速度を設定
        m_rb.linearVelocity = movementVec * SPEED;
    }

    void HaveItem()
    {
        m_gameDirector.AddHasItemNum(); // アイテム数を増やす
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ClearItem")) // クリアアイテムに衝突した場合
        {
            // 削除 (クラスを削除するため全て削除)
            collision.gameObject.SetActive(false); // オブジェクトを非アクティブにする
            // アイテムをカウント
            HaveItem();
        }
    }
}