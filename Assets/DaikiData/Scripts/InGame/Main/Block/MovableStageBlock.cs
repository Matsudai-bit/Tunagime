using UnityEngine;

using GridVec = GridPos;


/// <summary>
/// �������Ƃ��ł���X�e�[�W�u���b�N
/// </summary>
public class MovableStageBlock : MonoBehaviour
{
    private StageBlock m_stageBlock;    // �X�e�[�W�u���b�N


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        m_stageBlock = GetComponent<StageBlock>();

    }

    /// <summary>
    /// �ړ�
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

        // �V�K���W
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
                // X�ړ�����
                Move(new GridVec(
                    (int)(dir.x / Mathf.Abs(dir.x)), 0));
            }
            else
            {
                // Y�ړ�����
                Move(new GridVec(
                    0, (int)(-dir.z / Mathf.Abs(dir.z))));
            }
        }
    }

}
