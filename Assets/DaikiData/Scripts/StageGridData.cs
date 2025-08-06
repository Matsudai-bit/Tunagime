using UnityEngine;

/// <summary>
/// �X�e�[�W�̃O���b�h�Ǘ�
/// </summary>
public class StageGridData : MonoBehaviour
{
    private TileData[,] m_tileData = new TileData[,] { }; //�@�^�C���f�[�^

    // ���݂��f�[�^�ɕύX�����������ǂ���
    private bool m_isAmidaDataChanged = false;

    /// <summary>
    /// �^�C���f�[�^���擾���܂��B
    /// </summary>
    public TileData[,]  GetTileData
    {
        get{ return m_tileData; } 
    }

    /// <summary>
    /// �w�肵���O���b�h���W�ɑΉ�����^�C���f�[�^���擾���܂��B
    /// </summary>
    /// <param name="gridPos"></param>
    /// <returns></returns>
    public AmidaTube GetAmidaTube(GridPos gridPos)
    {
        return GetAmidaTube(gridPos.x, gridPos.y);
    }

    public AmidaTube GetAmidaTube(int x, int y)
    {
        // �͈͓����ǂ������m�F
        if (!MapData.GetInstance.CheckInnerGridPos(new GridPos(x, y)))
        {
            Debug.LogWarning($"GetAmida: Grid position ({x},{y}) is out of bounds.");
            return null;
        }
        // �^�C���f�[�^���炠�݂��`���[�u���擾
        TileData tile = m_tileData[y, x];
        return tile.amidaTube;
    }

    public TileObject GetTileObject(GridPos gridPos)
    {
        // �͈͓����ǂ������m�F
        if (!MapData.GetInstance.CheckInnerGridPos(gridPos))
        {
            Debug.LogWarning($"GetTileObject: Grid position ({gridPos.x},{gridPos.y}) is out of bounds.");
            return new TileObject(); // null�ł͂Ȃ��A�f�t�H���g��TileObject��Ԃ�
        }
        // �^�C���f�[�^����TileObject���擾
        TileData tile = m_tileData[gridPos.y, gridPos.x];
        return tile.tileObject;
    }

    /// <summary>
    /// �w�肵���O���b�h���W�ɂ��݂��`���[�u��ݒ肵�܂��B
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="amidaTube"></param>
    public void SetAmidaTube(GridPos gridPos, AmidaTube amidaTube)
    {
        // �͈͓����ǂ������m�F
        if (!MapData.GetInstance.CheckInnerGridPos(gridPos))
        {
            Debug.LogWarning($"SetAmidaTube: Grid position ({gridPos.x},{gridPos.y}) is out of bounds.");
            return;
        }
        // �^�C���f�[�^���擾
        TileData currentTile = m_tileData[gridPos.y, gridPos.x];
        // ���݂��`���[�u��ݒ�
        currentTile.amidaTube = amidaTube;
        // �ύX��z��ɔ��f
        m_tileData[gridPos.y, gridPos.x] = currentTile;

        // ���݂��f�[�^���ύX���ꂽ���Ƃ��L�^
        m_isAmidaDataChanged = true;
    }

    /// <summary>
    /// �^�C���f�[�^��ݒ肵�܂��B
    /// </summary>
    /// <param name="tileData"></param>
    public void SetData(TileData[,] tileData)
    {
        m_tileData = tileData;
    }

    /// <summary>
    /// �O���b�h�f�[�^�����������܂��B
    /// </summary>
    /// <param name="gridWidth"></param>
    /// <param name="gridHeight"></param>
    public void  Initialize(int gridWidth, int gridHeight)
    {
        m_tileData = new TileData[gridHeight, gridWidth];
    }


    /// <summary>
    /// �w�肵���^�C���̃^�C���I�u�W�F�N�g����菜��
    /// </summary>
    /// <param name="gridPos"></param>
    public GameObject RemoveGridData(GridPos gridPos)
    {
        // �͈͓����ǂ������m�F
        if (!MapData.GetInstance.CheckInnerGridPos(gridPos))
        {
            Debug.LogWarning($"RemoveTileObject: Grid position ({gridPos.x},{gridPos.y}) is out of bounds.");
            return null;
        }

        // �����̃^�C���f�[�^���擾 (�\���̂Ȃ̂ŃR�s�[�����)
        TileData currentTile = m_tileData[gridPos.y, gridPos.x];

        // �O���b�h�����菜��GameObject�̎Q�Ƃ�ێ�
        GameObject removedObject = currentTile.tileObject.gameObject;

        // �O���b�h���̎Q�Ƃ�null�ɂ���
        currentTile.tileObject.gameObject = null;
        m_tileData[gridPos.y, gridPos.x] = currentTile; // �ύX��z��ɔ��f

        if (removedObject != null)
        {
            Debug.Log($"RemoveTileObject: Removed {removedObject.name} from grid at ({gridPos.x},{gridPos.y}).");
        }
        else
        {
            Debug.Log($"RemoveTileObject: No object found at ({gridPos.x},{gridPos.y}) to remove.");
        }

        return removedObject; // ��菜�����I�u�W�F�N�g��Ԃ�
    }

    /// <summary>
    /// �w�肵���O���b�h���W�ɃI�u�W�F�N�g��z�u���܂��B
    /// ���̏ꏊ�Ɋ��ɑ��̃I�u�W�F�N�g�����݂��Ȃ��ꍇ�ɂ̂ݐݒu���܂��B
    /// </summary>
    /// <param name="gridPos">�z�u����O���b�h���W</param>
    /// <param name="objectToPlace">�z�u����GameObject</param>
    /// <returns>�I�u�W�F�N�g������ɔz�u���ꂽ�ꍇ��true�A���ɃI�u�W�F�N�g�����݂����ꍇ��false�B</returns>
    public bool TryPlaceTileObject(GridPos gridPos, GameObject objectToPlace)
    {
        // �͈͓����ǂ������m�F
        if (!MapData.GetInstance.CheckInnerGridPos(gridPos))
        {
            Debug.LogWarning($"TryPlaceTileObject: Grid position ({gridPos.x},{gridPos.y}) is out of bounds.");
            return false;
        }

        // �w�肵���ꏊ�ɃI�u�W�F�N�g�����ɑ��݂��邩�`�F�b�N
        // TileData�͍\���̂Ȃ̂ŁA�R�s�[���擾���ă`�F�b�N
        TileData currentTile = m_tileData[gridPos.y, gridPos.x];

        if (currentTile.tileObject.gameObject != null)
        {
            // ���ɃI�u�W�F�N�g�����݂���ꍇ�A�ݒu���Ȃ�
            Debug.Log($"TryPlaceTileObject: Position ({gridPos.x},{gridPos.y}) already has an object: {currentTile.tileObject.gameObject.name}. Placement failed.");
            return false;
        }

        // �I�u�W�F�N�g�����݂��Ȃ��ꍇ�A�V�����I�u�W�F�N�g��z�u
        currentTile.tileObject.gameObject = objectToPlace; // tileObject��ݒ�
        m_tileData[gridPos.y, gridPos.x] = currentTile; // �ύX��z��ɔ��f

        Debug.Log($"TryPlaceTileObject: Successfully placed {objectToPlace.name} at ({gridPos.x},{gridPos.y}).");
        return true;
    }

    /// <summary>
    /// ���݂��f�[�^���ύX���ꂽ���ǂ������m�F���܂��B
    /// </summary>
    /// <returns></returns>
    public bool IsAmidaDataChanged()
    {
        return m_isAmidaDataChanged;
    }

    /// <summary>
    /// ���݂��f�[�^�̕ύX�t���O��ݒ�
    /// </summary>
    public void SetAmidaDataChanged()
    {
        m_isAmidaDataChanged = true;
    }

    /// <summary>
    /// ���݂��f�[�^�̕ύX�t���O�����Z�b�g���܂��B
    /// </summary>
    public void ResetAmidaDataChanged()
    {
        m_isAmidaDataChanged = false;
    }


}