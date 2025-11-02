using DG.Tweening;
using UnityEngine;

public class ClearController : MonoBehaviour
{
    [Header("====== ゲームの親 ======")]
    [SerializeField] private Transform m_gameParent;    // ゲームの親オブジェクト

    [Header("====== クリア時に消すオブジェクトのトランスフォーム ======")]
    [SerializeField] private GameObject m_clearOffParent; // クリア時に消すオブジェクト

    [Header("====== クリアUIグループ ======")]
    [SerializeField] private GameObject m_clearUIGroup; // クリアUIグループ

    [Header("====== プレイヤー ======")]
    [SerializeField]　private GameObject m_player; // プレイヤーオブジェクト

    [Header("====== 想いのカケラの動き ======")]
    [SerializeField] private Vector3 m_feelingPieceUpVelocity   = new Vector3(0.0f, 5.0f, 0.0f); // 想いのカケラの位置
    [SerializeField] private Vector3 m_feelingPieceDownVelocity = new Vector3(1.0f, -3.0f, 0.0f); // 想いのカケラの位置



    public GameObject m_opaqueScreen; // クリア時に表示する不透明な画面

    private GameObject m_stageObject; // ステージオブジェクト

    private GameObject m_feelingPiece; // 感情ピースオブジェクト

    bool m_isClearEffectStarted = false; // クリアエフェクトが開始されたかどうか

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        // ゲームクリアエフェクト開始イベントを通知
        InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.GAME_CLEAR_EFFECT_START);

        var map = MapData.GetInstance;

        m_clearUIGroup.SetActive(true); // クリアUIグループを表示する

        // クリアUIグループを非表示にする
        m_opaqueScreen = Instantiate(m_opaqueScreen, m_clearUIGroup.transform);

        // クリア時に消すオブジェクトを最前面に移動
        m_opaqueScreen.transform.SetAsLastSibling();

        // Imageコンポーネントを取得して透明度を設定
        var opaqueImage = m_opaqueScreen.GetComponent<UnityEngine.UI.Image>();

        opaqueImage.color = new Color(1, 1, 1, 0); // 初期状態は透明

        // フェードインしてからフェードアウトする
        opaqueImage.DOFade(1.0f, 2.0f).SetEase(Ease.OutQuint).OnComplete(() =>
        {
            OnFadeInComplete(opaqueImage, map);
        });


    }

    private void OnFadeInComplete(UnityEngine.UI.Image opaqueImage, MapData map)
    {
        // クリア時に消すオブジェクトを非表示にする
        m_clearOffParent.SetActive(false);

        // フェードアウトが完了した後の処理
        opaqueImage.DOFade(0.0f, 1.0f).SetEase(Ease.OutCirc).OnComplete(OnFadeOutComplete);

        if (map.GetStageObject())
        {
            // ステージオブジェクトを生成する
            m_stageObject = Instantiate(map.GetStageObject(), m_gameParent);
        }
 
    }

    private void OnFadeOutComplete()
    {
        // ゲームクリアエフェクト開始イベントを通知
        InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.GAME_CLEAR_EFFECT_END);

        // フェードアウトが完了した後の処理
        StartSpawningFeelingPiece();
    }

    /// <summary>
    /// 感情ピースのスポーンを開始する
    /// </summary>
    private void StartSpawningFeelingPiece()
    {


        var map = MapData.GetInstance;

        m_feelingPiece = Instantiate(map.GetFeelingPiece(), m_gameParent);

        Vector3 stageObjectPosition = map.GetStageCenterPos();
        if (m_stageObject)
        {
            stageObjectPosition = m_stageObject.transform.position;
        }

        // 感情ピースの初期位置をステージオブジェクトの少し下に設定
        m_feelingPiece.transform.position = stageObjectPosition + new Vector3(0.0f, -1.0f, 0.0f);

        // 感情ピースを上に移動させるアニメーション
        m_feelingPiece.transform.DOBlendableMoveBy(m_feelingPieceUpVelocity, 1.0f).SetEase(Ease.InBack).SetDelay(0.5f).OnComplete(() =>
        {
            m_feelingPiece.transform.DOBlendableMoveBy(m_feelingPieceDownVelocity, 1.0f).SetEase(Ease.OutBack).SetDelay(0.3f).OnComplete(() =>
            {
                // 感情ピースのスポーン開始を通知
                InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.GOING_GET_FEELING_PIECE_START);

                m_isClearEffectStarted = true; // クリアエフェクトが開始された
            });

        });
    }

    private void Update()
    {
        // クリアエフェクトが開始されていない場合は何もしない
        if (m_isClearEffectStarted)
        {
            var distance = Vector3.Distance(m_player.transform.position, m_feelingPiece.transform.position);

            if (distance < 1.1f)
            {
                // プレイヤーが感情ピースに近づいた場合、感情ピースを消す
                Destroy(m_feelingPiece);
                // 感情ピースのスポーン完了を通知
                InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.GOING_GET_FEELING_PIECE_END);
                m_isClearEffectStarted = false; // クリアエフェクトが終了した
            }
            return;
        }
    }
}
