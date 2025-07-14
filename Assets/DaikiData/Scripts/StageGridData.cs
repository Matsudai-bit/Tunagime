using UnityEngine;

/// <summary>
/// ステージのグリッド管理
/// </summary>
public class StageGridData : MonoBehaviour
{
    //private GameObject[,] m_amidaFloorBlockGrid;   // あみだ層のブロックグリッド
    //private GameObject[,] m_topFloorBlockGrid;     // トップ層のブロックグリッド
    //private GameObject[,] m_amidaTubeGrid;         // あみだチューブのグリッド
    //private GameObject[,] m_topGimmickBlockGrid;   // ギミックブロックのグリッド

    //private GameObject[,] m_wallGrid;               // 壁のグリッド

    private TileData[,] m_tileData = new TileData[,] { }; //　タイルデータ
                                    //　
    public  TileData[,]  GetTileData
    {
        get{ return m_tileData; } 
    }

    public void SetData(TileData[,] tileData)
    {
        m_tileData = tileData;
    }

    public void  Initialize(int gridWidth, int gridHeight)
    {
        m_tileData = new TileData[gridHeight, gridWidth];
    }


    /// <summary>
    /// 指定したタイルのタイルオブジェクトを取り除く
    /// </summary>
    /// <param name="gridPos"></param>
    public GameObject RemoveGridData(GridPos gridPos)
    {
        // 範囲内かどうかを確認
        if (!MapData.GetInstance.CheckInnerGridPos(gridPos))
        {
            Debug.LogWarning($"RemoveTileObject: Grid position ({gridPos.x},{gridPos.y}) is out of bounds.");
            return null;
        }

        // 既存のタイルデータを取得 (構造体なのでコピーされる)
        TileData currentTile = m_tileData[gridPos.y, gridPos.x];

        // グリッドから取り除くGameObjectの参照を保持
        GameObject removedObject = currentTile.tileObject.gameObject;

        // グリッド内の参照をnullにする
        currentTile.tileObject.gameObject = null;
        m_tileData[gridPos.y, gridPos.x] = currentTile; // 変更を配列に反映

        if (removedObject != null)
        {
            Debug.Log($"RemoveTileObject: Removed {removedObject.name} from grid at ({gridPos.x},{gridPos.y}).");
        }
        else
        {
            Debug.Log($"RemoveTileObject: No object found at ({gridPos.x},{gridPos.y}) to remove.");
        }

        return removedObject; // 取り除いたオブジェクトを返す
    }

    /// <summary>
    /// 指定したグリッド座標にオブジェクトを配置します。
    /// その場所に既に他のオブジェクトが存在しない場合にのみ設置します。
    /// </summary>
    /// <param name="gridPos">配置するグリッド座標</param>
    /// <param name="objectToPlace">配置するGameObject</param>
    /// <returns>オブジェクトが正常に配置された場合はtrue、既にオブジェクトが存在した場合はfalse。</returns>
    public bool TryPlaceTileObject(GridPos gridPos, GameObject objectToPlace)
    {
        // 範囲内かどうかを確認
        if (!MapData.GetInstance.CheckInnerGridPos(gridPos))
        {
            Debug.LogWarning($"TryPlaceTileObject: Grid position ({gridPos.x},{gridPos.y}) is out of bounds.");
            return false;
        }

        // 指定した場所にオブジェクトが既に存在するかチェック
        // TileDataは構造体なので、コピーを取得してチェック
        TileData currentTile = m_tileData[gridPos.y, gridPos.x];

        if (currentTile.tileObject.gameObject != null)
        {
            // 既にオブジェクトが存在する場合、設置しない
            Debug.Log($"TryPlaceTileObject: Position ({gridPos.x},{gridPos.y}) already has an object: {currentTile.tileObject.gameObject.name}. Placement failed.");
            return false;
        }

        // オブジェクトが存在しない場合、新しいオブジェクトを配置
        currentTile.tileObject.gameObject = objectToPlace; // tileObjectを設定
        m_tileData[gridPos.y, gridPos.x] = currentTile; // 変更を配列に反映

        Debug.Log($"TryPlaceTileObject: Successfully placed {objectToPlace.name} at ({gridPos.x},{gridPos.y}).");
        return true;
    }

  
}