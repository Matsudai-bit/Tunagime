using UnityEngine;

/// <summary>
/// ステージのグリッド管理
/// </summary>
public class StageGridData : MonoBehaviour
{
    private TileData[,] m_tileData = new TileData[,] { }; //　タイルデータ

    // あみだデータに変更があったかどうか
    private bool m_isAmidaDataChanged = false;

    /// <summary>
    /// タイルデータを取得します。
    /// </summary>
    public TileData[,]  GetTileData
    {
        get{ return m_tileData; } 
    }

    /// <summary>
    /// 指定したグリッド座標に対応するタイルデータを取得します。
    /// </summary>
    /// <param name="gridPos"></param>
    /// <returns></returns>
    public AmidaTube GetAmidaTube(GridPos gridPos)
    {
        return GetAmidaTube(gridPos.x, gridPos.y);
    }

    public AmidaTube GetAmidaTube(int x, int y)
    {
        // 範囲内かどうかを確認
        if (!MapData.GetInstance.CheckInnerGridPos(new GridPos(x, y)))
        {
            Debug.LogWarning($"GetAmida: Grid position ({x},{y}) is out of bounds.");
            return null;
        }
        // タイルデータからあみだチューブを取得
        TileData tile = m_tileData[y, x];
        return tile.amidaTube;
    }

    public TileObject GetTileObject(GridPos gridPos)
    {
        // 範囲内かどうかを確認
        if (!MapData.GetInstance.CheckInnerGridPos(gridPos))
        {
            Debug.LogWarning($"GetTileObject: Grid position ({gridPos.x},{gridPos.y}) is out of bounds.");
            return new TileObject(); // nullではなく、デフォルトのTileObjectを返す
        }
        // タイルデータからTileObjectを取得
        TileData tile = m_tileData[gridPos.y, gridPos.x];
        return tile.tileObject;
    }

    /// <summary>
    /// 指定したグリッド座標にあみだチューブを設定します。
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="amidaTube"></param>
    public void SetAmidaTube(GridPos gridPos, AmidaTube amidaTube)
    {
        // 範囲内かどうかを確認
        if (!MapData.GetInstance.CheckInnerGridPos(gridPos))
        {
            Debug.LogWarning($"SetAmidaTube: Grid position ({gridPos.x},{gridPos.y}) is out of bounds.");
            return;
        }
        // タイルデータを取得
        TileData currentTile = m_tileData[gridPos.y, gridPos.x];
        // あみだチューブを設定
        currentTile.amidaTube = amidaTube;
        // 変更を配列に反映
        m_tileData[gridPos.y, gridPos.x] = currentTile;

        // あみだデータが変更されたことを記録
        m_isAmidaDataChanged = true;
    }

    /// <summary>
    /// タイルデータを設定します。
    /// </summary>
    /// <param name="tileData"></param>
    public void SetData(TileData[,] tileData)
    {
        m_tileData = tileData;
    }

    /// <summary>
    /// グリッドデータを初期化します。
    /// </summary>
    /// <param name="gridWidth"></param>
    /// <param name="gridHeight"></param>
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

    /// <summary>
    /// あみだデータが変更されたかどうかを確認します。
    /// </summary>
    /// <returns></returns>
    public bool IsAmidaDataChanged()
    {
        return m_isAmidaDataChanged;
    }

    /// <summary>
    /// あみだデータの変更フラグを設定
    /// </summary>
    public void SetAmidaDataChanged()
    {
        m_isAmidaDataChanged = true;
    }

    /// <summary>
    /// あみだデータの変更フラグをリセットします。
    /// </summary>
    public void ResetAmidaDataChanged()
    {
        m_isAmidaDataChanged = false;
    }


}