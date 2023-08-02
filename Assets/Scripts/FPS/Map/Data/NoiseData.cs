using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class NoiseData : UpdatableData {
    
    public Noise.NormalizeMode normalizeMode;
    
    public float noiseScale;
    
    //Variables for NoiseDetails
    [Range(1,20)]
    public int noiseLayers;
    //persistance and lacunarity for mor details
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;


    protected override void OnValidate() {
        if (lacunarity < 1) {
            lacunarity = 1;
        }

        if (noiseLayers < 1) {
            noiseLayers = 1;
        }
        if (noiseLayers > 30) {
            noiseLayers = 30;
        }
        
        base.OnValidate();
    }
}
