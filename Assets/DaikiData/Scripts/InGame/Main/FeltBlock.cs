using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.GridLayoutGroup;


/// <summary>
/// フェルトブロック
/// </summary>
public class FeltBlock : MonoBehaviour
{
    private StageBlock m_stageBlock; // ステージブロック

    private MeshRenderer m_meshRenderer = null; // メッシュレンダラー



    [SerializeField]
    private GameObject m_model; // フェルトブロックのモデル


    /// <summary>
    /// Awake is called when the script instance is being loaded
    /// </summary>
    void Awake()
    {
        m_stageBlock = GetComponent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("FeltBlock requires a StageBlock component.");
        }

        m_meshRenderer = m_model.GetComponent<MeshRenderer>();
        if (m_meshRenderer == null)
        {
            Debug.LogError("FeltBlock requires a MeshRenderer component.");
        }

    }

    public MeshRenderer meshRenderer
    {
        get { return m_meshRenderer; }

    }

    public StageBlock stageBlock
    {
        get { return m_stageBlock; }
    }

    public void SetModel(GameObject model)
    {
       var child = transform.GetChild(0);

        m_model.SetActive(false);
        m_model = Instantiate(model, child); ;

    }
}
