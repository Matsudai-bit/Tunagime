using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryIllustrationData", menuName = "DaikiData/StoryIllustrationData")]

/// <summary>
/// ストーリーイラストデータ
/// </summary>
public class StoryIllustrationData : ScriptableObject
{
    /// <summary>
    /// ワールドごとのストーリーイラスト情報
    /// </summary>
    [Serializable]
   public class  StoryIllustrationForWorld
    {
        public string worldName;      // ワールド名（エディタ表示用）
        public WorldID worldID;          // ワールドID
        public List<Sprite> illustrationSprites  = new();    // イラストスプライト
    }

    [SerializeField]
    private List<StoryIllustrationForWorld> storyIllustrationForWorlds = new(); // ワールドごとのストーリーイラスト情報リスト


    /// <summary>
    /// ストーリーイラスト辞書を取得
    /// </summary>
    /// <returns></returns>
    public Dictionary<WorldID, List<Sprite>> GetStoryIllustrationDict()
    {
        Dictionary<WorldID, List<Sprite>> dict = new();
        foreach (var item in storyIllustrationForWorlds)
        {
            dict[item.worldID] = item.illustrationSprites;
        }
        return dict;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Reset()
    {
        storyIllustrationForWorlds.Clear();
        foreach (WorldID worldID in System.Enum.GetValues(typeof(WorldID)))
        {
            storyIllustrationForWorlds.Add(new StoryIllustrationForWorld
            {
                worldName = worldID.ToString(),
                worldID = worldID,
                illustrationSprites = new List<Sprite>()
            });
        }
    }



}
