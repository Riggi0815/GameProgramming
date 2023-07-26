using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatableData : ScriptableObject {

    public event System.Action OnValuesUpdated;
    public bool autoUpdate;

    protected virtual void OnValidate() {
        if (autoUpdate) {
            NotifyUpdateValues();
        }
    }

    public void NotifyUpdateValues() {
        if (OnValuesUpdated != null) {
            OnValuesUpdated();
        }
    }
}