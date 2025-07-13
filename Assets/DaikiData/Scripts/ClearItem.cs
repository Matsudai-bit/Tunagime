using UnityEngine;

/// <summary>
/// クリアアイテム
/// </summary>
public class ClearItem : MonoBehaviour
{
    [SerializeField] GameObject m_itemObj;
    [SerializeField] SwitchType  m_switchType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_itemObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public SwitchType GetSwitchType()
    {
        return m_switchType;
    }

   
    public void SpawnItem()
    {
        m_itemObj.SetActive(true);
    }

}
