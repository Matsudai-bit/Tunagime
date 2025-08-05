// �t�@�C���̓ǂݏ����Ɏg�p
using System.IO;

// UnityEditor �̊g���p���O��ԁiEditorWindow ���j
using UnityEditor;

// Unity �̊�{�@�\�iTransform�Ȃǁj
using UnityEngine;

// JSON �V���A���C�Y�p�iNewtonsoft.Json��Unity������荂�@�\�j
using Newtonsoft.Json;

/// <summary>
/// �X�e�[�W�̊K�w�\�����G�f�B�^�ォ��ۑ��E�ǂݍ��݂ł���J�X�^���E�B���h�E
/// </summary>
public class StageLayoutEditorWindow : EditorWindow
{
    // ���[�U�[���I������Ώۂ� Transform�i���[�g�I�u�W�F�N�g�j
    Transform target;

    // JSON�t�@�C�����i������ł͎g�p����Ă��Ȃ��j
    string jsonName;

    /// <summary>
    /// ���j���[�ɃE�B���h�E��ǉ�
    /// </summary>
    [MenuItem("Tools/Hierarchy Saver")]
    static void ShowWindow() => GetWindow<StageLayoutEditorWindow>("Hierarchy Saver");

    /// <summary>
    /// �J�X�^���E�B���h�E��GUI�`�揈��
    /// </summary>
    void OnGUI()
    {
        // JSON�̏z�Q�Ƃ𖳎�����ݒ�i��F�e���q���e�̎Q�ƂȂǁj
        var settings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

        // �G�f�B�^���Transform���w��ł���悤�ɂ���i�C�ӂ̊K�w�̃��[�g�ɂł���j
        target = EditorGUILayout.ObjectField("Target Object", target, typeof(Transform), true) as Transform;

        // �� �u�ۑ��v�{�^��
        if (GUILayout.Button("Save Hierarchy"))
        {
            // �Ώ�Transform����K�w�\�����V���A���C�Y
            var data = StageLayoutSerializer.SerializeStage(target);

            // JSON������Ƃ��Đ��`���ĕۑ��i�C���f���g����j
            string json = JsonConvert.SerializeObject(data, Formatting.Indented, settings);

            // �t�@�C���ɏ����o���iAssets�z���ɕۑ��j
            File.WriteAllText("Assets/DaikiData/SaveData/Test.json", json);

            Debug.Log("Hierarchy saved.");
        }

        // �� �u�ǂݍ��݁v�{�^��
        if (GUILayout.Button("Load Hierarchy"))
        {
            // �X�e�[�W����Prefab�̎Q�Ə������� ScriptableObject �����[�h
            StagePrefabDatabase prefabDatabase = Resources.Load<StagePrefabDatabase>("StagePrefabDatabase");

            // JSON�t�@�C����ǂݍ���
            string json = File.ReadAllText("Assets/DaikiData/SaveData/Test.json");

            // JSON����K�w�f�[�^�ɕϊ��i�f�V���A���C�Y�j
            var data = JsonConvert.DeserializeObject<StageSerializableChildData>(json, settings);

            // �v���n�u�����g���ĊK�w�\���𕜌�
            StageLayoutSerializer.DeserializeStage(data, prefabDatabase);

            Debug.Log("Hierarchy loaded.");
        }
    }
}
