using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ワールド選択ボタン
/// </summary>
public class WorldSelectButton : MonoBehaviour
{
    [SerializeField]
    private List<WorldStageData> m_worldStageData;  // ワールドステージデータのリスト

    [SerializeField]
    private StageSelectButton m_stageSelectButton;

    [SerializeField]
    private GameObject m_stageSelectWindow;

    public void OnClickStage_n_1()
    {
        m_stageSelectButton.stageSettings = m_worldStageData[0].stageSettings.ToArray();
        ShowStageSelectWindow();
    }

    public void OnClickStage_n_2()
    {
        m_stageSelectButton.stageSettings = m_worldStageData[1].stageSettings.ToArray();
        ShowStageSelectWindow();

    }
    public void OnClickStage_n_3()
    {
        m_stageSelectButton.stageSettings = m_worldStageData[2].stageSettings.ToArray();
        ShowStageSelectWindow();
    }
    public void OnClickStage_n_4()
    {
        m_stageSelectButton.stageSettings = m_worldStageData[3].stageSettings.ToArray();
        ShowStageSelectWindow();
    }
    public void OnClickStage_n_5()
    {
        m_stageSelectButton.stageSettings = m_worldStageData[4].stageSettings.ToArray();
        ShowStageSelectWindow();
    }

    private void ShowStageSelectWindow()
    {
        m_stageSelectWindow.SetActive(true);
        m_stageSelectButton.Start();
    }
}
