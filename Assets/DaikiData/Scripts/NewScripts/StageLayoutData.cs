using System;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// ステージの生成形式　子データ
/// 
/// 一つの種類のオブジェクトのデータ群
/// </summary>
[Serializable]
public class StageSerializableChildData
{
    public string name;
    public string tag;
    public Vector3 localPosition;
    public Vector3 localRotation;
    public Vector3 localScale;

    public List<StageSerializableChildData > children = new(); // 子データ
            
    public List<SerializedComponentData> components = new();            // コンポーネント情報

}



/// <summary>
/// 生成形式のコンポーネントインターフェース
/// </summary>
public interface ISerializableComponent
{
    /// <summary>
    /// 保存用データの取得
    /// </summary>
    /// <returns></returns>
    object CaptureData();

    /// <summary>
    /// 保存用データからの復元
    /// </summary>
    /// <param name="data"></param>
    void ApplyData(object data); // 保存データから復元
}

/// <summary>
/// 
/// </summary>
[Serializable]
public class SerializedComponentData
{
    public string typeName;         // コンポーネントの型名
    public string payloadTypeName;  // ペイロード型名
    public string json;             // コンポーネントのデータをJSON文字列で格納
}