using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour {

    //Variables
    //ForNavmesh
    [SerializeField] private NavMeshSurface surface;

    //Drawmode (relevant in Editor)
    public enum DrawMode {
        NoiseMap,
        FalloffMap
    }
    [SerializeField] private DrawMode drawMode;

    //ScriptableObjects
    public TerrainData terrainData;
    public NoiseData noiseData;
    
    //Variables for Size
    public const int mapChunkSize = 95;

    //Editor Maps
    [Range(0,6)]
    [SerializeField] private int editorPrevLOD;

    public bool autoUpdate;
    
    //Falloff
    private float[,] falloffMap;
    public float a;
    public float b;

    //Shader
    public Gradient gradient;

    //Generate Map in Editor
    void OnValuesUpdated() {
        if (!Application.isPlaying) {
            DrawMapEditor();
        }
    }

    //Generate NoiseMap, apply on mesh and Draw Mesh
    private void Awake() {
        float minHeight = terrainData.minHeight;
        float maxHeight = terrainData.maxHeight;
        
        noiseData.seed = Random.Range(1, 100000);
        noiseData.offset = new Vector2(Random.Range(0, 100), Random.Range(0, 100));
        
        falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize, a, b);
        
        MapData mapData = GenerateMapData(Vector2.zero);
        
        MapDisplay display = FindObjectOfType<MapDisplay>();

        display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, editorPrevLOD, terrainData.useFlatShading, gradient, minHeight, maxHeight));
        
        surface.BuildNavMesh();
    }

    //Draws FalloffMap and NoiseMap in Editor
    public void DrawMapEditor() {
        
        MapData mapData = GenerateMapData(Vector2.zero);
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap) {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        }
        else if (drawMode == DrawMode.FalloffMap) {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize, a, b)));
        }
    }

    //Generate the Noise Map
    MapData GenerateMapData(Vector2 center) {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, noiseData.seed, noiseData.noiseScale, noiseData.noiseLayers, noiseData.persistance, noiseData.lacunarity, center + noiseData.offset, noiseData.normalizeMode);
        
        for (int y = 0; y < mapChunkSize; y++) {
            for (int x = 0; x < mapChunkSize; x++) {
                if (terrainData.useFalloff) {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }
            }
        }
        
        return new MapData(noiseMap);
    }

    //Validate in Editor
    private void OnValidate() {

        if (terrainData != null) {
            terrainData.OnValuesUpdated -= OnValuesUpdated;
            terrainData.OnValuesUpdated += OnValuesUpdated;
        }

        if (noiseData != null) {
            noiseData.OnValuesUpdated -= OnValuesUpdated;
            noiseData.OnValuesUpdated += OnValuesUpdated;
        }

        falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize, a, b);
    }

}

public struct MapData {
    public readonly float[,] heightMap;

    public MapData(float[,] heightMap) {
        this.heightMap = heightMap;
    }
}
