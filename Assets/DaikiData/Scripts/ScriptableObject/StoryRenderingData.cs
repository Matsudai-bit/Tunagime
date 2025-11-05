using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "StoryMovieData", menuName = "DaikiData/StoryMovieData")]

/// <summary>
/// ストーリーイラストデータ
/// </summary>
public class StoryRenderingData : ScriptableObject
{
    [Serializable]
    public class StoryData
    {
        public VideoClip videoClip;    // ストーリームービー
        public List<Sprite> illustrationSprites; // ストーリーイラストリスト
    }

    /// <summary>
    /// ワールドごとのストーリーイラスト情報
    /// </summary>
    [Serializable]
   public class  StoryMovieForNarrative
    {
        public string labelName;      
        public NarrativeContext.NarrativeState narrativeState;  // 物語の状態 
        public StoryData storyData = new StoryData();    // ストーリーデータ

    }

    [SerializeField]
    private List<StoryMovieForNarrative> storyRenderingForWorlds = new(); // ワールドごとのストーリーイラスト情報リスト


    /// <summary>
    /// ストーリーイラスト辞書を取得
    /// </summary>
    /// <returns></returns>
    public Dictionary<NarrativeContext.NarrativeState, StoryData> GetStoryRenderingDict()
    {
        Dictionary<NarrativeContext.NarrativeState, StoryData> dict = new();
        foreach (var item in storyRenderingForWorlds)
        {
            dict[item.narrativeState] = new StoryData();
            dict[item.narrativeState].videoClip = item.storyData.videoClip;
            dict[item.narrativeState].illustrationSprites = item.storyData.illustrationSprites;
        }
        return dict;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Reset()
    {
        storyRenderingForWorlds.Clear();
        foreach (NarrativeContext.NarrativeState state in System.Enum.GetValues(typeof(NarrativeContext.NarrativeState)))
        {
            storyRenderingForWorlds.Add(new StoryMovieForNarrative
            {
                labelName = state.ToString(),
                narrativeState = state,
                storyData = new StoryData()

            });
        }
    }



}
