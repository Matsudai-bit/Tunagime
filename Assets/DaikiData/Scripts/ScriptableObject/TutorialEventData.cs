using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static TutorialEventData;

[CreateAssetMenu(fileName = "TutorialEventData", menuName = "TutorialEventData")]

public class TutorialEventData : ScriptableObject
{
    /// <summary>
    /// インスペクター表示用
    /// </summary>
    [Serializable]
    public class EventPairTutorialForInspector
    {
        string label1 = "イベントID";
        public InGameFlowEventID flowEventID;   // イベント発生するタイミング

        string label2 = "キーボード用チュートリアルスプライト";
        public List<Sprite> pageKeyboardSprites;// キーボード用ページスプライト
        string label3 = "ゲームパッド用チュートリアルスプライト";
        public List<Sprite> pageGamepadSprites; // ゲームパッド用ページスプライト
    }

    /// <summary>
    /// 入力を考慮したチュートリアルスプライトデータ
    /// </summary>
    public class InputDataForTutorialSprite
    {
        public List<Sprite> pageKeyboardSprites;        // キーボード用ページスプライト
        public List<Sprite> pageGamepadSprites;         // ゲームパッド用ページスプライト

        public InputDataForTutorialSprite(List<Sprite> pageKeyboardSprites, List<Sprite> pageGamepadSprites)
        {
            this.pageKeyboardSprites = pageKeyboardSprites;
            this.pageGamepadSprites = pageGamepadSprites;
        }
    }

    [SerializeField]
    private List<EventPairTutorialForInspector> m_eventPairTutorials;

    /// <summary>
    /// チュートリアのイベント辞書の取得
    /// </summary>
    /// <returns></returns>
    public Dictionary<InGameFlowEventID, InputDataForTutorialSprite> GetTutorialEventDictionary()
    {
        var result = new Dictionary<InGameFlowEventID,InputDataForTutorialSprite>();

        foreach (var element in m_eventPairTutorials)
        {
            if (element.pageKeyboardSprites.Count <= 0) { continue; }
            if (element.pageGamepadSprites.Count <= 0) { continue; }


            result.Add(element.flowEventID,new InputDataForTutorialSprite( element.pageKeyboardSprites,element.pageGamepadSprites));
        }

        return result;
    }
}
