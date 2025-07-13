using UnityEngine;

public class GameDirector : MonoBehaviour
{
    private int m_hasItemNum;   // 所持クリアアイテム数

    [SerializeField] GameObject m_clearUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60; // 60fpsに設定

    }

    // Update is called once per frame
    void Update()
    {
        if (m_hasItemNum >= 2)
            m_clearUI.SetActive(true);
    }

    public void AddHasItemNum()
    {
        m_hasItemNum++;
    }
}
