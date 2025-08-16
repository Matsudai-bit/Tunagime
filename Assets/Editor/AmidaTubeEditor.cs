
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(StageGridData))]
public class AmidaTubeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
    }
}
