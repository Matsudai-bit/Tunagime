//using UnityEngine;

///// <summary>
///// ��]�\���݂��u���b�N
///// </summary>
//public class RotableAmidaTubeBlock : MonoBehaviour
//{
//    // Start is called once before the first execution of Update after the MonoBehaviour is created

//    private AmidaTubeBlock m_amidaTubeBlock;
//    private void Awake()
//    {
//        // ���݂��`���[�u�u���b�N�̎擾
//        m_amidaTubeBlock = GetComponent<AmidaTubeBlock>();
//    }
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        // �X�y�[�X
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            // **** ��Β��� **********
//            // �v���C���擾
//            GameObject[] player = GameObject.FindGameObjectsWithTag("Player");


//            // �������擾
//            float length = Vector3.Distance(player[0].transform.position, transform.position);

//            if (length > 2.0f) return;
            

       


//            Vector3 localAngle = m_amidaTubeBlock.GetFloorBlock().transform.localEulerAngles;

//            // Y����90�x��]
//            localAngle.y += 90.0f;

//            m_amidaTubeBlock.GetFloorBlock().transform.localEulerAngles = localAngle;

//            // ��]
//      //      m_amidaTubeBlock.GetAmidaTube().GetComponent<AmidaTube>().RotateClock();

//            // ��Ƃ���ʉߕ�����ς���
//            m_amidaTubeBlock.SetStandartDirectionPassage(m_amidaTubeBlock.GetAmidaTube().GetComponent<AmidaTube>().GetDirectionPassage());


//        }
//    }

//    private void OnCollisionStay(Collision collision)
//    {

//    }
//}
