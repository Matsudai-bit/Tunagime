using UnityEngine;

/// <summary>
/// �Q�[�����̃C���^���N�V�����C�x���g��\���񋓌^
/// </summary>
public enum InteractionEvent
{
    // **** �v���C���[�̍s���Ɋւ���C�x���g ****
    PLAYER_START_KNIT, // �v���C���[���҂ݕ����J�n����
    PLAYER_END_KNIT,  // �v���C���[���҂ݕ����I������

    // **** ���݂��̏�ԂɊւ���C�x���g ****
    CHANGED_AMIDAKUJI, // �҂ݕ��̏�Ԃ��ω�����
    FLOWWING_AMIDAKUJI, // �҂ݕ������ꂽ


    // **** �M�~�b�N�Ɋւ���C�x���g ****
    CONNECTED_REJECTION_SLOT, // ����̊j���ڑ����ꂽ
    DISCONNECTED_REJECTION_SLOT, // ����̊j���ؒf���ꂽ


}
