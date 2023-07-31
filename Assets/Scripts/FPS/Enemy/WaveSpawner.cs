using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
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

    public void SpawnEnemy() {
        for (int i = 0; i < 1; i++) {
            NavMeshAgent agent = enemiesToSpawn[0].GetComponent<NavMeshAgent>();

            NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

            int vertexIndex = Random.Range(0, triangulation.vertices.Length);

            NavMeshHit hit;
            if (!NavMesh.SamplePosition(triangulation.vertices[vertexIndex], out hit, 2f, -1)) {
                i = 0;
                continue;
            }
            
            if (hit.position.y < minHeight) {
                i = 0;
                continue;
            }
            
            agent.Warp(hit.position);
            agent.enabled = true;
            var obj =Instantiate(enemiesToSpawn[0], hit.position, Quaternion.identity);
            obj.transform.parent = transform;
            
            enemiesToSpawn.RemoveAt(0);

            if (enemiesToSpawn.Count == 0) {

                waveMulti += 2;

            }

            StartCoroutine(TimeBetwenEnemies());
            
            
        }
    }

    IEnumerator TimeBetwenEnemies() {
        yield return new WaitForSeconds(.5f);
    }

    [System.Serializable]
    public class Enemy {
        public GameObject enemyPrefab;
        public int cost;
    }
}
