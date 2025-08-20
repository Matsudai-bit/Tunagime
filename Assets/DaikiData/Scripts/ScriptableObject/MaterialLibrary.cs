using System;
using System.Collections.Generic;
using System.Linq; // ToDictionary���g�����߂ɕK�v
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialLibrary", menuName = "ScriptableObjects/MaterialLibrary")]
public class MaterialLibrary : ScriptableObject
{
    // �}�e���A���̃O���[�v���`
    public enum MaterialGroup
    {
        YARN,
        FELT_BLOCK,
        CURTAIN,
        CORE,
        FRAGMENT, // �ǉ�: �z���̒f��
    }

    [Serializable]
    public struct MaterialEntry
    {
        public EmotionCurrent.Type emotionType;
        public Material material;

        public MaterialEntry(EmotionCurrent.Type type, Material value) : this()
        {
            this.emotionType = type;
            this.material= value;
        }
    }

    // �}�e���A���O���[�v�Ƃ��̃G���g���̃y�A
    [Serializable]
    public struct MaterialGroupEntry
    {
        public MaterialGroup materialGroup;
        public List<MaterialEntry> materialEntries;
    }

    // Inspector�Őݒ肷�邽�߂̃��X�g
    [SerializeField]
    private List<MaterialGroupEntry> m_materialGroups = new List<MaterialGroupEntry>();

    // �����^�C���ŗ��p���鎫��
    private Dictionary<MaterialGroup, Dictionary<EmotionCurrent.Type, Material>> m_materialMap;

    private static MaterialLibrary s_instance;
    public static MaterialLibrary GetInstance
    {
        get
        {
            if (s_instance == null)
            {
                // ���\�[�X�t�H���_����X�N���v�^�u���I�u�W�F�N�g�����[�h
                s_instance = Resources.Load<MaterialLibrary>("MaterialLibrary");
                if (s_instance == null)
                {
                    Debug.LogError("MaterialLibrary�X�N���v�^�u���I�u�W�F�N�g��'Resources'�t�H���_�Ɍ�����܂���B");
                }
                else
                {
                    s_instance.InitializeLibrary();
                }
            }
            return s_instance;
        }
    }

    /// <summary>
    /// ���C�u�����̏����������BInspector�Őݒ肵�����X�g�������ɕϊ����܂��B
    /// </summary>
    private void InitializeLibrary()
    {
        m_materialMap = new Dictionary<MaterialGroup, Dictionary<EmotionCurrent.Type, Material>>();
        foreach (var groupEntry in m_materialGroups)
        {
            // �e�O���[�v�̃G���g���������ɕϊ����Am_materialMap�ɒǉ�
            m_materialMap[groupEntry.materialGroup] = groupEntry.materialEntries
                .ToDictionary(entry => entry.emotionType, entry => entry.material);
        }
        Debug.Log("[MaterialLibrary] Library initialized.");
    }

    /// <summary>
    /// �w�肳�ꂽ�}�e���A���O���[�v�Ɗ���^�C�v�ɑΉ�����}�e���A�����擾���܂��B
    /// </summary>
    /// <param name="materialGroup">�}�e���A���̎��</param>
    /// <param name="emotionType">�擾�������}�e���A���̊���^�C�v</param>
    /// <returns>�Ή�����}�e���A���B������Ȃ��ꍇ��null��Ԃ��܂��B</returns>
    public Material GetMaterial(MaterialGroup materialGroup, EmotionCurrent.Type emotionType)
    {
        // �܂��}�e���A���O���[�v�����݂��邩�m�F
        if (m_materialMap.TryGetValue(materialGroup, out var innerDictionary))
        {
            // ���Ɋ���^�C�v�ɑΉ�����}�e���A�������݂��邩�m�F
            if (innerDictionary.TryGetValue(emotionType, out Material material))
            {
                return material;
            }
        }

        Debug.LogWarning($"[MaterialLibrary] �}�e���A�� '{materialGroup}' �̊���^�C�v '{emotionType}' ��������܂���ł����B");
        return null;
    }

    public void Reset()
    {
        m_materialGroups.Add(new MaterialGroupEntry  {   materialGroup = MaterialGroup.YARN,   materialEntries = new List<MaterialEntry>()});
        m_materialGroups.Add(new MaterialGroupEntry { materialGroup = MaterialGroup.FELT_BLOCK, materialEntries = new List<MaterialEntry>() });
        m_materialGroups.Add(new MaterialGroupEntry { materialGroup = MaterialGroup.CURTAIN, materialEntries = new List<MaterialEntry>() });
        m_materialGroups.Add(new MaterialGroupEntry { materialGroup = MaterialGroup.CORE, materialEntries = new List<MaterialEntry>() });
        
        foreach (var group in m_materialGroups)
        {
            group.materialEntries.Add( new MaterialEntry( EmotionCurrent.Type.REJECTION, null));
            group.materialEntries.Add(new MaterialEntry(EmotionCurrent.Type.LONGING, null));
            group.materialEntries.Add(new MaterialEntry(EmotionCurrent.Type.COURAGE, null));
            group.materialEntries.Add(new MaterialEntry(EmotionCurrent.Type.KINDNESS, null));
            group.materialEntries.Add(new MaterialEntry(EmotionCurrent.Type.LOVE, null));
            group.materialEntries.Add(new MaterialEntry(EmotionCurrent.Type.FAITHFULNESS, null));
            group.materialEntries.Add(new MaterialEntry(EmotionCurrent.Type.NONE, null));
        }



    }
}