using UnityEngine;

/// <summary>
/// ������
/// </summary>
public class FloorBlockGenerator : MonoBehaviour
{

    [SerializeField] private GameObject m_floorBlockPrefab;    // �X�e�[�W�u���b�N
    [SerializeField] private GameObject m_floorBlockParent;    // �X�e�[�W�u���b�N�̐e   


    public GameObject line; // ���C���\���p�I�u�W�F�N�g


    /// <summary>
    /// �������Ă��Ȃ�����{���đ���
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="map"></param>
    /// <param name="generationPosY"></param>
    /// <returns></returns>
   public GameObject GenerateFloor(GridPos gridPos)
    {
        // �q�I�u�W�F�N�g���i�[����z��쐬
        var children = new Transform[m_floorBlockParent.transform.childCount];

        MapData map = MapData.GetInstance;

        float posX = (float)(gridPos.x) * map.GetCommonData().tileSize;
        float posY = (float)(gridPos.y) * map.GetCommonData().tileSize;

        Vector3 pos = map.ConvertGridToWorldPos(gridPos.x, gridPos.y);

        pos.y = map.GetCommonData().BaseTilePosY;

        // 0�`��-1�܂ł̎q�����Ԃɔz��Ɋi�[
        for (int i = 0; i < m_floorBlockParent.transform.childCount; i++)
        {
            // �q�v�f�̎擾
            var child = m_floorBlockParent.transform.GetChild(i);

            if (child.gameObject != null && child.gameObject.activeSelf == false)
            {
               child.gameObject.SetActive(true);
                child.transform.position = pos;
                return child.gameObject;
            }
        }


      

        // ����
        return Instantiate(m_floorBlockPrefab, pos, Quaternion.identity, m_floorBlockParent.transform);
    }



    /// <summary>
    /// ���̐���
    /// </summary>
    /// <param name="generationPosY"></param>
    /// <returns>���������O���b�h�f�[�^</returns>
    public GameObject[,] GenerateFloor(  bool grid)
    {
        GameObject[,] floorBlockGrid;

        MapData map = MapData.GetInstance;


        // �X�e�[�W�O���b�h�̐���
        floorBlockGrid = new GameObject[map.GetCommonData().height, map.GetCommonData().width];

        for (int cy = 0; cy < map.GetCommonData().height; cy++)
        {
            for (int cx = 0; cx < map.GetCommonData().width; cx++)
            {
                float posX = (float)(cx) * map.GetCommonData().tileSize;
                float posY = (float)(cy) * map.GetCommonData().tileSize;

                Vector3 pos = map.ConvertGridToWorldPos(cx, cy);

                pos.y = map.GetCommonData().BaseTilePosY;

                // ����
                floorBlockGrid[cy, cx] = Instantiate(m_floorBlockPrefab, pos, Quaternion.identity, m_floorBlockParent.transform);


                map.GetStageGridData().GetTileData[cy, cx].floor = floorBlockGrid[cy, cx];
            }

        }


        if (grid)
        // �O���b�h�̐��̕`��
        CreateGridEffects(map, map.GetCommonData().BaseTilePosY);

        // �㕔�̍��W�̎Z�o
        //       topPartPosY = generationPosY + (m_floorBlockPrefab.transform.localScale.y / 2.0f);

        var boxCollider = m_floorBlockParent.GetComponent<BoxCollider>();
        if (boxCollider)
        {
            float sizeX = (float)map.GetCommonData().width * map.GetCommonData().tileSize;
            float sizeY = 0.01f;
            float sizeZ = (float)map.GetCommonData().height * map.GetCommonData().tileSize;
            boxCollider.size =  new Vector3(sizeX, sizeY, sizeZ);

            
        }
        else
            Debug.LogWarning(m_floorBlockParent.name + "��BoxCollider��������܂���");


            // ���������O���b�h�f�[�^��Ԃ�
            return floorBlockGrid;
    }


    /// <summary>
    /// �O���b�h���C���̐���
    /// </summary>
    private void CreateGridEffects(MapData map, float generationPosY)
    {
        for (int x = 1; x < map.GetCommonData().width; x++)
        {
            GameObject obj = Instantiate(line, m_floorBlockParent.transform.GetChild(0));

            float worldX = map.ConvertGridToWorldPosX(x);

            obj.transform.position = new Vector3(
                worldX - map.GetCommonData().tileSize / 2,
                generationPosY + (map.GetCommonData().tileSize / 2 + 0.01f),
                -((float)(map.GetCommonData().height) / 2.0f) * map.GetCommonData().tileSize + map.GetCommonData().tileSize / 2.0f);

            obj.transform.localScale = new Vector3(1, 1, map.GetCommonData().height * map.GetCommonData().tileSize);    // �ǂꂾ���L�΂�����GetCommonData().height������
        }
        for (int y = 1; y < map.GetCommonData().height; y++)
        {
            GameObject obj = Instantiate(line, m_floorBlockParent.transform.GetChild(0));

            float worldZ = map.ConvertGridToWorldPosZ(y);

            obj.transform.position = new Vector3(
                -((float)(map.GetCommonData().width) / 2.0f) * map.GetCommonData().tileSize - map.GetCommonData().tileSize / 2.0f,
                generationPosY + (map.GetCommonData().tileSize / 2 + 0.01f),
                worldZ + map.GetCommonData().tileSize / 2);

            obj.transform.rotation = Quaternion.Euler(0, 90, 0);
            obj.transform.localScale = new Vector3(1, 1, map.GetCommonData().width * map.GetCommonData().tileSize); // �ǂꂾ���L�΂�����GetWidth()������
        }
    }
}
