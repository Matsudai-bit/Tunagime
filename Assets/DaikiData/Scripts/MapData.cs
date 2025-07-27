using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// グリッド座標
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

    // 1. staticなreadonlyフィールドでインスタンスを保持
    //    アプリケーション起動時にインスタンスが作成されます。
    private static MapData s_instance ;

 

    // 3. publicなstaticプロパティ
    //    唯一のインスタンスへアクセスするための窓口です。
    public static MapData GetInstance
    {
        get {
            // インスタンスがまだnullの場合
            if (s_instance == null)
            {
                // シーン内に既存のMapDataコンポーネントを探す

                // それでも見つからない場合（シーン内にまだない場合）
                if (s_instance == null)
                {
                    // 新しいGameObjectを作成し、MapDataコンポーネントを追加する
                    GameObject singletonObject = new GameObject(typeof(MapData).Name);
                    s_instance = singletonObject.AddComponent<MapData>();
                    Debug.Log($"[MapData] シングルトンを生成しました: {singletonObject.name}");
                }
            }
            return s_instance;
        }
    }

    /// <summary>
    /// 共通データ
    /// </summary>
    [System.Serializable]
    public struct CommonData
    {
        public int width;     // 横幅（タイル数
        public int height;    // 縦幅 (タイル数)
        public Vector2 center;// 中心座標
         
        public float tileSize; // タイルのサイズ

        public float BaseTilePosY;

    }


    [SerializeField] private CommonData m_commonData;


    private StageGridData m_stageGridData;

    private void Awake()
    {

        s_instance = this;

        // コンポーネント取得
        m_stageGridData = GetComponent<StageGridData>();
        m_stageGridData.Initialize(m_commonData.width, m_commonData.height);



    }

    /// <summary>
    /// 共通データの取得
    /// </summary>
    /// <returns>共通データ</returns>
    public CommonData GetCommonData()
    {
        return m_commonData;
    }

    /// <summary>
    /// ステージグリッドデータの取得
    /// </summary>
    /// <returns>ステージグリッドデータ</returns>
    public StageGridData GetStageGridData()
    {
        return m_stageGridData;
    }

    /// <summary>
    /// グリッド（ステージ）の左上座標の取得
    /// </summary>
    /// <returns></returns>
    public Vector2 GetStageLeftTopPos()
    {
        //Transform gridTileTopLefTrans = m_stageGridData.GetTileData[0, 0].floor.transform;
        //// グリッドのタイルの一番端（左上）の座標のを取得 (Ｙ軸は無視する)
        //Vector2 gridTileLeftTopPos = new Vector2(gridTileTopLefTrans.transform.position.x, gridTileTopLefTrans.transform.position.z);
        //// タイルの角(左上)座標を求める
        //Vector2 tileLeftTopPos = new Vector2(gridTileLeftTopPos.x - gridTileTopLefTrans.localScale.x / 2.0f, gridTileLeftTopPos.y + gridTileTopLefTrans.localScale.z / 2.0f);

        return new Vector2(m_commonData.center.x - (float)m_commonData.width / 2.0f, m_commonData.center.x + (float)m_commonData.height / 2.0f);
    }

    public Vector2 GetStageCenterPos()
    {
        return m_commonData.center;
    }


    /// <summary>
    /// グリッド座標からワールド座標へ変換
    /// </summary>
    /// <param name="x">セルX</param>
    /// <param name="y">セルY</param>
    /// 
    /// <returns>ワールド座標(Y座標は0)</returns>
    public Vector3 ConvertGridToWorldPos(int x, int y)
    {
        if (CheckInnerGridPos(new GridPos(x, y)) == false)
            Debug.LogWarning("ステージのグリッドの境界外を指定しています (x,y) = " + x + "," + y);

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
    /// 指定したセル座標がグリッドの範囲内にいるかどうか
    /// </summary>
    /// <param name="checkGridPos">チェックするグリッド座標</param>
    /// <returns>グリッドの範囲内にいる場合は true、それ以外の場合は false</returns>
    public bool CheckInnerGridPos(GridPos checkGridPos)
    {
        return CheckInnerGridPos(checkGridPos.x, checkGridPos.y);
    }

    public bool CheckInnerGridPos(int x, int y)
    {
        // グリッドの範囲内にいるかどうかをチェック
        if (x >= 0 && x < m_commonData.width &&
            y >= 0 && y < m_commonData.height)
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// 指定したワールド座標の真下の最も近い点を返す　真下に点が無い場合でも最も近い点を返す
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public GridPos GetClosestGridPos(Vector3 targetPos)
    {
      
        // ステージの角(左上)座標を取得
        Vector2 tileLeftTopPos = GetStageLeftTopPos();

        // 対象座標を2Dに変換する
        Vector2 targetPosVec2 = new Vector2(targetPos.x, targetPos.z);


        // 割合を求める 
        // 割合 : pos.x / 横の長さ * マス数
        float x = targetPosVec2.x  - tileLeftTopPos.x / ((float)m_commonData.width  * (float)m_commonData.tileSize) * (float)m_commonData.width;
        float y = tileLeftTopPos.y - targetPosVec2.y / ((float)m_commonData.height * (float)m_commonData.tileSize) * (float)m_commonData.height;

        GridPos gridPos = new GridPos();
        // 切り捨てて代入
        gridPos.x = (int)(x);
        gridPos.y = (int)(y);

        return gridPos;
    }


}
