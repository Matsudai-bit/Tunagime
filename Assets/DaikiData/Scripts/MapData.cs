using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �O���b�h���W
/// </summary>
[System.Serializable]
public struct GridPos
{
  public  int x;
  public  int y;

   public GridPos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static GridPos operator +(GridPos lhs, GridPos rhs) => new GridPos(lhs.x + rhs.x, lhs.y + rhs.y);
}

[System.Serializable]

public class MapData : MonoBehaviour
{

    // 1. static��readonly�t�B�[���h�ŃC���X�^���X��ێ�
    //    �A�v���P�[�V�����N�����ɃC���X�^���X���쐬����܂��B
    private static MapData s_instance ;

 

    // 3. public��static�v���p�e�B
    //    �B��̃C���X�^���X�փA�N�Z�X���邽�߂̑����ł��B
    public static MapData GetInstance
    {
        get {
            // �C���X�^���X���܂�null�̏ꍇ
            if (s_instance == null)
            {
                // �V�[�����Ɋ�����MapData�R���|�[�l���g��T��

                // ����ł�������Ȃ��ꍇ�i�V�[�����ɂ܂��Ȃ��ꍇ�j
                if (s_instance == null)
                {
                    // �V����GameObject���쐬���AMapData�R���|�[�l���g��ǉ�����
                    GameObject singletonObject = new GameObject(typeof(MapData).Name);
                    s_instance = singletonObject.AddComponent<MapData>();
                    Debug.Log($"[MapData] �V���O���g���𐶐����܂���: {singletonObject.name}");
                }
            }
            return s_instance;
        }
    }

    /// <summary>
    /// ���ʃf�[�^
    /// </summary>
    [System.Serializable]
    public struct CommonData
    {
        public int width;     // �����i�^�C����
        public int height;    // �c�� (�^�C����)
        public Vector2 center;// ���S���W
         
        public float tileSize; // �^�C���̃T�C�Y

        public float BaseTilePosY;

    }


    [SerializeField] private CommonData m_commonData;


    private StageGridData m_stageGridData;

    private void Awake()
    {

        s_instance = this;

        // �R���|�[�l���g�擾
        m_stageGridData = GetComponent<StageGridData>();
        m_stageGridData.Initialize(m_commonData.width, m_commonData.height);



    }

    /// <summary>
    /// ���ʃf�[�^�̎擾
    /// </summary>
    /// <returns>���ʃf�[�^</returns>
    public CommonData GetCommonData()
    {
        return m_commonData;
    }

    /// <summary>
    /// �X�e�[�W�O���b�h�f�[�^�̎擾
    /// </summary>
    /// <returns>�X�e�[�W�O���b�h�f�[�^</returns>
    public StageGridData GetStageGridData()
    {
        return m_stageGridData;
    }

    /// <summary>
    /// �O���b�h�i�X�e�[�W�j�̍�����W�̎擾
    /// </summary>
    /// <returns></returns>
    public Vector2 GetStageLeftTopPos()
    {
        //Transform gridTileTopLefTrans = m_stageGridData.GetTileData[0, 0].floor.transform;
        //// �O���b�h�̃^�C���̈�Ԓ[�i����j�̍��W�̂��擾 (�x���͖�������)
        //Vector2 gridTileLeftTopPos = new Vector2(gridTileTopLefTrans.transform.position.x, gridTileTopLefTrans.transform.position.z);
        //// �^�C���̊p(����)���W�����߂�
        //Vector2 tileLeftTopPos = new Vector2(gridTileLeftTopPos.x - gridTileTopLefTrans.localScale.x / 2.0f, gridTileLeftTopPos.y + gridTileTopLefTrans.localScale.z / 2.0f);

        return new Vector2(m_commonData.center.x - (float)m_commonData.width / 2.0f, m_commonData.center.x + (float)m_commonData.height / 2.0f);
    }

    public Vector2 GetStageCenterPos()
    {
        return m_commonData.center;
    }


    /// <summary>
    /// �O���b�h���W���烏�[���h���W�֕ϊ�
    /// </summary>
    /// <param name="x">�Z��X</param>
    /// <param name="y">�Z��Y</param>
    /// 
    /// <returns>���[���h���W(Y���W��0)</returns>
    public Vector3 ConvertGridToWorldPos(int x, int y)
    {
        if (CheckInnerGridPos(new GridPos(x, y)) == false)
            Debug.LogWarning("�X�e�[�W�̃O���b�h�̋��E�O���w�肵�Ă��܂� (x,y) = " + x + "," + y);

        Vector3 worldPos;
        worldPos.x = ConvertGridToWorldPosX(x);
        worldPos.y = 0.0f;
        worldPos.z = ConvertGridToWorldPosZ(y);

        return worldPos;
    }

    public float ConvertGridToWorldPosX(int x)
    {
        float posCoordinationValueX = (m_commonData.tileSize * m_commonData.width / 2.0f);

        return GetStageLeftTopPos().x + (float)x *  m_commonData.tileSize + m_commonData.tileSize / 2.0f;
    }

    public float ConvertGridToWorldPosZ(int y)
    {

        float posCoordinationValueZ = (m_commonData.tileSize * m_commonData.height / 2.0f);

        return GetStageLeftTopPos().y - (float)(y) * m_commonData.tileSize - m_commonData.tileSize / 2.0f;
    }

    /// <summary>
    /// �w�肵���Z�����W���O���b�h�͈͓̔��ɂ��邩�ǂ���
    /// </summary>
    /// <param name="checkGridPos">�`�F�b�N����O���b�h���W</param>
    /// <returns>�O���b�h�͈͓̔��ɂ���ꍇ�� true�A����ȊO�̏ꍇ�� false</returns>
    public bool CheckInnerGridPos(GridPos checkGridPos)
    {
        return CheckInnerGridPos(checkGridPos.x, checkGridPos.y);
    }

    public bool CheckInnerGridPos(int x, int y)
    {
        // �O���b�h�͈͓̔��ɂ��邩�ǂ������`�F�b�N
        if (x >= 0 && x < m_commonData.width &&
            y >= 0 && y < m_commonData.height)
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// �w�肵�����[���h���W�̐^���̍ł��߂��_��Ԃ��@�^���ɓ_�������ꍇ�ł��ł��߂��_��Ԃ�
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public GridPos GetClosestGridPos(Vector3 targetPos)
    {
      
        // �X�e�[�W�̊p(����)���W���擾
        Vector2 tileLeftTopPos = GetStageLeftTopPos();

        // �Ώۍ��W��2D�ɕϊ�����
        Vector2 targetPosVec2 = new Vector2(targetPos.x, targetPos.z);


        // ���������߂� 
        // ���� : pos.x / ���̒��� * �}�X��
        float x = targetPosVec2.x  - tileLeftTopPos.x / ((float)m_commonData.width  * (float)m_commonData.tileSize) * (float)m_commonData.width;
        float y = tileLeftTopPos.y - targetPosVec2.y / ((float)m_commonData.height * (float)m_commonData.tileSize) * (float)m_commonData.height;

        GridPos gridPos = new GridPos();
        // �؂�̂Ăđ��
        gridPos.x = (int)(x);
        gridPos.y = (int)(y);

        return gridPos;
    }


}
