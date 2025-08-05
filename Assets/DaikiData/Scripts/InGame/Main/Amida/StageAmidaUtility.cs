using UnityEngine;

/// <summary>
/// �X�e�[�W�̂��݂��֘A�̃��[�e�B���e�B�N���X
/// </summary>
public class StageAmidaUtility 
{
    /// <summary>
    /// �w�肳�ꂽ�O���b�h���W�ɑΉ����邠�݂��`���[�u�̏�Ԃ��`�F�b�N�B
    /// </summary>
    /// <param name="checkPos"></param>
    /// <param name="checkState"></param>
    /// <returns></returns>
    public static bool CheckAmidaState(GridPos checkPos, AmidaTube.State checkState)
    {
        // �w�肳�ꂽ�O���b�h���W�ɑΉ����邠�݂��`���[�u���擾
        AmidaTube amidaTube = MapData.GetInstance.GetStageGridData().GetAmidaTube(checkPos);

        // ���݂��`���[�u�����݂��Ȃ��ꍇ��false��Ԃ�
        if (amidaTube == null)
        {
            return false;
        }
        // ���݂��`���[�u�̏�Ԃ��`�F�b�N
        return amidaTube.GetState() == checkState;
    }

}
