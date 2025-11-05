using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

/// <summary>
/// ストーリーシーン全体を管理するクラス
/// </summary>
public class StoryDirector : MonoBehaviour
{
    #region クラス・構造体

    [Serializable]
    class InspectorStruct
    {
        public string labelName = "デバックモードを有効にするかどうか";    // ラベル名
        public bool   debugMode;    // デバッグモード

        public string label2Name =  "表示するワールドID"; // ストーリーイラストウィンドウコントローラープリファブのパス
        public NarrativeContext.NarrativeState state; // ストーリーイラストのワールドID
    }

    /// <summary>
    /// 再生状態
    /// </summary>
    enum PlayingState
    {
        IllustrationPlaying, // イラスト再生中
        VideoPlaying         // ビデオ再生中
    }


    #endregion


    [Header("物語の状況クラスインスタンス(デバック用)")]
    [SerializeField]
    private NarrativeContext m_narrativeContext = NarrativeContext.GetInstance; // 物語の状況クラスインスタンス

    [Header("設定")]
    [SerializeField]
    private InspectorStruct m_inspector = new InspectorStruct();

    [Header("ストーリーイラストウィンドウコントローラー")]
    [SerializeField]
    private StoryIllustrationWindowController m_storyIllustrationWindowController;

    [Header("BGMプレイヤー")]
    [SerializeField]
    private BgmPlayer m_bgmPlayer; // BGMプレイヤー

    [Header("ストーリービデオパネル")]
    [SerializeField]
    private GameObject m_storyVideoPanel; // ストーリービデオパネル

    [Header("ビデオプレイヤー")]
    [SerializeField]
    private VideoPlayer m_storyVideoPlayer; // ビデオプレイヤー

    [Header("ストーリーイラストデータ")]
    [SerializeField]
    private StoryRenderingData m_storyRenderingData;

    [Header("シーン遷移演出(フェードアウト)")]
    [SerializeField]
    private SceneTransitionEffect m_sceneTransitionFadeOut;

    private PlayerInput m_playerInput; // プレイヤー入力

    private PlayingState m_playingState; // 再生状態


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var storyRenderingDict = m_storyRenderingData.GetStoryRenderingDict();

        NarrativeContext.NarrativeState requestState;

        // デバッグモード判定
        if (m_inspector.debugMode)
        {
            // デバッグモード時はインスペクターで指定したワールドIDを使用
            requestState = m_inspector.state;
        }
        else
        {
            // 通常時はゲーム進行状況からワールドIDを取得
            requestState = NarrativeContext.GetInstance.GetNarrativeState;
        }

        if (requestState == NarrativeContext.NarrativeState.INTRODUCTION || requestState == NarrativeContext.NarrativeState.ENDING)
        {
            m_storyVideoPanel.SetActive(true);
            m_storyVideoPlayer.gameObject.SetActive(true);
            m_storyVideoPlayer.clip = storyRenderingDict[requestState].videoClip;
            m_storyVideoPlayer.Play();
            m_playingState = PlayingState.VideoPlaying;
        }
        else
        {
            m_storyIllustrationWindowController.gameObject.SetActive(true);
            m_bgmPlayer.PlayBGM();
            
            // ストーリーイラストウィンドウコントローラー初期化
            m_storyIllustrationWindowController.Initialize(storyRenderingDict[requestState].illustrationSprites);

            // ストーリー開始
            m_storyIllustrationWindowController.StartStory();
            m_playingState = PlayingState.IllustrationPlaying;
        }



            // プレイヤー入力取得
         m_playerInput = GetComponent<PlayerInput>();
        m_playerInput.actions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        m_playerInput.actions.Enable();

        if (m_playingState == PlayingState.VideoPlaying)
        {
            // ビデオ再生終了判定
            if (m_storyVideoPlayer.isPlaying == false)
            {
                // シーン遷移演出開始
                m_sceneTransitionFadeOut.StartTransition(() =>
                {
                    // 再生終了コールバック呼び出し
                    if (m_narrativeContext.OnFinishScene != null)
                    {
                        m_narrativeContext.OnFinishScene();
                    }
                });


            }
        }

    }

    /// <summary>
    /// ストーリーシーン終了時処理
    /// </summary>
    public void OnExitStoryScene(InputAction.CallbackContext context)
    {
        if (!context.performed){ return; }

        if (m_storyIllustrationWindowController.CanEndStory() == false) { return; }

        // シーン遷移演出開始
        m_sceneTransitionFadeOut.StartTransition(() =>
        {
            // 再生終了コールバック呼び出し
            if (m_narrativeContext.OnFinishScene != null)
            {
                m_narrativeContext.OnFinishScene();
            }
        });



    }

}
