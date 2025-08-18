using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// サテン床
/// </summary>
public class SatinFloor : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> m_satinModels = new List<GameObject>(); // サテンのモデルのリスト

    [SerializeField]
    private GameObject m_currentModel; // 現在のサテンモデル

    private void Awake()
    {
        // 初期化処理
        if (m_satinModels == null || m_satinModels.Count == 0)
        {
            Debug.LogError("Satin models list is empty or not assigned.");
        }

        // ランダムでモデルを選択
        int randomIndex = Random.Range(0, m_satinModels.Count);
        GameObject selectedModel = m_satinModels[randomIndex];
        // 選択したモデルをインスタンス化
        GameObject instance = Instantiate(selectedModel,  transform.position, transform.rotation, transform);

        // 現在のモデルを設定
        m_currentModel = instance;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
