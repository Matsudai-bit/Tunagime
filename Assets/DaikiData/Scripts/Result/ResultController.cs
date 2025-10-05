using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ResultController : MonoBehaviour
{
    public enum State
    {
        DISPLAY_STAGE_CLEAR_TEXT = 0, // ステージクリアテキスト表示  
        DISPLAY_CLEAR_TIME,     // クリアタイム表示
        COUTN_UP_CLEAR_TIME,    // クリアタイムカウントアップ
        DISPLAY_FEELING_PIECE,  // 想いのかけら表示
        TUNAGIME_JOU_ANIMATION, // つなぎ目表示
        DISPLAY_BUTTON,            // ボタン表示
        END
    }



    [Header("======= キャラクター関連 ======= ")]


    [Header("ツナギメのキャラクターオブジェクト")]
    [SerializeField]
    public GameObject m_tunagimeObject; // つなぎ目オブジェクト

    [SerializeField]
    private Animator m_tunagimeAnimator; // つなぎ目アニメーター

    [Header("想いのかけらオブジェクト")]
    [SerializeField]
    public GameObject m_feelingPiece; // 想いのかけらオブジェクト

    [Header("======= UI関連 ======= ")]

    [Header("クリア時間")]
    [SerializeField]
    public int m_clearTime; // クリアタイム（秒）

    [Header("クリアタイム表示用テキスト")]
    [SerializeField]
    public TextMeshProUGUI m_timeText; // クリアタイム表示用テキスト

    [Header("クリアタイムオブジェクト")]
    [SerializeField]
    public GameObject m_clearTimeObject;

    [Header("クリア文字オブジェクト")]
    [SerializeField]
    public RectTransform m_clearTextTransform; // クリア文字オブジェクト

    [Header("タイトルボタンオブジェクト")]
    [SerializeField]
    private GameObject m_stageSelectButton; // ステージセレクトボタン

    [Header("======= ゲーム進行データ(監視用) ======= ")]
    [SerializeField]
    private GameProgressData m_gameProgressData;

    private float m_currentUpperTime; // 現在のクリアタイム（秒）

    private float m_upperValue; // カウントアップの値

    private State m_currentState; // 現在の状態

    private int m_idleAnimationHash;

    private float m_feelingPieceBoundTimer; // 想いのかけらバウンドタイマー

    private Vector3 m_feelingPieceStartPosition; // 想いのかけら開始位置


    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {
        // ゲーム進行データの取得
        m_gameProgressData = GameProgressManager.Instance.GameProgressData;

        // クリアタイムの取得
        m_clearTime = (int)m_gameProgressData.clearTime;

        m_clearTimeObject.SetActive(false);
        m_feelingPiece.SetActive(false);
        m_stageSelectButton.SetActive(false);
        m_idleAnimationHash = 0;

        ChangeState(State.DISPLAY_STAGE_CLEAR_TEXT);


    }

    // Update is called once per frame
    void Update()
    {
        switch (m_currentState)
        {
            case State.COUTN_UP_CLEAR_TIME:
                UpdateCountUpClearTime();
                break;
            case State.TUNAGIME_JOU_ANIMATION:
            //    UpdateTunagimeJouAnimation();
                break;
            default:
                break;
        }

        if (m_tunagimeAnimator.enabled)
        {
            UpdateTunagimeJouAnimation();

        }

        // 想いのかけらが表示されている場合、バウンドさせる
        if (m_feelingPiece.activeSelf)
        {

            m_feelingPieceBoundTimer += Time.deltaTime * 100.0f;

            if (m_feelingPieceBoundTimer >= 360.0f)
            {
                m_feelingPieceBoundTimer = 0.0f;
            }

            float sindValue = Mathf.Cos(m_feelingPieceBoundTimer * Mathf.Deg2Rad);

            m_feelingPiece.transform.localPosition = m_feelingPieceStartPosition + new Vector3(0.0f, sindValue * 0.5f, 0.0f);
        }

      

    }

    /// <summary>
    /// ステージクリアテキスト表示開始
    /// </summary>
    void StartDisplayStageClearText()
    {
        // クリア文字を表示
        m_clearTextTransform.DOMove(m_clearTextTransform.position + new Vector3(0.0f, 100.0f, 0.0f), 1.0f).SetEase(Ease.OutBounce).From().OnComplete(() =>
        {
            ChangeState(State.DISPLAY_CLEAR_TIME);
        });
    }

    /// <summary>
    /// クリアタイム表示開始
    /// </summary>
    void StartDisplayClearTime()
    {
        RectTransform clearTimeRect = m_clearTimeObject.GetComponent<RectTransform>();

        // クリア文字を表示
        clearTimeRect.DOMove(clearTimeRect.position + new Vector3(200.0f,0.0f, 0.0f), 0.5f).SetEase(Ease.OutSine).From().SetDelay(0.5f).OnComplete(() =>
        {
            ChangeState(State.COUTN_UP_CLEAR_TIME);
        });
        m_clearTimeObject.SetActive(true);
    }

    void UpdateCountUpClearTime()
    {
        m_currentUpperTime += m_upperValue * Time.deltaTime;

        if (m_currentUpperTime >= m_clearTime)
        {
            m_currentUpperTime = m_clearTime;
            ChangeState(State.DISPLAY_FEELING_PIECE);
        }

        int seconds = (int)m_currentUpperTime % 60;

        int minutes = (int)m_currentUpperTime / 60;

        m_timeText.text = string.Format("{0:00}", minutes) + " : " + string.Format("{0:00}", seconds);
    }



    /// <summary>
    /// クリアタイムカウントアップ開始
    /// </summary>
    void StartCountUpClearTime()
    {
        m_upperValue = m_clearTime / 2.0f;
    }


    void UpdateTunagimeJouAnimation()
    {
        if (m_idleAnimationHash == 0) { return; }

        var animationState = m_tunagimeAnimator.GetCurrentAnimatorStateInfo(0);

        if (animationState.fullPathHash != m_idleAnimationHash && animationState.normalizedTime >= 0.6f)
        {
            m_tunagimeAnimator.enabled = false;

            if (m_currentState != State.END)
                ChangeState(State.DISPLAY_BUTTON);
        }
    }

    /// <summary>
    /// つなぎ目アニメーション開始
    /// </summary>

    void StartTunagimeJouAnimation()
    {
        var animationState = m_tunagimeAnimator.GetCurrentAnimatorStateInfo(0);
        m_idleAnimationHash = animationState.fullPathHash;

        m_tunagimeAnimator.SetTrigger("Joy");
    }

    void StartDisplayFeelingPiece()
    {
        Transform feelingPieceTransform = m_feelingPiece.transform;

        feelingPieceTransform.localScale = Vector3.zero;

        feelingPieceTransform.DOScale(Vector3.one, 2.0f).SetEase(Ease.OutBack);


        feelingPieceTransform.DOPunchRotation(new Vector3(100f, 180f, -145f),2.0f, 8).From().SetEase(Ease.OutSine).OnComplete(() =>
        {

            ChangeState(State.TUNAGIME_JOU_ANIMATION);

        });
       

        m_feelingPiece.SetActive(true);

        m_feelingPieceBoundTimer = 90.0f;
        m_feelingPieceStartPosition = m_feelingPiece.transform.localPosition;
    }

    /// <summary>
    /// ボタン表示開始
    /// </summary>
    void StartDisplayButton()
    {
        var button = m_stageSelectButton.GetComponent<Button>();

        m_stageSelectButton.SetActive(true);

        button.OnPointerEnter(null);

        // フェードしてくる
        var image = m_stageSelectButton.GetComponent<Image>();

        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);

        image.DOFade(1.0f, 1.0f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            ChangeState(State.END);

        });
    }


    /// <summary>
    /// 状態変更
    /// </summary>
    /// <param name="newState"></param>
    void ChangeState(State newState)
    {
        if (m_currentState == State.END)
        {
            return;
        }

        Debug.Log("State Change : " + m_currentState.ToString() + " -> " + newState.ToString());

        m_currentState = newState;

        switch (m_currentState)
        {
            case State.DISPLAY_STAGE_CLEAR_TEXT:
                StartDisplayStageClearText();
                break;
            case State.DISPLAY_CLEAR_TIME:
                StartDisplayClearTime();
                break;
            case State.COUTN_UP_CLEAR_TIME:
                StartCountUpClearTime();
                break;
            case State.DISPLAY_FEELING_PIECE:
                StartDisplayFeelingPiece();
                break;
            case State.TUNAGIME_JOU_ANIMATION:
                StartTunagimeJouAnimation();
                break;
            case State.DISPLAY_BUTTON:
                StartDisplayButton();
                break;

            case State.END:
                break;

            default:
                break;
        }


    }

    /// <summary>
    /// ステージセレクトボタンが押されたときの処理
    /// </summary>
    /// <param name="inputValue"></param>
    public void OnSubmit(InputValue inputValue)
    {
        if (m_currentState != State.END)
        {
            FinishForce();
            return;
        }
        else
        {
            // ステージセレクトシーンへ遷移
            UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelectScene");
        }
     
    }

    /// <summary>
    /// 強制終了
    /// </summary>
    private void FinishForce()
    {
        ChangeState(State.END);

        m_clearTimeObject.transform.DOComplete();
        m_feelingPiece.transform.DOComplete();
        m_stageSelectButton.transform.DOComplete();
        m_clearTextTransform.DOComplete();

        // 全てのオブジェクトを表示
        m_clearTimeObject.SetActive(true);
        m_feelingPiece.SetActive(true);
        m_stageSelectButton.SetActive(true);

        // クリア時間を設定
        m_currentUpperTime = m_clearTime;
        int seconds = (int)m_currentUpperTime % 60;
        int minutes = (int)m_currentUpperTime / 60;
        m_timeText.text = string.Format("{0:00}", minutes) + " : " + string.Format("{0:00}", seconds);

        // つなぎ目アニメーションを終了させる

        if (m_tunagimeAnimator.enabled == false)
        {
            return;
        }
        var animationState = m_tunagimeAnimator.GetCurrentAnimatorStateInfo(0);
        m_idleAnimationHash = animationState.fullPathHash;

        m_tunagimeAnimator.SetTrigger("Joy");

    }
}
