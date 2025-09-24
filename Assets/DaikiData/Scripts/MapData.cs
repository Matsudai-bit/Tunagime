using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// グリッド座標
/// </summary>
[System.Serializable]
public struct GridPos
{
    public static readonly GridPos UP = new GridPos(0, 1);
    public static readonly GridPos DOWN = new GridPos(0, -1);

    public int x;
    public int y;

    public GridPos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static GridPos operator +(GridPos lhs, GridPos rhs) => new GridPos(lhs.x + rhs.x, lhs.y + rhs.y);
    public static GridPos operator -(GridPos lhs, GridPos rhs) => new GridPos(lhs.x - rhs.x, lhs.y - rhs.y);

    // == 演算子のオーバーロード
    public static bool operator ==(GridPos lhs, GridPos rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y;
    }

    // != 演算子のオーバーロード
    public static bool operator !=(GridPos lhs, GridPos rhs)
    {
        return !(lhs == rhs);
    }

    // Equals メソッドのオーバーライド (object版)
    public override bool Equals(object obj)
    {
        if (obj is GridPos other)
        {
            return Equals(other); // IEquatable版を呼び出す
        }
        return false;
    }

    // IEquatable<T> インターフェースの実装
    public bool Equals(GridPos other)
    {
        return x == other.x && y == other.y;
    }

    // GetHashCode メソッドのオーバーライド
    public override int GetHashCode()
    {
        // Tuple.GetHashCode() を使うと簡単に複数の値を組み合わせたハッシュを生成できる
        return (x, y).GetHashCode();
    }
}

[System.Serializable]

public class MapData : MonoBehaviour
{
    [Header("====== ステージ生成器(何ステージ)の設定 ======")]
    [SerializeField] 
    private GameObject m_stageGenerator; // ステージ生成器の参照

    // 1. staticなreadonlyフィールドでインスタンスを保持
    //    アプリケーション起動時にインスタンスが作成されます。
    private static MapData s_instance ;


    // 唯一のインスタンスにアクセスするためのプロパティ
    public static MapData GetInstance
    {
        get
        {
            // シーン上にインスタンスが存在しない場合
            if (s_instance == null)
            {
                // シーン内からMapDataを検索

                // それでも見つからない場合
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

    private void Awake()
    {
        // 既にインスタンスが存在する場合
        if (s_instance != null && s_instance != this)
        {
            // このインスタンスを破棄して、重複を避ける
            Destroy(this.gameObject);
            return;
        }

        // シーン上の唯一のインスタンスとして自身を登録
        s_instance = this;

        // シーンを跨いで存続させる
        DontDestroyOnLoad(this.gameObject);
        Initialize();
     
    }

    /// <summary>
    /// マップデータの初期化
    /// </summary>
    public void Initialize()
    {
        InitializeMapData();

        // コンポーネント取得
        m_stageGridData = GetComponent<StageGridData>();
        m_stageGridData.Initialize(m_commonData.width, m_commonData.height);
        
    }

    public void InitializeMapData()
    {
        if (m_stageSetting != null)
        {
            // ステージ設定を取得
            m_mapSetting = m_stageSetting.mapSetting;
            m_stageGenerator = m_stageSetting.stageGenerator;

            SoundManager.GetInstance.PlayBGM(m_mapSetting.bgmID);

        }



        // マップ設定を受け取って初期化
        if (m_mapSetting != null)
        {
            m_commonData.width = m_mapSetting.width;
            m_commonData.height = m_mapSetting.height;
            m_commonData.center = m_mapSetting.center;
            m_commonData.tileSize = m_mapSetting.tileSize;
            m_commonData.BaseTilePosY = m_mapSetting.BaseTilePosY;
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

     private MapSetting m_mapSetting; // マップ設定


    [SerializeField] private CommonData m_commonData;

    [Header("====== ステージ設定 ======")]
    [SerializeField] private StageSetting m_stageSetting; // ステージ設定

    private StageGridData m_stageGridData;

    /// <summary>
    /// 共通データの取得
    /// </summary>
    /// <returns>共通データ</returns>
    public CommonData GetCommonData()
    {
        return m_commonData;
    }

    /// <summary>
    /// ステージの背景プレハブの取得
    /// </summary>
    /// <returns></returns>
    public GameObject GetStagePrefab()
    {
        return m_stageSetting.backgroundPrefab;
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
        {
            Debug.LogWarning("ステージのグリッドの境界外を指定しています (x,y) = " + x + "," + y);
        }

        Vector3 worldPos;
        worldPos.x = ConvertGridToWorldPosX(x);
        worldPos.y = 0.0f;
        worldPos.z = ConvertGridToWorldPosZ(y);

        return worldPos;
    }
    public Vector3 ConvertGridToWorldPos(GridPos gridPos)
    {
        return ConvertGridToWorldPos(gridPos.x, gridPos.y);
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

        if (x <0 )
        {
            gridPos.x = Mathf.CeilToInt(x) + -1;
        }
        else
        {
            // 切り捨てて代入
            gridPos.x = (int)(x);
        }

        if (y < 0)
        {
            gridPos.y = Mathf.CeilToInt(y) + -1;
        }
        else
        {
            gridPos.y = (int)(y);

        }


        return gridPos;
    }

    public void SetStageGenerator(GameObject stageGenerator)
    {
        m_stageGenerator = stageGenerator;
    }

    public void SetStageSetting(StageSetting stageSetting)
    {
        m_stageSetting = stageSetting;
        InitializeMapData();
    }

    public GameObject GetStageGenerator()
    {
        return m_stageGenerator;
    }

}
