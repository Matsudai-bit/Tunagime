using Cysharp.Threading.Tasks;
using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class GameDirector : MonoBehaviour, IInGameFlowEventObserver
{
    bool m_isFirstUpdate = true;
    bool m_isSecondUpdate = true;

    [Header("デバックモードを有効にする")]
    [SerializeField]
    private bool m_isDebugMode = false;

    [Header("開始状態")]
    [SerializeField]
    InGameFlowEventID m_startState ;

    [Header("ゲーム開始UIパネル")]
    [SerializeField] GameObject m_gameStartUIPanel;

    [Header("クリアUIパネル")]
    [SerializeField] GameObject m_clearUIPanel;

    [Header("プレイヤーの入力システム")]
    [SerializeField] PlayerInput m_playerInput;

    [Header("ゲーム時間")]
    [SerializeField]
    private float m_gameTime = 0.0f;

    [Header("ステージマネージャー")]
    [SerializeField] private StageManager m_stageManager;

    [Header("エフェクトコントローラー")]
    [SerializeField] private EffectController m_effectController;

    [Header("チュートリアコントローラ")]
    [SerializeField] private TutorialWindowController m_tutorialController;

    [Header("ポーズウィンドウ")]
    [SerializeField] private PauseWindowController m_pauseWindowController;


    [Header("画面遷移(フェードイン)イメージ")]
    [SerializeField]
    private SceneTransitionEffect m_sceneTransitionFadeInEffect; // 画面遷移(フェードイン)イメージ

    [Header("画面遷移(フェードアウト)イメージ")]
    [SerializeField]
    private SceneTransitionEffect m_sceneTransitionFadeOutEffect; // 画面遷移(フェードアウト)イメージ

    private bool m_isGameClear = false;

    private InGameFlowEventID m_currentEventID ;

    

    private Action m_onStartUpdating ; // 更新開始時の処理

    void Awake()
    {
        m_playerInput = GetComponent<PlayerInput>();
        if (m_isDebugMode == false)
        {
            m_startState = GameProgressManager.Instance.GameProgressData.startState;
        }
        // 初期状態を設定
        m_currentEventID = m_startState;

        

        // ゲームフロウイベントの登録
        InGameFlowEventMessenger.GetInstance.RegisterObserver(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      

        SoundManager.GetInstance.RequestAllStopping();

        // フェードインエフェクトの開始
        m_sceneTransitionFadeInEffect.StartTransition(
            () => { });

        // 
        MapData.GetInstance.SetClearConditionChecker(GetComponent<ClearConditionChecker>());
        // 60fpsに設定
        Application.targetFrameRate = 60;

        if (m_startState != InGameFlowEventID.ZOOM_OUT_PLAYER_START)
        {
            // ゲーム時間を前のシーンから引き継ぐ
            m_gameTime = GameProgressManager.Instance.GameProgressData.clearTime;
        }
     

        var map = MapData.GetInstance;
        // マップデータの初期化
        map.Initialize();

        // エフェクトコントローラーにボリュームプロファイルを設定
        m_effectController.SetVolumeProfile(map.MapSetting.volumeProfile);
        // ステージパーティクルの設定
        m_effectController.PlayParticle(map.MapSetting.stageEffectParticlePrefab);

        // BGMの再生
        SoundManager.GetInstance.PlayBGM(map.MapSetting.bgmID);

        // ステージの生成
        m_stageManager.Generate(map.GetStageGenerator(), this);

        // 前のシーンがゲームプレイだったかどうかをリセットする
        GameProgressManager.Instance.GameProgressData.isPrevSceneGamePlayed = true;
        // 前のシーンがゲームクリアだったかどうかをリセットする
        GameProgressManager.Instance.GameProgressData.isPrevSceneGameCleared = false;


        m_isFirstUpdate = true;
    }

    void OnDestroy()
    {
        // ゲームフロウイベントの登録解除
        InGameFlowEventMessenger.GetInstance.RemoveObserver(this);
    }   

    // Update is called once per frame
    void Update()
    {
        if (m_isFirstUpdate)
        {
            m_isFirstUpdate = false;
            Debug.Log("ゲーム開始通知 現在の状態 : " + m_startState);

            // ゲーム開始のイベントを通知
            InGameFlowEventMessenger.GetInstance.Notify(m_startState);

            return;
        }
           

        if (m_isSecondUpdate)
        {
            if (m_onStartUpdating != null)
            {
                m_onStartUpdating();
            }

            m_isSecondUpdate = false;

            return;
        }


        


        if (m_isGameClear == false)
        {
            // ゲーム時間を更新
            m_gameTime += Time.deltaTime;
        }

    }

    // ゲームがクリアした時に呼ばれる
    public void OnGameClear()
    {
        if (m_isGameClear) { return; }
        
        // クリアUIを表示する処理
        Debug.Log("ゲームクリア！");
        m_isGameClear = true;
        SoundManager.GetInstance.RequestPlaying(SoundID.SE_INGAME_CLEAR, false);

        DOVirtual.DelayedCall(1.0f, () =>
        {
            // ここにゲームクリアの処理を追加
            GameProgressManager.Instance.GameProgressData.clearTime = m_gameTime;


            // クリアUIのパネルを表示
            m_clearUIPanel.SetActive(true);
        });
    }

    /// <summary>
    /// ステージセレクトシーンに遷移
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    async UniTask WaitAndLoadStageSelectScene(int waitTime)
    {
        await UniTask.Delay(waitTime);
       // InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.GAME_END);
    }

    public void LoadResultScene()
    {
        SoundManager.GetInstance.RequestAllStopping();
        SceneTransitionManager.GetInstance.TransitionToScene("ResultScene", m_sceneTransitionFadeOutEffect);


    }

    /// <summary>
    /// ゲームフロウイベント
    /// </summary>
    /// <param name="eventID"></param>
    public void OnEvent(InGameFlowEventID eventID)
    {
        if (eventID != InGameFlowEventID.END_PAUSE_MENU &&
            eventID != InGameFlowEventID.START_PAUSE_MENU&&
            eventID != InGameFlowEventID.TUTORIAL_END &&
            eventID != InGameFlowEventID.TUTORIAL_START &&
            eventID != InGameFlowEventID.START_PAUSE_MENU)
        {
            m_currentEventID = eventID;

        }
      

        switch (eventID)
        {
            case InGameFlowEventID.ZOOM_OUT_PLAYER_START:
                Debug.Log("カメラズームの開始 ============================================================================================");

                // 更新開始時の処理を設定
                m_onStartUpdating = () =>
                {
                    StopPlayerInput();
                };

                break;

            // ズームアウト終了イベント
            case InGameFlowEventID.ZOOM_OUT_PLAYER_END:
                Debug.Log("カメラズームの終了 ============================================================================================");
                // イントロシーケンス開始イベントを通知
                InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.INTRO_SEQUENCE_START);
                break;

            // イントロシーケンス終了イベント
            case InGameFlowEventID.INTRO_SEQUENCE_END:
                // ゲーム開始イベントを通知
                InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.GAME_START_EFFECT_START);
                //StartPlayerInput();
                break;
            case InGameFlowEventID.GAME_START_EFFECT_START:
                Debug.Log("ゲーム開始演出の開始 ============================================================================================");

                // ゲーム開始UIパネルを表示する
                m_gameStartUIPanel.SetActive(true);
                break;

            case InGameFlowEventID.GAME_START_EFFECT_END:
                Debug.Log("ゲーム開始演出の終了 ============================================================================================");

                // ゲームプレイ開始イベントを通知
                InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.GAME_PLAYING_START);
                break;
            case InGameFlowEventID.GAME_PLAYING_START:
                Debug.Log("ゲームプレイの開始 ============================================================================================");

                if (m_isSecondUpdate)
                {
                    // 更新開始時の処理を設定
                    m_onStartUpdating = () =>
                    {
                        StartPlayerInput();
                    };
                }
                else
                {
                    StartPlayerInput();
                }

                    break;
            case InGameFlowEventID.GAME_CLEAR:
                Debug.Log("ゲームクリア ============================================================================================");
  
                // ゲームクリア
                OnGameClear();
    

                break;

            case InGameFlowEventID.GAME_PLAYING_END:
                Debug.Log("ゲームプレイの終了 ============================================================================================");
                {


                    DOVirtual.DelayedCall(2.0f, () =>
                    {
                        // 結果シーンへ遷移
                        GameProgressManager.Instance.GameProgressData.isPrevSceneGameCleared = true;

                        LoadResultScene();

                    });

                }
                break;

            case InGameFlowEventID.GAME_CLEAR_EFFECT_START:
                Debug.Log("ゲームクリア演出の開始 ============================================================================================");
                StopPlayerInput();
                break;
            case InGameFlowEventID.GAME_CLEAR_EFFECT_END:
                // ゲームプレイ終了イベントを通知
                Debug.Log("ゲームクリア演出の終了 ============================================================================================");

                break;

            case InGameFlowEventID.GOING_GET_FEELING_PIECE_START:
                Debug.Log("想いの欠片取得イベントの開始 ============================================================================================");
                StartPlayerInput();
                //m_playerInput.actions.Enable();
                break;
            case InGameFlowEventID.GOING_GET_FEELING_PIECE_END:
                Debug.Log("想いの欠片取得イベントの終了 ============================================================================================");

                InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.GAME_PLAYING_END);
                break;
            case InGameFlowEventID.TUTORIAL_START:
                Debug.Log("チュートリアルの開始 ============================================================================================");


                if (m_isSecondUpdate)
                {
                    // 更新開始時の処理を設定
                    m_onStartUpdating = () =>
                    {
                        StartTutorial();
                    };
                }
                else
                {
                    StartTutorial();

                }
                break;
            case InGameFlowEventID.TUTORIAL_END:
                Debug.Log("ゲーム開始演出の終了 ============================================================================================");

                StartPlayerInput();
                break;

            case InGameFlowEventID.END_PAUSE_MENU:
                Debug.Log("ポーズメニュー終了 ============================================================================================");
                // 現在の状態に応じてプレイヤー入力を再開
                if ((m_currentEventID == InGameFlowEventID.GAME_PLAYING_START && !m_tutorialController.gameObject.activeSelf) ||
                    m_currentEventID == InGameFlowEventID.GOING_GET_FEELING_PIECE_START || m_currentEventID == InGameFlowEventID.GAME_CLEAR)
                {
                    StartPlayerInput();
                }
                else
                {
                    StopPlayerInput();
                }
                    break;
        }

        if (MapData.GetInstance.StageSetting.tutorialEventData)
        {
            var tutorialEventDict = MapData.GetInstance.StageSetting.tutorialEventData.GetTutorialEventDictionary();
            TutorialEventData.InputDataForTutorialSprite pageSprites;
            if (tutorialEventDict.TryGetValue(eventID, out pageSprites))
            {
                if (pageSprites.pageKeyboardSprites.Count > 0  &&
                    pageSprites.pageGamepadSprites.Count > 0)
                {
                  
                    m_tutorialController.Initialize(pageSprites);
                    // チュートリアル開始を通知
                    InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.TUTORIAL_START);
                }

            }
        }
      
    }


    void StartTutorial()
    {
        m_tutorialController.StartTutorial();
        StopPlayerInput();
    }

    /// <summary>
    /// プレイヤー入力停止
    /// </summary>
    void StopPlayerInput()
    {
        Debug.Log("プレイヤー入力停止 ---------------------------");
        m_playerInput.actions.Enable();
        m_playerInput.actions.FindActionMap("UI").Disable();
        m_playerInput.actions.FindActionMap("Player").Disable();

        m_playerInput.SwitchCurrentActionMap("TutorialWindow");
    }

    /// <summary>
    /// プレイヤー入力開始
    /// </summary>
    void StartPlayerInput()
    {
        Debug.Log("プレイヤー入力開始 ---------------------------");
        m_playerInput.actions.Enable();
        m_playerInput.actions.FindActionMap("UI").Disable();
        m_playerInput.actions.FindActionMap("TutorialWindow").Disable();

        m_playerInput.SwitchCurrentActionMap("Player");
        m_playerInput.currentActionMap.Enable();

    }

    /// <summary>
    /// ポーズウィンドウを開く
    /// </summary>
    /// <param name="context"></param>
    public void OpenPauseWindow(InputAction.CallbackContext context)
    {
        if (context.performed == false) return;
        if (m_pauseWindowController.gameObject.activeSelf) return;

        // ポーズメニューを開くメッセージを送る
        InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.START_PAUSE_MENU);

        // ポーズウィンドウを開く
        m_pauseWindowController.RequestOpenPause(m_playerInput, () => { InGameFlowEventMessenger.GetInstance.Notify(InGameFlowEventID.END_PAUSE_MENU); });
    }

    /// <summary>
    /// シーンをリロードする
    /// </summary>
    /// <param name="context"></param>
    public void ReLoadScene(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        //// リロード
        //GameProgressManager.Instance.GameProgressData.startState = InGameFlowEventID.GAME_PLAYING_START;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        //// クリア時間を保持する
        //GameProgressManager.Instance.GameProgressData.clearTime = m_gameTime;

            UndoTransactionMessenger.GetInstance.NotifyUndo();

    }

    public void ReturnStageSelectScene()
    {
        // ステージセレクトシーンに戻る

        GameProgressManager.Instance.GameProgressData.isPrevSceneGameCleared = false;
        SceneTransitionManager.GetInstance.TransitionToScene("StageSelectScene", m_sceneTransitionFadeOutEffect);

    }
}

