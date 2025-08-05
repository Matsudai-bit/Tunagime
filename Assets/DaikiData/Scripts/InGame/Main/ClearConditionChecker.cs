using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// クリア条件をチェックするクラス
/// </summary>
public class ClearConditionChecker : MonoBehaviour
{
    [Header("クリア条件の設定")]
    [SerializeField]
    private List<TerminusFeelingSlot> terminusFeelingSlots = new List<TerminusFeelingSlot>(); // 終点にある型のリスト

    private GameDirector m_gameDirector;

    private void Awake()
    {
        // GameDirectorのインスタンスを取得
        m_gameDirector = GetComponent<GameDirector>();
        if (m_gameDirector == null)
        {
            Debug.LogError("GameDirector instance not found in the scene.");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        // クリア条件のチェック
        if (CheckClearCondition())
        {
            Debug.Log("クリア条件を達成しました！");
            // クリア処理をここに追加
            if (m_gameDirector != null)
            {
                m_gameDirector.OnGameClear(); // ゲームクリアの処理を呼び出す
            }

        }

    }

    private bool CheckClearCondition()
    {
        // クリア条件を満たしているかどうかをチェック
        foreach (var slot in terminusFeelingSlots)
        {
            if (!slot.IsConnected())
            {
                return false; // 1つでも繋がっていない場合はクリア条件未達成
            }
        }
        return true; // 全ての終点が繋がっている場合はクリア条件達成
    }

    /// <summary>
    /// 終点にある型を追加するメソッド
    /// </summary>
    /// <param name="slot"></param>
    public void AddTerminusFeelingSlot(TerminusFeelingSlot slot) // いつかインターフェースを貰う形にしたい
    {
        if (!terminusFeelingSlots.Contains(slot))
        {
            terminusFeelingSlots.Add(slot);
        }
    }
}
