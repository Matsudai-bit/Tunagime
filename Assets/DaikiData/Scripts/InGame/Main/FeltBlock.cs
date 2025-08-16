using UnityEngine;


/// <summary>
/// �t�F���g�u���b�N
/// </summary>
public class FeltBlock : MonoBehaviour
{
    private StageBlock m_stageBlock; // �X�e�[�W�u���b�N

    private MeshRenderer m_meshRenderer = null; // ���b�V�������_���[

    [SerializeField]
    private GameObject m_model; // �t�F���g�u���b�N�̃��f��

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

    /// <summary>
    /// �X�e�[�W�u���b�N���擾���܂��B
    /// </summary>
    public StageBlock stageBlock
    {
        get { return m_stageBlock; }
    }

    /// <summary>
    /// �t�F���g�u���b�N���ړ�����
    /// </summary>
    /// <param name="velocity"></param>
    public void Move(GridPos velocity)
    {
        GridPos newGridPos = m_stageBlock.GetGridPos() + velocity;

        // �X�e�[�W�u���b�N�̈ʒu���X�V
        m_stageBlock.UpdatePosition(newGridPos);
    }

    public MeshRenderer meshRenderer
    {
        get { return m_meshRenderer; }
        set { m_meshRenderer = value; }
    }

}
