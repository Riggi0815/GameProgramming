using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class NoiseData : UpdatableData {
    
    public Noise.NormalizeMode normalizeMode;
    
    public float noiseScale;
    
    //Variables for Details
    [Range(1,20)]
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;


    protected override void OnValidate() {
        if (lacunarity < 1) {
            lacunarity = 1;
        }

        if (octaves < 1) {
            octaves = 1;
        }
        if (octaves > 30) {
            octaves = 30;
        }
        
        base.OnValidate();
    }
}