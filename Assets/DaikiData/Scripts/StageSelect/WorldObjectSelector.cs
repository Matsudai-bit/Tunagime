using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ワールドオブジェクトセレクター
/// </summary>
public class WorldObjectSelector : MonoBehaviour
{
    public enum ScrollDirection
    {
        LEFT = -1,
        RIGHT = 1,
    }

    [Header("ワールドオブジェクト")]
    [SerializeField]
    private List<GameObject> m_worldObjects = new(); // ワールドオブジェクト


    [Header("ワールドオブジェクトの親オブジェクト")]
    [SerializeField]
    private Transform m_worldObjectParent; // ワールドオブジェクトの親オブジェクト 

    [Header("ワールドオブジェクトボタンコントローラの取得 (現在選択しているものを取得するよう)")]
    [SerializeField]
    private WorldSelectButtonController m_worldSelectButtonController;

    [Header("ステージセレクトカメラ")]
    [SerializeField]
    private StageSelectCamera m_stageSelectCamera;

    [Header("各XYの距離")]
    [SerializeField]
    Vector3 m_distance = new Vector3(15.0f, 10.0f, 0.0f);


    [Header("ターゲット地点")]
    [SerializeField]
    private Vector3 m_targetPosition = Vector3.zero; // ターゲットのTransform


    // 現在のワールドオブジェクト
    private GameObject m_currentWorldObject ;

    private WorldID m_currentWorldID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ワールドオブジェクトを変更する
    /// </summary>
    public void ChangeWorldObject(WorldID worldID, ScrollDirection scrollDirection)
    {
        
    

        if (m_currentWorldObject != null)
        {
            var endPosition = new Vector3(m_distance.x, -m_distance.y, 0.0f);

            if (scrollDirection == ScrollDirection.LEFT)
            {
                endPosition = -endPosition;
            }

            // 現在のワールドオブジェクトを削除する
            GameObject prevWorldObject = m_currentWorldObject;
            prevWorldObject.transform.DOMove(endPosition, 1.0f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                Destroy(prevWorldObject);
            });

        }

        // ワールドオブジェクトを生成する
        m_currentWorldObject = Instantiate(m_worldObjects[(int)(worldID)], m_worldObjectParent);

        // 新しいオブジェクトを挿入する
        var startPosition = new Vector3(-m_distance.x, m_distance.y, 0.0f);


        if (scrollDirection == ScrollDirection.LEFT)
        {
            startPosition = -startPosition;
        }

        m_currentWorldObject.transform.position = startPosition;
        m_currentWorldObject.transform.DOMove(m_targetPosition, 1.0f).SetEase(Ease.OutCubic);

        // ステージのTransformを格納するクラスをセットする
        m_stageSelectCamera.SetStageTransformsForWorldObject(m_currentWorldObject.GetComponent<StageTransformsForWorldObject>());
    }

    /// <summary>
    /// ステージを変更する
    /// </summary>
    /// <param name="stageID"></param>
    public void ChangeStage(StageID stageID)
    {
        m_stageSelectCamera.MoveForStageID(stageID);
    }

    /// <summary>
    /// カメラの位置をリセットする
    /// </summary>
    public void ResetCameraPosition()
    {
        // カメラをスタート地点に移動する
        m_stageSelectCamera.MoveCameraStartPosition();
    }
}
