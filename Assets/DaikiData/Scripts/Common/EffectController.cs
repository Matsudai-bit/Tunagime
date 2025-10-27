using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// エフェクトコントローラー
/// </summary>
public class EffectController : MonoBehaviour
{
    private List<ParticleSystem> m_particles = new(); // ステージエフェクトのパーティクルシステム

    [Header("====== ボリュームコンポーネント ======")]
    [SerializeField]
    Volume m_volume; // ボリュームコンポーネント


    private 

    void Awake()
    {
        if (m_volume == null)
        {
            Debug.LogError("EffectController: Volume component is not assigned.");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


        // ボリュームコンポーネントを有効化
        m_volume.enabled = true;

    }

    /// <summary>
    /// ボリュームプロファイルを設定する
    /// </summary>
    /// <param name="profile"></param>
    public void SetVolumeProfile(VolumeProfile profile)
    {
        if (m_volume != null)
        {
            m_volume.profile = profile;
            
        }
    }

    /// <summary>
    /// パーティクルシステムを再生する
    /// </summary>
    /// <param name="particleSystem"></param>
    public void PlayParticle(GameObject particleObject)
    {

        // 新しいパーティクルシステムをインスタンス化
        GameObject newParticleObject = Instantiate(particleObject, transform.position, Quaternion.identity, gameObject.transform);

        // パーティクルシステムを再生
        newParticleObject.GetComponent<ParticleSystem>().Play();

        // リストに追加
        m_particles.Add(newParticleObject.GetComponent<ParticleSystem>());

    }


}
