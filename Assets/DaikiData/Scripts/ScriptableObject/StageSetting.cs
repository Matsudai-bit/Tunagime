using UnityEngine;

[CreateAssetMenu(fileName = "StageSetting", menuName = "StageSetting")]
public class StageSetting : ScriptableObject
{
    public MapSetting mapSetting; // MapSettingの参照

    public GameObject backgroundPrefab; // 背景プレハブ

    public GameObject stageGenerator; // StageGeneratorの参照



}