using UnityEngine;

/// <summary>
/// �t���A(�w)�Ǘ�
/// </summary>
public class FloorManager : MonoBehaviour
{

    ///// <summary>
    ///// �t���A�̖���
    ///// </summary>
    //public enum FloorType
    //{
    //    AMIDA,  // ���݂��w
    //    TOP     // �g�b�v�w
    //}

    ///// <summary>
    ///// ���̏㕔��Y���W
    ///// </summary>
    //private struct FloorTopPartPosYData
    //{
    //    public float topFloor;
    //    public float amidaFloor;
    //}

    //[SerializeField]�@private StageGridData m_stageGridData;

    //FloorTopPartPosYData m_floorTopPartPosYData;


    ///// <summary>
    ///// �w�肵���w�̏㕔��Y���W�̎擾
    ///// </summary>
    ///// <param name="floorType"></param>
    ///// <returns></returns>
    //public float GetFloorTopPartPosY(FloorType floorType)
    //{
    //    switch (floorType)
    //    {
    //        case FloorType.AMIDA:
    //            return m_floorTopPartPosYData.amidaFloor;
    //        case FloorType.TOP:
    //            return m_floorTopPartPosYData.topFloor;

    //        default:
    //            return 0.0f;
    //    }
    //}

    ///// <summary>
    ///// �w�肵���w�̏㕔��Y���W�̐ݒ�
    ///// </summary>
    ///// <param name="floorType"></param>
    ///// <returns></returns>
    //public void SetFloorTopPartPosY(FloorType floorType, float topPartPosY)
    //{
    //    switch (floorType)
    //    {
    //        case FloorType.AMIDA:
    //            m_floorTopPartPosYData.amidaFloor = topPartPosY;
    //            break;
    //        case FloorType.TOP:
    //            m_floorTopPartPosYData.topFloor = topPartPosY;
    //            break;
    //    }
    //}


    ///// <summary>
    ///// �w�肵���t���A�̃O���b�h�f�[�^�̎擾
    ///// </summary>
    ///// <param name="floorType"></param>
    ///// <returns></returns>
    //public GameObject[,] GetFloor(FloorType floorType)
    //{
    //    switch (floorType)
    //    {
    //        case FloorType.AMIDA:
    //            return m_stageGridData.GetAmidaFloorBlockGrid();
    //        case FloorType.TOP:
    //            return m_stageGridData.GetTopFloorBlockGrid();

    //        default:
    //            return null;
    //    }
    //}

    ///// <summary>
    ///// �w�肵���t���A�̃O���b�h�f�[�^�̐ݒ�
    ///// </summary>
    ///// <param name="floorType"></param>
    ///// <param name="grid"></param>
    //public void SetFloor(FloorType floorType, GameObject[,] grid)
    //{
    //    switch (floorType)
    //    {
    //        case FloorType.AMIDA:
    //            m_stageGridData.SetAmidaFloorBlockGrid(grid);
    //            break;
    //        case FloorType.TOP:
    //            m_stageGridData.SetTopFloorBlockGrid(grid);
    //            break;
    //    }
    //}

    ///// <summary>
    ///// �w�肵���t���A�̃M�~�b�N�O���b�h�f�[�^�̎擾
    ///// </summary>
    ///// <param name="floorType"></param>
    ///// <returns></returns>
    //public GameObject[,] GetGimmickFloor(FloorType floorType)
    //{
    //    switch (floorType)
    //    {
    //        case FloorType.AMIDA:
    //            return m_stageGridData.GetAmidaTubeGrid();
    //        case FloorType.TOP:
    //            return m_stageGridData.GetTopGimmickBlockGrid();

    //        default:
    //            return null;
    //    }
    //}

    ///// <summary>
    ///// �w�肵���t���A�̃M�~�b�N�O���b�h�f�[�^�̐ݒ�
    ///// </summary>
    ///// <param name="floorType"></param>
    ///// <param name="grid"></param>
    //public void SetGimmickFloor(FloorType floorType, GameObject[,] grid)
    //{
    //    switch (floorType)
    //    {
    //        case FloorType.AMIDA:
    //            m_stageGridData.SetAmidaTubeGrid(grid);
    //            break;
    //        case FloorType.TOP:
    //            m_stageGridData.SetTopGimmickBlockGrid(grid);
    //            break;
    //    }
    //}
}
