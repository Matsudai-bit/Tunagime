using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

/// <summary>
///  ロゴシーンのコントローラー
/// </summary>
public class LogoSceneController : MonoBehaviour
{
    [Header("ロゴムービ")]
    [SerializeField]
    private VideoPlayer m_logoVideoPlayer;

    private void Awake()
    {
        if (m_logoVideoPlayer != null && m_logoVideoPlayer.isPlaying == false)
        {
            m_logoVideoPlayer.Play();
        }
        else
        {
            Debug.LogError("LogoSceneController: Logo Video Player is not assigned.");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        // ロゴムービーの再生が終了したら次のシーンへ遷移する処理をここに追加
        if (m_logoVideoPlayer != null && !m_logoVideoPlayer.isPlaying)
        {
            SceneManager.LoadScene("TitleScene");

        }
    }
}
