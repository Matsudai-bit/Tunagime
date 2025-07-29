using UnityEngine;

/// <summary>
/// 終点にある型
/// </summary>
public class TerminusFeelingSlot : MonoBehaviour
{
    private StageBlock m_stageBlock;    // ステージブロック

    private void Awake()
    {
        // ステージブロックを取得
        m_stageBlock = GetComponent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("TerminusFeelingSlot requires a StageBlock component.");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 繋がっているかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsConnected()
    {


        // ステージブロックが存在し、接続されているかどうかを確認
        //return m_stageBlock != null && m_stageBlock.IsConnected();

        return true;
    }
}
