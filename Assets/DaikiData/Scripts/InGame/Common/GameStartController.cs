using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// ゲーム開始コントローラー
/// </summary>
public class GameStartController : MonoBehaviour
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


    private void OnEnable()
    {
        // ゲーム開始UIパネルを表示する
        m_startUIPanel.SetActive(true);

        // RectTransformの位置を取得
        var leftCharacterRectTransform = m_startCharacterLeft.GetComponent<RectTransform>();
        var rightCharacterRectTransform = m_startCharacterRight.GetComponent<RectTransform>();

        // 最初にいる場所を目標座標とする
        var targetLeftCharacterPosition = leftCharacterRectTransform.position;
        var targetRightCharacterPosition= rightCharacterRectTransform.position;

        // 開始位置を設定
        leftCharacterRectTransform.position = targetLeftCharacterPosition + new Vector3(-500.0f, 0.0f, 0.0f);
        rightCharacterRectTransform.position = targetRightCharacterPosition + new Vector3(500.0f, 0.0f, 0.0f);


        // イージングで移動
        leftCharacterRectTransform.DOMove(targetLeftCharacterPosition, 2.0f).SetEase(Ease.OutBounce);
        rightCharacterRectTransform.DOMove(targetRightCharacterPosition, 2.0f).SetEase(Ease.OutBounce).OnComplete(() =>
        {

            // ゲーム開始エフェクト終了イベントを通知
            InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.GAME_START_EFFECT_END);
            gameObject.SetActive(false);

            // ゲーム開始UIパネルを非表示にする
            m_startUIPanel.SetActive(false);

        });


    }
}
