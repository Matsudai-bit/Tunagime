using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header( "====== �X�e�[�W������(���X�e�[�W)�̐ݒ� ======")]
    [SerializeField]
    private GameObject m_stageGenerator;

    [Header("====== �e�e�̐ݒ� ======")]
    [SerializeField]

    [Header("���݂��`���[�u")]
    private Transform m_amidaParent;

    [Header("��")]
    [SerializeField]
    private Transform m_floorBlockParent;

    [Header("�M�~�b�N�֘A")]
    [SerializeField]
    private Transform m_gimmickParent;

    [Header("======�Q�[���f�B���N�^�[�̐ݒ� ======")]
    [SerializeField]�@private GameDirector m_gameDirector; // �Q�[���f�B���N�^�[�̃C���X�^���X

    [Header("====== �N���A��Ԃ̃`�F�b�J�[�̐ݒ� ======")]
    [SerializeField] private ClearConditionChecker m_clearConditionChecker; // �N���A��Ԃ̃`�F�b�J�[

    [Header("====== ���݂��}�l�[�W���[�̐ݒ� ======")]
    [SerializeField] private AmidaManager m_amidaManager; // ���݂��}�l�[�W���[�̃C���X�^���X


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �X�e�[�W������̃C���X�^���X�𐶐�
        if (m_stageGenerator != null)
        {
            m_stageGenerator = Instantiate(m_stageGenerator, Vector3.zero, Quaternion.identity);
            m_stageGenerator.GetComponent<StageGenerator>().Generate(
                m_amidaManager,
                m_amidaParent,
                m_floorBlockParent,
                m_gimmickParent,
                m_clearConditionChecker
            );
        }
        else
        {
            Debug.LogError("�X�e�[�W�����킪�ݒ肳��Ă��܂���B");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
