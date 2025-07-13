using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;




public class SwitchManager : MonoBehaviour
{
    List<Switch>                    m_switches;
    List<SwitchTypeController>      m_switchTypeController;

    [SerializeField] private ClearItem m_clearItemA;
    [SerializeField] private ClearItem m_clearItemB;

    private void Awake()
    {
        m_switches      = new List<Switch>();
        m_switchTypeController   = new List<SwitchTypeController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<SwitchType> pushSwitches = new List<SwitchType>();

        

        foreach (var sw in m_switches)
        {
            if (sw.IsPush())
            {
                pushSwitches.Add(sw.GetSwitchType());

                if (m_clearItemA != null && sw.GetSwitchType() == m_clearItemA.GetSwitchType() )
                {
                    m_clearItemA.SpawnItem();
                    m_clearItemA = null;
                }
                if (m_clearItemB != null && sw.GetSwitchType() == m_clearItemB.GetSwitchType() )
                {
                    m_clearItemB.SpawnItem();
                    m_clearItemB = null;
                }

            }
        }



        foreach (var switchTypeController in m_switchTypeController)
        {

            int count = 0;

            foreach (var switchType in switchTypeController.GetSwitchType())
            {




                foreach (var pushSwitch in pushSwitches)
                {
                    if (switchType == pushSwitch)
                        count++;
                }


                int c = switchTypeController.GetSwitchType().Count();

                if (c == count)
                    switchTypeController.Erase();
                else
                    switchTypeController.Appearance();
            }




        }



    }

    /// <summary>
    /// ƒXƒCƒbƒ`•Ç‚Ì“o˜^
    /// </summary>
    /// <param name="switchWall"></param>
    public void AddSwitchTypeController(SwitchTypeController switchWall)
    {
        m_switchTypeController.Add(switchWall);
    }

    /// <summary>
    /// ƒXƒCƒbƒ`‚Ì“o˜^
    /// </summary>
    /// <param name="switchWall"></param>
    public void AddSwitch(Switch addSwitch)
    {
        m_switches.Add(addSwitch);
    }
}
