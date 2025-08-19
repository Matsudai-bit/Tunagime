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

    public void Initialize(List<FeltBlock> feltBlocks)
    {
        m_feltBlocks.Clear();
        m_feltBlocks.AddRange(feltBlocks);

        foreach (var feltBlock in m_feltBlocks)
        {
            feltBlock.gameObject.transform.SetParent(this.transform); // フェルトブロックの親をペアワッペンに設定
            feltBlock.SetPairBadge(this); // ペアワッペンを設定するメソッドを呼び出す

        }

        // ここで必要な初期化処理を追加
        
    }

    public void Move(GridPos velocity)
    {
        foreach (var feltBlock in m_feltBlocks)
        {
            feltBlock.Move(velocity); // 各フェルトブロックを移動
        }
    }

    public bool CanMove(GridPos moveDirection)
    {

        return m_feltBlocks.TrueForAll(feltBlock => feltBlock.CanMove(moveDirection));
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
