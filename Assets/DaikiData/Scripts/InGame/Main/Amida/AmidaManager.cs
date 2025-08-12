
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ����
/// </summary>
public enum Direction
{
    UP,
    DOWN,
    RIGHT,
}

/// <summary>
/// ���݂������̊Ǘ��N���X�B���݂������̃p�C�v������ړ������A�F�̕ύX�Ȃǂ��Ǘ����܂��B
/// </summary>
public class AmidaManager : MonoBehaviour , IGameInteractionObserver
{

    private List<FeelingSlot> m_feelingSlots = new List<FeelingSlot>(); // �z���̌^�̃��X�g

    // �x���ŒH�邽�߂̃t���O
    private bool m_isFollowingAmida = false;

    private bool m_connectedRejectionSlot = false; // �I�_�̋���̊j���ڑ�����Ă��邩�ǂ���


    private void Awake()
    {

    }

    /// <summary>
    /// �Q�[���J�n���̏����������B���݂��`���[�u�̍쐬���s���܂��B
    /// </summary>
    void Start()
    {
        GameInteractionEventMessenger.GetInstance.RegisterObserver(this); // �C�x���g���󂯎�邽�߂ɃI�u�U�[�o�[��o�^
        // ���݂���H��
        m_isFollowingAmida = true;

        m_connectedRejectionSlot = false; // ������Ԃł͐ڑ�����Ă��Ȃ�
    }

    /// <summary>
    /// �X�V�����B
    /// </summary>
    void Update()
    {
        var map = MapData.GetInstance;
        var stageGridData = map.GetStageGridData();

        int LOOP_COUNT = 1; // ���݂��`���[�u��H���

        // ���݂��`���[�u��H�鏈��
        if (m_isFollowingAmida || Input.GetKeyDown(KeyCode.Space))
        {
            // ���ׂĂ̂��݂��`���[�u�̃^�C�v�����Z�b�g
            ResetAllAmidaTubeType();

            // ���݂��`���[�u��H��񐔏��������s
            for (int i = 0; i < LOOP_COUNT; i++) 
            {
                foreach (var slot in m_feelingSlots)
                {
                    // �X�^�[�g�ʒu
                    var startPos = slot.StageBlock.GetGridPos() + new GridPos(1, 0);
                    // ���݂��`���[�u�̎擾
                    var startAmidaTube = stageGridData.GetAmidaTube(startPos);
                    // �z���̎�ނ̎擾
                    var emotionType = slot.GetEmotionType();

                    // �擪�̂��݂��`���[�u�̐ݒ�
                    startAmidaTube.SetEmotionCurrentType(YarnMaterialGetter.MaterialType.INPUT, emotionType); // �X���b�g�̃}�e���A����ݒ�
                    startAmidaTube.SetEmotionCurrentType(YarnMaterialGetter.MaterialType.OUTPUT, emotionType); // �X���b�g�̃}�e���A����ݒ�

                    // ���݂��`���[�u��H�鏈��
                    FollowTheAmidaTube(startAmidaTube, AmidaTube.Direction.RIGHT);

                }

            }

            // �H�邱�Ƃ��I���������Ƃ�ʒm���� (�Ăяo���^�C�~���O�厖)
            GameInteractionEventMessenger.GetInstance.Notify(InteractionEvent.FLOWWING_AMIDAKUJI); // ���݂��������ύX���ꂽ���Ƃ�ʒm

            if (m_connectedRejectionSlot)
            {
                // �I�_�̋���̊j���ڑ�����Ă���ꍇ�́A����̊j�̃}�e���A����K�p
                ApplyAllAmidaTubeRejectionMaterial();
            }
            else
            {
                // �I�_�̋���̊j���ڑ�����Ă��Ȃ��ꍇ�́A�ʏ�̃}�e���A����K�p
                ApplyAllAmidaTubeMaterial();
            }



            m_isFollowingAmida = false; // �t���O�����Z�b�g


        }



        stageGridData.ResetAmidaDataChanged();

    }

    /// <summary>
    /// �w�肵�����݂��`���[�u��H��
    /// </summary>
    /// <param name="followAmida"></param>
    /// <param name="prevFollowDir"></param>
    private void FollowTheAmidaTube(AmidaTube followAmida, AmidaTube.Direction prevFollowDir)
    {
        if (followAmida == null)
        {
            Debug.LogError("FollowTheAmidaTube: followAmida is null");
            return;
        }

        MapData map = MapData.GetInstance;
        var stageGridData = map.GetStageGridData();

        TileObject tileObject = stageGridData.GetTileData[followAmida.GetGridPos().y, followAmida.GetGridPos().x].tileObject;


        // �t�F���g�u���b�N������ꍇ�͏������I��
        if (tileObject.stageBlock != null )
        {
            if ( tileObject.stageBlock.GetBlockType() == StageBlock.BlockType.FELT_BLOCK)
                return;
        }


        // ���ɐi�ޕ���������
        // ���� : ���݂̂��݂����i�ނ��Ƃ̂ł���������擾�@->�@�Ȃ������������΂�������D��I�ɑI�� -> ���̕����ɂ��݂������邩�̊m�F

        followAmida.UpdateMeshMaterialsBasedOnAmidaState(prevFollowDir);

        var followDir = followAmida.GetFollowDirection();

        var neighborAmida = followAmida.GetNeighbor();




        // �D�悷��������Ɋm�F
        if (prevFollowDir == AmidaTube.Direction.RIGHT)
        {
            if (neighborAmida.up != null)
            {
                // ��ɐi��
                Debug.Log("Moving UP");
                FollowTheAmidaTube(neighborAmida.up, followDir);
                return;
            }
            else if (neighborAmida.down != null)
            {
                // ���ɐi��
                Debug.Log("Moving DOWN");
                FollowTheAmidaTube(neighborAmida.down, followDir);
                return;
            }
            else if (neighborAmida.right != null)
            {
                // �E�ɐi��
                Debug.Log("Moving RIGHT");
                FollowTheAmidaTube(neighborAmida.right, followDir);
                return;
            }

        }

        if (prevFollowDir == AmidaTube.Direction.UP)
        {
            if (neighborAmida.right != null)
            {
                // �E�ɐi��
                Debug.Log("Moving RIGHT");
                FollowTheAmidaTube(neighborAmida.right, AmidaTube.Direction.RIGHT);
                return;
            }
            else if (neighborAmida.up != null)
            {
                // ��ɐi��
                Debug.Log("Moving UP");
                FollowTheAmidaTube(neighborAmida.up, AmidaTube.Direction.UP);
                return;
            }
        }

        if (prevFollowDir == AmidaTube.Direction.DOWN)
        {
            if (neighborAmida.right != null)
            {
                // �E�ɐi��
                Debug.Log("Moving RIGHT");
                FollowTheAmidaTube(neighborAmida.right, AmidaTube.Direction.RIGHT);
                return;
            }
 
            else if (neighborAmida.down != null)
            {
                // ���ɐi��
                Debug.Log("Moving DOWN");
                FollowTheAmidaTube(neighborAmida.down, AmidaTube.Direction.DOWN);
                return;
            }
        }

    }

    /// <summary>
    /// �z���̌^��ǉ����܂��B
    /// </summary>
    /// <param name="slot"></param>
    public void AddFeelingSlot(FeelingSlot slot)
    {

        if (slot == null)
        {
            Debug.LogError("AddFeelingSlot: slot is null");
            return;
        }
        m_feelingSlots.Add(slot);
        Debug.Log($"[AmidaManager] Added FeelingSlot: {slot.name}");
    }

    /// <summary>
    /// ���ׂĂ̂��݂��`���[�u�̃}�e���A����K�p���܂��B 
    /// </summary>
    void ApplyAllAmidaTubeMaterial()
    {
        var map = MapData.GetInstance;
        var stageGridData = map.GetStageGridData();
        foreach (var tile in map.GetStageGridData().GetTileData)
        {
            if (tile.amidaTube != null)
            {
                tile.amidaTube.ApplyMaterial();
            }
        }
        Debug.Log("[AmidaManager] All AmidaTube materials applied.");
    }

    /// <summary>
    /// ���ׂĂ̂��݂��`���[�u�̋���}�e���A����K�p
    /// </summary>
    private void ApplyAllAmidaTubeRejectionMaterial()
    {
        var map = MapData.GetInstance;
        var stageGridData = map.GetStageGridData();
        foreach (var tile in stageGridData.GetTileData)
        {
            if (tile.amidaTube != null)
            {
                tile.amidaTube.ApplyRejectionMaterial();
            }
        }
        Debug.Log("[AmidaManager] All AmidaTube materials set to rejection.");
    }

    /// <summary>
    /// ���ׂĂ̂��݂��`���[�u�̃^�C�v�����Z�b�g���܂��B
    /// </summary>
    void ResetAllAmidaTubeType()
    {
        var map = MapData.GetInstance;
        var stageGridData = map.GetStageGridData();
        foreach (var tile in stageGridData.GetTileData)
        {
            if (tile.amidaTube != null)
            {
                tile.amidaTube.ResetEmotionCurrentType();
            }
        }
        Debug.Log("[AmidaManager] All AmidaTube types reset.");
    }

    /// <summary>
    /// �Q�[�����̃C�x���g���󂯎�邽�߂̃C���^�[�t�F�[�X���\�b�h
    /// </summary>
    /// <param name="eventID"></param>
    public void  OnEvent(InteractionEvent eventID)
    {
        switch (eventID)
        {
            case InteractionEvent.CHANGED_AMIDAKUJI:
                // ���݂��f�[�^���ύX���ꂽ�ꍇ�̏���
                m_isFollowingAmida = true; // ���݂���H��t���O�𗧂Ă�
                break;
            case InteractionEvent.CONNECTED_REJECTION_SLOT:
                // �I�_�̋���̊j���ڑ����ꂽ�ꍇ�̏���
                m_connectedRejectionSlot = true; // �ڑ���Ԃ��X�V
                ApplyAllAmidaTubeRejectionMaterial(); // ����̊j�̃}�e���A����K�p
                break;
            case InteractionEvent.DISCONNECTED_REJECTION_SLOT:
                // �I�_�̋���̊j���ؒf���ꂽ�ꍇ�̏���
                m_connectedRejectionSlot = false; // �ڑ���Ԃ��X�V
                m_isFollowingAmida = true; // ���݂���H��t���O�𗧂Ă�
                break;
            case InteractionEvent.PUSH_FELTBLOCK:
                // �t�F���g�u���b�N�������ꂽ�ꍇ�̏���
                m_isFollowingAmida = true; // ���݂���H��t���O�𗧂Ă�
                break;
        }
    }

    // �폜��
    private void OnDestroy()
    {
        // �Q�[���C���^���N�V�����C�x���g�̃I�u�U�[�o�[������
        GameInteractionEventMessenger.GetInstance.RemoveObserver(this);
    }

}

