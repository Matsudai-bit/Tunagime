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
    /// グリッド座標からワールド座標へ変換
    /// </summary>
    /// <param name="x">セルX</param>
    /// <param name="y">セルY</param>
    /// 
    /// <returns>ワールド座標(Y座標は0)</returns>
    public Vector3 ConvertGridToWorldPos(int x, int y)
    {
        

        Vector3 worldPos;
        worldPos.x = ConvertGridToWorldPosX(x);
        worldPos.y = 0.0f;
        worldPos.z = ConvertGridToWorldPosZ(y);

        return worldPos;
    }

    public float ConvertGridToWorldPosX(int x)
    {
        float posCoordinationValueX = (m_commonData.tileSize * m_commonData.width / 2.0f);

        return (float)(x) * m_commonData.tileSize - posCoordinationValueX + m_commonData.tileSize / 2.0f;
    }

    public float ConvertGridToWorldPosZ(int y)
    {

        float posCoordinationValueZ = (m_commonData.tileSize * m_commonData.height / 2.0f);

        return  posCoordinationValueZ - (float)(y) * m_commonData.tileSize;
    }

    /// <summary>
    /// 指定したセル座標がグリッドの範囲内にいるかどうか
    /// </summary>
    /// <param name="checkGridPos">チェックするグリッド座標</param>
    /// <returns>グリッドの範囲内にいる場合は true、それ以外の場合は false</returns>
    public bool CheckInnerGridPos(GridPos checkGridPos)
    {
        // グリッドの範囲内にいるかどうかをチェック
        if (checkGridPos.x >= 0 && checkGridPos.x < m_commonData.width &&
            checkGridPos.y >= 0 && checkGridPos.y < m_commonData.height)
        {
            return true;
        }
        return false;
    }


}
