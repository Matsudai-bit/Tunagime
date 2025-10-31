using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ポーズメニューのマニュアルコントローラー
/// </summary>
public class PauseMenuManualController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_manualIcons = new(); // マニュアルページのリスト

    /// <summary>
    /// ポーズメニューが開かれたときの処理
    /// </summary>
    public void OnOpenPause()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < m_manualIcons.Count; i++)
        {
            var icon = m_manualIcons[i];
            // アイコンを右にスライドさせてから元の位置に戻すアニメーション
            Vector2 targetIconPosition = icon.GetComponent<RectTransform>().anchoredPosition;

            // 最初に右に移動させる
            icon.GetComponent<RectTransform>().anchoredPosition += new Vector2(300.0f, 0);

            // 元の位置に戻すアニメーション
            icon.GetComponent<RectTransform>().DOAnchorPosX(targetIconPosition.x, 0.5f).SetEase(Ease.OutCubic).SetDelay(i * 0.1f);

            icon.gameObject.SetActive(true);
        }
    }
}
