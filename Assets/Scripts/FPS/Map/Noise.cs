using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise {
    //Generate Noise Map
    //Size, Scale and Details of the Map
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset) {
        
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random numberGen = new System.Random(seed);
        Vector2[] octavesOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++) {
            float offsetX = numberGen.Next(-100000, 100000) + offset.x;
            float offsetY = numberGen.Next(-100000, 100000) -  offset.y;
            octavesOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0) {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;
            
        //setting Value for Noise
        for (var y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++) {
                    float sampleX = (x - halfWidth) / scale * frequency + octavesOffsets[i].x * frequency;
                    float sampleY = (y - halfHeight) / scale * frequency + octavesOffsets[i].y * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                }else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        for (var y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}
