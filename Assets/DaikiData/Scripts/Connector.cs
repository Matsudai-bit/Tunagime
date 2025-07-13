using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// コネクター
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


        // 所持しているスイッチの確認
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

        // 所持しているスイッチの確認
        foreach (var obj in m_switches)
        {
            // 一つでも押されていなければ
            if (obj.GetComponent<Switch>().IsPush())
                pushedSwitchCount++;

 
        }

        if (pushedSwitchCount >= 2)
            /// 2個異常押されていたら押す
            m_mySwitch.Push();
        else
            m_mySwitch.Release();
            


    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="map"></param>
    public void Initialize(AmidaManager amidaManager)
    {

        // あみだ管理の取得
        m_amidaManager = amidaManager;


    }

    /// <summary>
    /// スイッチの登録
    /// </summary>
    /// <param name="addSwitch"></param>
    public void AddSwitch(GameObject addSwitch)
    {
        m_switches.Add(addSwitch);
    }
}
