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

    private void FixedUpdate()
    {
        
    }

    public void Initialize(List<FeltBlock> feltBlocks)
    {
        m_feltBlocks.Clear();
        m_feltBlocks.AddRange(feltBlocks);

        foreach (var feltBlock in m_feltBlocks)
        {
            feltBlock.SetPairBadge(this); // �y�A���b�y����ݒ肷�郁�\�b�h���Ăяo��

        }        
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

    public bool CanSlide()
    {
        // ���ׂẴt�F���g�u���b�N���X���C�h�\���`�F�b�N
        return m_feltBlocks.TrueForAll(feltBlock => feltBlock.CanSlide());
    }

    public void Slide(GridPos velocity)
    {
        foreach (var feltBlock in m_feltBlocks)
        {
            feltBlock.StartSlide(velocity); // �e�t�F���g�u���b�N���X���C�h
        }
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
