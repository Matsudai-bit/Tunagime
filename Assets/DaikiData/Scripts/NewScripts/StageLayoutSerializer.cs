// �K�v�Ȗ��O��Ԃ��C���|�[�g
using Newtonsoft.Json;                 // ���@�\�� JSON �V���A���C�U�iJsonUtility ���_��j
using System;
using System.Linq;
using UnityEngine;

// �X�e�[�W�̃I�u�W�F�N�g�z�u���V���A���C�Y/�f�V���A���C�Y���郆�[�e�B���e�B�N���X
public class StageLayoutSerializer : MonoBehaviour
{
    /// <summary>
    /// �w�肳�ꂽ Transform�i���[�g�j����ċA�I�ɃX�e�[�W�����V���A���C�Y����
    /// </summary>
    public static StageSerializableChildData SerializeStage(Transform transform)
    {
        // ���݂�Transform�̊�{���i���O�A�^�O�A�ʒu�A��]�A�X�P�[���j��ۑ�
        var data = new StageSerializableChildData
        {
            name = transform.name,
            tag = transform.tag,
            localPosition = transform.localPosition,
            localRotation = transform.localEulerAngles,
            localScale = transform.localScale
        };

        // ���� GameObject �ɃA�^�b�`����Ă��� MonoBehaviour �R���|�[�l���g���擾
        var components = transform.gameObject.GetComponents<MonoBehaviour>();

        // �e�R���|�[�l���g�̒�����V���A���C�Y�\�Ȃ��̂����𒊏o
        foreach (var comp in components)
        {
            if (comp is ISerializableComponent serializable)
            {
                // �V���A���C�Y�p�f�[�^���擾
                var payload = serializable.CaptureData();

                // JSON ������ɕϊ��iUnity������JsonUtility���g�p�j
                var json = JsonUtility.ToJson(payload);

                // �ۑ��p�̍\���̂ɒǉ��iMonoBehaviour�̌^��Payload�̌^�̗�����ێ��j
                data.components.Add(new SerializedComponentData
                {
                    typeName = comp.GetType().AssemblyQualifiedName,           // ��: AmidaTube
                    payloadTypeName = payload.GetType().AssemblyQualifiedName,        // ��: AmidaTubeData
                    json = json                                            // ���ۂ̃f�[�^�iJSON�j
                });
            }
        }

        // �q�I�u�W�F�N�g���ċA�I�ɕۑ�
        foreach (Transform child in transform)
        {
            data.children.Add(SerializeStage(child));
        }

        return data;
    }

    /// <summary>
    /// �ۑ����ꂽ�X�e�[�W�f�[�^����GameObject�c���[���č\�z����
    /// </summary>
    public static GameObject DeserializeStage(StageSerializableChildData data, StagePrefabDatabase stageyPrefab)
    {
        // �^�O�ɉ������v���n�u���擾�i��FSwitch�AAmidaTube�Ȃǁj
        var prefab = stageyPrefab.GetPrefab(data.tag);

        GameObject instance = null;

        if (prefab != null)
        {
            // �v���n�u����C���X�^���X����
            instance = Instantiate(prefab);
            instance.gameObject.name = data.name;
            instance.gameObject.tag = data.tag;
            instance.transform.localPosition = data.localPosition;
            instance.transform.localEulerAngles = data.localRotation;
            instance.transform.localScale = data.localScale;

            // �e�V���A���C�Y�ς݃R���|�[�l���g�𕜌�
            foreach (var compData in data.components)
            {
                // MonoBehaviour�̌^���擾
                Type type = Type.GetType(compData.typeName);
                var component = instance.GetComponent(type);

                // �Y������ ISerializableComponent �������Ă��邩�m�F
                if (component is ISerializableComponent serializable)
                {
                    // JSON�����Ώۂ́u�f�[�^�^�v�iAmidaTubeData�Ȃǁj�𖼑O����擾
                    string payloadTypeName = compData.payloadTypeName.Replace("+", ".");
                    Type payloadType = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .FirstOrDefault(t => t.AssemblyQualifiedName == payloadTypeName);

                    if (payloadType == null)
                    {
                        Debug.LogWarning($"�^��������܂���: {compData.typeName}");
                        continue;
                    }

                    // JSON �� �f�[�^�N���X�ɕϊ��i�f�V���A���C�Y�j
                    var payload = JsonConvert.DeserializeObject(compData.json, payloadType);
                    if (payload == null)
                    {
                        Debug.LogWarning($"�f�V���A���C�Y���s: JSON={compData.json}, �^={payloadType.FullName}");
                        continue;
                    }

                    // ���������f�[�^���R���|�[�l���g�֓K�p
                    serializable.ApplyData(payload);
                }
            }
        }
        else
        {
            // �v���n�u��������Ȃ������ꍇ�͋��GameObject�𐶐�
            instance = new GameObject(data.name);
            instance.transform.name = data.name;
            instance.transform.localPosition = data.localPosition;
            instance.transform.localEulerAngles = data.localRotation;
            instance.transform.localScale = data.localScale;
        }

        // �q�f�[�^���ċA�I�ɕ���
        foreach (var childData in data.children)
        {
            GameObject child = DeserializeStage(childData, stageyPrefab);

            // ���[���h���W���ێ������ɐe�ɂԂ牺����i���[�J��Transform�j
            child.transform.SetParent(instance.transform, false);
        }

        return instance;
    }
}
