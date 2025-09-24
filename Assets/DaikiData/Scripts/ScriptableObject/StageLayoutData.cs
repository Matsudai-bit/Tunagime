using System;
using System.Collections.Generic;
using UnityEngine;
using static StageLayoutData;


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


    // === 2. 各グリッドのデータを管理するクラス ===
    [Serializable]
    public class GridData
    {
        public string positionLabel;
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
    /// ギミック生成データ
    /// </summary>
    [System.Serializable]
    public class GimmickData
    {
        public GridPos gridPos;
        public GenerationType blockType; // ブロックの種類
        public EmotionCurrent.Type emotionType; // 感情タイプ
        public Vector3 rotate;          // 回転
        public bool changeSatinFloor;

        public GimmickData()
        {
            gridPos = new GridPos();
            blockType = GenerationType.NONE;
            rotate = Vector3.zero;
            emotionType = EmotionCurrent.Type.NONE;
            changeSatinFloor = false;
        }
    }


    // === 3. ステージ全体のグリッドデータリスト (Y軸のレイアウト) ===
    [Serializable]
    public class RootLayout
    {
        public string positionYLabel;
        public List<GridData> gridDataList = new List<GridData>();
    }

    // メインのレイアウトデータリスト
    [Header("グリッドデータ")]
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

        StageLayoutData stageLayoutData = (StageLayoutData)target;

        // ボタンの描画
        if (GUILayout.Button("グリッドを初期化"))
        {
            // 初期化ロジック
            if (stageLayoutData.mapSetting.width <= 0 || stageLayoutData.mapSetting.height <= 0)
            {
                Debug.LogError("widthとheightは0より大きい値に設定してください。");
                return;
            }

            // 全リストをクリアして新しい構造を再構築
            stageLayoutData.rootLayoutList.Clear();

            for (int y = 0; y < stageLayoutData.mapSetting.height; y++)
            {
                RootLayout newRoot = new RootLayout();
                newRoot.positionYLabel = $"Row {y+1}";

                for (int x = 0; x < stageLayoutData.mapSetting.width; x++)
                {
                    GridData newGrid = new GridData();
                    newGrid.positionLabel = $"Grid ({x+1}, {y+1})";
                    newGrid.gimmickDataArray = new GimmickData();
                    newGrid.gimmickDataArray.gridPos = new GridPos(x+1, y+1);
                    //GimmickData generationGimmickData  = new();
                    //newGrid.gimmickDataArray = generationGimmickData;
                    newRoot.gridDataList.Add(newGrid);
                }
                stageLayoutData.rootLayoutList.Add(newRoot);
            }

            Debug.Log($"ステージグリッドデータを初期化しました: {stageLayoutData.mapSetting.width}x{stageLayoutData.mapSetting.height}");

            // 変更を保存
            EditorUtility.SetDirty(stageLayoutData);
            AssetDatabase.SaveAssets();
        }
    }
}
#endif