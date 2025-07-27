
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 方向
/// </summary>
public enum Direction
{
    UP,
    DOWN,
    RIGHT,
}

/// <summary>
/// あみだくじの管理クラス。あみだくじのパイプ生成や移動処理、色の変更などを管理します。
/// </summary>
public class AmidaManager : MonoBehaviour
{

    // 遅延で辿るためのフラグ
    private bool m_isFollowingAmida = false;



    private void Awake()
    {

    }

    /// <summary>
    /// ゲーム開始時の初期化処理。あみだチューブの作成を行います。
    /// </summary>
    void Start()
    {


    }

    /// <summary>
    /// 更新処理。
    /// </summary>
    void Update()
    {
        var map = MapData.GetInstance;
        var stageGridData = map.GetStageGridData();

        

        // あみだチューブを辿る処理
        if (m_isFollowingAmida || Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < 2; i++)
            {
                // あみだチューブを辿る処理
                FollowTheAmidaTube(stageGridData.GetAmidaTube(0, 1), AmidaTube.Direction.RIGHT);
                FollowTheAmidaTube(stageGridData.GetAmidaTube(0, 3), AmidaTube.Direction.RIGHT);
                FollowTheAmidaTube(stageGridData.GetAmidaTube(0, 5), AmidaTube.Direction.RIGHT);
            }

            m_isFollowingAmida = false; // フラグをリセット
        }

        if (stageGridData.IsAmidaDataChanged())
        {
            m_isFollowingAmida = true;


        }

        stageGridData.ResetAmidaDataChanged();

    }

    /// <summary>
    /// 指定したあみだチューブを辿る
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

        // 次に進む方向を決定

        // 流れ : 現在のあみだが進むことのできる方向を取得　->　曲がる方向があればそっちを優先的に選択 -> その方向にあみだがあるかの確認

        followAmida.ChangeMaterial(prevFollowDir);

        var followDir = followAmida.GetFollowDirection();

        var neighborAmida = followAmida.GetNeighbor();

        // 優先する方向を先に確認
        if (prevFollowDir == AmidaTube.Direction.RIGHT)
        {
            if (neighborAmida.up != null)
            {
                // 上に進む
                Debug.Log("Moving UP");
                FollowTheAmidaTube(neighborAmida.up, followDir);
                return;
            }
            else if (neighborAmida.down != null)
            {
                // 下に進む
                Debug.Log("Moving DOWN");
                FollowTheAmidaTube(neighborAmida.down, followDir);
                return;
            }
            else if (neighborAmida.right != null)
            {
                // 左に進む
                Debug.Log("Moving RIGHT");
                FollowTheAmidaTube(neighborAmida.right, followDir);
                return;
            }

        }

        if (prevFollowDir == AmidaTube.Direction.UP)
        {
            if (neighborAmida.right != null)
            {
                // 右に進む
                Debug.Log("Moving RIGHT");
                FollowTheAmidaTube(neighborAmida.right, AmidaTube.Direction.RIGHT);
                return;
            }
            else if (neighborAmida.down != null)
            {
                // 下に進む
                Debug.Log("Moving DOWN");
                FollowTheAmidaTube(neighborAmida.down, AmidaTube.Direction.DOWN);
                return;
            }

            else if (neighborAmida.right != null)
            {
                // 左に進む
                Debug.Log("Moving Right");
                FollowTheAmidaTube(neighborAmida.right, AmidaTube.Direction.RIGHT);
                return;
            }
        }

        if (prevFollowDir == AmidaTube.Direction.DOWN)
        {
            if (neighborAmida.right != null)
            {
                // 右に進む
                Debug.Log("Moving RIGHT");
                FollowTheAmidaTube(neighborAmida.right, AmidaTube.Direction.RIGHT);
                return;
            }
            else if (neighborAmida.up != null)
            {
                // 上に進む
                Debug.Log("Moving UP");
                FollowTheAmidaTube(neighborAmida.up, AmidaTube.Direction.UP);
                return;
            }
            else if (neighborAmida.right != null)
            {
                // 左に進む
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

