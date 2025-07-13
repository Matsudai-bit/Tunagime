using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float m_speed = 5.0f; // �ړ����x
    private Rigidbody m_rb;           // Rigidbody�R���|�[�l���g

    private StageBlock m_stageBlock;

    [SerializeField] private GameDirector m_gameDirector;


    void Awake()
    {
        m_stageBlock = GetComponent<StageBlock>();

        Application.targetFrameRate = 60;
    }

    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="map"></param>
    public void Initialize(GridPos gridPos)
    {
 
        // �M�~�b�N�̋��ʏ�����
        m_stageBlock.Initialize(gridPos);
    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Rigidbody�R���|�[�l���g�̎擾
        m_rb = GetComponent<Rigidbody>();

        

    }

    // Update is called once per frame
    void Update()
    {
        // �����[�h
        if (Input.GetKeyDown(KeyCode.Q)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    private void FixedUpdate()
    {
        // ����
        Move();
    }

    /// <summary>
    /// �ړ��x�N�g���̎擾
    /// </summary>
    /// <returns>�ړ��x�N�g��</returns>
    private Vector3 GetMovementVec()
    {
        // �L�[���͂ňړ�
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // ���͂��Ȃ����
        if (Mathf.Approximately(x, 0.0f) && Mathf.Approximately(z, 0.0f))
        {
            // �ړ���~
            m_rb.linearVelocity = new Vector3(0.0f, m_rb.linearVelocity.y, 0.0f);
       
            return Vector3.zero;
        }

        // �ړ������̌v�Z
        Vector3 moveVec = new Vector3(x, 0.0f, z);

 

        return moveVec;


    }

    /// <summary>
    /// ����
    /// </summary>
    private void Move()
    {
        // �ړ��x�N�g���̎擾
        Vector3 movementVec = GetMovementVec();

        if (movementVec.magnitude > 0.0f)
        {

            // �P�ʃx�N�g���ł͂Ȃ��ꍇ���K��
            if (movementVec.magnitude > 1.0f)
            {
                movementVec.Normalize();
            }





        }

        // �����x
        m_rb.linearVelocity = movementVec * m_speed;



    }

    void HaveItem()
    {
        m_gameDirector.AddHasItemNum();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ClearItem"))
        {
            // �폜(�N���X�𕪂���ׂ�����)
            collision.gameObject.SetActive(false);
            // �A�C�e�����l����
            HaveItem();

        }
    }
}
