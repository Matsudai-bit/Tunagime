using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

/// <summary>
/// プレイヤー（想いの断片）の挙動を制御するクラス。
/// マップ上を自動で移動し、他の断片とマージする機能を持つ。
/// </summary>
[RequireComponent(typeof(StageBlock))]
public class Fragment : MonoBehaviour
{
    // === フィールドとプロパティ ===

    // 他のコンポーネントへの参照
    private StageBlock m_stageBlock;
    [SerializeField] private MeshRenderer m_meshRenderer;


    // 現在の状態を管理する変数
    [Header("=== 状態管理 ===")]
    [SerializeField]
    private State m_currentState = State.MOVING;
    [SerializeField]
    private MovementDirectionID m_currentMovementDirection = MovementDirectionID.RIGHT;

    // 横方向の移動を記憶するための変数
    private MovementDirectionID m_currentSideDirection = MovementDirectionID.RIGHT;
    // 1フレーム前のグリッド座標を保持
    private GridPos m_prevGridPos = new GridPos();

    // プレイヤーのレベル
    [Header("=== レベル ===")]
    [SerializeField]
    private Level m_level = Level.LEVEL_1;

    // === 設定パラメータ ===

    [Header("=== 移動設定 ===")]
    [Tooltip("断片が移動する速さ")]
    [SerializeField] private float m_speed = 0.1f;
    [Tooltip("あみだの結び目判定に使用するレイキャストの距離")]
    [SerializeField] private float m_raycastDistance = 0.1f;

    [Header("=== 想いの断片の大きさ ===")]
    [SerializeField] private float LEVEL_1_SIZE = 0.75f; // レベル1の断片のサイズ
    [SerializeField] private float LEVEL_2_SIZE = 1.0f; // レベル2の断片のサイズ
    [SerializeField] private float LEVEL_3_SIZE = 1.75f; // レベル3の断片のサイズ

    [Header("=== 大きさの変わる速度 ===")]
    [SerializeField] private float SIZE_CHANGE_DURATION = 2.0f; // サイズ変更にかかる時間

    // === 公開プロパティ ===

    public MovementDirectionID MovementDirection => m_currentMovementDirection;
    public MovementDirectionID CurrentSideDirection => m_currentSideDirection;
    public Level level => m_level;
    public MeshRenderer MeshRenderer => m_meshRenderer != null ? m_meshRenderer : null;

    // === 列挙型 ===

    /// <summary>
    /// 移動方向の列挙型
    /// </summary>
    public enum MovementDirectionID
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    /// <summary>
    /// 断片のレベル
    /// </summary>
    public enum Level
    {
        LEVEL_1 = 1,
        LEVEL_2,
        LEVEL_3,
        LEVEL_4,
    }

    /// <summary>
    /// 断片の現在の状態
    /// </summary>
    public enum State
    {
        MOVING, // 移動中
        MERGING, // マージ中
    }

    // === MonoBehaviourライフサイクルメソッド ===

    private void Awake()
    {
        InitializeComponents();
    }

    private void Start()
    {
        // 初期状態の設定
        SetUpSizeForLevel(); // レベルに応じてサイズを設定
    }

    private void FixedUpdate()
    {
        // 現在の状態がMOVINGの場合のみ、移動処理を実行
        if (m_currentState == State.MOVING)
        {
            MoveOnGrid();
        }
    }

    private void Update()
    {
        if (m_level == Level.LEVEL_4)
        {
            var core =  StageObjectFactory.GetInstance().GenerateCarriableCore(null,m_stageBlock.GetGridPos(), GetComponent<EmotionCurrent>().CurrentType);
            MapData.GetInstance.GetStageGridData().RemoveGridDataGameObject(m_stageBlock.GetGridPos()); // グリッドデータから削除
            MapData.GetInstance.GetStageGridData().TryPlaceTileObject(m_stageBlock.GetGridPos(), core); // グリッドデータに登録
            gameObject.SetActive(false); // レベル4の断片は非アクティブにする
            return;
        }

        if (m_currentState == State.MOVING)
        {
            if (TryFindFragmentToMovementDirection(out Fragment findingFragment))
            {
                // 断片が見つかった場合、マージリクエストを送る
                if (findingFragment != null)
                {
                    RequestMerge(findingFragment);
                    findingFragment.RequestMerge(this);

                }
            }
        }
    }

    // === メインの処理メソッド ===

    /// <summary>
    /// ステージのグリッドに沿って断片を移動させる主要な処理
    /// </summary>
    private void MoveOnGrid()
    {
        var map = MapData.GetInstance;
        var stageGridData = map.GetStageGridData();
        GridPos currentGridPos = m_stageBlock.GetGridPos();
        // 移動先に障害物がないか確認し、あれば方向転換する
        CheckAndReverseDirection();
        // グリッド座標が変化した場合、あみだの結び目判定を行う
        if (m_prevGridPos != currentGridPos)
        {
            HandleAmidaTubeLogic(currentGridPos, stageGridData, map);
        }



        // 実際の移動処理
        ApplyMovement(map);

        // 現在のグリッド座標を更新
        m_stageBlock.UpdatePosition(map.GetClosestGridPos(transform.position), false);
    }

    /// <summary>
    /// コンポーネントの初期化を行う
    /// </summary>
    private void InitializeComponents()
    {
        m_stageBlock = GetComponent<StageBlock>();
        if (m_stageBlock == null)
        {
            Debug.LogError("Fragment requires a StageBlock component.");
        }
    }

    /// <summary>
    /// あみだの結び目に到達した場合、移動方向を切り替える
    /// </summary>
    /// <param name="currentGridPos">現在のグリッド座標</param>
    /// <param name="stageGridData">ステージのグリッドデータ</param>
    /// <param name="map">マップデータ</param>
    private void HandleAmidaTubeLogic(GridPos currentGridPos, StageGridData stageGridData, MapData map)
    {
        // タイルの中心座標を取得し、プレイヤーが中心に到達したかを判定する
        if (IsNearTileCenter(currentGridPos, map))
        {
            var amidaTube = stageGridData.GetAmidaTube(currentGridPos);

            if (amidaTube != null)
            {
                // あみだの結び目の状態に応じて方向を切り替える
                switch (amidaTube.GetState())
                {
                    case AmidaTube.State.KNOT_DOWN:
                        // 上方向から来た場合、横方向へ。それ以外は下方向へ
                        m_currentMovementDirection = (m_currentMovementDirection == MovementDirectionID.UP) ? m_currentSideDirection : MovementDirectionID.DOWN;
                        break;
                    case AmidaTube.State.KNOT_UP:
                        // 下方向から来た場合、横方向へ。それ以外は上方向へ
                        m_currentMovementDirection = (m_currentMovementDirection == MovementDirectionID.DOWN) ? m_currentSideDirection : MovementDirectionID.UP;
                        break;
                }
            }
            m_prevGridPos = m_stageBlock.GetGridPos();

        }
    }

    /// <summary>
    /// プレイヤーがタイルの中心に近づいたかを判定する
    /// </summary>
    /// <param name="currentGridPos">現在のグリッド座標</param>
    /// <param name="map">マップデータ</param>
    /// <returns>中心に到達したかどうかの真偽値</returns>
    private bool IsNearTileCenter(GridPos currentGridPos, MapData map)
    {
        Vector3 centerTileWorldPos = map.ConvertGridToWorldPos(currentGridPos.x, currentGridPos.y);

        var movedDirection = GetMovementDirectionVector(m_currentMovementDirection);   

        if (Mathf.Approximately(0.0f, movedDirection.x)) centerTileWorldPos.x = 0.0f;

        if (Mathf.Approximately(0.0f, movedDirection.z)) centerTileWorldPos.z = 0.0f;


        if ((m_currentMovementDirection == MovementDirectionID.RIGHT || m_currentMovementDirection == MovementDirectionID.DOWN))

        {
            var direction = (centerTileWorldPos - transform.position);

            if (m_currentMovementDirection == MovementDirectionID.RIGHT && direction.x >= 0.0f) return true;
            if (m_currentMovementDirection == MovementDirectionID.DOWN && direction.z >= 0.0f) return true;
        }

        else
        {
            var direction = (centerTileWorldPos - transform.position);
            if (m_currentMovementDirection == MovementDirectionID.LEFT && direction.x <= 0.0f) return true;
            if (m_currentMovementDirection == MovementDirectionID.UP && direction.z <= 0.0f) return true;
        }

            return false;
    }

    /// <summary>
    /// 前方に障害物がないか確認し、あれば方向転換する
    /// </summary>
    private void CheckAndReverseDirection()
    {
        // 現在の移動方向に応じたベクトルを取得
        Vector3 movedDirection = GetMovementDirectionVector(m_currentMovementDirection);
        Ray ray = new Ray(m_stageBlock.transform.position, movedDirection);

        if (Physics.Raycast(ray, out RaycastHit hit, m_raycastDistance))
        {
            if (!hit.collider.gameObject.CompareTag("Player"))
            {
                // プレイヤー以外に衝突した場合、方向を反転
                m_currentMovementDirection = ReverseDirection(m_currentMovementDirection);
                m_prevGridPos = m_prevGridPos + m_stageBlock.GetGridPos();
            }
        }
    }

    /// <summary>
    /// 実際の移動を適用する
    /// </summary>
    /// <param name="map">マップデータ</param>
    private void ApplyMovement(MapData map)
    {
        // 速度を適用して位置を更新
        transform.position += GetMovementDirectionVector(m_currentMovementDirection) * m_speed;

        // 横方向の移動を記憶
        if (m_currentMovementDirection == MovementDirectionID.LEFT || m_currentMovementDirection == MovementDirectionID.RIGHT)
        {
            m_currentSideDirection = m_currentMovementDirection;
        }

        // 移動方向に応じてZまたはX座標をグリッドに合わせる
        var closestGridPos = map.GetClosestGridPos(transform.position);
        var snappedPosition = map.ConvertGridToWorldPos(closestGridPos);

        if (m_currentMovementDirection == MovementDirectionID.LEFT || m_currentMovementDirection == MovementDirectionID.RIGHT)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, snappedPosition.z);
        }
        else
        {
            transform.position = new Vector3(snappedPosition.x, transform.position.y, transform.position.z);
        }
    }

    // === ユーティリティメソッド ===

    /// <summary>
    /// 移動方向IDを対応するVector3に変換する
    /// </summary>
    /// <param name="movementDirection">移動方向ID</param>
    /// <returns>対応するVector3ベクトル</returns>
    private static Vector3 GetMovementDirectionVector(MovementDirectionID movementDirection)
    {
        return movementDirection switch
        {
            MovementDirectionID.UP => Vector3.forward,
            MovementDirectionID.DOWN => Vector3.back,
            MovementDirectionID.LEFT => Vector3.left,
            MovementDirectionID.RIGHT => Vector3.right,
            _ => Vector3.zero
        };
    }

    /// <summary>
    /// 移動方向を反転させる
    /// </summary>
    /// <param name="direction">現在の方向ID</param>
    /// <returns>反転後の方向ID</returns>
    private static MovementDirectionID ReverseDirection(MovementDirectionID direction)
    {
        return direction switch
        {
            MovementDirectionID.UP => MovementDirectionID.DOWN,
            MovementDirectionID.DOWN => MovementDirectionID.UP,
            MovementDirectionID.LEFT => MovementDirectionID.RIGHT,
            MovementDirectionID.RIGHT => MovementDirectionID.LEFT,
            _ => direction
        };
    }



    /// <summary>
    /// 移動方向にに想いの断片があるかを確認する
    /// </summary>
    /// <returns></returns>
    private bool TryFindFragmentToMovementDirection(out Fragment findingFragment)
    {
        RaycastHit hit;
        Vector3 movedDirection = GetMovementDirectionVector(m_currentMovementDirection);
        Ray ray = new Ray(m_stageBlock.transform.position, movedDirection);
        if (Physics.Raycast(ray, out hit, m_raycastDistance + 1.0f))
        {
    
            // 断片が見つかった場合、マージリクエストを送る
            Fragment fragment = hit.collider.GetComponent<Fragment>();
            if (fragment != null && hit.collider.GetComponent<EmotionCurrent>().CurrentType == GetComponent<EmotionCurrent>().CurrentType)
            {
                findingFragment = fragment;
                return true;
            }
            
        }
        findingFragment = null;
        return false;
    }

    /// <summary>
    /// 想いの断片をマージするリクエストを送る
    /// </summary>
    /// <param name="fragment">マージする相手の断片</param>
    public void RequestMerge(Fragment fragment)
    {
        if ((int)(fragment.level) < (int)(m_level))
        {
            MergeFragment(fragment);
            return;
        }

        if (m_currentSideDirection == MovementDirectionID.RIGHT)
        {

            if (fragment.CurrentSideDirection != m_currentSideDirection || m_currentMovementDirection == MovementDirectionID.DOWN)
            {
                MergeFragment(fragment);
                return;
            }
        }

        gameObject.SetActive(false); // 自分自身を非アクティブにする
        MapData.GetInstance.GetStageGridData().RemoveGridDataGameObject(m_stageBlock.GetGridPos()); // グリッドデータから削除


    }

    private void MergeFragment(Fragment fragment)
    {
        // 断片のレベルを更新
        IncrementLevel(fragment.level);
        // レベルに応じてサイズを設定
        SetUpSizeForLevel(); 

        
    }

    void IncrementLevel(Level level)
    {
        m_level = (Level)(Math.Min((int)(m_level) + (int)(level), (int)(Level.LEVEL_4))); // レベルを更新

    }

    /// <summary>
    /// レベルに応じて断片のサイズを設定する
    /// </summary>
    void SetUpSizeForLevel()
    {
        float targetSize = LEVEL_1_SIZE; // デフォルトはレベル1のサイズ

        switch (m_level)
        {
            case Level.LEVEL_2:
                targetSize = LEVEL_2_SIZE;
                break;
            case Level.LEVEL_3:
                targetSize = LEVEL_3_SIZE;
                break;
            case Level.LEVEL_4:
                // レベル4は最大サイズなので特に設定しない
                break;
        }

        

        transform.DOScale(targetSize, 1.0f).SetEase(Ease.OutBounce, SIZE_CHANGE_DURATION);
        m_raycastDistance = 0.5f * targetSize; // レベルに応じてレイキャストの距離を調整
    }

}