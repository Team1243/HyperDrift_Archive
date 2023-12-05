using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CarDataSheetLoader))]
public class SheetLoaderBtn : Editor
{
#if UNITY_EDITOR
    public override async void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CarDataSheetLoader generator = (CarDataSheetLoader)target;
        if (GUILayout.Button("Load Data From Sheed"))
        {
            await generator.LoadDataAsync();
        }
    }
#endif
}