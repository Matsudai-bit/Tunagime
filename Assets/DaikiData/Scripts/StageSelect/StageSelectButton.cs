using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    public StageSetting[] stageSettings ;

    public List<Button> stageButtons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        for (int i = 0; i < stageButtons.Count; i++)
        {
            if (stageSettings[i])
            {
                stageButtons[i].interactable = true; // ステージ設定が存在する場合、ボタンを有効化
            }
            else
            {
                stageButtons[i].interactable = false; // ステージ設定が存在しない場合、ボタンを無効化
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnClickStage_n_1()
    {
        MapData.GetInstance.SetStageSetting(stageSettings[0]);
        PlayGameStage(); // ステージ選択後にゲームプレイシーンへ遷移
    }

    public void OnClickStage_n_2()
    {
        MapData.GetInstance.SetStageSetting(stageSettings[1]);
        PlayGameStage(); // ステージ選択後にゲームプレイシーンへ遷移

    }
    public void OnClickStage_n_3()
    {
        MapData.GetInstance.SetStageSetting(stageSettings[2]);
        PlayGameStage(); // ステージ選択後にゲームプレイシーンへ遷移

    }
    public void OnClickStage_n_4()
    {
        MapData.GetInstance.SetStageSetting(stageSettings[3]);
        PlayGameStage(); // ステージ選択後にゲームプレイシーンへ遷移

    }
    public void OnClickStage_n_5()
    {
        MapData.GetInstance.SetStageSetting(stageSettings[4]);
        PlayGameStage(); // ステージ選択後にゲームプレイシーンへ遷移

    }

    private void PlayGameStage()
    {

        SceneManager.LoadScene("GameplayScene");
    }
}
