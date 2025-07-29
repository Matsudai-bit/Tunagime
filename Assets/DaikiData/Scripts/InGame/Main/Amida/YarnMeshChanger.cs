using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// モデルの階層に付ける
/// </summary>
public class YarnMeshChanger : MonoBehaviour
{
    private GameObject m_currentMeshInstance; // 現在のメッシュ
    private AmidaTube.State m_currentMeshType = AmidaTube.State.NONE; // 現在のメッシュのタイプ

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

        // 既に同じタイプのメッシュが設定されている場合は何もしない
        if (m_currentMeshType == type)
        {
            return; 
        }

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

            m_currentMeshType = type; // 現在のメッシュタイプを更新

            Debug.Log($"'{gameObject.name}' のメッシュを '{type}' に切り替えました。");
        }
        else
        {
            Debug.LogWarning($"'{gameObject.name}' のメッシュを切り替えできませんでした: '{type}' が見つかりません。");
        }
    }

    /// <summary>
    /// 指定されたマテリアルタイプのマテリアルを取得します。
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Material GetMaterial(YarnMaterialGetter.MaterialType type)
    {
        YarnMaterialGetter materialGetter = m_currentMeshInstance.GetComponent<YarnMaterialGetter>();
        if (materialGetter != null)
        {
            var meshRenderer = materialGetter.GetMeshRenderer(type);
            if (meshRenderer)
            {
                return meshRenderer.material;
            }
            else
            {
                Debug.LogWarning($"'{gameObject.name}' のマテリアルタイプ '{type}' が見つかりません。");
            }
        }
        else
        {
            Debug.LogWarning($"'{gameObject.name}' にYarnMaterialGetterが見つかりません。");
        }
        return null;
    }

    public EmotionCurrent.Type GetEmotionType(YarnMaterialGetter.MaterialType type)
    {
        YarnMaterialGetter materialGetter = m_currentMeshInstance.GetComponent<YarnMaterialGetter>();
        if (materialGetter != null)
        {
            return materialGetter.GetEmotionType(type);
        }
        else
        {
            Debug.LogWarning($"'{gameObject.name}' にYarnMaterialGetterが見つかりません。");
        }
        return EmotionCurrent.Type.NONE;
    }

    /// <summary>
    /// 指定されたマテリアルにメッシュのマテリアルを変更します。
    /// </summary>
    /// <param name="material"></param>
    /// <param name="type"></param>
    public void ChangeMaterial(Material material, EmotionCurrent.Type changeEmotionType, YarnMaterialGetter.MaterialType type)
    {
        if (m_currentMeshInstance == null)
        {
            Debug.LogWarning("現在のメッシュインスタンスがありません。メッシュを設定してください。");
            return;
        }
        YarnMaterialGetter materialGetter = m_currentMeshInstance.GetComponent<YarnMaterialGetter>();
        if (materialGetter != null)
        {
            // マテリアルを変更
            MeshRenderer meshRenderer = materialGetter.GetMeshRenderer(type);
            if (meshRenderer == null)
            {
                Debug.LogWarning($"'{gameObject.name}' のマテリアルタイプ '{type}' のMeshRendererが見つかりません。");
                return;
            }
            meshRenderer.material = material;

            // 想いの種類を設定
             materialGetter.SetEmotionType(type, changeEmotionType) ;
          



            Debug.Log($"'{gameObject.name}' のメッシュのマテリアルを '{type}' に変更しました。");
        }
        else
        {
            Debug.LogWarning($"'{gameObject.name}' にYarnMaterialGetterが見つかりません。メッシュのマテリアルを変更できません。");
        }
    }
}
