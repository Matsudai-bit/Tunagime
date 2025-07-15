using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float m_speed = 5.0f; // �ړ����x
    private Rigidbody m_rb;           // Rigidbody�R���|�[�l���g

    private StageBlock m_stageBlock;

    [SerializeField] private GameDirector m_gameDirector;
    [SerializeField] private AmidaTubeGenerator m_amidaGenerator;

    private StageBlock m_fluffBall;


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
        var map = MapData.GetInstance;

        // �����[�h
        if (Input.GetKeyDown(KeyCode.Q)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (m_fluffBall)
            {        // �߂��O���b�h���W�̎擾
                var clossesPos = map.GetClosestGridPos(transform.position);
                var generateAmida = m_amidaGenerator.GenerateAmidaBridge(clossesPos);
                

                m_fluffBall = null;
            }

        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (m_fluffBall)
            {
                // �߂��O���b�h���W�̎擾
                var clossesPos = map.GetClosestGridPos(transform.position);
                m_fluffBall.UpdatePosition(clossesPos);

                m_fluffBall.SetActive(true);

                map.GetStageGridData().TryPlaceTileObject(clossesPos, m_fluffBall.GetComponent<GameObject>());
                m_fluffBall = null;

            }
            else 
            {

                // �߂��O���b�h���W�̎擾
                var clossesPos = map.GetClosestGridPos(transform.position);

                // �����p�ϐ� : ���ʕ����x�N�g��
                Vector3 forward = transform.forward;

                GridPos forward2D;
                // ���ʃO���b�h�����̎擾

                // ���ʃx�N�g���̐������r���đ傫�����𐳖ʃx�N�g���Ƃ��đI��
                // ���� : �߂��Ⴍ�����Y��Ȏ΂߂̏ꍇ�o�O��\������ Round���g�p���Ă��邽�ߒv���I�ł͂Ȃ��Ɣ��f
                forward2D = (Mathf.Abs(forward.x) > Mathf.Abs(forward.z))
                    ? new GridPos((int)Mathf.Round(forward.x), 0)
                    : new GridPos(0, -(int)Mathf.Round(forward.z));
                GridPos checkPos = clossesPos + forward2D;

                // �łĂ����O���b�h���W���O���b�h�͈͓����`�F�b�N
                if (map.CheckInnerGridPos(checkPos))
                {
                    TileObject tileObject = map.GetStageGridData().GetTileData[checkPos.y, checkPos.x].tileObject;

                    // �ю����������̏���
                    if (tileObject.type == TileType.FLUFF_BALL && tileObject.gameObject)
                    {
                        GameObject gameObj = tileObject.gameObject;
                        m_fluffBall = gameObj.GetComponent<StageBlock>();
                        map.GetStageGridData().RemoveGridData(checkPos);
                        Debug.Log(gameObj.name);

                        m_fluffBall.SetActive(false);
                    }
                }
            }

          
        }

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
