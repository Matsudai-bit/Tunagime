using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ローディングシーンのコントローラー
/// </summary>
public class LoadingSceneController : MonoBehaviour
{
    [SerializeField] private GameObject m_loadingUI;
    [SerializeField] private Slider m_slider;

    [SerializeField] public string sceneName ;

    void Start()
    {
        // 次のシーン名を取得
        sceneName = LoadingSceneRequest.GetInstance.NextSceneName;
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        m_loadingUI.SetActive(true);
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("GameplayScene");
        while (!async.isDone)
        {
            m_slider.value = async.progress;
            yield return null;
        }
    }
}
