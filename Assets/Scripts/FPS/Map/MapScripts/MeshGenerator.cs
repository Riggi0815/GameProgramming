using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator {

    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve _heightCurve, int levelOfDetail, bool useFlatShading, Gradient gradient, float minHeight, float maxHeight) {

        
        AnimationCurve heightCurve = new AnimationCurve(_heightCurve.keys);
        
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        int meshSimplify = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
        int verticesPerLine = (width - 1) / meshSimplify + 1;

        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine, useFlatShading);
        int vertexIndex = 0;

        for (int y = 0; y < height; y+= meshSimplify) {
            for (int x = 0; x < width; x+= meshSimplify) {
                    meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
                    if (meshData.vertices[vertexIndex].y > maxHeight) {
                        maxHeight = meshData.vertices[vertexIndex].y;
                    }
                    if (meshData.vertices[vertexIndex].y < minHeight) {
                        minHeight = meshData.vertices[vertexIndex].y;
                    }

                    meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                    float heightY = Mathf.InverseLerp(minHeight, maxHeight, meshData.vertices[vertexIndex].y);
                    Debug.Log(heightY);
                    meshData.colors[vertexIndex] = gradient.Evaluate(heightY);

                if (x < width - 1 && y < height -1) {
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }
                
                vertexIndex++;
            }
        }
        
        meshData.ProcessMesh();

        return meshData;
    }
    
}

public class MeshData {
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;
    public Color[] colors;

    private int triangleIndex;
    private bool useFlatShading;

    public MeshData(int meshWidth, int meshHeight, bool useFlatShading) {
        this.useFlatShading = useFlatShading;
        
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        colors = new Color[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c) {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1 ] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public void ProcessMesh() {
        if (useFlatShading) {
            Flatshading();
        }
    }

    void Flatshading() {
        Vector3[] flatShadedVertecies = new Vector3[triangles.Length];
        Vector2[] flatShadedUvs = new Vector2[triangles.Length];
        Color[] flatShadedColors = new Color[triangles.Length];

        for (int i = 0; i < triangles.Length; i++) {
            flatShadedVertecies[i] = vertices[triangles[i]];
            flatShadedUvs[i] = uvs[triangles[i]];
            flatShadedColors[i] = colors[triangles[i]];
            triangles[i] = i;
        }
        vertices = flatShadedVertecies;
        uvs = flatShadedUvs;
        colors = flatShadedColors;

    }

    public Mesh CreateMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.colors = colors;
        if (useFlatShading) {
            mesh.RecalculateNormals();
        }
        return mesh;
    }
}
