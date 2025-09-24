using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// サウンド管理クラス シングルトンクラス
/// </summary>
public class SoundManager : MonoBehaviour
{
    private static SoundManager s_instance = null;

    public  GameSoundData m_gameSoundData ;

    private Dictionary<SoundID, SoundData> m_soundDictionary = new();
    private List<AudioSource> m_audioSourceList = new();

    private AudioSource m_bgmAudioSource;

    /// <summary>
    /// 唯一のインスタンスにアクセスするためのプロパティ
    /// </summary>
    public static SoundManager GetInstance
    {
        get
        {
            // シーン上にインスタンスが存在しない場合
            if (s_instance == null)
            {
                // シーン内からSoundManagerを検索

                // それでも見つからない場合
                if (s_instance == null)
                {
                    // 新しいGameObjectを作成し、SoundManagerコンポーネントを追加する
                    GameObject singletonObject = new GameObject(typeof(SoundManager).Name);
                    s_instance = singletonObject.AddComponent<SoundManager>();
                    Debug.Log($"[SoundManager] シングルトンを生成しました: {singletonObject.name}");
                }
            }
            return s_instance;
        }
    }

    private void Awake()
    {
        // 既にインスタンスが存在する場合
        if (s_instance != null && s_instance != this)
        {
            // このインスタンスを破棄して、重複を避ける
            Destroy(this.gameObject);
            return;
        }

        // シーン上の唯一のインスタンスとして自身を登録
        s_instance = this;

        // シーンを跨いで存続させる
        DontDestroyOnLoad(this.gameObject);

        Initialize();

    }

    private void Start()
    {
        // 初期化
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        ////auidioSourceList配列の数だけAudioSourceを自分自身に生成して配列に格納
        //for (var i = 0; i < m_audioSourceList.Count; ++i)
        //{
        //    m_audioSourceList[i] = gameObject.AddComponent<AudioSource>();
        //}

        // サウンドデータのインスタンスを取得
        m_gameSoundData = GameSoundData.GetInstance; 

        // サウンドデータを辞書に変換
        foreach (var soundData in m_gameSoundData.soundData)
        {
            m_soundDictionary.Add(soundData.id, soundData);
        }
    }

    //未使用のAudioSourceの取得 全て使用中の場合はnullを返却
    private AudioSource GetUnusedAudioSource()
    {
        for (var i = 0; i < m_audioSourceList.Count; ++i)
        {
            if (m_audioSourceList[i].isPlaying == false) return m_audioSourceList[i];
        }

        // 見つからなかった場合生成する
        var audioSource = gameObject.AddComponent<AudioSource>();
        m_audioSourceList.Add(audioSource);

        return audioSource; //未使用のAudioSourceは見つかりませんでした
    }

    //指定されたAudioClipを未使用のAudioSourceで再生
    private void Play(AudioClip clip)
    {
        var audioSource = GetUnusedAudioSource();
        if (audioSource == null) return; //再生できませんでした
        audioSource.clip = clip;
        audioSource.Play();
    }

    //指定された別名で登録されたAudioClipを再生
    public void PlayBGM(SoundID id)
    {
        if (m_soundDictionary.TryGetValue(id, out var soundData)) //管理用Dictionary から、別名で探索
        {
            // BGM用のAudioSourceが存在しない場合は生成
            if (m_bgmAudioSource == null)
            {
                m_bgmAudioSource = gameObject.AddComponent<AudioSource>();
            }

            // BGM用のAudioSourceを設定して再生
            m_bgmAudioSource.clip = soundData.clip;
            m_bgmAudioSource.loop = true;
            m_bgmAudioSource.volume = 0.2f;
            m_bgmAudioSource.Play();
            //Play(soundData.clip); //見つかったら、再生
        }
        else
        {
            Debug.LogWarning($"その別名は登録されていません:{name}");
        }
    }

    public void PlaySE(SoundID id)
    {
        if (m_soundDictionary.TryGetValue(id, out var soundData)) //管理用Dictionary から、別名で探索
        {
 
            Play(soundData.clip); //見つかったら、再生

        }
        else
        {
            Debug.LogWarning($"その別名は登録されていません:{name}");
        }
    }
}
