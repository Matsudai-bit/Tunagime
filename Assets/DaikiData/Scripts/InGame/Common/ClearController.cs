using DG.Tweening;
using UnityEngine;

public class ClearController : MonoBehaviour
{
    [Header("====== クリア時に消すオブジェクトのトランスフォーム ======")]
    [SerializeField] private GameObject m_clearOffParent; // クリア時に消すオブジェクト

    [Header("====== クリアUIグループ ======")]
    [SerializeField] private GameObject m_clearUIGroup; // クリアUIグループ

    public GameObject m_opaqueScreen; // クリア時に表示する不透明な画面

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        m_clearUIGroup.SetActive(true); // クリアUIグループを表示する

        // クリアUIグループを非表示にする
        m_opaqueScreen = Instantiate(m_opaqueScreen, m_clearUIGroup.transform);

        m_opaqueScreen.transform.SetAsLastSibling(); // クリア時に消すオブジェクトを最前面に移動

        var opaqueImage = m_opaqueScreen.GetComponent<UnityEngine.UI.Image>();

        opaqueImage.color = new Color(1, 1, 1, 0); // 初期状態は透明

        // フェードインしてからフェードアウトする
        opaqueImage.DOFade(1.0f, 5.0f).SetEase(Ease.OutQuint).OnComplete(() =>
        {
            // クリア時に消すオブジェクトを非表示にする
            m_clearOffParent.SetActive(false); 
            // フェードアウトが完了した後の処理
            opaqueImage.DOFade(0.0f, 2.0f).SetEase(Ease.OutCirc);
        });


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
