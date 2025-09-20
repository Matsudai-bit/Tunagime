using UnityEngine;


public class CoreMove : MoveTile
{
    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="velocity"></param>
    public override void Move(GridPos velocity)
    {
        var map = MapData.GetInstance; // MapDataのインスタンスを取得
        var stageGridData = map.GetStageGridData(); // ステージグリッドデータを取得

        var movementPosition = stageBlock.GetGridPos() + velocity; // 移動先の位置を計算

        // 配置予定箇所のタイルを取得
        var tile = stageGridData.GetTileObject(movementPosition);

        if (tile.stageBlock != null && tile.stageBlock.GetBlockType() == StageBlock.BlockType.FEELING_SLOT)
        {
            var feelingSlot = tile.gameObject.GetComponent<FeelingSlot>();
            if (feelingSlot != null)
            {
                // 想いの核を配置するスロットに感情タイプを設定
                feelingSlot.InsertCore(GetComponent<FeelingCore>());
                map.GetStageGridData().RemoveGridDataGameObject(stageBlock.GetGridPos());
                gameObject.SetActive(false); // オブジェクトを非表示にする
            }
            else
            {
                Debug.LogError("FeelingSlot コンポーネントが見つかりません。");
            }
            return;
        }

        gameObject.SetActive(true); // オブジェクトを表示する

        stageBlock.UpdatePosition(movementPosition); // StageBlockの位置を更新
        // グリッドデータに綿毛ボールを配置
        map.GetStageGridData().TryPlaceTileObject(movementPosition, gameObject);

        if (CanSlide()) ChangeState(State.SLIDE);
        else ChangeState(State.IDLE);
    }

    public override bool IsObstacleInPath(GridPos moveDirection)
    {
        // ステージブロックが移動可能かチェック
        GridPos currentGridPos = stageBlock.GetGridPos();

        GridPos targetGridPos = currentGridPos + moveDirection;

        // StageBlockのグリッド位置を取得
        MapData map = MapData.GetInstance; // マップデータを取得
        TileObject targetTileObject = map.GetStageGridData().GetTileObject(targetGridPos);

        if ((targetTileObject.stageBlock != null))
        {
            var feelingSlot = targetTileObject.gameObject.GetComponent<FeelingSlot>();
            if (feelingSlot != null && !feelingSlot.IsInsertCore())
            {

                return true;
            }
        }

     

        return targetTileObject.gameObject == null;
    }

}
