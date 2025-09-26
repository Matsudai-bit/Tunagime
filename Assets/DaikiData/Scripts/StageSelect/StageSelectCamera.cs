using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ステージセレクトのカメラクラス
/// </summary>
public class StageSelectCamera : MonoBehaviour
{
    [SerializeField]
    private StageTransformsForWorldObject m_stageTransformsForWorldObject; // ステージのTransformを格納するクラス

    Transform m_startTransform;// スタート地点のTransform

    private void Awake()
    {
        m_startTransform = transform;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var stageTransforms = m_stageTransformsForWorldObject.StageTransforms;

        var targetTransform = stageTransforms[StageID.STAGE_1];

        MoveTarget(targetTransform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MoveTarget(Transform targetTransform)
    {
        // スクリーン座標取得  
        var screenPos = new Vector2(Screen.width - Screen.width / 4, Screen.height / 2);
        // ワールド座標に変換  
        var worldPos = Camera.main.ScreenToWorldPoint(screenPos);



        var targetDirection = targetTransform.position - transform.position;
        var velocity = (targetDirection.magnitude - m_stageTransformsForWorldObject.worldTransform.localScale.x) * targetDirection.normalized;

        Quaternion.LookRotation(targetDirection.normalized, Vector3.up) ;

        transform.DORotateQuaternion
            (Quaternion.LookRotation(targetDirection.normalized, Vector3.up), 1.0f).SetEase(Ease.OutQuint);
        transform.DOBlendableMoveBy(velocity, 1.5f).SetEase(Ease.OutQuint);
    }
}
