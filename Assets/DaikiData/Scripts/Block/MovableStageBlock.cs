using UnityEngine;

using GridVec = GridPos;


/// <summary>
/// 動くことができるステージブロック
/// </summary>
public class MovableStageBlock : MonoBehaviour
{
    private StageBlock m_stageBlock;    // ステージブロック


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        m_stageBlock = GetComponent<StageBlock>();

    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="gridVec"></param>
    void Move(GridVec gridVec)
    {
        GridPos gridPos = m_stageBlock.GetGridPos();

        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position,new Vector3((float)(gridVec.x), 0.0f, (float)(-gridVec.y))), out hit, 0.5f))
        {
            if (hit.collider.gameObject.CompareTag("Wall")) return;
        }

        // 新規座標
        gridPos.x = gridPos.x + gridVec.x;
        gridPos.y = gridPos.y + gridVec.y;

        m_stageBlock.UpdatePosition(gridPos);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 dir = transform.position - collision.gameObject.transform.position; ;

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
            {
                // X移動する
                Move(new GridVec(
                    (int)(dir.x / Mathf.Abs(dir.x)), 0));
            }
            else
            {
                // Y移動する
                Move(new GridVec(
                    0, (int)(-dir.z / Mathf.Abs(dir.z))));
            }
        }
    }

}
