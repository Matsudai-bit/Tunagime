using UnityEngine;

public struct TileData 
{
    public TileObject tileObject;
    public AmidaTube amidaTube;
    public GameDirector floor;
}


public enum TileType
{
    FLUFF_BALL, // �ю���
}

public struct TileObject
{
    public GameObject gameObject;
    public TileType type;
}
