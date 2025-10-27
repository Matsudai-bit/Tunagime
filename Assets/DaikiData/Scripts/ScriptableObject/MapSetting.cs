using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "MapSetting", menuName = "MapSetting")]
public class MapSetting : ScriptableObject
{
    public int width;     // 横幅（タイル数
    public int height;    // 縦幅 (タイル数)
    public Vector2 center;// 中心座標

    public float tileSize; // タイルのサイズ

    public float BaseTilePosY;  // 基準タイルのY座標

    public SoundID bgmID;       // BGMのID

    public VolumeProfile volumeProfile; // ポストプロセスのボリュームプロファイル

    public GameObject stageEffectParticlePrefab; // ステージエフェクトのパーティクルシステムプレハブ

    public GameObject feelingPiece; // 想いのカケラプレハブ



}
