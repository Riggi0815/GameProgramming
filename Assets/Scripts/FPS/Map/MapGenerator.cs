using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public enum DrawMode {
        NoiseMap,
        ColorMap,
        Mesh
    }

    [SerializeField] private DrawMode drawMode;

    //Variables for Size
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeigth;
    [SerializeField] private float noiseScale;
    
    //Variables for Details
    [SerializeField] private int octaves;
    [Range(0, 1)]
    [SerializeField] private float persistance;
    [SerializeField] private float lacunarity;

    [SerializeField] private int seed;
    [SerializeField] private Vector2 offset;

    [SerializeField] private float meshHeightMultiplier;
    [SerializeField] private AnimationCurve meshHeightCurve; 
    
    public bool autoUpdate;

    [SerializeField] private TerrainType[] regions;

    //Generate the Noise Map
    public void GenerateMap() {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeigth, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colorMap = new Color[mapWidth * mapHeigth];
        for (int y = 0; y < mapHeigth; y++) {
            for (int x = 0; x < mapWidth; x++) {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++) {
                    if (currentHeight <= regions[i].height) {
                        colorMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap) {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColorMap) {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeigth));
        }
        else if (drawMode == DrawMode.Mesh) {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve),
                TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeigth));
        }
        
    }

    private void OnValidate() {
        if (mapWidth < 1) {
            mapWidth = 1;
        }

        if (mapHeigth < 1) {
            mapHeigth = 1;
        }

        if (lacunarity < 1) {
            lacunarity = 1;
        }

        if (octaves < 1) {
            octaves = 1;
        }
        if (octaves > 30) {
            octaves = 30;
        }
    }
}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color color;
    
}
