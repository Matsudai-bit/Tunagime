using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// �R�l�N�^�[
/// </summary>
public class Connector : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_switches = new List<GameObject>();

    private Switch m_mySwitch;

    private AmidaManager m_amidaManager;

    private void Awake()
    {
        m_mySwitch = GetComponent<Switch>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        GameObject nearObj = null;


        // �������Ă���X�C�b�`�̊m�F
        foreach (var obj in m_switches)
        {
            if (nearObj == null)
            {
                nearObj = obj;
                continue;
            }

            if (nearObj.transform.position.x > obj.transform.position.x)
                nearObj = obj;


        }

        if (nearObj == null) return;

        GridPos gridPos = new GridPos(nearObj.GetComponent<StageBlock>().GetGridPos().x, nearObj.GetComponent<StageBlock>().GetGridPos().y);
        gridPos.x += 1;

        m_amidaManager.RequestStartAmida(gridPos, Electric.ElectricFlowType.RED, Direction.RIGHT);

        int pushedSwitchCount = 0;

        // �������Ă���X�C�b�`�̊m�F
        foreach (var obj in m_switches)
        {
            // ��ł�������Ă��Ȃ����
            if (obj.GetComponent<Switch>().IsPush())
                pushedSwitchCount++;

 
        }

        if (pushedSwitchCount >= 2)
            /// 2�ُ퉟����Ă����牟��
            m_mySwitch.Push();
        else
            m_mySwitch.Release();
            


    }

    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="map"></param>
    public void Initialize(AmidaManager amidaManager)
    {

        // ���݂��Ǘ��̎擾
        m_amidaManager = amidaManager;


    }

    /// <summary>
    /// �X�C�b�`�̓o�^
    /// </summary>
    /// <param name="addSwitch"></param>
    public void AddSwitch(GameObject addSwitch)
    {
        m_switches.Add(addSwitch);
    }
}
