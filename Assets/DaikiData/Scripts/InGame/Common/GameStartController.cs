using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// ゲーム開始コントローラー
/// </summary>
public class GameStartController 
    : MonoBehaviour
    , IInGameFlowEventObserver
{

    [Header("====== 開始左の文字 ======")]
    [SerializeField]
    private GameObject m_startCharacterLeft; // 開始左の文字

    [Header("====== 開始右の文字 ======")]
    [SerializeField]
    private GameObject m_startCharacterRight; // 開始右の文字


    [Header("====== ゲーム開始UIパネル ======")]
    [SerializeField]
    private GameObject m_startUIPanel; // ゲーム開始UIパネル

    private Tween m_soundTween;

    private void Awake()
    {
        // オブザーバーに登録
        InGameFlowEventMessenger.GetInstance.RegisterObserver(this);
    }

    private void OnDestroy()
    {
        // オブザーバーから登録解除
        InGameFlowEventMessenger.GetInstance.RemoveObserver(this);
    }



    private void OnEnable()
    {
        // ゲーム開始UIパネルを表示する
        m_startUIPanel.SetActive(true);

        ControlCharacter();
    }


    void ControlCharacter()
    {
        // RectTransformの位置を取得
        var leftCharacterRectTransform = m_startCharacterLeft.GetComponent<RectTransform>();
        var rightCharacterRectTransform = m_startCharacterRight.GetComponent<RectTransform>();

        // 最初にいる場所を目標座標とする
        var targetLeftCharacterPosition = leftCharacterRectTransform.position;
        var targetRightCharacterPosition = rightCharacterRectTransform.position;

        // 開始位置を設定
        leftCharacterRectTransform.position = targetLeftCharacterPosition + new Vector3(-1000.0f, 0.0f, 0.0f);
        rightCharacterRectTransform.position = targetRightCharacterPosition + new Vector3(1000.0f, 0.0f, 0.0f);



        // イージングで移動
        leftCharacterRectTransform.DOMove(targetLeftCharacterPosition, 2.0f).SetEase(Ease.OutBounce).SetDelay(0.5f);
        rightCharacterRectTransform.DOMove(targetRightCharacterPosition, 2.0f).SetEase(Ease.OutBounce).SetDelay(0.5f).OnComplete(() =>
        {
            m_startCharacterLeft.GetComponent<TextMeshProUGUI>().DOFade(0.0f, 1.0f).SetDelay(0.5f);
            m_startCharacterRight.GetComponent<TextMeshProUGUI>().DOFade(0.0f, 1.0f).SetDelay(0.5f).OnComplete(() =>
            {
                // ゲーム開始エフェクト終了イベントを通知
                InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.GAME_START_EFFECT_END);
                gameObject.SetActive(false);

                // ゲーム開始UIパネルを非表示にする
                m_startUIPanel.SetActive(false);
            });

        
        });


        // 0.5秒待ってから音を鳴らす
        m_soundTween = DOVirtual.DelayedCall(1.1f, () =>
        {
            SoundManager.GetInstance.RequestPlaying(SoundID.SE_INGAME_STARTING_GAME, false);
        });
    }

    /// <summary>
    /// イベント受信処理
    /// </summary>
    /// <param name="eventID"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void OnEvent(InGameFlowEventID eventID)
    {
        switch (eventID)
        {
            // 一時停止関連
        case InGameFlowEventID.START_PAUSE_MENU:
            PauseDOTween();
            break;

        case InGameFlowEventID.END_PAUSE_MENU:
            // 一時停止を解除する
            ResumeDOTween();

            break;
        }
    }

    /// <summary>
    /// 一時停止
    /// </summary>
    private void PauseDOTween()
    {
        // RectTransformの位置を取得
        m_startCharacterLeft.GetComponent<RectTransform>()  .DOPause();
        m_startCharacterRight.GetComponent<RectTransform>() .DOPause();

        m_startCharacterLeft.GetComponent<TextMeshProUGUI>().DOPause();
        m_startCharacterRight.GetComponent<TextMeshProUGUI>().DOPause();

        m_soundTween.Pause();
    }

    /// <summary>
    /// 一時停止解除
    /// </summary>
    private void ResumeDOTween()
    {
        // RectTransformの位置を取得
        m_startCharacterLeft.GetComponent<RectTransform>().DOPlay();
        m_startCharacterRight.GetComponent<RectTransform>().DOPlay();
        m_startCharacterLeft.GetComponent<TextMeshProUGUI>().DOPlay();
        m_startCharacterRight.GetComponent<TextMeshProUGUI>().DOPlay();

        m_soundTween.Play();
    }
}


