using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// モデルの階層に付ける
/// </summary>
public class YarnMeshChanger : MonoBehaviour
{
    private GameObject m_currentMeshInstance; // 現在のメッシュ

    private Dictionary<AmidaTube.State, GameObject> m_usedMeshes = new Dictionary<AmidaTube.State, GameObject>(); // 使用したメッシュを保管しておく(メモリ効率をよくするため)

    // 初期メッシュとして設定したいAmidaTube.State (インスペクターでドロップダウンから選択)
    [SerializeField] private AmidaTube.State m_initialShapeType = AmidaTube.State.NONE; // デフォルトをNoneにする

    private void Start()
    {
        if (AmidaTube.State.NONE != m_initialShapeType)
            SetMesh(m_initialShapeType);   
    }

    /// <summary>
    /// 指定されたAmidaTube.ShapeTypeのメッシュに切り替えます。
    /// MeshLibraryからプレハブを取得し、現在のオブジェクトの子としてインスタンス化します。
    /// </summary>
    /// <param name="type">切り替えたいメッシュのAmidaTube.State</param>
    public void SetMesh(AmidaTube.State type)
    {
        if (m_currentMeshInstance != null)
        {
            m_currentMeshInstance.SetActive(false);
            
        }

        GameObject meshPrefab = YarnMeshLibrary.Instance.GetMeshPrefab(type); // AmidaTube.ShapeTypeで取得

        if (meshPrefab != null)
        {
            // 辞書に登録されていない場合
            if (m_usedMeshes.ContainsKey(type) == false)
            {
                m_usedMeshes[type] = Instantiate(meshPrefab, transform);
            }

            m_currentMeshInstance = m_usedMeshes[type];

            m_currentMeshInstance.name = type.ToString(); // enumの名前をGameObject名にする
            m_currentMeshInstance.transform.localPosition = Vector3.zero;
            m_currentMeshInstance.transform.localRotation = Quaternion.identity;
            m_currentMeshInstance.transform.localScale = Vector3.one;

            Debug.Log($"'{gameObject.name}' のメッシュを '{type}' に切り替えました。");
        }
        else
        {
            Debug.LogWarning($"'{gameObject.name}' のメッシュを切り替えできませんでした: '{type}' が見つかりません。");
        }
    }
}
