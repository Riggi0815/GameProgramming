using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TerrainData : UpdatableData {
    
    public float uniformScale = 1f;
    
    public bool useFlatShading;
    public bool useFalloff;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;
    
    //puts the Map Heights betwen 0 and 1
    public float minHeight {
        get {
            return uniformScale * meshHeightMultiplier * meshHeightCurve.Evaluate(0);
        }
    }
    
    public float maxHeight {
        get {
            return uniformScale * meshHeightMultiplier * meshHeightCurve.Evaluate(1);
        }
    }
}
