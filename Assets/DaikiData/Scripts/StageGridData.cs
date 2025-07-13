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
    public TileData[,]  GetTileData()
    {
        return m_tileData;
    }

    public void SetData(TileData[,] tileData)
    {
        m_tileData = tileData;
    }

    public void  Initialize(int gridWidth, int gridHeight)
    {
        m_tileData = new TileData[gridHeight, gridWidth];
    }

    ///// <summary>
    ///// ���݂��w�̃u���b�N�O���b�h��ݒ肷��
    ///// </summary>
    ///// <param name="grid">�ݒ肷��O���b�h</param>
    //public void SetAmidaFloorBlockGrid(GameObject[,] grid)
    //{
    //    m_amidaFloorBlockGrid = grid;
    //}

    ///// <summary>
    ///// ���݂��w�̃u���b�N�O���b�h���擾����
    ///// </summary>
    ///// <returns>���݂��w�̃u���b�N�O���b�h</returns>
    //public GameObject[,] GetAmidaFloorBlockGrid()
    //{
    //    return m_amidaFloorBlockGrid;
    //}

    ///// <summary>
    ///// �g�b�v�w�̃u���b�N�O���b�h��ݒ肷��
    ///// </summary>
    ///// <param name="grid">�ݒ肷��O���b�h</param>
    //public void SetTopFloorBlockGrid(GameObject[,] grid)
    //{
    //    m_topFloorBlockGrid = grid;
    //}

    ///// <summary>
    ///// �g�b�v�w�̃u���b�N�O���b�h���擾����
    ///// </summary>
    ///// <returns>�g�b�v�w�̃u���b�N�O���b�h</returns>
    //public GameObject[,] GetTopFloorBlockGrid()
    //{
    //    return m_topFloorBlockGrid;
    //}

    ///// <summary>
    ///// ���݂��`���[�u�̃O���b�h��ݒ肷��
    ///// </summary>
    ///// <param name="grid">�ݒ肷��O���b�h</param>
    //public void SetAmidaTubeGrid(GameObject[,] grid)
    //{
    //    m_amidaTubeGrid = grid;
    //}

    ///// <summary>
    ///// ���݂��`���[�u�̃O���b�h���擾����
    ///// </summary>
    ///// <returns>���݂��`���[�u�̃O���b�h</returns>
    //public GameObject[,] GetAmidaTubeGrid()
    //{
    //    return m_amidaTubeGrid;
    //}

    ///// <summary>
    ///// �M�~�b�N�u���b�N�̃O���b�h��ݒ肷��
    ///// </summary>
    ///// <param name="grid">�ݒ肷��O���b�h</param>
    //public void SetTopGimmickBlockGrid(GameObject[,] grid)
    //{
    //    m_topGimmickBlockGrid = grid;
    //}

    ///// <summary>
    ///// �M�~�b�N�u���b�N�̃O���b�h���擾����
    ///// </summary>
    ///// <returns>�M�~�b�N�u���b�N�̃O���b�h</returns>
    //public GameObject[,] GetTopGimmickBlockGrid()
    //{
    //    return m_topGimmickBlockGrid;
    //}

    ///// <summary>
    ///// �ǂ̃O���b�h��ݒ肷��
    ///// </summary>
    ///// <param name="grid">�ݒ肷��O���b�h</param>
    //public void SetWallGrid(GameObject[,] grid)
    //{
    //    m_wallGrid = grid;
    //}

    ///// <summary>
    ///// �ǂ̃O���b�h���擾����
    ///// </summary>
    ///// <returns>�ǂ̃O���b�h</returns>
    //public GameObject[,] GetWallGrid()
    //{
    //    return m_wallGrid;
    //}
}