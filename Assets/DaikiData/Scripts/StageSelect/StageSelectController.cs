using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ステージセレクトのコントローラー
/// </summary>
public class StageSelectController : MonoBehaviour
{
   public enum State
    {
        START_CHANGING_STAGE_SELECTION,  // 開始時のステージセレクトに切り替え中
        STAGE_SELECT,
        CHANGING_WORLD_SELECTING, // ステージセレクトに切り替え中
        WORLD_SELECT,
    }

    [Header("ボタンの親オブジェクト")]
    [SerializeField]
    private Transform m_buttonParent; // ボタンの親オブジェクト

    [Header("ワールドオブジェクトセレクター")]
    [SerializeField]
    private WorldObjectSelector m_worldObjectSelector; // ワールドオブジェクトセレクター

    private WorldStageData m_worldStageData;

    private GameObject m_buttonPrefab; // ボタンのプレハブ

    private List<GameObject> m_buttonObjects = new ();  // ステージセレクトボタンのリスト



    private int m_currentButtonIndex = 0; // 現在のステージID
    private State m_currentState = State.STAGE_SELECT; // 現在の状態

    private GameObject m_rootWorldButton; // 親のワールドボタン

    private WorldID m_currentWorldID = WorldID.World_1; // 現在のワールドID

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
       
        ChangeState(State.START_CHANGING_STAGE_SELECTION);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ステージセレクトに切り替え開始
    /// </summary>
    private void StartChangingStageSelection()
    {

        if (m_buttonObjects.Count > 0)
        {
            // ボタンの作成
            m_buttonObjects[0] = m_rootWorldButton;

            int buttonCount = Enum.GetValues(typeof(StageID)).Length;
            for (int i = 1; i <= buttonCount; i++)
            {
                var buttonObject = m_buttonObjects[i];
                buttonObject.SetActive(true);
                int stageIndex = i - 1;
                var button = buttonObject.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {

                    var gameProgressData = GameProgressManager.Instance.GameProgressData;
                    gameProgressData.worldID = m_currentWorldID;
                    gameProgressData.stageID = (StageID)stageIndex;

                    MapData.GetInstance.SetStageSetting(m_worldStageData.stageSettings[stageIndex]);

                    SceneManager.LoadScene("GameplayScene");
                });
            }
        }
        else
        {
            // ボタンの作成
            int buttonCount = Enum.GetValues(typeof(StageID)).Length;

            m_buttonObjects.Add(m_rootWorldButton);
            for (int i = 1; i <= buttonCount; i++)
            {
                StageID stageID = (StageID)i;
                GameObject buttonObject = Instantiate(m_buttonPrefab, m_buttonParent);
                m_buttonObjects.Add(buttonObject);

                buttonObject.name = "StageButton_" + stageID.ToString();
                var text = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
                if (text)
                {
                    text.text = "ステージ" + i.ToString();
                }

                int stageIndex = i - 1;
                var button = buttonObject.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {

                    var gameProgressData =  GameProgressManager.Instance.GameProgressData;
                    gameProgressData.worldID = m_currentWorldID;
                    gameProgressData.stageID = (StageID)stageIndex;

                    MapData.GetInstance.SetStageSetting(m_worldStageData.stageSettings[stageIndex]);

                    SceneManager.LoadScene("GameplayScene");
                });
            }
        }

        AlignButton();

        m_currentButtonIndex = (int)StageID.STAGE_1; // 初期ステージIDを設定

    }

    /// <summary>
    /// ボタンのプレハブを設定
    /// </summary>
    /// <param name="prefab"></param>
    public void SetButtonPrefab(GameObject prefab)
    {
        m_buttonPrefab = prefab;
    }


    public void SetRootWorldButton(GameObject root)
    {
        m_rootWorldButton = root;
    }

    public void SetWorldStageData(WorldStageData data)
    {
        m_worldStageData = data;
    }

    public void SetCurrentWorldID(WorldID id)
    {
        m_currentWorldID = id;
    }


    /// <summary>
    /// ボタンの位置を整列
    /// </summary>
    void AlignButton()
    {
        Vector2 basePosition = m_buttonObjects[0].GetComponent<RectTransform>().anchoredPosition; // 基準位置

        basePosition.y += m_buttonObjects[0].GetComponent<Image>().sprite.bounds.size.y * 10.0f;// m_buttonObjects[0].GetComponent<RectTransform>().localScale.y;

        float offsetY = 150.0f; // Y方向のオフセット（ボタン間の距離）
        float offsetX = 42.0f; // X方向のオフセット（必要に応じて調整)

        for (int i = 1; i < m_buttonObjects.Count; i++)
        {


            var element = m_buttonObjects[i];
            if (element != null)
            {

                var rectTransform = element.GetComponent<RectTransform>();

                rectTransform.anchoredPosition = basePosition;
                // イージングをかける
                var targetPosition = new Vector2(basePosition.x + offsetX * i, basePosition.y - i * offsetY);
                float duration = 1.0f ;
               var d =  rectTransform.DOLocalMove(targetPosition, duration).SetEase(Ease.OutCubic).SetDelay(1.0f / 5);

                // 最後のボタンのアニメーション完了時に状態を変更
                if (i == m_buttonObjects.Count - 1)
                {
                    d.OnComplete(() =>
                    {
                        ChangeState(State.STAGE_SELECT);
                        UpdateButtonState();
                    });
                }
             
            }
        }
    }

    private void ChangeState(State newState)
    {
        m_currentState = newState;

        switch (newState)
        {
            case State.START_CHANGING_STAGE_SELECTION:
                StartChangingStageSelection();
                break;
            case State.STAGE_SELECT:
                break;
            case State.CHANGING_WORLD_SELECTING:
                // ワールドセレクトに切り替え中の処理
                break;
            case State.WORLD_SELECT:
                for (int i = 0; i < m_buttonObjects.Count; i++)
                {
                    if (i == 0) continue; // 最初のボタンはワールドボタンなのでスキップ

                    m_buttonObjects[i].SetActive(false);
                }
                // ワールドセレクト状態に入ったときの処理
                break;
            default:
                Debug.LogWarning("Unknown state: " + newState);
                break;
        }
    }

    /// <summary>
    /// ボタンの表示・非表示を更新
    /// </summary>
    private void UpdateButtonState()
    {
        for (int i = 0; i < m_buttonObjects.Count;i++)
        {
            var element = m_buttonObjects[i];

            var targetButton = element.GetComponent<Button>();

            if (i == m_currentButtonIndex)
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
        if (m_currentState != State.STAGE_SELECT)
        {
            return;
        }

        Debug.Log("OnNavigate called");

        Vector2 input = value.Get<Vector2>();

        int newStageID = (int)m_currentButtonIndex;

        // 上下の入力に基づいてワールドIDを変更

        int buttonCount = Enum.GetValues(typeof(StageID)).Length + 1;

      // 下入力
        if (input.y < 0)
        {
            // 循環させるために剰余を取る
            newStageID = (newStageID + 1) ;
        }
        // 上入力
        else if (input.y > 0)
        {
            // 負の数に対応するために、長さを足してから剰余を取る
            newStageID = (newStageID - 1 );
        }

        newStageID = Math.Clamp(newStageID, 0, Enum.GetValues(typeof(StageID)).Length );

        if (newStageID != (int)m_currentButtonIndex)
        {
            m_currentButtonIndex = newStageID;
            UpdateButtonState();

            if (m_currentButtonIndex >= 1)
            {
                m_worldObjectSelector.ChangeStage((StageID)(m_currentButtonIndex - 1));
            }
            else
            {
                m_worldObjectSelector.ResetCameraPosition();
            }
            Debug.Log($"Switched to {m_currentButtonIndex}");
        }
    }

    /// <summary>
    /// 現在のワールドIDに対応するボタンをクリック
    /// </summary>
    /// <param name="value"></param>
    public void OnSubmit(InputValue value)
    {
        if (m_currentState != State.STAGE_SELECT)
        {
            return;
        }

        Debug.Log($"OnSubmit called for {m_currentButtonIndex}");

        GameObject buttonObj = m_buttonObjects[m_currentButtonIndex];

        Button button = buttonObj.GetComponent<Button>();

        Debug.Log($"Clicked button for {m_currentButtonIndex}");
        if (m_currentButtonIndex == 0)
        {
            // ワールドボタンが押された場合、ワールドセレクトに戻る
            ExitStageSelect();
            return;
        }

        if (button != null)
        {
            
            button.onClick.Invoke();
        }
        else
        {
            Debug.LogWarning($"Button component not found on {buttonObj.name}");
        }
    }

    public void OnCancel(InputValue value)
    {
        ExitStageSelect();
    }

    public State GetCurrentState()
    {
        return m_currentState;
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
    /// ステージセレクトを終了してワールドセレクトに戻る
    /// </summary>
    void ExitStageSelect()
    {
        if (m_currentState != State.STAGE_SELECT)
        {
            return;
        }
        Debug.Log("OnCancel called");
        ChangeState(State.WORLD_SELECT);
        UpdateButtonState();
        // ワールドオブジェクトセレクターにワールドセレクトに戻るよう指示
        m_worldObjectSelector.ResetCameraPosition();
    }
}
