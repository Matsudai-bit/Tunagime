using UnityEngine;

/// <summary>
/// ステージのあみだ関連のユーティリティクラス
/// </summary>
public class StageAmidaUtility 
{
    /// <summary>
    /// 指定されたグリッド座標に対応するあみだチューブの状態をチェック。
    /// </summary>
    /// <param name="checkPos"></param>
    /// <param name="checkState"></param>
    /// <returns></returns>
    public static bool CheckAmidaState(GridPos checkPos, AmidaTube.State checkState)
    {
        // 指定されたグリッド座標に対応するあみだチューブを取得
        AmidaTube amidaTube = MapData.GetInstance.GetStageGridData().GetAmidaTube(checkPos);

        // あみだチューブが存在しない場合はfalseを返す
        if (amidaTube == null)
        {
            return false;
        }
        // あみだチューブの状態をチェック
        return amidaTube.GetState() == checkState;
    }

    public static AmidaTube FindClosestAmidaTube(Vector3 position)
    {
        var map = MapData.GetInstance;
        var gridData = map.GetStageGridData();

        // 全ての最近点のグリッド座標を取得
        GridPos centerGridPos = map.GetClosestGridPos(position);

        // チェックするグリッドの範囲を定義（中心とその周囲8つのグリッド）
        GridPos[] checkOffsets = new GridPos[]
        {
            new GridPos(0, 0),   // 中心
            new GridPos(-1, 0),  // 左
            new GridPos(1, 0),   // 右
            new GridPos(0, -1),  // 下
            new GridPos(0, 1),   // 上
            new GridPos(-1, -1), // 左下
            new GridPos(-1, 1),  // 左上
            new GridPos(1, -1),  // 右下
            new GridPos(1, 1)    // 右上
        };

        // それぞれのあみだチューブをチェックして最も近い(ワールド座標状)ものを見つける
        AmidaTube closestTube = null;
        float closestDistanceSqr = float.MaxValue;
        foreach (var offset in checkOffsets)
        {
            GridPos checkGridPos = centerGridPos + offset;
            AmidaTube tube = gridData.GetAmidaTube(checkGridPos);
            if (tube != null)
            {
                Vector3 tubeWorldPos = map.ConvertGridToWorldPos(checkGridPos);
                float distanceSqr = (position - tubeWorldPos).sqrMagnitude;
                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestTube = tube;
                }
            }
        }


        return closestTube;
    }
}
