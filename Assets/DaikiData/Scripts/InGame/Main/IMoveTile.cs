using UnityEngine;

/// <summary>
/// タイル移動オブジェクトのインターフェース
/// </summary>
public interface IMoveTile 
{
    /// <summary>
    /// 移動要求
    /// </summary>
    /// <param name="velocity"></param>
    void RequestMove(GridPos velocity);


    /// <summary>
    /// 指定した方向に移動可能かどうかを判定
    /// </summary>
    /// <param name="moveDirection"></param>
    /// <returns></returns>
    bool CanMove(GridPos moveDirection);

    /// <summary>
    /// 移動用のTransformを取得
    /// </summary>
    /// <returns></returns>
    Transform GetMoveTransform();
}
