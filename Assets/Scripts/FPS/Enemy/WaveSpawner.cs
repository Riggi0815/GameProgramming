using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour {
    public List<Enemy> enemies = new List<Enemy>();
    public int curWave;
    public int waveValue;
    public int waveMulti;

    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    private int enemyCount;
    
    [SerializeField] private float minHeight;

    private void Start() {
        GenerateWave();
    }

    private void FixedUpdate() {
        if (enemiesToSpawn.Count > 0) {
            SpawnEnemy();
        }

        if (transform.childCount == 0) {
            GenerateWave();
        }

    }

    public void GenerateWave() {
        waveValue = curWave * waveMulti;
        GenerateEnemies();
    }

    public void GenerateEnemies() {
        List<GameObject> generatedEnemies = new List<GameObject>();
        while (waveValue > 0) {

            int randomEnemyId = Random.Range(0, enemies.Count);
            int randomEnemyCost = enemies[randomEnemyId].cost;

            if (waveValue-randomEnemyCost >= 0) {
                generatedEnemies.Add(enemies[randomEnemyId].enemyPrefab);
                waveValue -= randomEnemyCost;
            } 
            else if (waveValue <= 0) {
                break;
            }
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }

    // Start is called before the first frame update
    public void SpawnEnemy() {
        while (enemiesToSpawn.Count > 0) {

            NavMeshAgent agent = enemiesToSpawn[0].GetComponent<NavMeshAgent>();

            NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

            int vertexIndex = Random.Range(0, triangulation.vertices.Length);

            NavMeshHit hit;
            if (!NavMesh.SamplePosition(triangulation.vertices[vertexIndex], out hit, 2f, -1)) {
                continue;
            }
            
            if (hit.position.y < minHeight) {
                continue;
            }
            
            agent.Warp(hit.position);
            agent.enabled = true;
            var obj =Instantiate(enemiesToSpawn[0], hit.position, Quaternion.identity);
            obj.transform.parent = transform;
            
            enemiesToSpawn.RemoveAt(0);

        }

        waveMulti += 2;
    }

    [System.Serializable]
    public class Enemy {
        public GameObject enemyPrefab;
        public int cost;
    }
}
