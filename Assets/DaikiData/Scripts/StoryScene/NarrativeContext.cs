using System;
using UnityEngine;

/// <summary>
/// 物語の状況クラス シングルトン
/// </summary>
public class NarrativeContext 
{
    static private NarrativeContext m_instance;

    private NarrativeState m_state; // 物語の状態

    public NarrativeState GetNarrativeState
    {
        get { return m_state; }
    }

    /// <summary>
    /// 物語の状態
    /// </summary>
    [Serializable]
    public enum NarrativeState
    {
        INTRODUCTION,   // 導入

        WORLD_1,        // ワールド1
        WORLD_2,        // ワールド2
        WORLD_3,        // ワールド3
        WORLD_4,        // ワールド4

        ENDING          // エンディング
    }

    [Header("現在の物語")]
    [SerializeField]
    private NarrativeState m_currentState = NarrativeState.INTRODUCTION;


    /// <summary>
    /// インスタンスの取得
    /// </summary>
    static public NarrativeContext GetInstance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new NarrativeContext();
            }
            return m_instance;
        }
    }

    public void SetNarrativeState(NarrativeState state)
    {
        m_state = state;
    }
}
