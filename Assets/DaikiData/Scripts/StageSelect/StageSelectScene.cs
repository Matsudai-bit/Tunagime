using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �X�e�[�W�I���V�[���̊Ǘ��N���X
/// </summary>
public class StageSelectScene : MonoBehaviour
{
    [SerializeField] private GameObject m_stage1_1; // �X�e�[�W1-1�̃v���n�u
    [SerializeField] private GameObject m_stage1_2; // �X�e�[�W1-2�̃v���n�u
    [SerializeField] private GameObject m_stage1_3; // �X�e�[�W1-3�̃v���n�u
    [SerializeField] private GameObject m_stage1_4; // �X�e�[�W1-4�̃v���n�u
    [SerializeField] private GameObject m_stage1_5; // �X�e�[�W1-5�̃v���n�u

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickStage1_1()
    {
        MapData.GetInstance.SetStageGenerator(m_stage1_1);
        PlayGameStage(); // �X�e�[�W�I����ɃQ�[���v���C�V�[���֑J��
    }

    public void OnClickStage1_2()
    {
        MapData.GetInstance.SetStageGenerator(m_stage1_2);
        PlayGameStage(); // �X�e�[�W�I����ɃQ�[���v���C�V�[���֑J��

    }
    public void OnClickStage1_3()
    {
        MapData.GetInstance.SetStageGenerator(m_stage1_3);
        PlayGameStage(); // �X�e�[�W�I����ɃQ�[���v���C�V�[���֑J��

    }
    public void OnClickStage1_4()
    {
        MapData.GetInstance.SetStageGenerator(m_stage1_4);
        PlayGameStage(); // �X�e�[�W�I����ɃQ�[���v���C�V�[���֑J��

    }
    public void OnClickStage1_5()
    {
        MapData.GetInstance.SetStageGenerator(m_stage1_5);
        PlayGameStage(); // �X�e�[�W�I����ɃQ�[���v���C�V�[���֑J��

    }

    private void PlayGameStage()
    {
        SceneManager.LoadScene("GameplayScene");
    }
}
