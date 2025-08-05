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

    private PlayerStateMachine m_stateMachine; // プレイヤーの状態マシン


    void Awake()
    {
        m_stageBlock = GetComponent<StageBlock>();

        // プレイヤーの状態マシンを初期化
        m_stateMachine = new PlayerStateMachine(this);

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

        m_stateMachine.RequestStateChange(PlayerStateID.IDLE); // 初期状態をIDLEに設定
    }

    // Update is called once per frame
    void Update()
    {

        // プレイヤーの状態マシンの更新
        m_stateMachine.UpdateState();

        var map = MapData.GetInstance;

        // リロード
        if (Input.GetKeyDown(KeyCode.Q)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

       

    
        
    }

    private void FixedUpdate()
    {
        // プレイヤーの状態マシンの更新
        m_stateMachine.FixedUpdateState();

        //// 回転処理
        //Rotate();

        //if (IsMoving()) // 移動している場合
        //{
        //    // 歩行アニメーションを有効にする
        //    m_animator.SetBool("Walk", true);
        //    // 移動処理
        //    Move();
        //}
        //else // 移動していない場合
        //{
        //    // 歩行アニメーションを無効にする
        //    m_animator.SetBool("Walk", false);
        //}
    }


    /// <summary>
    /// 移動しているかどうかの判定
    /// </summary>
    /// <returns></returns>
    public bool IsMoving()
    {
        // キー入力で移動
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // 入力がない場合
        if (Mathf.Approximately(x, 0.0f) && Mathf.Approximately(z, 0.0f))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    public void Move()
    {
        // キー入力で移動
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movementVec = Vector3.forward * z + Vector3.right * x;

        // 移動ベクトルの大きさが0でない場合
        if (movementVec.magnitude > 0.0f)
        {
            // スケールベクトルではない場合を考慮
            if (movementVec.magnitude > 1.0f)
            {
                movementVec.Normalize(); // 正規化
            }
            // 移動速度を設定
            m_rb.linearVelocity = movementVec * SPEED;
            // 徐々に回転
            Quaternion targetRotation = Quaternion.LookRotation(movementVec, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, ROTATE_SPEED * Time.fixedDeltaTime);
        }
        else
        {
            // 入力がない場合は速度をゼロにする
            m_rb.linearVelocity = Vector3.zero;
        }


    }

    /// <summary>
    /// プレイヤーの状態マシンを取得
    /// </summary>
    /// <returns></returns>
    public Animator GetAnimator()
    {
        return m_animator;
    }

    public PlayerStateMachine GetStateMachine()
    {
        return m_stateMachine;
    }

    public void SetFluffBall(StageBlock fluffBall)
    {


        m_fluffBall = fluffBall; // 綿毛ボールを設定
    }


    /// <summary>
    /// 綿毛ボールを離す
    /// </summary>
    public void ReleaseFluffBall()
    {
        if (m_fluffBall != null)
        {
            m_fluffBall = null; // 綿毛ボールをnullに設定
        }
    }


    public StageBlock GetFluffBall()
    {
        return m_fluffBall; // 綿毛ボールを取得
    }

    public GridPos GetForwardGridPos()
    {
        var map = MapData.GetInstance; // マップを取得

        // 最も近いグリッド位置の取得
        var closestPos = map.GetClosestGridPos(transform.position);

        Vector3 forward = transform.forward;

        // 各軸の方向の大きさを比較して大きい方を正規化し、グリッド方向として選択
        // 注意 : 小数点が絡むため、厳密ではない場合があるのでRoundを使用しているため不適切と判断
        GridPos forward2D = (Mathf.Abs(forward.x) > Mathf.Abs(forward.z))
            ? new GridPos((int)Mathf.Round(forward.x), 0)
            : new GridPos(0, -(int)Mathf.Round(forward.z));

        return closestPos + forward2D; // チェックするグリッド位置
    }

    /// <summary>
    /// 綿毛ボールを拾う処理に挑戦
    /// </summary>
    /// <returns></returns>
    public bool TryPickUp()
    {
        var map = MapData.GetInstance; // マップを取得

        // Zキーを押したときの処理  綿毛ボールを持ってない場合
        if (Input.GetKeyDown(KeyCode.Z) &&
            m_fluffBall == false)
        {
            GridPos checkPos = GetForwardGridPos(); // チェックするグリッド位置

            // 拾うグリッド位置がグリッド範囲内かチェック
            if (map.CheckInnerGridPos(checkPos))
            {
                TileObject tileObject = map.GetStageGridData().GetTileData[checkPos.y, checkPos.x].tileObject;

                // 拾えるアイテムかどうかの判定 (綿毛ボールの場合)
                if (tileObject.type == TileType.FLUFF_BALL && tileObject.gameObject)
                {

                    // 綿毛ボールを拾う状態に切り替える
                    m_stateMachine.RequestStateChange(PlayerStateID.PICK_UP); // 綿毛ボールを拾う状態に変更

                    return true; // 綿毛ボールを拾う処理が成功した
                }
            }

        }

        return false; // 綿毛ボールを拾う処理が失敗した

    }

    /// <summary>
    /// 綿毛ボールを置く処理に挑戦
    /// </summary>
    /// <returns></returns>
    public bool TryPickDown()
    {
        // Zキーを押したときの処理
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (m_fluffBall) // 綿毛ボールを持っている場合
            {
 
                // 綿毛ボールを置く状態に切り替える
                m_stateMachine.RequestStateChange(PlayerStateID.PICK_DOWN); // 綿毛ボールを置く状態に変更

                return true; // 綿毛ボールを置く処理が成功した
            }
        }

        return false; // 綿毛ボールを置く処理が失敗した

    }


    /// <summary>
    /// 編むことができるかどうかの判定
    /// </summary>
    /// <returns>  </returns>
    private bool CanKnit()
    {
        var stageGridData = MapData.GetInstance.GetStageGridData(); // マップを取得
        GridPos myPos = MapData.GetInstance.GetClosestGridPos(transform.position); // チェックするグリッド位置
        

        // 自分のいるグリッドの上下にノーマル状態のあみだがあるかどうか
        if (stageGridData.GetAmidaTube(myPos) == null &&
            StageAmidaUtility.CheckAmidaState(myPos + new GridPos(0, 1), AmidaTube.State.NORMAL) &&
            StageAmidaUtility.CheckAmidaState(myPos + new GridPos(0, -1), AmidaTube.State.NORMAL))
        {
            return true; // 編むことが出来る
        }

        return false; // 編むことが出来ない

    }

    public void TryKnot()
    {
        // Xキーを押したときの処理
        if (Input.GetKeyDown(KeyCode.X) && CanKnit())
        {
            if (m_fluffBall) // 綿毛ボールを持っている場合
            {


                // 編む状態に切り替える
                m_stateMachine.RequestStateChange(PlayerStateID.KNOT); 

            }
        }
    }

    /// <summary>
    /// ゲームオブジェクトの取得
    /// </summary>
    /// <returns></returns>
    public GameObject gameObject
    {
        get
        {
            return base.gameObject;
        }
    }

}