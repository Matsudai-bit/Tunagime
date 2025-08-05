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

}
