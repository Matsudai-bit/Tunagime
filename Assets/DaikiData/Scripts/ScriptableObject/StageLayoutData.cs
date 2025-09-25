using System;
using System.Collections.Generic;
using UnityEngine;
using static StageLayoutData;
using NUnit.Framework;
using Unity.Burst.CompilerServices;




// エディタ専用の機能を含むため、UNITY_EDITORディレクティブを使用
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "StageLayoutData", menuName = "Stage/StageLayoutData")]
public class StageLayoutData : ScriptableObject
{
    // === 1. ステージのサイズ設定 ===
    [Header("ステージデータ設定")]
    public MapSetting mapSetting; // MapSettingの参照


    // === 2. 配置データを管理するクラス ===
    [Serializable]
    public class GridData
    {
        public string label;
        public GimmickData gimmickDataArray;
    }
    public enum GenerationType
    {
        FELT_BLOCK,             // フェルトブロック
        FLUFF_BALL,             // 毛糸玉
        FELT_BLOCK_NO_MOVEMENT, // 動かないフェルトブロック
        FLOOR,                  // 床
        TERMINUS,               // 終点
        CURTAIN,                // カーテン
        SATIN_FLOOR,            // サテン床
        PAIR_BADGE,             // ペアバッジ
        FRAGMENT,               // 想いの断片
        TERMINUS_SLOT_EMPTY,    // 終点の想いの核（空）
        NONE,                 // なし
    }

    /// <summary>
    /// あみだの線を引く行を指定するクラス
    /// </summary>
    [Serializable]
    public class GenerationAmidaLine
    {
        public string label; // 行のインデックス
        public bool createHorizonalAmidaLine;   // 線を引くかどうか
        public GenerationAmidaLine(string label, bool createHorizonalAmidaLine)
        {
            this.label =　label;
            this.createHorizonalAmidaLine = createHorizonalAmidaLine;
        }
    }




    /// <summary>
    /// ギミック生成データ
    /// </summary>
    [System.Serializable]
    public class GimmickData
    {
        public GridPos gridPos;
        public GenerationType blockType; // ブロックの種類
        public EmotionCurrent.Type emotionType; // 感情タイプ
        public int typeNumber;          // 回転
        public bool changeToSatinFloor;   // サテン床に変化するかどうか
        public bool placeAmidaTube;       // あみだの筒を配置するかどうか

        public GimmickData()
        {
            gridPos = new GridPos();
            blockType = GenerationType.NONE;
            typeNumber = 0;
            emotionType = EmotionCurrent.Type.NONE;
            changeToSatinFloor = false;
        }
    }


    // === 3. ステージ全体のグリッドデータリスト (Y軸のレイアウト) ===
    [Serializable]
    public class RootLayout
    {
        public string label;
        public bool createHorizonalAmidaLine;   // 横線を引くかどうか
        public List<GridData> gridDataList = new List<GridData>();


        public void SetLabel(int rowIndex)
        {
            label = (rowIndex + 1).ToString() + "行目 ------------------------------------------------------------------------------------------------";
        }

        public RootLayout()
        {
            label = "Row";
            gridDataList = new List<GridData>();
            createHorizonalAmidaLine = false;
        }

    }
    [Header(" ステージの横幅(確認用) ")]
    [SerializeField] public int width;
    [Header(" ステージの縦幅(確認用) ")]
    [SerializeField] public int height;

    [Space(3.0f)]


    // メインのレイアウトデータリスト
    [Header("======== ギミック配置データ ========")]
    public List<RootLayout> rootLayoutList = new List<RootLayout>();


}

// === 4. Editor専用のボタンとロジック ===
// このクラスは、StageLayoutDataのカスタムInspectorとして機能します。
#if UNITY_EDITOR
[CustomEditor(typeof(StageLayoutData))]
public class StageLayoutDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // デフォルトのインスペクターを描画（width, height, rootLayoutListが表示される）
        DrawDefaultInspector();


        // ボタンの描画
        if (GUILayout.Button("初期化"))
        {
        StageLayoutData stageLayoutData = (StageLayoutData)target;
            // 初期化ロジック
            if (stageLayoutData.mapSetting.width <= 0 || stageLayoutData.mapSetting.height <= 0)
            {
                Debug.LogError("widthとheightは0より大きい値に設定してください。");
                return;
            }

            stageLayoutData.width = stageLayoutData.mapSetting.width;
            stageLayoutData.height = stageLayoutData.mapSetting.height;


            InitializeGimmickGrid(stageLayoutData);

            // 変更を保存
            EditorUtility.SetDirty(stageLayoutData);
            AssetDatabase.SaveAssets();
        }
    }

    /// <summary>
    /// ギミックグリッドの初期化
    /// </summary>
    /// <param name="stageLayoutData"></param>
    void InitializeGimmickGrid(StageLayoutData stageLayoutData)
    {
        // 全リストをクリアして新しい構造を再構築
        stageLayoutData.rootLayoutList.Clear();

        for (int y = 0; y < stageLayoutData.mapSetting.height; y++)
        {
            RootLayout newRoot = new RootLayout();
            newRoot.SetLabel(y);
            newRoot.createHorizonalAmidaLine = false;

            for (int x = 0; x < stageLayoutData.mapSetting.width; x++)
            {
                GridData newGrid = new GridData();
                newGrid.label = $"Grid ({x + 1}, {y + 1})";
                newGrid.gimmickDataArray = new GimmickData();
                newGrid.gimmickDataArray.gridPos = new GridPos(x + 1, y + 1);
                //GimmickData generationGimmickData  = new();
                //newGrid.gimmickDataArray = generationGimmickData;
                newRoot.gridDataList.Add(newGrid);
            }
            stageLayoutData.rootLayoutList.Add(newRoot);
        }

        Debug.Log($"ステージグリッドデータを初期化しました: {stageLayoutData.mapSetting.width}x{stageLayoutData.mapSetting.height}");

    }

}
#endif