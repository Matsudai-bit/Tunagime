using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ポーズメニューのマニュアルコントローラー
/// </summary>
public class PauseMenuManualController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_manualIcons = new(); // マニュアルページのリスト

    private float m_initialPositionX; // 初期位置X
    private void Awake()
    {
        if (m_manualIcons.Count == 0) return;

        var icon = m_manualIcons[0];
        // アイコンを右にスライドさせてから元の位置に戻すアニメーション
        m_initialPositionX = icon.GetComponent<RectTransform>().anchoredPosition.x;
    }

    /// <summary>
    /// ポーズメニューが開かれたときの処理
    /// </summary>
    public void OnOpenPause(float duration)
    {
        gameObject.SetActive(true);

        for (int i = 0; i < m_manualIcons.Count; i++)
        {
            var icon = m_manualIcons[i];
          
            var rectTransform = icon.GetComponent<RectTransform>();

            rectTransform.anchoredPosition = new Vector2(m_initialPositionX, rectTransform.anchoredPosition.y);

            // 最初に右に移動させる
            rectTransform.anchoredPosition += new Vector2(300.0f, 0);

            // 元の位置に戻すアニメーション
            rectTransform.DOAnchorPosX(m_initialPositionX, duration).SetEase(Ease.OutCubic).SetDelay(i * 0.1f);

            icon.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// ポーズメニューが閉じられたときの処理
    /// </summary>
    public void OnClosePause(float duration)
    {

        // イージングを使って右にスライドさせるアニメーション
        for (int i = 0; i < m_manualIcons.Count; i++)
        {
            var icon = m_manualIcons[i];
            Vector2 targetIconPosition = icon.GetComponent<RectTransform>().anchoredPosition + new Vector2(300.0f, 0);
            // 右にスライドさせるアニメーション
            icon.GetComponent<RectTransform>().DOAnchorPosX(targetIconPosition.x, duration - 0.1f * m_manualIcons.Count).SetEase(Ease.InCubic).SetDelay(i * 0.1f).OnComplete(() =>
            {
                icon.gameObject.SetActive(false);

                if (i == m_manualIcons.Count - 1)
                {
                    // 最後のアイコンのアニメーションが終わったら非表示にする
                    gameObject.SetActive(false);
                }
            });
        }

    }
}
