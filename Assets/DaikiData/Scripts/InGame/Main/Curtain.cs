using UnityEngine;
using UnityEngine.UIElements.Experimental;
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

    private float m_elapsedTime = 0.0f; // 経過時間

    private float m_openingTime = 2.5f; // カーテンを開く時間
    private float m_closingTime = 1.5f; // カーテンを閉じる時間


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
    }

    void Start()
    {
        // マップの設定
        var stageGridData = MapData.GetInstance.GetStageGridData();

        // ステージブロックのグリッド位置を取得
        GridPos gridPos = m_stageBlock.GetGridPos();

        // 左右のフェルトブロックのグリッド位置を設定
        GridPos forwardDirection = GetForwardDirection();

        m_feltBlock_R.stageBlock.SetGridPos(gridPos + new GridPos(forwardDirection.y, forwardDirection.x));
        m_feltBlock_L.stageBlock.SetGridPos(gridPos + new GridPos(-forwardDirection.y, forwardDirection.x));

        // フェルトブロックのマップへ追加
        stageGridData.TryPlaceTileObject(m_feltBlock_R.stageBlock.GetGridPos(), m_curtainModel_R);
        stageGridData.TryPlaceTileObject(m_feltBlock_L.stageBlock.GetGridPos(), m_curtainModel_L);

        m_state = State.OPENING; // カーテンの状態を開く状態に設定
        StartOpenCurtain(); // カーテンを開く処理を開始

    }

    // Update is called once per frame
    void Update()
    {


        switch (m_state)
        {
            case State.OPENING:
                Open();
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
        // 現在のカーテンの位置Xを取得
        m_startPos_R = m_curtainModel_R.transform.localPosition;

        // カーテンの終了位置Xを設定
        m_endPos_R = m_startPos_R +  m_movementVector;

        // カーテンを開く処理


            m_state = State.OPENING;
        float backPower = 2.5f;

        m_curtainModel_R.transform.DOLocalMove(m_endPos_R, m_openingTime).SetEase(Ease.OutBack,  backPower);
        m_curtainModel_L.transform.DOLocalMove(-m_endPos_R, m_openingTime).SetEase(Ease.OutBack, backPower);

        // スケールを調整
        m_curtainModel_R.transform.DOScaleX(m_targetScaleX, m_openingTime).SetEase(Ease.OutBack,  backPower);
        m_curtainModel_L.transform.DOScaleX(m_targetScaleX, m_openingTime).SetEase(Ease.OutBack,  backPower).OnComplete(() =>
        {
            // 開いた状態に移行
            m_state = State.OPENING_FINISHED;
            m_elapsedTime = 0.0f; // 経過時間をリセット
        });

    }

    private void Open()
    {
        m_elapsedTime += Time.deltaTime;

        // カーテンを開く処理

        

        //// 移動
        //m_curtainModel_R.transform.localPosition = Vector3.Lerp(m_startPos_R, m_endPos_R, m_elapsedTime/ m_openingTime);
        //m_curtainModel_L.transform.localPosition = Vector3.Lerp(-m_startPos_R, -m_endPos_R, m_elapsedTime / m_openingTime);



        //m_curtainModel_R.transform.localScale = new Vector3(Mathf.Lerp(1.0f, m_targetScaleX, m_elapsedTime / m_openingTime), 1.0f, 1.0f);
        //m_curtainModel_L.transform.localScale = new Vector3(Mathf.Lerp(1.0f, m_targetScaleX, m_elapsedTime / m_openingTime), 1.0f, 1.0f);

        //if (m_elapsedTime >= m_openingTime)
        //{
        //    // 開いた状態に移行
        //    m_state = State.OPENING_FINISHED;
        //    m_elapsedTime = 0.0f; // 経過時間をリセット
        //}
    }


    private void StartCloseCurtain()
    {
        // カーテンを閉じる処理
        m_curtainModel_L.transform.localPosition += new Vector3(0.1f, 0, 0);
        m_curtainModel_R.transform.localPosition += new Vector3(-0.1f, 0, 0);
        // 閉じた状態に移行
        if (m_curtainModel_L.transform.localPosition.x >= 0.0f && m_curtainModel_R.transform.localPosition.x <= 0.0f)
        {
            m_state = State.CLOSING_FINISHED;
        }
    }

}
