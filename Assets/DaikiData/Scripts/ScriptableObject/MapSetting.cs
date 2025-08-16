using UnityEngine;

[CreateAssetMenu(fileName = "MapSetting", menuName = "MapSetting")]
public class MapSetting : ScriptableObject
{
    public int width;     // �����i�^�C����
    public int height;    // �c�� (�^�C����)
    public Vector2 center;// ���S���W

    public float tileSize; // �^�C���̃T�C�Y

    public float BaseTilePosY;

    public GameObject backgroundPrefab; // �w�i�v���n�u




}
