using System;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// �X�e�[�W�̐����`���@�q�f�[�^
/// 
/// ��̎�ނ̃I�u�W�F�N�g�̃f�[�^�Q
/// </summary>
[Serializable]
public class StageSerializableChildData
{
    public string name;
    public string tag;
    public Vector3 localPosition;
    public Vector3 localRotation;
    public Vector3 localScale;

    public List<StageSerializableChildData > children = new(); // �q�f�[�^
            
    public List<SerializedComponentData> components = new();            // �R���|�[�l���g���

}



/// <summary>
/// �����`���̃R���|�[�l���g�C���^�[�t�F�[�X
/// </summary>
public interface ISerializableComponent
{
    /// <summary>
    /// �ۑ��p�f�[�^�̎擾
    /// </summary>
    /// <returns></returns>
    object CaptureData();

    /// <summary>
    /// �ۑ��p�f�[�^����̕���
    /// </summary>
    /// <param name="data"></param>
    void ApplyData(object data); // �ۑ��f�[�^���畜��
}

/// <summary>
/// 
/// </summary>
[Serializable]
public class SerializedComponentData
{
    public string typeName;         // �R���|�[�l���g�̌^��
    public string payloadTypeName;  // �y�C���[�h�^��
    public string json;             // �R���|�[�l���g�̃f�[�^��JSON������Ŋi�[
}