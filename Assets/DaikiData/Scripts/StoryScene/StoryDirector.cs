using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// ストーリーシーン全体を管理するクラス
/// </summary>
public class StoryDirector : MonoBehaviour
{
    #region インスペクター用

    [Serializable]
    class InspectorStruct
    {
        public string labelName = "デバックモードを有効にするかどうか";    // ラベル名
        public bool   debugMode;    // デバッグモード

        public string label2Name =  "表示するワールドID"; // ストーリーイラストウィンドウコントローラープリファブのパス
        public WorldID storyWorldID; // ストーリーイラストのワールドID
    }
    #endregion


    [Header("設定")]
    [SerializeField]
    private InspectorStruct m_inspector = new InspectorStruct();

    [Header("ストーリーイラストウィンドウコントローラー")]
    [SerializeField]
    private StoryIllustrationWindowController m_storyIllustrationWindowController;



    [Header("ストーリーイラストデータ")]
    [SerializeField]
    private StoryIllustrationData m_storyIllustrationData;

    [Header("シーン遷移演出(フェードアウト)")]
    [SerializeField]
    private SceneTransitionEffect m_sceneTransitionFadeOut;

    private PlayerInput m_playerInput; // プレイヤー入力

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var storyIllustrationDict = m_storyIllustrationData.GetStoryIllustrationDict();

        WorldID currentWorldID;

        // デバッグモード判定
        if (m_inspector.debugMode)
        {
            // デバッグモード時はインスペクターで指定したワールドIDを使用
            currentWorldID = m_inspector.storyWorldID;
        }
        else
        {
            // 通常時はゲーム進行状況からワールドIDを取得
            currentWorldID = GameProgressManager.Instance.GameProgressData.worldID;
        }

        // ストーリーイラストウィンドウコントローラー初期化
        m_storyIllustrationWindowController.Initialize(storyIllustrationDict[currentWorldID]);
        // ストーリー開始
        m_storyIllustrationWindowController.StartStory();

        // プレイヤー入力取得
        m_playerInput = GetComponent<PlayerInput>();
        m_playerInput.actions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        m_playerInput.actions.Enable();

    }

    /// <summary>
    /// ストーリーシーン終了時処理
    /// </summary>
    public void OnExitStoryScene(InputAction.CallbackContext context)
    {
        if (!context.performed){ return; }

        if (m_storyIllustrationWindowController.CanEndStory() == false) { return; }

        m_sceneTransitionFadeOut.StartTransition(() =>
        {
            SceneManager.LoadScene("StageSelectScene");
        });

    }

}
