
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 方向
/// </summary>
public enum Direction
{
    UP,
    DOWN,
    CENTER,
    RIGHT,
    LEFT
}

/// <summary>
/// あみだくじの管理クラス。あみだくじのパイプ生成や移動処理、色の変更などを管理します。
/// </summary>
public class AmidaManager : MonoBehaviour
{
    private GameObject[,] m_amidaTubeGrid;   // あみだのグリッド

    public MapData m_map; // マップデータ

    public Material m_amidaBaseMaterial; // 基本マテリアル
    public Material m_amidaElectricityMaterial; // 電気のマテリアル
    public Material m_amidaRedElectricityMaterial; // 赤い電気のマテリアル

    // テクスチャ
    public Texture m_baseTexture;
    public Texture m_blueTexture;
    public Texture m_greenTexture;

    private GameObject[,] m_prevAmidaGimmickGrid; // 前回のあみだギミックグリッド
    private GameObject[,] m_prevTopGimmickGrid; // 前回のトップギミックグリッド

    // あみだの移動キャッシュ
    struct AmidaMoveCash
    {
        public GridPos gridPos; // グリッド位置
        public Direction movementDir; // 移動方向
        public Electric.ElectricFlowType electricFlowType; // 電気の種類
    }

    private List<AmidaMoveCash> m_amidaMoveCash = new List<AmidaMoveCash>(); // あみだの移動キャッシュリスト
    private List<AmidaMoveCash> m_amidaPrevMoveCash = new List<AmidaMoveCash>(); // 前回のあみだの移動キャッシュリスト

    private bool m_startAmida = false; // あみだの開始フラグ

    private void Awake()
    {
        
    }

    /// <summary>
    /// ゲーム開始時の初期化処理。あみだチューブの作成を行います。
    /// </summary>
    void Start()
    {
        // 元のグリッドを取得
        m_prevAmidaGimmickGrid = new GameObject[m_map.GetCommonData().height, m_map.GetCommonData().width];
        m_prevTopGimmickGrid = new GameObject[m_map.GetCommonData().height, m_map.GetCommonData().width];

    }

    /// <summary>
    /// 更新処理。
    /// </summary>
    void Update()
    {
        //// あみだグリッドが未設定の場合、設定する
        //if (m_amidaTubeGrid == null)
        //    m_amidaTubeGrid = m_map.GetFloorManager().GetGimmickFloor(FloorManager.FloorType.AMIDA);

        //// あみだの開始フラグが立っている場合、あみだをリセットして再開始する
        //if (m_startAmida)
        //{
        //    ResetAllBlockColor();
        //    foreach (var cash in m_amidaMoveCash)
        //    {
        //        StartAmidakuji(cash.gridPos, cash.electricFlowType, cash.movementDir);
        //    }
        //    m_startAmida = false;
        //}

        //// 元のグリッドを取得
        //GameObject[,] originalTopGrid = m_map.GetFloorManager().GetGimmickFloor(FloorManager.FloorType.TOP);
        //GameObject[,] originalAmidaGrid = m_map.GetFloorManager().GetGimmickFloor(FloorManager.FloorType.AMIDA);

        //// グリッドの内容を比較し、変更があればコピー
        //for (int i = 0; i < originalAmidaGrid.GetLength(0); i++)
        //{
        //    for (int j = 0; j < originalAmidaGrid.GetLength(1); j++)
        //    {
        //        if (m_prevAmidaGimmickGrid[i, j] != originalAmidaGrid[i, j] ||
        //            m_prevTopGimmickGrid[i, j] != originalTopGrid[i, j])
        //        {
        //            m_startAmida = true;
        //            m_prevTopGimmickGrid[i, j] = originalTopGrid[i, j];
        //            m_prevAmidaGimmickGrid[i, j] = originalAmidaGrid[i, j];
        //        }
        //    }
        //}

        //// あみだの開始フラグが立っている場合、前回の移動キャッシュを保存し、現在の移動キャッシュをクリアする
        //if (m_startAmida)
        //    m_amidaPrevMoveCash = new List<AmidaMoveCash>(m_amidaMoveCash);

        //m_amidaMoveCash.Clear();
    }

    /// <summary>
    /// あみだチューブの追加要求
    /// </summary>
    /// <param name="gridPos">グリッド位置</param>
    /// <param name="amidaTubeObject">あみだチューブオブジェクト</param>
    /// <returns>true 成功 false 失敗</returns>
    public bool RequestAddGridAmidaTube(GridPos gridPos, GameObject amidaTubeObject)
    {
        //m_amidaTubeGrid = m_map.GetFloorManager().GetGimmickFloor(FloorManager.FloorType.AMIDA);

        //// コンポーネントを持っていない場合は以下の処理を飛ばす
        //if (amidaTubeObject.GetComponent<AmidaTube>() == false) return false;
        //// グリッドの範囲内でなければ以下の処理を飛ばす
        //if (m_map.CheckInnerGridPos(gridPos) == false) return false;
        //// グリッドに他のオブジェクトが入っていれば以下の処理を飛ばす
        //if (m_amidaTubeGrid[gridPos.y, gridPos.x] != null)
        //{
        //    return false;
        //    //amidaTubeObject.SetActive(false);
        //}

        //AmidaTube amidaTube = amidaTubeObject.GetComponent<AmidaTube>();

        //// 追加する
        //m_amidaTubeGrid[gridPos.y, gridPos.x] = amidaTubeObject;

        ////m_amidaTubeGrid = m_map.GetFloorManager().SetGimmickFloor(FloorManager.FloorType.AMIDA, m_amidaTubeGrid);


        //// 通過方向を設定
        //SetTubeConnections(gridPos, amidaTube.GetDirectionPassage());

        return true;
    }

    /// <summary>
    /// 指定したグリッドのデータを削除する
    /// </summary>
    /// <param name="gridPos">グリッド位置</param>
    /// <returns>true 成功 false 失敗</returns>
    public bool RequestRemoveGridAmidaTube(GridPos gridPos)
    {
        // グリッドの範囲内でなければ以下の処理を飛ばす
        if (m_map.CheckInnerGridPos(gridPos) == false) return false;

        m_amidaTubeGrid[gridPos.y, gridPos.x].SetActive(true);

        m_amidaTubeGrid[gridPos.y, gridPos.x] = null;
        // あみだを削除したためどの方向も向いていないものを作成
        AmidaTube.DirectionPassage initPassage = new AmidaTube.DirectionPassage();

        // 通過方向を見直す
        SetTubeConnections(gridPos, initPassage);

        return true;
    }

    /// <summary>
    /// 指定されたグリッド座標の通路方向で各通過方向を設定する関数
    /// </summary>
    /// <param name="gridPos">基準グリッド座標</param>
    /// <param name="checkDirectionPassage">通過方向</param>
    private void SetTubeConnections(GridPos gridPos, AmidaTube.DirectionPassage checkDirectionPassage)
    {
        // 各方向のグリッド位置を計算
        GridPos[] positions = {
            new GridPos(gridPos.x, gridPos.y - 1), // up
            new GridPos(gridPos.x, gridPos.y + 1), // down
            new GridPos(gridPos.x + 1, gridPos.y), // right
            new GridPos(gridPos.x - 1, gridPos.y)  // left
        };

        // 各方向のAmidaTubeを取得
        AmidaTube[] amidaTubes = new AmidaTube[4];
        for (int i = 0; i < positions.Length; i++)
        {
            if (m_map.CheckInnerGridPos(positions[i]))
            {
                amidaTubes[i] = m_amidaTubeGrid[positions[i].y, positions[i].x]?.GetComponent<AmidaTube>();
            }
        }

        // 通過方向を設定
        EnsureTubeConnection(checkDirectionPassage, amidaTubes[0], Direction.UP);
        EnsureTubeConnection(checkDirectionPassage, amidaTubes[1], Direction.DOWN);
        EnsureTubeConnection(checkDirectionPassage, amidaTubes[2], Direction.RIGHT);
        EnsureTubeConnection(checkDirectionPassage, amidaTubes[3], Direction.LEFT);
    }

    /// <summary>
    /// 指定した方向に移動できるかどうか
    /// </summary>
    /// <param name="gridPos">現在のグリッド位置</param>
    /// <param name="direction">移動方向</param>
    /// <returns>移動できる場合は true、それ以外の場合は false</returns>
    private bool CanMovementDirection(GridPos gridPos, Direction direction)
    {
        if (CanMove(gridPos) == false) return false;

        AmidaTube currentAmida = m_amidaTubeGrid[gridPos.y, gridPos.x].GetComponent<AmidaTube>();
        AmidaTube.DirectionPassage passage = currentAmida.GetDirectionPassage();

        switch(direction)
        {
            case Direction.UP:
                if (!passage.down) return false;
                break;
            case Direction.DOWN:
                if (!passage.up) return false;
                break;
            case Direction.RIGHT:
                if (!passage.left) return false;
                break;
            case Direction.LEFT:
                if (!passage.right) return false;
                break;

        }

        return true;
    }

    /// <summary>
    /// 通過方向のチューブと繋がっていない場合新しくチューブを設定する関数
    /// </summary>
    /// <param name="checkDirectionPassage">通過方向</param>
    /// <param name="neighborTube">隣接するあみだチューブ</param>
    /// <param name="checkDirection">調べる方向</param>
    private void EnsureTubeConnection(AmidaTube.DirectionPassage checkDirectionPassage, AmidaTube neighborTube, Direction checkDirection)
    {
        if (neighborTube != null)
        {
            AmidaTube.DirectionPassage directionPassage = neighborTube.GetDirectionPassage();
            switch (checkDirection)
            {
                case Direction.UP:
                    if (checkDirectionPassage.up && !directionPassage.down)
                        directionPassage.down = true;
                    else
                        directionPassage.down = false;
                    neighborTube.SetDirectionPassage(directionPassage);
                    break;
                case Direction.DOWN:
                    if (checkDirectionPassage.down && !directionPassage.up)
                        directionPassage.up = true;
                    else
                        directionPassage.up = false;
                    neighborTube.SetDirectionPassage(directionPassage);
                    break;
                case Direction.RIGHT:
                    if (checkDirectionPassage.right && !directionPassage.left)
                        directionPassage.left = true;
                    else
                        directionPassage.left = false;
                    neighborTube.SetDirectionPassage(directionPassage);
                    break;
                case Direction.LEFT:
                    if (checkDirectionPassage.left && !directionPassage.right)
                        directionPassage.right = true;
                    else
                        directionPassage.right = false;
                    neighborTube.SetDirectionPassage(directionPassage);
                    break;
            }
        }
    }

    /// <summary>
    /// あみだの方向を選択する処理
    /// </summary>
    /// <param name="gridPos">グリッド位置</param>
    /// <param name="movementDirection">移動方向</param>
    /// <param name="electricFlowType">電気の種類</param>
    /// <returns>移動できる場合は true、それ以外の場合は false</returns>
    private bool SelectAmidaDirection(GridPos gridPos, Direction movementDirection, Electric.ElectricFlowType electricFlowType)
    {
        // 活動していない場合は処理を飛ばす
        if (m_amidaTubeGrid[gridPos.y, gridPos.x] == null || !m_amidaTubeGrid[gridPos.y, gridPos.x].activeSelf) return false;

        AmidaTube.DirectionPassage passage = m_amidaTubeGrid[gridPos.y, gridPos.x].GetComponent<AmidaTube>().GetDirectionPassage();

        // 横方向に移動
        if (movementDirection == Direction.RIGHT || movementDirection == Direction.LEFT)
        {
            if (passage.up)
            {
                Move(gridPos, Direction.UP, electricFlowType);
                return true;
            }
            if (passage.down)
            {
                Move(gridPos, Direction.DOWN, electricFlowType);
                return true;
            }

            // 同じ方向に進み続ける
            Move(gridPos, movementDirection, electricFlowType);
            return true;
        }

        // 縦方向に移動
        if (movementDirection == Direction.UP || movementDirection == Direction.DOWN)
        {
            if (passage.right)
            {
                Move(gridPos, Direction.RIGHT, electricFlowType);
                return true;
            }

            // 同じ方向に進み続ける
            Move(gridPos, movementDirection, electricFlowType);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 指定方向に移動させる処理
    /// </summary>
    /// <param name="gridPos">グリッド位置</param>
    /// <param name="movementDirection">移動方向</param>
    /// <param name="electricFlowType">電気の種類</param>
    private void Move(GridPos gridPos, Direction movementDirection, Electric.ElectricFlowType electricFlowType)
    {
        // 活動していない場合は処理を飛ばす
        if (m_amidaTubeGrid[gridPos.y, gridPos.x] == null || !m_amidaTubeGrid[gridPos.y, gridPos.x].activeSelf) return;

        AmidaTube.DirectionPassage passage = m_amidaTubeGrid[gridPos.y, gridPos.x].GetComponent<AmidaTube>().GetDirectionPassage();

        // 移動しているオブジェクト
        GameObject movingObject = m_amidaTubeGrid[gridPos.y, gridPos.x];

        // 事前に移動方向を保存
        Direction prevMovementDirection = movementDirection;

        bool endMovement = false;

        // x方向に移動
        if (movementDirection == Direction.RIGHT)
        {
            if (gridPos.x + 1 < m_map.GetCommonData().width)
                gridPos.x = gridPos.x + 1;
            else
                endMovement = true;

            movementDirection = Direction.RIGHT;
        }
        // y方向に移動
        else if (movementDirection == Direction.DOWN)
        {
            if (gridPos.y + 1 < m_map.GetCommonData().height)
                gridPos.y = gridPos.y + 1;
            else
                endMovement = true;

            movementDirection = Direction.DOWN;
        }
        // y方向に移動
        else if (movementDirection == Direction.UP)
        {
            if (gridPos.y - 1 >= 0)
                gridPos.y = gridPos.y - 1;
            else
                endMovement = true;

            movementDirection = Direction.UP;
        }
        else
            endMovement = true;

        if (movingObject)
        // 移動していたオブジェクトに電気を流す
        ConductElectricity(GetElectricMaterial(electricFlowType).color, movingObject, prevMovementDirection, electricFlowType);

        // 終了する場合
        if (endMovement) return;

        // 進んだ先も確認する
        if (CanMove(gridPos) == false) return;

        // 移動オブジェクトの更新
        movingObject = m_amidaTubeGrid[gridPos.y, gridPos.x].gameObject;

        // 反対方向にする
        Direction counterDirection = movementDirection;
        switch (movementDirection)
        {
            case Direction.UP: counterDirection = Direction.DOWN; break;
            case Direction.DOWN: counterDirection = Direction.UP; break;
            case Direction.LEFT: counterDirection = Direction.RIGHT; break;
            case Direction.RIGHT: counterDirection = Direction.LEFT; break;
        }

        // 新しい移動オブジェクトの色を変更する
        ConductElectricity(GetElectricMaterial(electricFlowType).color, movingObject, counterDirection, electricFlowType);

        // あみだの方向を決める
        SelectAmidaDirection(gridPos, movementDirection, electricFlowType);
    }

    /// <summary>
    /// あみだの色を変更する処理
    /// </summary>
    /// <param name="color">変更色</param>
    /// <param name="amidaObject">変更するあみだオブジェクト</param>
    /// <param name="direction">移動方向</param>
    /// <param name="electricFlowType">電気の種類</param>
    private void ConductElectricity(Color32 color, GameObject amidaObject, Direction direction, Electric.ElectricFlowType electricFlowType)
    {
        AmidaTube amidaTube = amidaObject.GetComponent<AmidaTube>();
        //amidaTube.ConductElectricity(color, direction, electricFlowType);
        amidaTube.ConductElectricity(m_baseTexture);
    }
    /// <summary>
    /// あみだくじを開始する処理
    /// </summary>
    /// <param name="gridPos">グリッド位置</param>
    /// <param name="electricFlowType">電気の種類</param>
    /// <param name="startDirection">開始方向</param>
    private void StartAmidakuji(GridPos gridPos, Electric.ElectricFlowType electricFlowType, Direction startDirection)
    {
        // 移動可能かどうかを確認
        if (CanMovementDirection(gridPos, startDirection) == false) return;

        // 初期の移動方向を設定
        Direction movementDirection = Direction.RIGHT;

        // 通過方向に基づいて移動方向を決定
        if (m_amidaTubeGrid[gridPos.y, gridPos.x].GetComponent<AmidaTube>().GetDirectionPassage().right)
            movementDirection = Direction.RIGHT;
        if (m_amidaTubeGrid[gridPos.y, gridPos.x].GetComponent<AmidaTube>().GetDirectionPassage().down)
            movementDirection = Direction.DOWN;
        if (m_amidaTubeGrid[gridPos.y, gridPos.x].GetComponent<AmidaTube>().GetDirectionPassage().up)
            movementDirection = Direction.UP;

        // 反対方向を設定
        Direction counterDirection = startDirection;
        switch (movementDirection)
        {
            case Direction.UP: counterDirection = Direction.DOWN; break;
            case Direction.DOWN: counterDirection = Direction.UP; break;
            case Direction.LEFT: counterDirection = Direction.RIGHT; break;
            case Direction.RIGHT: counterDirection = Direction.LEFT; break;
        }

        // あみだオブジェクトの色を変更
        ConductElectricity(GetElectricMaterial(electricFlowType).color, m_amidaTubeGrid[gridPos.y, gridPos.x].gameObject, Direction.LEFT, electricFlowType);

        // 右方向に移動
        Move(gridPos, movementDirection, electricFlowType);
    }

    /// <summary>
    /// 全てのブロックの色をリセットする
    /// </summary>
    private void ResetAllBlockColor()
    {
        for (int cy = 0; cy < m_map.GetCommonData().height; cy++)
        {
            for (int cx = 0; cx < m_map.GetCommonData().width; cx++)
            {
                if (m_amidaTubeGrid[cy, cx] && m_amidaTubeGrid[cy, cx].CompareTag("AmidaTube"))
                    m_amidaTubeGrid[cy, cx].GetComponent<AmidaTube>().ResetState();
            }
        }
    }

    /// <summary>
    /// 移動できるかどうかを確認する
    /// </summary>
    /// <param name="gridPos">移動する予定のグリッド位置</param>
    /// <returns>移動できる場合は true、それ以外の場合は false</returns>
    private bool CanMove(GridPos gridPos)
    {
        if (m_map.CheckInnerGridPos(gridPos) == false) return false;
        if (m_amidaTubeGrid[gridPos.y, gridPos.x] == null || !m_amidaTubeGrid[gridPos.y, gridPos.x].activeSelf) return false;
        if (m_amidaTubeGrid[gridPos.y, gridPos.x].tag != "AmidaTube") return false;

        return true;
    }

    /// <summary>
    /// 電気の種類に応じたマテリアルを取得する
    /// </summary>
    /// <param name="electricFlowType">電気の種類</param>
    /// <returns>対応するマテリアル</returns>
    private Material GetElectricMaterial(Electric.ElectricFlowType electricFlowType)
    {
        switch (electricFlowType)
        {
            case Electric.ElectricFlowType.NORMAL:
                return m_amidaElectricityMaterial;
            case Electric.ElectricFlowType.RED:
                return m_amidaRedElectricityMaterial;
        }

        return m_amidaElectricityMaterial;
    }

    /// <summary>
    /// あみだくじの開始要求を追加する
    /// </summary>
    /// <param name="gridPos">グリッド位置</param>
    /// <param name="electricFlowType">電気の種類</param>
    /// <param name="startDirection">開始方向</param>
    public void RequestStartAmida(GridPos gridPos, Electric.ElectricFlowType electricFlowType, Direction startDirection)
    {
        AmidaMoveCash amidaMoveCash;
        amidaMoveCash.gridPos = gridPos;
        amidaMoveCash.electricFlowType = electricFlowType;
        amidaMoveCash.movementDir = startDirection;

        m_amidaMoveCash.Add(amidaMoveCash);
    }
}
