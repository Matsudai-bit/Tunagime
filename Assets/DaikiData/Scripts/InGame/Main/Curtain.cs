using UnityEngine;
using DG.Tweening;	//DOTweenを使うときはこのusingを入れる


/// <summary>
/// カーテン
/// </summary>
public class Curtain : MonoBehaviour
{
    /// <summary>
    /// カーテンの状態を表す列挙型
    /// </summary>
    enum State
    {
        NONE = 0, // 何もしない状態
       
        OPENING, // 開いている状態
        CLOSING, // 閉じている状態

        OPENING_FINISHED, // 開いた状態
        CLOSING_FINISHED, // 閉じた状態
    }

    [SerializeField] private GameObject m_curtainModel_L = null; // カーテンモデル（左）
    [SerializeField] private GameObject m_curtainModel_R = null; // カーテンモデル（右）

    [SerializeField] private FeltBlock m_feltBlock_L = null; // フェルトブロック（カーテンの左にあるブロック）
    [SerializeField] private FeltBlock m_feltBlock_R = null; // フェルトブロック（カーテンの右にあるブロック）


    private StageBlock m_stageBlock = null; // ステージブロック

    private State m_state = State.OPENING; // カーテンの状態

    private Vector3 m_startPos_R ; // カーテンの開始位置（X軸方向）
    private Vector3 m_endPos_R;    // カーテンの開始位置（X軸方向）

    private float m_targetScaleX = 0.5f; // カーテンの目標スケール（X軸方向）

    private Vector3 m_movementVector = new Vector3(0.22f, 0.0f, 0.0f); // カーテンを開く/閉じる際の移動量（X軸方向）

    [SerializeField]
    private float m_openingTime = 2.5f; // カーテンを開く時間
    [SerializeField]
    private float m_closingTime = 1.5f; // カーテンを閉じる時間

    private Collider m_collider; // カーテンのコライダー

    private EmotionCurrent m_emotionCurrent;  // 想いの種類


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        // ステージブロックを取得
        m_stageBlock = GetComponentInParent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("Curtain: StageBlock not found in parent.");
        }
        // カーテンモデルの初期化
        if (m_curtainModel_L == null || m_curtainModel_R == null)
        {
            Debug.LogError("Curtain: Curtain models are not assigned.");
        }

        // フェルトブロックの初期化
        if (m_feltBlock_L == null || m_feltBlock_R == null)
        {
            Debug.LogError("Curtain: FeltBlocks are not assigned.");
        }

        // カーテンのコライダーを取得
        m_collider = GetComponent<Collider>();

        // 想いの種類の取得
        m_emotionCurrent = GetComponentInParent<EmotionCurrent>();
    }

    void Start()
    {

        // 現在のカーテンの位置Xを取得
        m_startPos_R = m_curtainModel_R.transform.localPosition;
        // カーテンの終了位置Xを設定
        m_endPos_R = m_startPos_R + m_movementVector;

        // マップの設定
        var stageGridData = MapData.GetInstance.GetStageGridData();

        // ステージブロックのグリッド位置を取得
        GridPos gridPos = m_stageBlock.GetGridPos();

        // 左右のフェルトブロックのグリッド位置を設定
        GridPos forwardDirection = GetForwardDirection();

        m_feltBlock_R.stageBlock.SetGridPos(gridPos + new GridPos(forwardDirection.y, forwardDirection.x));
        m_feltBlock_L.stageBlock.SetGridPos(gridPos + new GridPos(-forwardDirection.y, -forwardDirection.x));

        // フェルトブロックのマップへ追加
        stageGridData.TryPlaceTileObject(m_feltBlock_R.stageBlock.GetGridPos(), m_feltBlock_R.gameObject);
        stageGridData.TryPlaceTileObject(m_feltBlock_L.stageBlock.GetGridPos(), m_feltBlock_L.gameObject);

        m_state = State.CLOSING_FINISHED; // 初期状態を閉じた状態に設定

        TryChangeState();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_state == State.OPENING_FINISHED || m_state == State.CLOSING_FINISHED)
        {
            TryChangeState();

        }

        switch (m_state)
        {
            case State.OPENING:
                break;
            case State.CLOSING:
                // カーテンを閉じる処理
                
                break;
            case State.OPENING_FINISHED:
                // 開いた状態の処理
                break;
            case State.CLOSING_FINISHED:
                // 閉じた状態の処理
                break;
        }
    }

    /// <summary>
    /// 前方のグリッド方向を取得
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


    private void StartOpenCurtain()
    {

        m_collider.enabled = false; // カーテンのコライダーを無効化
        var stageGridData = MapData.GetInstance.GetStageGridData();
        // ステージブロックのグリッド位置を取得して、カーテンのグリッドデータから削除
        stageGridData.RemoveGridDataGameObject(m_stageBlock.GetGridPos()); // ステージブロックのグリッドデータから削除


        // カーテンを開く処理

        float backPower = 2.5f;

        m_curtainModel_R.transform.DOLocalMove(m_endPos_R, m_openingTime).SetEase(Ease.OutBack,  backPower);
        m_curtainModel_L.transform.DOLocalMove(new Vector3 (-m_endPos_R.x, m_endPos_R.y, m_endPos_R.z), m_openingTime).SetEase(Ease.OutBack, backPower);

        // スケールを調整
        m_curtainModel_R.transform.DOScaleX(m_targetScaleX, m_openingTime).SetEase(Ease.OutBack,  backPower);
        m_curtainModel_L.transform.DOScaleX(m_targetScaleX, m_openingTime).SetEase(Ease.OutBack,  backPower).OnComplete(() =>
        {
            // 開いた状態に移行
            ChangeState(State.OPENING_FINISHED);
        });

    }

    private void StartCloseCurtain()
    {
        m_collider.enabled = true; // カーテンのコライダーを無効化

        var stageGridData = MapData.GetInstance.GetStageGridData();
        // ステージブロックのグリッド位置を取得して、カーテンのグリッドデータに追加
        stageGridData.TryPlaceTileObject(m_stageBlock.GetGridPos(), gameObject);


        // カーテンを閉じる処理
        float backPower = 1.0f;

        m_curtainModel_R.transform.DOLocalMove(m_startPos_R, m_closingTime).SetEase(Ease.OutBack, backPower);
        m_curtainModel_L.transform.DOLocalMove(new Vector3(-m_startPos_R.x, m_startPos_R.y, m_startPos_R.z), m_closingTime).SetEase(Ease.OutBack, backPower);

        // スケールを調整
        m_curtainModel_R.transform.DOScaleX(1.0f, m_closingTime).SetEase(Ease.OutBack, backPower);
        m_curtainModel_L.transform.DOScaleX(1.0f, m_closingTime).SetEase(Ease.OutBack, backPower).OnComplete(() =>
        {
            // 閉じた状態に移行
            ChangeState(State.CLOSING_FINISHED);
        });
    }

    private void TryChangeState()
    {

        // 型の繋がりモニター
        var slotStateMonitor = FeelingSlotStateMonitor.GetInstance;
        if (slotStateMonitor.IsConnected(m_emotionCurrent.CurrentType))
        {
            // カーテンの状態を開く状態に設定
            if (m_state == State.CLOSING_FINISHED)
            {
                ChangeState(State.OPENING);
            }
        }
        else
        {
            // カーテンの状態を閉じる状態に設定
            if (m_state == State.OPENING_FINISHED)
            {
                if (CanClose())
                    ChangeState(State.CLOSING);
            }
        }
    }

    private void ChangeState(State newState)
    {
        // カーテンの状態を変更
        m_state = newState;
        switch (m_state)
        {
            case State.OPENING:
                StartOpenCurtain();
                break;
            case State.CLOSING:
                StartCloseCurtain();
                break;
            case State.OPENING_FINISHED:
                // 開いた状態の処理
                break;
            case State.CLOSING_FINISHED:
                // 閉じた状態の処理

                break;
        }
    }


    private bool CanClose()
    {
        // 左側にレイを飛ばして自分以外のオブジェクトがあればできない
        RaycastHit hit;
        if (Physics.Raycast(m_curtainModel_L.transform.position, -m_curtainModel_L.transform.right, out hit, 0.5f))
        {
            
            if (hit.collider.gameObject != null && hit.collider.gameObject != gameObject)
            {
                return false; // 他のフェルトブロックがある場合は閉じられない
            }
        }
        return true;
    }

    public void SetMaterial(Material material)
    {
        if (m_curtainModel_L != null && m_curtainModel_R != null)
        {
            m_curtainModel_L.GetComponent<MeshRenderer>().material = material;
            m_curtainModel_R.GetComponent<MeshRenderer>().material = material;
        }
        else
        {
            Debug.LogError("Curtain: Curtain models are not assigned.");
        }
    }
}
