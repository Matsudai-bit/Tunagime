using UnityEngine;

/// <summary>
/// 想いの型
/// </summary>
public class FeelingSlot : MonoBehaviour 
{
    [SerializeField] private FeelingCore m_feelingCore;           // 想いの核

    private EmotionCurrent m_emotionCurrent;  // 想いの種類

    [Header("マテリアル設定用")]
    [SerializeField]
    private MeshRenderer m_meshRenderer; // メッシュレンダラー

    void Awake()
    {
        m_emotionCurrent = GetComponent<EmotionCurrent>(); // EmotionCurrentコンポーネントを取得
        if (m_emotionCurrent == null)
        {
            Debug.LogError("EmotionCurrent が null です");
        }
    }

    /// <summary>
    /// スタートメソッド
    /// </summary>
    private void Start()
    {
        if (m_feelingCore.GetEmotionType() != EmotionCurrent.Type.NONE)
        {
            // マテリアルライブラリの初期化
            var coreMaterial = MaterialLibrary.GetInstance.GetMaterial(MaterialLibrary.MaterialGroup.CORE, m_feelingCore.GetEmotionType());
            // 想いの核のマテリアルを設定
            m_feelingCore.MeshRenderer.material = coreMaterial; 
        }
        else
        {
            // 想いの核が設定されていない場合は非アクティブにする
            m_feelingCore.SetActive(false); 
        }
    }

    void Update()
    {
        // 感情の種類に応じてマテリアルを更新
        if (FeelingSlotStateMonitor.GetInstance.IsConnected(GetEmotionType()))
        {
            // 接続されている場合はスロットのマテリアルを点灯状態に変更
            var slotMaterial = MaterialLibrary.GetInstance.GetMaterial(MaterialLibrary.MaterialGroup.FEELING_SLOT, GetEmotionType());
            m_meshRenderer.material = slotMaterial;
        }
        else
        {
            // 接続されていない場合はスロットのマテリアルを通常状態に変更
            var slotMaterial = MaterialLibrary.GetInstance.GetMaterial(MaterialLibrary.MaterialGroup.FEELING_SLOT, EmotionCurrent.Type.NONE);
            m_meshRenderer.material = slotMaterial;
        }
    }


    /// <summary>
    /// 現在の感情タイプを取得する
    /// </summary>
    /// <returns>想いの種類</returns>
    public EmotionCurrent.Type GetEmotionType()
    {
        return m_emotionCurrent.CurrentType; // 想いの流れの種類を取得
    }

    public void SetEmotionType(EmotionCurrent.Type type)
    {
        m_emotionCurrent.CurrentType = type;// 想いの流れの種類を設定
    }


    /// <summary>
    /// ステージブロックの取得
    /// </summary>
    public StageBlock StageBlock
    {
        get
        {
            return GetComponent<StageBlock>(); // StageBlockコンポーネントを取得
        }
    }

    /// <summary>
    /// 想いの核を挿入する
    /// </summary>
    /// <param name="feelingCore"></param>
    public void InsertCore(FeelingCore feelingCore)
    {
        // 挿入する想いの核の感情タイプが異なる場合は処理を中断
        if (feelingCore.GetEmotionType() != m_emotionCurrent.CurrentType) return;

        if (CanInsertCore(feelingCore.GetEmotionType()))
        {
            Debug.LogError("既に想いの核が挿入されています。");
            return; // 既に想いの核が挿入されている場合は処理を中断
        }

        m_feelingCore.MeshRenderer.material = feelingCore.MeshRenderer.material;    // 想いの核のマテリアルを設定
        m_feelingCore.SetEmotionType(feelingCore.GetEmotionType());                 // 想いの核の感情タイプを設定
        m_feelingCore.SetActive(true);                                               // 想いの核をアクティブにする
    }
    /// <summary>
    /// 想いの核が挿入できるかどうか
    /// </summary>
    /// <param name="emotionType"></param>
    /// <returns></returns>
    public bool CanInsertCore(EmotionCurrent.Type emotionType)
    {
        return m_feelingCore.GetEmotionType() == EmotionCurrent.Type.NONE && emotionType != m_emotionCurrent.CurrentType; 
    }

    /// <summary>
    /// /// 想いの核が挿入されているかどうかを判定
    /// </summary>
    /// <returns></returns>
    public bool IsInsertCore()
    {
        return m_feelingCore.GetEmotionType() != EmotionCurrent.Type.NONE; // 想いの核が挿入されているかどうかを判定
    }

    public FeelingCore GetFeelingCore()
    {
        return m_feelingCore; // 想いの核を取得
    }

    /// <summary>
    /// 想いの核が挿入されているかどうか
    /// </summary>
    /// <returns></returns>

}
