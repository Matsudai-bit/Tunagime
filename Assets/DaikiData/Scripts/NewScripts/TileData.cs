using UnityEngine;

public struct TileData 
{
    public TileObject tileObject;　// タイル上のオブジェクト情報
    public AmidaTube amidaTube;    // あみだチューブ情報
    public GameObject floor;       // 床のGameObject

    public TileData(TileObject tileObject, AmidaTube amidaTube, GameObject floor)
    {
        this.tileObject = tileObject;
        this.amidaTube = amidaTube;
        this.floor = floor;
    }
}


/// <summary>
/// タイルの種類を定義する列挙型
/// </summary>
public enum TileType
{
    FLUFF_BALL, // 毛糸玉
}

public struct TileObject
{
    public GameObject gameObject;
    public TileType type;
}
