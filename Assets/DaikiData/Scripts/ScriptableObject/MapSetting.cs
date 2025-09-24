using UnityEngine;

[CreateAssetMenu(fileName = "MapSetting", menuName = "MapSetting")]
public class MapSetting : ScriptableObject
{
    public int width;     // 横幅（タイル数
    public int height;    // 縦幅 (タイル数)
    public Vector2 center;// 中心座標

    public float tileSize; // タイルのサイズ

    public float BaseTilePosY;

    public SoundID bgmID;



}
