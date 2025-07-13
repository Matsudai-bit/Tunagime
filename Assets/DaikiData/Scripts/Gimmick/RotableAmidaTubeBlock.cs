//using UnityEngine;

///// <summary>
///// 回転可能あみだブロック
///// </summary>
//public class RotableAmidaTubeBlock : MonoBehaviour
//{
//    // Start is called once before the first execution of Update after the MonoBehaviour is created

//    private AmidaTubeBlock m_amidaTubeBlock;
//    private void Awake()
//    {
//        // あみだチューブブロックの取得
//        m_amidaTubeBlock = GetComponent<AmidaTubeBlock>();
//    }
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        // スペース
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            // **** 絶対直す **********
//            // プレイヤ取得
//            GameObject[] player = GameObject.FindGameObjectsWithTag("Player");


//            // 距離を取得
//            float length = Vector3.Distance(player[0].transform.position, transform.position);

//            if (length > 2.0f) return;
            

       


//            Vector3 localAngle = m_amidaTubeBlock.GetFloorBlock().transform.localEulerAngles;

//            // Y軸を90度回転
//            localAngle.y += 90.0f;

//            m_amidaTubeBlock.GetFloorBlock().transform.localEulerAngles = localAngle;

//            // 回転
//      //      m_amidaTubeBlock.GetAmidaTube().GetComponent<AmidaTube>().RotateClock();

//            // 基準とする通過方向を変える
//            m_amidaTubeBlock.SetStandartDirectionPassage(m_amidaTubeBlock.GetAmidaTube().GetComponent<AmidaTube>().GetDirectionPassage());


//        }
//    }

//    private void OnCollisionStay(Collision collision)
//    {

//    }
//}
