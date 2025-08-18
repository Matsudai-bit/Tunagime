using UnityEngine;

/// <summary>
/// ステージブロックに関するクラス
/// </summary>
public class StageBlock : MonoBehaviour
{
    [SerializeField] private GridPos m_gridPos;

    [SerializeField] private BlockType m_blockType = BlockType.NONE; // ブロックの種類

    [SerializeField] private bool m_canInteract = false; // インタラクト可能かどうか
    public enum BlockType
    {
        NONE = 0, // 何もない

        FELT_BLOCK, // フェルトブロック
        FLUFF_BALL, // 毛糸球
        FEELING_SLOT, // エモーションスロット
        AMIDA_TUBE, // あみだチューブ
        CURTAIN, // カーテン
        SATIN_FLOOR, // サテン床
    }

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="map"></param>
    public void Initialize(GridPos gridPos)
    {
        MapData map = MapData.GetInstance;

        // グリッド座標の設定
        m_gridPos = gridPos;


       // グリッド座標から変換
        transform.position = map.ConvertGridToWorldPos(gridPos.x, gridPos.y);
        transform.position += new Vector3(0.0f, MapData.GetInstance.GetCommonData().BaseTilePosY );




    }

    /// <summary>
    /// グリッド位置を設定する
    /// </summary>
    /// <param name="gridPos">設定するグリッド位置</param>
    public void SetGridPos(GridPos gridPos)
    {
        m_gridPos = gridPos;
    }


    /// <summary>
    /// グリッド位置を取得する
    /// </summary>
    /// <returns>グリッド位置</returns>
    public  GridPos GetGridPos()
    {
        return m_gridPos;
    }



    /// <summary>
    /// 座標の更新
    /// </summary>
    /// <param name="gridPos"></param>
    public void UpdatePosition(GridPos gridPos)
    {
        MapData map = MapData.GetInstance;


        if (map.CheckInnerGridPos(gridPos) == false) return;

        var tileData = MapData.GetInstance.GetStageGridData().GetTileData;

        // 何かオブジェクトがいる場合以下の処理を飛ばす
        if (tileData[gridPos.y, gridPos.x].tileObject.gameObject != null) return;
        

        // 新し座標
        Vector3 newPos = map.ConvertGridToWorldPos(gridPos.x, gridPos.y);

        // 現在いる場所をnullにする
        tileData[m_gridPos.y, m_gridPos.x].tileObject.gameObject = null;

        // 移動元を削除
        MapData.GetInstance.GetStageGridData().RemoveGridDataGameObject(m_gridPos);

        //グリッド座標の更新
        m_gridPos = gridPos;

        // 座標の更新
        transform.position = newPos + new Vector3(0.0f, transform.position.y, 0.0f);

        // 移動先の座標に移動する
        MapData.GetInstance.GetStageGridData().TryPlaceTileObject(m_gridPos, gameObject);


    }

    public void SetActive(bool activeSelf)
    {
        gameObject.SetActive(activeSelf);
    }

    public BlockType GetBlockType()
    {
        return m_blockType;
    }

    public void SetBlockType(BlockType blockType)
    {
        m_blockType = blockType;
    }

    /// <summary>
    /// インタラクト可能かどうかを取得する
    /// </summary>
    /// <returns></returns>
    public bool CanInteract()
    {
        return m_canInteract;
    }
}