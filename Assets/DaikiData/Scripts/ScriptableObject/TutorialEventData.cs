using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialEventData", menuName = "TutorialEventData")]

public class TutorialEventData : ScriptableObject
{
    [Serializable]
    public struct EventPairTutorial
    {
        public InGameFlowEventID flowEventID;   // イベント発生するタイミング
        public List<Sprite> pageSprites;        // ページスプライト

    }

    [SerializeField]
    private List<EventPairTutorial> m_eventPairTutorials;

    /// <summary>
    /// チュートリアのイベント辞書の取得
    /// </summary>
    /// <returns></returns>
    public Dictionary<InGameFlowEventID, List<Sprite>> GetTutorialEventDictionary()
    {
        var result = new Dictionary<InGameFlowEventID, List<Sprite>>();

        foreach (var element in m_eventPairTutorials)
        {
            if (element.pageSprites.Count <= 0) { continue; }
            result.Add(element.flowEventID, element.pageSprites);
        }

        return result;
    }
}
