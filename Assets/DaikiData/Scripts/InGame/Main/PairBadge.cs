using UnityEngine;

/// <summary>
/// ペアワッペン
/// </summary>
public class PairBadge : MonoBehaviour
{
    private FeltBlock m_feltBlock_A; // 所属するフェルトブロックA
    private FeltBlock m_feltBlock_B;   // 所属するフェルトブロックB

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Initialize(FeltBlock feltBlockA, FeltBlock feltBlockB)
    {
        m_feltBlock_A = feltBlockA;
        m_feltBlock_B = feltBlockB;
        // ここで必要な初期化処理を追加
    }

    public bool CanMove(GridPos moveDirection)
    {
        // ペアワッペンが移動可能かどうかを判断するロジックを実装
        // 例えば、両方のフェルトブロックが同じ方向に移動できるかどうかをチェックする
        return m_feltBlock_A.CanMove(moveDirection) && m_feltBlock_B.CanMove(moveDirection);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
