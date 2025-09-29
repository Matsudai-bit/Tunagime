using System;
using System.Collections.Generic;
using UnityEngine;

public class StageTransformsForWorldObject : MonoBehaviour
{
    /// <summary>
    /// ステージのTransformを格納するクラス
    /// </summary>
    [Serializable]
    public class StageTransform
    {
        public string stageName; // ステージ名
        public StageID stageID; // ステージID
        public Transform stageTransform; // ステージのTransform
    }

    [Header("ステージのTransformを格納する配列")]
    public List<StageTransform> m_stageTransforms = new(); // ステージのTransformを格納する配列

    private Dictionary<StageID, Transform> m_stageTransformDict; // ステージのTransformを格納する辞書

    public Dictionary<StageID, Transform> StageTransforms { get { return m_stageTransformDict; } }

    [SerializeField]
    public Transform worldTransform; // ワールドのTransform


    private void Awake()
    {
        // 辞書を初期化
        m_stageTransformDict = new Dictionary<StageID, Transform>();
        foreach (var stageTransform in m_stageTransforms)
        {
            if ( stageTransform.stageTransform != null)
            {
                m_stageTransformDict[stageTransform.stageID] = stageTransform.stageTransform;
            }
        }
    }

    /// <summary>
    /// リセット時に呼ばれる関数
    /// </summary>
    private void Reset()
    {
        for (int i = 0; i < 5; i++)
        {
            var stageTransform = new StageTransform
            {
                stageName = "Stage " + (i + 1),
                stageID = (StageID)i,
                stageTransform = null
            };
            m_stageTransforms.Add(stageTransform);
        }
    }


}
