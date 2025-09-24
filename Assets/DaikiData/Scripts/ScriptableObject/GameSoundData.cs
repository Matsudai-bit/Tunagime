using System;
using UnityEngine;

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
}

