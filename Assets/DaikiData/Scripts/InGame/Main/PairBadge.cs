using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �y�A���b�y��
/// </summary>
public class PairBadge : MonoBehaviour
{

    private List<FeltBlock> m_feltBlocks = new List<FeltBlock>(); // ��������t�F���g�u���b�N�̃��X�g

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Initialize(List<FeltBlock> feltBlocks)
    {
        m_feltBlocks.Clear();
        m_feltBlocks.AddRange(feltBlocks);

        foreach (var feltBlock in m_feltBlocks)
        {
            feltBlock.gameObject.transform.SetParent(this.transform); // �t�F���g�u���b�N�̐e���y�A���b�y���ɐݒ�
            feltBlock.SetPairBadge(this); // �y�A���b�y����ݒ肷�郁�\�b�h���Ăяo��

        }

        // �����ŕK�v�ȏ�����������ǉ�
        
    }

    public void Move(GridPos velocity)
    {
        foreach (var feltBlock in m_feltBlocks)
        {
            feltBlock.Move(velocity); // �e�t�F���g�u���b�N���ړ�
        }
    }

    public bool CanMove(GridPos moveDirection)
    {

        return m_feltBlocks.TrueForAll(feltBlock => feltBlock.CanMove(moveDirection));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<FeltBlock> GetFeltBlocks()
    {
        return m_feltBlocks;
    }
}
