using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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

    private Carryable m_carryingObj; // 持ち運び可能なオブジェクト

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

    public void SetCarryingObject(Carryable carryingObj)
    {


        m_carryingObj = carryingObj; // 運んでいるオブジェクトを設定
    }


    /// <summary>
    /// 運んでいるオブジェクトを下ろす
    /// </summary>
    public void DropCarryingObject()
    {
        if (m_carryingObj != null)
        {
            m_carryingObj = null; // 運んでいるオブジェクトをnullに設定
        }
    }


    /// <summary>
    /// 現在持っているオブジェクトを取得
    /// </summary>
    /// <returns></returns>
    public Carryable GetCarryingObject()
    {
        return m_carryingObj; // 運んでいるオブジェクトを取得
    }

    /// <summary>
    /// プレイヤーの前方のグリッド位置を取得
    /// </summary>
    /// <returns></returns>
    public GridPos GetForwardGridPos()
    {
        var map = MapData.GetInstance; // マップを取得

        // 最も近いグリッド位置の取得
        var closestPos = map.GetClosestGridPos(transform.position);

        GridPos forward2D = GetForwardDirection(); // プレイヤーの前方のグリッド方向

        return closestPos + forward2D; // チェックするグリッド位置
    }

    /// <summary>
    /// プレイヤーの前方のグリッド方向を取得
    /// </summary>
    /// <returns></returns>
    public GridPos GetForwardDirection()
    {
        Vector3 forward = transform.forward;
        // 各軸の方向の大きさを比較して大きい方を正規化し、グリッド方向として選択
        // 注意 : 小数点が絡むため、厳密ではない場合があるのでRoundを使用しているため不適切と判断
        GridPos forward2D = (Mathf.Abs(forward.x) > Mathf.Abs(forward.z))
            ? new GridPos((int)Mathf.Round(forward.x), 0)
            : new GridPos(0, -(int)Mathf.Round(forward.z));
        return forward2D; // チェックするグリッド位置
    }

    /// <summary>
    /// 運んでいるオブジェクトを拾う処理に挑戦
    /// </summary>
    /// <returns></returns>
    public bool TryPickUp()
    {
        var map = MapData.GetInstance; // マップを取得

        // Zキーを押したときの処理  運んでいるオブジェクトを持ってない場合
        if (Input.GetKeyDown(KeyCode.Z) &&
            m_carryingObj == false)
        {
            GridPos pickingUpPos = GetForwardGridPos(); // チェックするグリッド位置

            // 拾うグリッド位置がグリッド範囲内かチェック
            if (map.CheckInnerGridPos(pickingUpPos))
            {
                TileObject tileObject = map.GetStageGridData().GetTileData[pickingUpPos.y, pickingUpPos.x].tileObject;

                // 拾えるアイテムかどうかの判定 (運んでいるオブジェクトの場合)
                if (tileObject.gameObject?.GetComponent<Carryable>() != null && tileObject.gameObject)
                {

                    // 運んでいるオブジェクトを拾う状態に切り替える
                    m_stateMachine.RequestStateChange(PlayerStateID.PICK_UP); // 運んでいるオブジェクトを拾う状態に変更

                    return true; // 運んでいるオブジェクトを拾う処理が成功した
                }
            }

        }

        return false; // 運んでいるオブジェクトを拾う処理が失敗した

    }

    /// <summary>
    /// 運んでいるオブジェクトを置く処理に挑戦
    /// </summary>
    /// <returns></returns>
    public bool TryPickDown()
    {
        // Zキーを押したときの処理
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (m_carryingObj) // 運んでいるオブジェクトを持っている場合
            {
                var stageGridData = MapData.GetInstance.GetStageGridData(); // マップを取得

                // 置く位置
                GridPos placementPos = GetForwardGridPos(); // 前方のグリッド位置

                // 自分のいるグリッドの上下にノーマル状態のあみだがあるかどうか
                if (stageGridData.GetTileObject(placementPos).gameObject == null && MapData.GetInstance.CheckInnerGridPos(placementPos))
                {
                    // 運んでいるオブジェクトを置く状態に切り替える
                    m_stateMachine.RequestStateChange(PlayerStateID.PICK_DOWN);

                    return true; // 運んでいるオブジェクトを置く処理が成功した                }
                    
                }
            }
        }

        return false; // 運んでいるオブジェクトを置く処理が失敗した

    }


    /// <summary>
    /// 編むことができるかどうかの判定
    /// </summary>
    /// <returns>  </returns>
    public bool CanKnit()
    {
        var stageGridData = MapData.GetInstance.GetStageGridData(); // マップを取得

        // 編む位置
        GridPos knittingPos = GetForwardGridPos(); // 前方のグリッド位置

        // 自分のいるグリッドの上下にノーマル状態のあみだがあるかどうか
        if (stageGridData.GetAmidaTube(knittingPos) == null &&
            StageAmidaUtility.CheckAmidaState(knittingPos + new GridPos(0, 1), AmidaTube.State.NORMAL) &&
            StageAmidaUtility.CheckAmidaState(knittingPos + new GridPos(0, -1), AmidaTube.State.NORMAL))
        {
            return true; // 編むことが出来る
        }

        return false; // 編むことが出来ない

    }

    public void TryKnit()
    {
        // Xキーを押したときの処理
        if (Input.GetKeyDown(KeyCode.X) && CanKnit())
        {
            if (m_carryingObj) // 運んでいるオブジェクトを持っている場合
            {


                // 編む状態に切り替える
                m_stateMachine.RequestStateChange(PlayerStateID.KNIT); 

            }
        }
    }

    /// <summary>
    /// アミダを押すことができるかどうかの判定
    /// </summary>
    public bool TryPushBlock()
    {
        // 正面に半ブロック分のレイを飛ばす
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // マップの取得
            var map = MapData.GetInstance;
            // レイの飛ばす距離を設定
            float rayDistance = (float)(map.GetCommonData().width) / 2.0f;

            // レイキャストを使用して、プレイヤーの前方にあるオブジェクトを検出
            RaycastHit hit;
            if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit, rayDistance))
            {
                // レイが当たったオブジェクトがステージブロックであるかチェック
                if (hit.collider.gameObject?.GetComponent<FeltBlock>() != null)
                {
                    m_stateMachine.RequestStateChange(PlayerStateID.PUSH_BLOCK);
                }
            }

        }

        return true;
    }

    /// <summary>
    /// ゲームオブジェクトの取得
    /// </summary>
    /// <returns></returns>
    public GameObject GetGameObject
    {
        get
        {
            return base.gameObject;
        }
    }

}