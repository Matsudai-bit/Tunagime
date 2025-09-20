using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ペアワッペン
/// </summary>
public class PairBadge : MonoBehaviour
{

    private List<FeltBlock> m_feltBlocks = new List<FeltBlock>(); // 所属するフェルトブロックのリスト

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public void Initialize(List<FeltBlock> feltBlocks)
    {
        m_feltBlocks.Clear();
        m_feltBlocks.AddRange(feltBlocks);

        foreach (var feltBlock in m_feltBlocks)
        {
            feltBlock.GetComponent<FeltBlockMove>().SetPairBadge(this); // ペアワッペンを設定するメソッドを呼び出す

        }        
    }

    public void Move(GridPos velocity)
    {
        foreach (var feltBlock in m_feltBlocks)
        {
            feltBlock.GetComponent<FeltBlockMove>().Move(velocity); // 各フェルトブロックを移動
        }
    }

    public bool CanMove(GridPos moveDirection)
    {

        return m_feltBlocks.TrueForAll(feltBlock => feltBlock.GetComponent<FeltBlockMove>().IsObstacleInPath(moveDirection));
    }

    public bool CanSlide()
    {
        // すべてのフェルトブロックがスライド可能かチェック
        return m_feltBlocks.TrueForAll(feltBlock => feltBlock.GetComponent<FeltBlockMove>().IsSlippery());
    }

    public void Slide(GridPos velocity)
    {
        foreach (var feltBlock in m_feltBlocks)
        {
            feltBlock.GetComponent<FeltBlockMove>().StartSlide(velocity); // 各フェルトブロックをスライド
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<FeltBlock> GetFeltBlocks()
    {
        return m_feltBlocks;
    }
}
