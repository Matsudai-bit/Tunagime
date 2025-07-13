using UnityEngine;

public class GameDirector : MonoBehaviour
{
    private int m_hasItemNum;   // �����N���A�A�C�e����

    [SerializeField] GameObject m_clearUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60; // 60fps�ɐݒ�

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
