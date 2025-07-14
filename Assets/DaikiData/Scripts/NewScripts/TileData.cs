using UnityEngine;

public struct TileData 
{
    public TileObject tileObject;
    public AmidaTube amidaTube;
    public GameObject floor;

    public TileData(TileObject tileObject, AmidaTube amidaTube, GameObject floor)
    {
        this.tileObject = tileObject;
        this.amidaTube = amidaTube;
        this.floor = floor;
    }
}


public enum TileType
{
    FLUFF_BALL, // –ÑŽ…‹Ê
}

public struct TileObject
{
    public GameObject gameObject;
    public TileType type;
}
