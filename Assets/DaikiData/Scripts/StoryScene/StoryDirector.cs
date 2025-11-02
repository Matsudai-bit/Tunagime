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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene("StoryScene");
        }
    }

}
