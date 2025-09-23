using DG.Tweening;
using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;

/// <summary>
/// インゲームカメラ
/// </summary>
public class InGameCamera : MonoBehaviour
{
    private Vector3 m_targetPosition;        // 目標座標

    GameObject m_player;              // プレイヤー

    [SerializeField]
    private Vector3 m_startFocusPosition;    // 初期注視点

    private Quaternion m_startFocusRotate;   // 初期注視点の回転

    private State m_state;


    private Quaternion m_targetRotate;

    [SerializeField]
    private float START_GAME_STARTING_TIME = 3.0f; // ゲーム開始状態の時間

    /// <summary>
    /// 状態
    /// </summary>
    enum State
    {
        FOCUS_PLAYER,       // プレイヤー注視状態
        GAME_SATARTING,     // ゲーム開始
        GAME_PLAYING,       // ゲーム中
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // プレイヤーの取得
        m_player = GameObject.FindGameObjectWithTag("Player");

        m_startFocusPosition = m_player.transform.position ;
        m_startFocusRotate = Quaternion.LookRotation(new Vector3(0.0f, 0.0f, 1.0f));

        // 目標座標
        m_targetPosition = transform.position;
        m_targetRotate = transform.rotation;

        // 最初の状態
        ChangeState(State.FOCUS_PLAYER);
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_state)
        {
            case State.GAME_SATARTING:
                UpdateGameStartingState();
                break;
            case State.GAME_PLAYING:
                // 何もしない
                break;
        }
    }

    /// <summary>
    /// ゲーム開始状態の開始
    /// </summary>
    void StartGameStartingState()
    {
  

        transform.DOBlendableMoveBy(m_targetPosition - transform.position,START_GAME_STARTING_TIME).SetEase(Ease.InOutSine);
        transform.DORotateQuaternion(m_targetRotate, START_GAME_STARTING_TIME * 0.6f).SetEase(Ease.InOutSine);
    }

    /// <summary>
    /// プレイヤー注視状態の開始
    /// </summary>
    void StartFocusPlayerState()
    {
        transform.position = m_startFocusPosition + new Vector3(0.0f, 1.5f, -m_player.transform.localScale.z);
        transform.rotation = m_startFocusRotate;

        // 注視点を初期位置に
        transform.DOBlendableMoveBy(new Vector3(0.0f, 0.0f, -8.0f), 2.0f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            ChangeState(State.GAME_SATARTING);
        }); ;
    }

    void StartGamePlayingState()
    {

    }

    /// <summary>
    /// ゲーム開始状態の更新
    /// </summary>
    void UpdateGameStartingState()
    {
   //     // 時間を進める
   //     m_currentTime += Time.deltaTime;

   //     // 補間率
   //     float lerpRate = m_currentTime / START_GAME_STARTING_TIME;
   //     // 補間率の上限
   //     lerpRate = Mathf.Min(lerpRate, 1.0f);

   //     transform.position = Vector3.Lerp(m_startFocusPosition, m_targetPosition, lerpRate);
   //     transform.rotation = Quaternion.Slerp(m_startFocusRotate, m_targetRotate, Mathf.Min(1.0f,lerpRate*1.8f));

        

   ////     transform.LookAt(m_startFocusPosition);


   //     if (lerpRate >= 1.0f)
   //     {
   //         m_state = State.GAME_PLAYING;
   //     }
    }

    void ChangeState(State state)
    {
        m_state = state;
        
        switch (m_state)
        {
            case State.FOCUS_PLAYER:
                StartFocusPlayerState();
                break;
            case State.GAME_SATARTING:
                StartGameStartingState();
                break;
            case State.GAME_PLAYING:
                StartGamePlayingState();
                break;
        }

    }
}
