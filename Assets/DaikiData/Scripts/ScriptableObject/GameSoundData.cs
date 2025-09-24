using System;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

/// <summary>
/// サウンドID
/// </summary>
[Serializable]
public struct SoundData
{
    public SoundID id;
    public AudioClip clip;
}

/// <summary>
/// サウンドデータのScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "GameSoundData", menuName = "ScriptableObjects/GameSoundData")]
public class GameSoundData : ScriptableObject
{
    public SoundData[] soundData;

    private static GameSoundData s_instance;



    public static GameSoundData GetInstance
    {
        get
        {
            if (s_instance == null)
            {
                // リソースフォルダからスクリプタブルオブジェクトをロード
                s_instance = Resources.Load<GameSoundData>("GameSoundData");
                if (s_instance == null)
                {
                    Debug.LogError("GameSoundDataスクリプタブルオブジェクトが'Resources'フォルダに見つかりません。");
                }

            }
            return s_instance;
        }
    }

}

