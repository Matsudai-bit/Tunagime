using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour , IGameInteractionObserver
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

    private GameObject m_targetObject; // ターゲットオブジェクト

    private Vector3 m_prevVelocity; // 前回の速度
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

        // ゲームインタラクションイベントのオブザーバーを登録
        GameInteractionEventMessenger.GetInstance.RegisterObserver(this); 
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
    /// 前回の移動速度を保存する
    /// </summary>
    public void SavePreviousMoveVelocity()
    {
        if (m_rb != null)
        {
            m_prevVelocity = m_rb.linearVelocity; // 前回の速度を保存
        }
    }

    /// <summary>
    /// 前回の移動速度を取得する
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPreviousMoveVelocity()
    {
        if (m_rb != null)
        {
            return m_prevVelocity; // 前回の速度を取得
        }
        return Vector3.zero; // Rigidbodyがnullの場合はゼロベクトルを返す
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

            //if (  m_rb.rotation.eulerAngles == Vector3.zero)
            //{
            m_rb.rotation = Quaternion.Slerp(m_rb.rotation, targetRotation, ROTATE_SPEED * Time.fixedDeltaTime);
            //}
            //else
            //{
            //    m_rb.rotation = Quaternion.RotateTowards(m_rb.rotation, targetRotation, ROTATE_SPEED * Time.fixedDeltaTime);
            //}


        }
        else
        {
            // 移動していない場合は速度をゼロにする
            StopMove();
        }


    }

    /// <summary>
    /// プレイヤーの移動を停止する
    /// </summary>
    public void StopMove()
    {
        // Rigidbodyの速度をゼロにする
        if (m_rb != null)
        {
            m_rb.linearVelocity = Vector3.zero; // 速度をゼロに設定
            m_rb.angularVelocity = Vector3.zero; // 回転速度もゼロに設定
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
    public bool TryPutDown()
    {
        // Zキーを押したときの処理
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (m_carryingObj) // 運んでいるオブジェクトを持っている場合
            {
                var stageGridData = MapData.GetInstance.GetStageGridData(); // マップを取得

                // 置く位置
                GridPos placementPos = GetForwardGridPos(); // 前方のグリッド位置

                // 自分の正面のグリッドがnullかどうか
               // if (stageGridData.GetTileObject(placementPos).gameObject == null && MapData.GetInstance.CheckInnerGridPos(placementPos))
                if (m_targetObject != null && MapData.GetInstance.CheckInnerGridPos(placementPos))
                {
                    // 運んでいるオブジェクトを置く状態に切り替える
                    m_stateMachine.RequestStateChange(PlayerStateID.PUT_DOWN);

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

    public void TryUnknit()
    {
        // Xキーを押したときの処理
        if (Input.GetKeyDown(KeyCode.X))
        {   // 解く位置
            GridPos unknittingPos = GetForwardGridPos(); // 前方のグリッド位置

            // 状態が橋なら解く
            if (StageAmidaUtility.CheckAmidaState(unknittingPos, AmidaTube.State.BRIDGE))
            {
                // アミダを解く状態に切り替える
                m_stateMachine.RequestStateChange(PlayerStateID.UNKNIT);
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
           

            if (m_targetObject?.GetComponent<IMoveTile>() != null )
            {
                // 押しだすブロックの一個置く側に空間が空いていれば押し出すことが出来る
                var stageBlock = m_targetObject.GetComponent<IMoveTile>();

                if (stageBlock.CanMove(GetForwardDirection()))
                {
                    // 押し出す状態に切り替える
                    m_stateMachine.RequestStateChange(PlayerStateID.PUSH_BLOCK);
                    return true; // 押し出す処理が成功した
                }
            }
        }

        return false;
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


    /// <summary>
    /// 指定したレイヤーのウェイトを、一定時間かけて目標値まで変更するリクエストを送る。
    /// </summary>
    /// <param name="layerIndex"></param>
    /// <param name="targetWeight"></param>
    /// <param name="duration"></param>
    public void RequestTransitionLayerWeight(string layerName, float targetWeight, float duration)
    {
        int layerIndex = m_animator.GetLayerIndex(layerName);
        if (layerIndex < 0)
        {
            Debug.LogError($"Animator does not have a layer named '{layerName}'.");
            return;
        }
        // コルーチンを開始
        StartCoroutine(TransitionLayerWeight(layerIndex, targetWeight, duration));
    }

    /// <summary>
    /// 指定したレイヤーのウェイトを、一定時間かけて目標値まで変更するコルーチン。
    /// </summary>
    public IEnumerator TransitionLayerWeight(int layerIndex, float targetWeight, float duration)
    {
        float startWeight = m_animator.GetLayerWeight(layerIndex);
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float newWeight = Mathf.Lerp(startWeight, targetWeight, time / duration);
            m_animator.SetLayerWeight(layerIndex, newWeight);
            yield return null;
        }

        // 最後にウェイトを確実に目標値に設定
        m_animator.SetLayerWeight(layerIndex, targetWeight);
    }

    public void TryForwardFloorSetting()
    {
        // プレイヤーの前方のグリッド位置を取得
        var forwardPos = GetForwardGridPos();
        // マップを取得
        var map = MapData.GetInstance;
        // 前方のグリッド位置がグリッド範囲内かチェック
        if (map.CheckInnerGridPos(forwardPos))
        {
            // 前方のグリッド位置のタイルオブジェクトを取得
            var tileData = map.GetStageGridData().GetTileData[forwardPos.y, forwardPos.x];
            GameObject floor = tileData.floor;

            if (floor != null && tileData.tileObject.gameObject == null)
            {
                // ターゲットオブジェクトを設定
                SetTargetObject(floor);
             
            }
            else
            {
                if (m_targetObject != null)
                {
                    // ターゲットオブジェクトが存在する場合はリセット
                    ResetTargetObject();
                }
                return;
            }
        }
        else
        {
            // グリッド範囲外の場合はターゲットオブジェクトをnullに設定
            m_targetObject = null;
        }
    }

    public void TryForwardSlotSetting()
    {
        // プレイヤーの前方のグリッド位置を取得
        var forwardPos = GetForwardGridPos();

        // マップを取得
        var map = MapData.GetInstance;
        // 前方のグリッド位置がグリッド範囲内かチェック
        if (map.CheckInnerGridPos(forwardPos))
        {
            // 前方のグリッド位置のタイルオブジェクトを取得
            TileObject tileObject = map.GetStageGridData().GetTileData[forwardPos.y, forwardPos.x].tileObject;

            if (tileObject.stageBlock != null && tileObject.stageBlock.GetBlockType() == StageBlock.BlockType.FEELING_SLOT)
            {
                var feelingSlot = tileObject.stageBlock?.GetComponent<FeelingSlot>();
                if (feelingSlot != null && feelingSlot.GetEmotionType() == EmotionCurrent.Type.NONE)
                {
                    // ターゲットオブジェクトを設定
                    SetTargetObject(tileObject.gameObject);
                }
               
            }
        }
    }


    public void TryForwardObjectSetting()
    {
        // プレイヤーの前方のグリッド位置を取得
        var forwardPos = GetForwardGridPos();

        // マップを取得
        var map = MapData.GetInstance;
        // 前方のグリッド位置がグリッド範囲内かチェック
        if (map.CheckInnerGridPos(forwardPos))
        {
            // 前方のグリッド位置のタイルオブジェクトを取得
            TileObject tileObject = map.GetStageGridData().GetTileData[forwardPos.y, forwardPos.x].tileObject;

            // タイルオブジェクトが存在する場合
            if (tileObject.gameObject != null && tileObject.stageBlock.CanInteract())
            {
                // ターゲットオブジェクトを設定
                SetTargetObject(tileObject.gameObject);
                return;
            }

            // タイルオブジェクトが存在しない場合、アミダのブリッジ状態のオブジェクトをチェック
            AmidaTube amidaTube = map.GetStageGridData().GetAmidaTube(forwardPos);
            if (amidaTube != null && amidaTube.GetState() == AmidaTube.State.BRIDGE)
            {
                SetTargetObject(amidaTube.gameObject);
                return; // アミダのブリッジ状態のオブジェクトがある場合はターゲットオブジェクトを設定して終了
            }
            else
            {
                if (m_targetObject != null)
                {
                    // ターゲットオブジェクトが存在する場合はリセット
                    ResetTargetObject();
                }
                return;
            }
        }
        else
        {
            // グリッド範囲外の場合はターゲットオブジェクトをnullに設定
            m_targetObject = null;
        }
    }

    /// <summary>
    /// ターゲットオブジェクトを設定する。テスト用
    /// </summary>
    /// <param name="targetObject"></param>
    public void SetTargetObject(GameObject targetObject)
    {
        if (m_targetObject == targetObject)
        {
            return; // 既に同じオブジェクトが設定されている場合は何もしない
        }
        ResetTargetObject();

        m_targetObject = targetObject; // ターゲットオブジェクトを設定


        // レイヤー名の変更
        m_targetObject.layer = LayerMask.NameToLayer("Outline");
        SetLayerRecursively(m_targetObject.transform, LayerMask.NameToLayer("Outline"));

    }

    /// <summary>
    /// ターゲットオブジェクトをリセットする。
    /// </summary>
    public void ResetTargetObject()
    {
        if (m_targetObject != null)
        {

            // レイヤー名の変更
            m_targetObject.layer = LayerMask.NameToLayer("Default");
            SetLayerRecursively(m_targetObject.transform, LayerMask.NameToLayer("Default"));

            // 既にターゲットオブジェクトが設定されている場合は、前のオブジェクトのアルファ値を最大に戻す
            Renderer renderer = m_targetObject?.GetComponentInChildren<Renderer>();

            // 親から子までレイヤー名の変
            m_targetObject = null; // ターゲットオブジェクトをnullに設定
        }
    }


    /// <summary>
    /// 指定したオブジェクトとその子オブジェクトのレイヤーを再帰的に設定する。
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="newLayer"></param>
    private void SetLayerRecursively(Transform obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.gameObject.layer = newLayer;

        foreach (Transform child in obj)
        {
            SetLayerRecursively(child, newLayer);
        }
    }


    /// <summary>
    /// プレイヤーの現在のグリッド位置を取得する
    /// </summary>
    /// <returns></returns>
    public GridPos GetGridPosition()
    {
        // 最も近いグリッド位置の取得
        return MapData.GetInstance.GetClosestGridPos(transform.position);
    }

    /// <summary>
    /// プレイヤーの前方のワールド座標を取得する
    /// </summary>
    /// <returns></returns>
    public Vector3 GetForwardDirectionForGrid()
    {
        // プレイヤーの前方のグリッド方向を取得
        GridPos forward2D = GetForwardDirection(); // プレイヤーの前方のグリッド方向
        // グリッド座標からワールド座標に変換
        return new Vector3 (forward2D.x, 0.0f, -forward2D.y);
    }

    public GameObject GetTargetObject()
    {
        return m_targetObject; // ターゲットオブジェクトを取得
    }

    public Rigidbody GetRigidbody()
    {
        return m_rb; // Rigidbodyコンポーネントを取得
    }

    public void OnEvent(InteractionEvent eventID)
    {

    }

    // 削除時
    private void OnDestroy()
    {
        // ゲームインタラクションイベントのオブザーバーを解除
        GameInteractionEventMessenger.GetInstance.RemoveObserver(this);
    }
}