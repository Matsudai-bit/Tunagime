using DG.Tweening;
using UnityEngine;

/// <summary>
/// タイル移動
/// </summary>
public class FeltBlockMove 
    : MoveTile
{

    private PairBadge m_pairBadge; // ペアワッペン


    public override bool CanMove(GridPos moveDirection)
    { 
        // ペアワッペンがある場合はペアワッペンの移動可能かチェック
        if (m_pairBadge != null)
        {
            return m_pairBadge.CanMove(moveDirection);
        }

        return IsObstacleInPath(moveDirection); // ペアワッペンがない場合は自分自身の移動可能かチェック
    }


    /// <summary>
    /// フェルトブロックを移動する
    /// </summary>
    /// <param name="velocity"></param>
    public override void RequestMove(GridPos velocity)
    {
        if (m_pairBadge != null)
        {
            // ペアワッペンがある場合はペアワッペンに移動を依頼
            m_pairBadge.Move(velocity);
            return;
        }

        Move(velocity); // ペアワッペンがない場合は自分自身を移動

    }

    public override void StartMove()
    {
        if (m_pairBadge)
        {
            // ペアワッペンがある場合はペアワッペンの移動を開始
            m_pairBadge.StartMove();
            return;
        }

        ChangeState(State.MOVE);
    }

    public override bool CanSlide()
    {
        if (m_pairBadge != null)
        {
            // ペアワッペンがある場合はペアワッペンのスライド可能かチェック
            return m_pairBadge.CanSlide();
        }

        return IsSlippery(); // ペアワッペンがない場合は自分自身のスライド可能かチェック
    }

    public void SetPairBadge(PairBadge pairBadge)
    {
        m_pairBadge = pairBadge;
        transform.SetParent(pairBadge.transform); // ペアワッペンの子として設定
    }

    public override void RequestSlide(GridPos velocity)
    {
        //if (m_pairBadge != null)
        //{
        //    // ペアワッペンがある場合はペアワッペンにスライドを依頼
        //    m_pairBadge.Slide(velocity);
        //    return;
        //}
        StartSlide(velocity); // ペアワッペンがない場合は自分自身をスライド
    }

    /// <summary>
    /// 移動するTransformを取得
    /// </summary>
    /// <returns></returns>
    public override Transform GetMoveTransform()
    {
        if (m_pairBadge != null)
        {
            // ペアワッペンがある場合はペアワッペンのTransformを返す
            return m_pairBadge.transform;
        }
        return transform;
    }

}
