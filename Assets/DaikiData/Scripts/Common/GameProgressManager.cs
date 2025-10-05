using UnityEngine;

/// <summary>
/// ゲームの進行状況を管理するクラス シングルトン
/// </summary>
public class GameProgressManager 
{
    GameProgressData m_gameProgressData ;

    private GameProgressManager()
    {
        m_gameProgressData = new();
    }

    public GameProgressData GameProgressData
    {
        get { return m_gameProgressData; }
        set { m_gameProgressData = value; }
    }

    // シングルトンインスタンス
    private static GameProgressManager s_instance;

    // シングルトンインスタンスのプロパティ
    public static GameProgressManager Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new GameProgressManager();
                if (s_instance.m_gameProgressData == null)
                {
                    Debug.LogError("GameProgressDataが見つかりません。Resourcesフォルダに配置してください。");
                }
            }
            return s_instance;
        }
    }

}
