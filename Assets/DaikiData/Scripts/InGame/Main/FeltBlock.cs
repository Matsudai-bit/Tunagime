using UnityEngine;


/// <summary>
/// フェルトブロック
/// </summary>
public class FeltBlock : MonoBehaviour
{
    private StageBlock m_stageBlock; // ステージブロック

    private MeshRenderer m_meshRenderer = null; // メッシュレンダラー

    [SerializeField]
    private GameObject m_model; // フェルトブロックのモデル

    /// <summary>
    /// Awake is called when the script instance is being loaded
    /// </summary>
    void Awake()
    {
        m_stageBlock = GetComponent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("FeltBlock requires a StageBlock component.");
        }

        m_meshRenderer = m_model.GetComponent<MeshRenderer>();
        if (m_meshRenderer == null)
        {
            Debug.LogError("FeltBlock requires a MeshRenderer component.");
        }
    }

    /// <summary>
    /// ステージブロックを取得します。
    /// </summary>
    public StageBlock stageBlock
    {
        get { return m_stageBlock; }
    }

    /// <summary>
    /// フェルトブロックを移動する
    /// </summary>
    /// <param name="velocity"></param>
    public void Move(GridPos velocity)
    {
        GridPos newGridPos = m_stageBlock.GetGridPos() + velocity;

        // ステージブロックの位置を更新
        m_stageBlock.UpdatePosition(newGridPos);
    }

    public MeshRenderer meshRenderer
    {
        get { return m_meshRenderer; }
        set { m_meshRenderer = value; }
    }

}
