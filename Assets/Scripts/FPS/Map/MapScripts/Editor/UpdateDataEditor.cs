using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UpdatableData),true)]
public class UpdateDataEditor : Editor {

    //Only for Editor
    public override void OnInspectorGUI() {
        
        base.OnInspectorGUI();

        UpdatableData data = (UpdatableData)target;

        if (GUILayout.Button("Update")) {
            data.NotifyUpdateValues();
        }

    }
    
}
