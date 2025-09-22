using UnityEngine;

public struct TileData 
{
    public TileObject tileObject;�@// �^�C����̃I�u�W�F�N�g���
    public AmidaTube amidaTube;    // ���݂��`���[�u���
    public GameObject floor;       // ����GameObject

    public TileData(TileObject tileObject, AmidaTube amidaTube, GameObject floor)
    {
        this.tileObject = tileObject;
        this.amidaTube = amidaTube;
        this.floor = floor;
    }
}


/// <summary>
/// �^�C���̎�ނ��`����񋓌^
/// </summary>
public enum TileType
{
    PLAYER_INTERACTION, // �v���C���[�����삷��^�C��
    PLAYER_NON_INTERACTION, // �v���C���[�����삵�Ȃ��^�C��
}

public struct TileObject
{
    public GameObject gameObject;
    public StageBlock stageBlock; // ���̃^�C����������StageBlock
}
