
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
public class AmidaManager : MonoBehaviour , IGameInteractionObserver
{

    private List<FeelingSlot> m_feelingSlots = new List<FeelingSlot>(); // 想いの型のリスト

    // 遅延で辿るためのフラグ
    private bool m_isFollowingAmida = false;

    private bool m_connectedRejectionSlot = false; // 終点の拒絶の核が接続されているかどうか


    private void Awake()
    {

    }

    /// <summary>
    /// ゲーム開始時の初期化処理。あみだチューブの作成を行います。
    /// </summary>
    void Start()
    {
        GameInteractionEventMessenger.GetInstance.RegisterObserver(this); // イベントを受け取るためにオブザーバーを登録
        // あみだを辿る
        m_isFollowingAmida = true;

        m_connectedRejectionSlot = false; // 初期状態では接続されていない
    }

    /// <summary>
    /// 更新処理。
    /// </summary>
    void Update()
    {
        var map = MapData.GetInstance;
        var stageGridData = map.GetStageGridData();

        int LOOP_COUNT = 1; // あみだチューブを辿る回数

        // あみだチューブを辿る処理
        if (m_isFollowingAmida || Input.GetKeyDown(KeyCode.Space))
        {
            // すべてのあみだチューブのタイプをリセット
            ResetAllAmidaTubeType();

            // あみだチューブを辿る回数処理を実行
            for (int i = 0; i < LOOP_COUNT; i++) 
            {
                foreach (var slot in m_feelingSlots)
                {
                    // スタート位置
                    var startPos = slot.StageBlock.GetGridPos() + new GridPos(1, 0);
                    // あみだチューブの取得
                    var startAmidaTube = stageGridData.GetAmidaTube(startPos);
                    // 想いの種類の取得
                    var emotionType = slot.GetEmotionType();

                    // 先頭のあみだチューブの設定
                    startAmidaTube.SetEmotionCurrentType(YarnMaterialGetter.MaterialType.INPUT, emotionType); // スロットのマテリアルを設定
                    startAmidaTube.SetEmotionCurrentType(YarnMaterialGetter.MaterialType.OUTPUT, emotionType); // スロットのマテリアルを設定

                    // あみだチューブを辿る処理
                    FollowTheAmidaTube(startAmidaTube, AmidaTube.Direction.RIGHT);

                }

            }

            // 辿ることが終了したことを通知する (呼び出しタイミング大事)
            GameInteractionEventMessenger.GetInstance.Notify(InteractionEvent.FLOWWING_AMIDAKUJI); // あみだくじが変更されたことを通知

            if (m_connectedRejectionSlot)
            {
                // 終点の拒絶の核が接続されている場合は、拒絶の核のマテリアルを適用
                ApplyAllAmidaTubeRejectionMaterial();
            }
            else
            {
                // 終点の拒絶の核が接続されていない場合は、通常のマテリアルを適用
                ApplyAllAmidaTubeMaterial();
            }



            m_isFollowingAmida = false; // フラグをリセット


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

        MapData map = MapData.GetInstance;
        var stageGridData = map.GetStageGridData();

        TileObject tileObject = stageGridData.GetTileData[followAmida.GetGridPos().y, followAmida.GetGridPos().x].tileObject;


        // フェルトブロックがある場合は処理を終了
        if (tileObject.stageBlock != null )
        {
            if ( tileObject.stageBlock.GetBlockType() == StageBlock.BlockType.FELT_BLOCK)
                return;
        }


        // 次に進む方向を決定
        // 流れ : 現在のあみだが進むことのできる方向を取得　->　曲がる方向があればそっちを優先的に選択 -> その方向にあみだがあるかの確認

        followAmida.UpdateMeshMaterialsBasedOnAmidaState(prevFollowDir);

        var followDir = followAmida.GetFollowDirection();

        var neighborAmida = followAmida.GetNeighbor();




        // 優先する方向を先に確認
        if (prevFollowDir == AmidaTube.Direction.RIGHT)
        {
            if (neighborAmida.up != null)
            {
                // 上に進む
                //Debug.Log("Moving UP");
                FollowTheAmidaTube(neighborAmida.up, followDir);
                return;
            }
            else if (neighborAmida.down != null)
            {
                // 下に進む
               // Debug.Log("Moving DOWN");
                FollowTheAmidaTube(neighborAmida.down, followDir);
                return;
            }
            else if (neighborAmida.right != null)
            {
                // 右に進む
                //Debug.Log("Moving RIGHT");
                FollowTheAmidaTube(neighborAmida.right, followDir);
                return;
            }

        }

        if (prevFollowDir == AmidaTube.Direction.UP)
        {
            if (neighborAmida.right != null)
            {
                // 右に進む
                //Debug.Log("Moving RIGHT");
                FollowTheAmidaTube(neighborAmida.right, AmidaTube.Direction.RIGHT);
                return;
            }
            else if (neighborAmida.up != null)
            {
                // 上に進む
                //Debug.Log("Moving UP");
                FollowTheAmidaTube(neighborAmida.up, AmidaTube.Direction.UP);
                return;
            }
        }

        if (prevFollowDir == AmidaTube.Direction.DOWN)
        {
            if (neighborAmida.right != null)
            {
                // 右に進む
                //Debug.Log("Moving RIGHT");
                FollowTheAmidaTube(neighborAmida.right, AmidaTube.Direction.RIGHT);
                return;
            }
 
            else if (neighborAmida.down != null)
            {
                // 下に進む
                //Debug.Log("Moving DOWN");
                FollowTheAmidaTube(neighborAmida.down, AmidaTube.Direction.DOWN);
                return;
            }
        }

    }

    /// <summary>
    /// 想いの型を追加します。
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
        //Debug.Log($"[AmidaManager] Added FeelingSlot: {slot.name}");
    }

    /// <summary>
    /// すべてのあみだチューブのマテリアルを適用します。 
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
        //Debug.Log("[AmidaManager] All AmidaTube materials applied.");
    }

    /// <summary>
    /// すべてのあみだチューブの拒絶マテリアルを適用
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
        //Debug.Log("[AmidaManager] All AmidaTube materials set to rejection.");
    }

    /// <summary>
    /// すべてのあみだチューブのタイプをリセットします。
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
        //Debug.Log("[AmidaManager] All AmidaTube types reset.");
    }

    /// <summary>
    /// ゲーム内のイベントを受け取るためのインターフェースメソッド
    /// </summary>
    /// <param name="eventID"></param>
    public void  OnEvent(InteractionEvent eventID)
    {
        switch (eventID)
        {
            case InteractionEvent.CHANGED_AMIDAKUJI:
                // あみだデータが変更された場合の処理
                m_isFollowingAmida = true; // あみだを辿るフラグを立てる
                break;
            case InteractionEvent.CONNECTED_REJECTION_SLOT:
                // 終点の拒絶の核が接続された場合の処理
                m_connectedRejectionSlot = true; // 接続状態を更新
                ApplyAllAmidaTubeRejectionMaterial(); // 拒絶の核のマテリアルを適用
                break;
            case InteractionEvent.DISCONNECTED_REJECTION_SLOT:
                // 終点の拒絶の核が切断された場合の処理
                m_connectedRejectionSlot = false; // 接続状態を更新
                m_isFollowingAmida = true; // あみだを辿るフラグを立てる
                break;
            case InteractionEvent.PUSH_FELTBLOCK:
                // フェルトブロックが押された場合の処理
                m_isFollowingAmida = true; // あみだを辿るフラグを立てる
                break;
        }
    }

    // 削除時
    private void OnDestroy()
    {
        // ゲームインタラクションイベントのオブザーバーを解除
        GameInteractionEventMessenger.GetInstance.RemoveObserver(this);
    }

}

