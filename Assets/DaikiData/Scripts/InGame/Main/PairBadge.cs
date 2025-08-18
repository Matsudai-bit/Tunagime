using UnityEngine;

/// <summary>
/// �y�A���b�y��
/// </summary>
public class PairBadge : MonoBehaviour
{
    private FeltBlock m_feltBlock_A; // ��������t�F���g�u���b�NA
    private FeltBlock m_feltBlock_B;   // ��������t�F���g�u���b�NB

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Initialize(FeltBlock feltBlockA, FeltBlock feltBlockB)
    {
        m_feltBlock_A = feltBlockA;
        m_feltBlock_B = feltBlockB;
        // �����ŕK�v�ȏ�����������ǉ�
    }

    public bool CanMove(GridPos moveDirection)
    {
        // �y�A���b�y�����ړ��\���ǂ����𔻒f���郍�W�b�N������
        // �Ⴆ�΁A�����̃t�F���g�u���b�N�����������Ɉړ��ł��邩�ǂ������`�F�b�N����
        return m_feltBlock_A.CanMove(moveDirection) && m_feltBlock_B.CanMove(moveDirection);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
