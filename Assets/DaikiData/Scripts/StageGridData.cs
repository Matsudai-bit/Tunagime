using UnityEngine;

/// <summary>
/// �X�e�[�W�̃O���b�h�Ǘ�
/// </summary>
public class StageGridData : MonoBehaviour
{
    //private GameObject[,] m_amidaFloorBlockGrid;   // ���݂��w�̃u���b�N�O���b�h
    //private GameObject[,] m_topFloorBlockGrid;     // �g�b�v�w�̃u���b�N�O���b�h
    //private GameObject[,] m_amidaTubeGrid;         // ���݂��`���[�u�̃O���b�h
    //private GameObject[,] m_topGimmickBlockGrid;   // �M�~�b�N�u���b�N�̃O���b�h

    //private GameObject[,] m_wallGrid;               // �ǂ̃O���b�h

    private TileData[,] m_tileData = new TileData[,] { }; //�@�^�C���f�[�^
                                    //�@
    public  TileData[,]  GetTileData
    {
        get{ return m_tileData; } 
    }

    public void SetData(TileData[,] tileData)
    {
        m_tileData = tileData;
    }

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

  
}