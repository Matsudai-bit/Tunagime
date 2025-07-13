
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// スイッチの種類
/// </summary>
public enum SwitchType
{
    YELLOW,
    RED,
    PURPLE
}

public class SwitchTypeController : MonoBehaviour
{
    private List<SwitchType> m_wallType;    // 壁の種類混合可能

    
    /// <summary>
    /// 種類の取得
    /// </summary>
    /// <returns>壁の種類</returns>
    public List<SwitchType> GetSwitchType()
    {
        return m_wallType;
    }

    /// <summary>
    /// 種類の設定
    /// </summary>
    public void SetSwitchType(List<SwitchType> switchType)
    {
        m_wallType = switchType;
    }

    /// <summary>
    /// 消す
    /// </summary>
    public void Erase()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 出現する
    /// </summary>
    public void Appearance()
    {
        gameObject.SetActive(true);

    }


}