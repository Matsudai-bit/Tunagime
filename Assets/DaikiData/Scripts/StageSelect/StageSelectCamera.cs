using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ステージセレクトのカメラクラス
/// </summary>
public class StageSelectCamera : MonoBehaviour
{
    [Header("ステージのTransformを格納するクラス")]
    [SerializeField]
    private StageTransformsForWorldObject m_stageTransformsForWorldObject; // ステージのTransformを格納するクラス

    Vector3 m_startPosition;// スタート地点のTransform

    private void Awake()
    {
        m_startPosition = transform.position;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ターゲットの位置に移動する
    /// </summary>
    /// <param name="targetPosition"></param>
    void MoveTarget(Vector3 targetPosition)
    {
        var targetDirection = targetPosition - m_startPosition;
        var velocity = (targetDirection.magnitude - m_stageTransformsForWorldObject.worldTransform.localScale.x) * targetDirection.normalized;

        targetPosition = m_startPosition + velocity;

        Quaternion.LookRotation(targetDirection.normalized, Vector3.up) ;

        transform.DORotateQuaternion
            (Quaternion.LookRotation(targetDirection.normalized, Vector3.up), 1.0f).SetEase(Ease.OutQuint);
        transform.DOMove(targetPosition, 1.5f).SetEase(Ease.OutQuint);
    }

    /// <summary>
    /// スタート地点に移動する
    /// </summary>
    public void MoveCameraStartPosition()
    {
        // スタート地点に移動
        MoveTarget(m_startPosition);
    }

    /// <summary>
    /// ステージのTransformを格納するクラスをセットする
    /// </summary>
    /// <param name="stageTransformsForWorldObject"></param>
    public void SetStageTransformsForWorldObject(StageTransformsForWorldObject stageTransformsForWorldObject)
    {
        m_stageTransformsForWorldObject = stageTransformsForWorldObject;
    }

    /// <summary>
    /// ステージIDに応じてカメラを移動させる
    /// </summary>
    /// <param name="stageID"></param>
    public void MoveForStageID(StageID stageID)
    {
        var stageTransforms = m_stageTransformsForWorldObject.StageTransforms;


        MoveTarget(stageTransforms[stageID].position);

    }

}
