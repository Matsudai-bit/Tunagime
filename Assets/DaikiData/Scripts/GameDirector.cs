using UnityEngine;

public class GameDirector : MonoBehaviour
{

    [SerializeField] GameObject m_clearUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60; // 60fps�ɐݒ�

    }

    // Update is called once per frame
    void Update()
    {


        // Esc�L�[�������ꂽ��Q�[�����I��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("�Q�[�����I�����܂����B");
        }
    }

    // �Q�[�����N���A�������ɌĂ΂��
    public void OnGameClear()
    {
        // �N���AUI��\�����鏈��
        Debug.Log("�Q�[���N���A�I");
        // �����ɃQ�[���N���A�̏�����ǉ�
        m_clearUI.SetActive(true);
    }
}

