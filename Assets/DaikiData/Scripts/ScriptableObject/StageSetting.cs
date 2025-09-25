using UnityEngine;

[CreateAssetMenu(fileName = "StageSetting", menuName = "StageSetting")]
public class StageSetting : ScriptableObject
{
    public MapSetting mapSetting; // MapSettingの参照

    [Header("背景")]
    public GameObject backgroundPrefab; // 背景プレハブ

    [Header("ステージ生成器")]
    public GameObject stageGenerator; // StageGeneratorの参照

    [Header("ステージオブジェクト")]
    public GameObject stageObject; // ステージオブジェクトの親オブジェクト



}