using System;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

/// <summary>
/// �Q�[�����Ŏ����^�щ\�ȃI�u�W�F�N�g�B
/// </summary>
public class Carryable : MonoBehaviour
{
    
    private StageBlock m_stageBlock; // ����Carryable��������StageBlock

    private Action<GridPos> m_onDropAction; // �u���Ƃ��̏�����ݒ肷�邽�߂̃A�N�V����

    private void Awake()
    {
        // StageBlock�R���|�[�l���g���擾
        m_stageBlock = GetComponent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("Carryable must be attached to a GameObject with a StageBlock component.");
        }
    }

    public StageBlock stageBlock
    {
        get { return m_stageBlock; }
    }

    /// <summary>
    /// ����Carryable�I�u�W�F�N�g���E�������B
    /// </summary>
    public void OnPickUp()
    {
        gameObject.SetActive(false); // �I�u�W�F�N�g���\���ɂ���
    }

    /// <summary>
    /// ����Carryable�I�u�W�F�N�g��u�������B
    /// </summary>
    public void OnDrop( GridPos  placementPos)
    {
        m_onDropAction(placementPos);
    }

    /// <summary>
    /// �u���Ƃ��̏����ɍs��������ݒ肷��
    /// </summary>
    /// <param name="onDropAction"></param>
    public void SetOnDropAction(Action<GridPos> onDropAction)
    {
        m_onDropAction = onDropAction;
    }
}
