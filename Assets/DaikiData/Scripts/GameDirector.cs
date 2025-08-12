using UnityEngine;

public class GameDirector : MonoBehaviour
{

    [SerializeField] GameObject m_clearUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60; // 60fpsに設定

    }

    // Update is called once per frame
    void Update()
    {


        // Escキーが押されたらゲームを終了
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("ゲームを終了しました。");
        }

        // Tabが押されたらステージ選択にいく
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // タイトルシーンに遷移する処理
            UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelectScene");
            Debug.Log("タイトルシーンに戻ります。");
        }
    }

    // ゲームがクリアした時に呼ばれる
    public void OnGameClear()
    {
        // クリアUIを表示する処理
        Debug.Log("ゲームクリア！");
        // ここにゲームクリアの処理を追加
        m_clearUI.SetActive(true);
    }
}

