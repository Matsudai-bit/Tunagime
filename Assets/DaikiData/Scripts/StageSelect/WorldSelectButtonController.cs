using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WorldSelectButtonController : MonoBehaviour
{
    /// <summary>
    /// ワールドセレクトボタンデータ
    /// </summary>
    [Serializable]
    public class WorldSelectButtonData
    {
        public string worldName;
        public WorldID worldID;
        public GameObject gameObject;
    }

    [Serializable]
    enum State
    {
        WORLD_SELECT,
        CHANGING_STAGE_SELECT, // ステージセレクトに切り替え中
        STAGE_SELECT,
        CHANGING_WORLD_SELECT, // ワールドセレクトに切り替え中
    }


    // 変数宣言 -----------------------------

    [Header("ワールドセレクトボタンデータ")]
    [SerializeField]
    public WorldSelectButtonData[] m_worldSelectButtonDataArray = new WorldSelectButtonData[5]; // ワールドセレクトボタンデータの配列

    [Header("ステージセレクトコントローラ")]
    [SerializeField]
    private StageSelectController m_stageSelectController; // ステージセレクトコントローラ

    [Header("各ワールドのボタンの情報")]
    [SerializeField]
    StageSelectButtonDataForWorld m_stageSelectButtonDataForWorld; // 各ワールドのボタンの情報

    [Header("ワールドオブジェクトセレクター")]
    [SerializeField]
    private WorldObjectSelector m_worldObjectSelector; // ワールドオブジェクトセレクター
    private Dictionary<WorldID, GameObject> m_worldSelectButtonDictionary = new Dictionary<WorldID, GameObject>(); // 辞書型でワールドIDとボタンのGameObjectを紐付け


    private WorldID m_currentWorldID; // 現在のワールドID

    private State m_currentState = State.WORLD_SELECT; // 現在の状態

    private Dictionary<WorldID, Vector3> m_layoutPositions = new Dictionary<WorldID, Vector3>(); // ボタンのレイアウト位置リスト

   
    void Awake()
    {
        // 配列のデータを辞書に変換
        foreach (var data in m_worldSelectButtonDataArray)
        {
            m_worldSelectButtonDictionary[data.worldID] = data.gameObject;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_currentWorldID = WorldID.World_1; // 初期ワールドIDを設定
        UpdateButtonState();
        AlignButton();

        // 各ボタンの初期位置を保存
        foreach (var button in m_worldSelectButtonDictionary)
        {
            var rectTransform = button.Value.GetComponent<RectTransform>();
            m_layoutPositions.Add(button.Key, rectTransform.anchoredPosition);
        }

        ChangeState(State.WORLD_SELECT);

        ChangeWorldObject(m_currentWorldID, WorldObjectSelector.ScrollDirection.RIGHT);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_stageSelectController.GetCurrentState() == StageSelectController.State.WORLD_SELECT && 
            m_currentState == State.STAGE_SELECT)
        {
            // ステージセレクトからワールドセレクトに戻る
            ChangeState(State.CHANGING_WORLD_SELECT);
        }
    }


    private void Reset()
    {
        // 配列を初期化
        m_worldSelectButtonDataArray = new WorldSelectButtonData[Enum.GetValues(typeof(WorldID)).Length];
        for (int i = 0; i < m_worldSelectButtonDataArray.Length; i++)
        {
            m_worldSelectButtonDataArray[i] = new WorldSelectButtonData
            {
                worldName = ((WorldID)i).ToString(),
                worldID = (WorldID)i,
                gameObject = null
            };
        }
    }


    /// <summary>
    /// ボタンの位置を整列
    /// </summary>
    private void AlignButton()
    {
        var rectTransform = m_worldSelectButtonDictionary[WorldID.World_1].GetComponent<RectTransform>();

        Vector2 basePosition = rectTransform.anchoredPosition; // 基準位置
        //Vector2 basePosition = m_worldSelectButtonDictionary[WorldID.World_1].transform.position; // 基準位置

        float offsetY = 194.0f; // Y方向のオフセット（ボタン間の距離）
        float offsetX = 42.0f; // X方向のオフセット（必要に応じて調整)
        for (int i = 0; i < m_worldSelectButtonDataArray.Length; i++)
        {
            var buttonData = m_worldSelectButtonDataArray[i];
            if (buttonData.gameObject != null)
            {
                var rectTransformAnother = buttonData.gameObject.GetComponent<RectTransform>();
                rectTransformAnother.anchoredPosition = new Vector2(basePosition.x + offsetX * i, basePosition.y - i * offsetY);
            }
        }
    }

    /// <summary>
    /// 状態を変更
    /// </summary>
    /// <param name="newState"></param>
    private void ChangeState(State newState)
    {
        m_currentState = newState;
        switch (newState)
        {
            case State.WORLD_SELECT:
                // ワールドセレクト状態の処理
                break;
            case State.CHANGING_STAGE_SELECT:
                // ステージセレクトに切り替え中の処理
                StartChangingStageSelect();
                break;
            case State.STAGE_SELECT:
                // ステージセレクト状態の処理
                m_stageSelectController.SetButtonPrefab(m_stageSelectButtonDataForWorld.stageSelectButtonData[(int)m_currentWorldID].buttonPrefab);
                m_stageSelectController.SetRootWorldButton(m_worldSelectButtonDictionary[m_currentWorldID]);
                m_stageSelectController.gameObject.SetActive(true);
                m_stageSelectController.SetCurrentWorldID(m_currentWorldID);
                break;
            case State.CHANGING_WORLD_SELECT:
                
                StartChangingWorldSelect();
                break;
            default:
                Debug.LogWarning("Unknown state");
                break;
        }
    }

    void StartChangingWorldSelect()
    {
        // ステージセレクトのボタンを非表示にする
        m_stageSelectController.gameObject.SetActive(false);
        // 各ボタンを元の位置に戻す
        foreach (var kvp in m_worldSelectButtonDictionary)
        {
            var button = kvp.Value;
            button.SetActive(true);
            button.GetComponent<Image>().DOFade(1, 1.5f).SetEase(Ease.InOutSine);

            // 目標位置の取得 (orld_1の位置に移動)
            var targetPosition = m_layoutPositions[kvp.Key];
            // 現在のボタンの取得
            var rectTransform = button.GetComponent<RectTransform>();

            button.transform.DOLocalMove(m_layoutPositions[kvp.Key], 1.0f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                ChangeState(State.WORLD_SELECT);
                Debug.Log("Changed to WORLD_SELECT state");
                UpdateButtonState();
            });

            // テキストを復活
            var text = button.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.DOFade(1, 1.5f).SetEase(Ease.InOutSine);
            }
        }
    }

    /// <summary>
    /// ステージセレクトに切り替え開始
    /// </summary>
    void StartChangingStageSelect()
    {
        // 目標位置の取得 (orld_1の位置に移動)
        var targetPosition = m_layoutPositions[WorldID.World_1];

        // 現在のボタンの取得
        var currentButton = m_worldSelectButtonDictionary[m_currentWorldID];
        
        var rectTransform = currentButton.GetComponent<RectTransform>();

        rectTransform.DOLocalMove(targetPosition, 1.0f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            ChangeState(State.STAGE_SELECT);
            Debug.Log("Changed to STAGE_SELECT state");
        });

        // その他のボタンをフェードアウト
        foreach (var kvp in m_worldSelectButtonDictionary)
        {
            if (kvp.Key != m_currentWorldID)
            {
                // それぞれのボタンをフェードアウト
                var button = kvp.Value;
                button.GetComponent<Image>().DOFade(0, 0.5f).SetEase(Ease.InOutSine).OnComplete(() =>
                {
                    button.SetActive(false);

                });

                // テキストもフェードアウト
                var text = button.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null)
                {
                    text.DOFade(0, 0.5f).SetEase(Ease.InOutSine);
                }


            }
        }
    }


    /// <summary>
    /// ボタンの表示・非表示を更新
    /// </summary>
    private void UpdateButtonState()
    {
        foreach (var kvp in m_worldSelectButtonDictionary)
        {
            var targetButton = kvp.Value.GetComponent<Button>();

            if (kvp.Key == m_currentWorldID)
            {
                targetButton.OnPointerEnter(null);
            }
            else
            {
                targetButton.OnPointerExit(null);
            }
        }
    }


    /// <summary>
    /// 上下の入力に基づいてワールドIDを変更
    /// </summary>
    /// <param name="value"></param>
    public void OnNavigate(InputValue value)
    {

        if (m_currentState != State.WORLD_SELECT)
        {
            return;
        }

        Debug.Log("OnNavigate called");

        Vector2 input = value.Get<Vector2>();

        int newWorldID = (int)m_currentWorldID;

        // 上下の入力に基づいてワールドIDを変更

        // 下入力
        if (input.y < 0)
        {
            // 循環させるために剰余を取る
            newWorldID = (newWorldID + 1) ;
        }
        // 上入力
        else if (input.y > 0)
        {
            // 負の数に対応するために、長さを足してから剰余を取る
            newWorldID = (newWorldID - 1 );
        }

        newWorldID = Math.Clamp(newWorldID, 0, Enum.GetValues(typeof(WorldID)).Length - 1);

        if (newWorldID != (int)m_currentWorldID)
        {
            m_currentWorldID = (WorldID)newWorldID;
            UpdateButtonState();
            ChangeWorldObject(m_currentWorldID, (WorldObjectSelector.ScrollDirection)(-input.y));
            Debug.Log($"Switched to {m_currentWorldID}");
        }
    }

    /// <summary>
    /// 現在のワールドIDに対応するボタンをクリック
    /// </summary>
    /// <param name="value"></param>
    public void OnSubmit(InputValue value)
    {
        if (m_currentState != State.WORLD_SELECT)
        {
            return;
        }

        Debug.Log($"OnSubmit called for {m_currentWorldID}");
        if (m_worldSelectButtonDictionary.TryGetValue(m_currentWorldID, out GameObject buttonObj))
        {
            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                // 
                button.OnSubmit(null);
                ChangeState(State.CHANGING_STAGE_SELECT);
                Debug.Log($"Clicked button for {m_currentWorldID}");
            }
            else
            {
                Debug.LogWarning($"Button component not found on {buttonObj.name}");
            }
        }
        else
        {
            Debug.LogWarning($"No button found for WorldID {m_currentWorldID}");
        }
    }

    /// <summary>
    /// 指定したボタンにホバーイベントを送信
    /// </summary>
    /// <param name="targetButton"></param>
    public void HoverButton(Button targetButton)
    {
        if (targetButton == null) return;

        targetButton.OnPointerEnter(null);
    }

    /// <summary>
    /// 現在のワールドIDを取得
    /// </summary>
    /// <returns></returns>
    public WorldID GetCurrentWorldID()
    {
        return m_currentWorldID;
    }

    void ChangeWorldObject(WorldID worldID, WorldObjectSelector.ScrollDirection scrollDirection)
    {
        m_worldObjectSelector.ChangeWorldObject(worldID, scrollDirection);
    }

}
