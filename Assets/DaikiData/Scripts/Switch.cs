using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// スイッチに関するクラス
/// </summary>
public class Switch : MonoBehaviour
{


    private SwitchType m_switchType;

    [SerializeField] private bool m_isPush = false; // 押されているかどうか

    /// <summary>
    /// 押されているかどうか
    /// </summary>
    /// <returns>true 押されている</returns>
    public bool IsPush()
    {
        return m_isPush;
    }

    /// <summary>
    /// 押す
    /// </summary>
    public void Push()
    {

        m_isPush = true;
    }

    /// <summary>
    /// 離す
    /// </summary>
    public void Release()
    {
        m_isPush = false;
    }


    /// <summary>
    /// 種類の取得
    /// </summary>
    /// <returns>壁の種類</returns>
    public SwitchType GetSwitchType()
    {
        return m_switchType;
    }

    /// <summary>
    /// 種類の設定
    /// </summary>
    public void SetSwitchType(SwitchType switchType)
    {
        m_switchType = switchType;
    }


}
