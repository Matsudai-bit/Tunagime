using UnityEngine;

public struct TileData 
{
    public TileObject tileObject;
    public AmidaTube amidaTube;
    public GameDirector floor;
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
