using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ステージ選択シーンの管理クラス
/// </summary>
public class StageSelectScene : MonoBehaviour
{
    [SerializeField] private GameObject m_stage1_1; // ステージ1-1のプレハブ
    [SerializeField] private GameObject m_stage1_2; // ステージ1-2のプレハブ
    [SerializeField] private GameObject m_stage1_3; // ステージ1-3のプレハブ
    [SerializeField] private GameObject m_stage1_4; // ステージ1-4のプレハブ
    [SerializeField] private GameObject m_stage1_5; // ステージ1-5のプレハブ

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
        PlayGameStage(); // ステージ選択後にゲームプレイシーンへ遷移
    }

    public void OnClickStage1_2()
    {
        MapData.GetInstance.SetStageGenerator(m_stage1_2);
        PlayGameStage(); // ステージ選択後にゲームプレイシーンへ遷移

    }
    public void OnClickStage1_3()
    {
        MapData.GetInstance.SetStageGenerator(m_stage1_3);
        PlayGameStage(); // ステージ選択後にゲームプレイシーンへ遷移

    }
    public void OnClickStage1_4()
    {
        MapData.GetInstance.SetStageGenerator(m_stage1_4);
        PlayGameStage(); // ステージ選択後にゲームプレイシーンへ遷移

    }
    public void OnClickStage1_5()
    {
        MapData.GetInstance.SetStageGenerator(m_stage1_5);
        PlayGameStage(); // ステージ選択後にゲームプレイシーンへ遷移

    }

    private void PlayGameStage()
    {
        SceneManager.LoadScene("GameplayScene");
    }
}
