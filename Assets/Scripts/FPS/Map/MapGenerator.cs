using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour {

    public enum DrawMode {
        NoiseMap,
        ColorMap,
        FalloffMap
    }
    [SerializeField] private DrawMode drawMode;

    public Noise.NormalizeMode normalizeMode;

    //Variables for Size
    public const int mapChunkSize = 241;
    [Range(0,6)]
    [SerializeField] private int editorPrevLOD;
    [SerializeField] private float noiseScale;
    
    //Variables for Details
    [Range(1,20)]
    [SerializeField] private int octaves;
    [Range(0, 1)]
    [SerializeField] private float persistance;
    [SerializeField] private float lacunarity;

    public int seed;
    [SerializeField] private Vector2 offset;

    public bool useFalloff;

    [SerializeField] private float meshHeightMultiplier;
    [SerializeField] private AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    [SerializeField] private TerrainType[] regions;

    private float[,] falloffMap;

    private void Start() {
        seed = Random.Range(1, 100000);
        
        falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);
        
        MapData mapData = GenerateMapData(Vector2.zero);
        
        MapDisplay display = FindObjectOfType<MapDisplay>();
        
        display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, editorPrevLOD), TextureGenerator.TextureFromColorMap(mapData.colorMap, mapChunkSize, mapChunkSize));
        
    }

    public void DrawMapEditor() {
        
        MapData mapData = GenerateMapData(Vector2.zero);
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap) {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        }
        else if (drawMode == DrawMode.ColorMap) {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(mapData.colorMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.FalloffMap) {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize)));
        }
    }

    //Generate the Noise & Color Map
    MapData GenerateMapData(Vector2 center) {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, center + offset, normalizeMode);

        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++) {
            for (int x = 0; x < mapChunkSize; x++) {
                if (useFalloff) {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }
                
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++) {
                    if (currentHeight >= regions[i].height) {
                        colorMap[y * mapChunkSize + x] = regions[i].color;
                    } else {
                        break;
                    }
                }
            }
        }
        
        return new MapData(noiseMap, colorMap);
    }

    private void OnValidate() {

        if (lacunarity < 1) {
            lacunarity = 1;
        }

        if (octaves < 1) {
            octaves = 1;
        }
        if (octaves > 30) {
            octaves = 30;
        }

        falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);
    }

}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color color;
    
}

public struct MapData {
    public readonly float[,] heightMap;
    public readonly Color[] colorMap;

    public MapData(float[,] heightMap, Color[] colorMap) {
        this.heightMap = heightMap;
        this.colorMap = colorMap;
    }
}
