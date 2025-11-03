using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// 入力ガイド切り替えクラス
/// </summary>
public class InputGuideSwitcher : MonoBehaviour
{
    [Serializable]
    class InputGuideData
    {
        public string label1 = "キーボード入力ガイド"; // ラベル1
        public Sprite keyboardGuideSprite; // キーボード入力ガイドスプライト

        public string label2 = "ゲームパッド入力ガイド"; // ラベル2
        public Sprite gamepadGuideSprite;  // ゲームパッド入力ガイドスプライト
    }

    [Header("コンポーネント ======================================-")]

    [Header("プレイヤー入力")]
    [SerializeField]
    private PlayerInput m_playerInput; // プレイヤー入力

    [Header("入力ガイドデータ")]
    [SerializeField]
    private InputGuideData m_inputGuideData; // 入力ガイドデータ

    private Image m_image; // ガイド画像

    void Awake()
    {
        m_image = GetComponent<Image>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 現在の制御スキームを取得
        var currentControlScheme = m_playerInput.currentControlScheme;
        if (currentControlScheme == "Keyboard&Mouse")
        {
            // キーボード入力ガイドに切り替え
            SwitchToKeyboardGuide();
        }
        else if (currentControlScheme == "Gamepad")
        {
            // ゲームパッド入力ガイドに切り替え
            SwitchToGamepadGuide();
        }
    }

    /// <summary>
    /// キーボード入力ガイドに切り替え
    /// </summary>
    void SwitchToKeyboardGuide()
    {
        m_image.sprite = m_inputGuideData.keyboardGuideSprite;
    }

    /// <summary>
    /// ゲームパッド入力ガイドに切り替え
    /// </summary>
    void SwitchToGamepadGuide()
    {
        m_image.sprite = m_inputGuideData.gamepadGuideSprite;
    }

}
