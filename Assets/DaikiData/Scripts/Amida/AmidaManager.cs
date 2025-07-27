
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
public class AmidaManager : MonoBehaviour
{

    // �x���ŒH�邽�߂̃t���O
    private bool m_isFollowingAmida = false;



    private void Awake()
    {

    }

    /// <summary>
    /// �Q�[���J�n���̏����������B���݂��`���[�u�̍쐬���s���܂��B
    /// </summary>
    void Start()
    {


    }

    /// <summary>
    /// �X�V�����B
    /// </summary>
    void Update()
    {
        var map = MapData.GetInstance;
        var stageGridData = map.GetStageGridData();

        

        // ���݂��`���[�u��H�鏈��
        if (m_isFollowingAmida || Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < 2; i++)
            {
                // ���݂��`���[�u��H�鏈��
                FollowTheAmidaTube(stageGridData.GetAmidaTube(0, 1), AmidaTube.Direction.RIGHT);
                FollowTheAmidaTube(stageGridData.GetAmidaTube(0, 3), AmidaTube.Direction.RIGHT);
                FollowTheAmidaTube(stageGridData.GetAmidaTube(0, 5), AmidaTube.Direction.RIGHT);
            }

            m_isFollowingAmida = false; // �t���O�����Z�b�g
        }

        if (stageGridData.IsAmidaDataChanged())
        {
            m_isFollowingAmida = true;


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

        // ���ɐi�ޕ���������

        // ���� : ���݂̂��݂����i�ނ��Ƃ̂ł���������擾�@->�@�Ȃ������������΂�������D��I�ɑI�� -> ���̕����ɂ��݂������邩�̊m�F

        followAmida.ChangeMaterial(prevFollowDir);

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
                // ���ɐi��
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
            else if (neighborAmida.down != null)
            {
                // ���ɐi��
                Debug.Log("Moving DOWN");
                FollowTheAmidaTube(neighborAmida.down, AmidaTube.Direction.DOWN);
                return;
            }

            else if (neighborAmida.right != null)
            {
                // ���ɐi��
                Debug.Log("Moving Right");
                FollowTheAmidaTube(neighborAmida.right, AmidaTube.Direction.RIGHT);
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
            else if (neighborAmida.up != null)
            {
                // ��ɐi��
                Debug.Log("Moving UP");
                FollowTheAmidaTube(neighborAmida.up, AmidaTube.Direction.UP);
                return;
            }
            else if (neighborAmida.right != null)
            {
                // ���ɐi��
                Debug.Log("Moving RIGHT");
                FollowTheAmidaTube(neighborAmida.right, AmidaTube.Direction.RIGHT);
                return;
            }
        }







        //var nextFollowAmida = followAmida.GetNeighbor(prevFollowDir);

        //if (nextFollowAmida == null)
        //{
        //    Debug.LogError("FollowTheAmidaTube: nextFollowAmida is null");
        //    return;
        //}

    }


}

