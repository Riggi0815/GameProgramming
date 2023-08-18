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
    
    //Variables
    //For Waves
    public List<Enemy> enemies = new List<Enemy>();
    public int curWave;
    public int waveValue;
    public int waveMulti;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    private int enemyCount;
    
    //for Spawning
    [SerializeField] private float minHeight;

    //Generate First Wave
    private void Start() {
        GenerateWave();
    }

    //Spaws Enemys and Generates Waves
    private void FixedUpdate() {
        if (enemiesToSpawn.Count > 0) {
            SpawnEnemy();
        }

        if (transform.childCount == 0) {
            GenerateWave();
        }

    }

    //How large is the wave
    public void GenerateWave() {
        waveValue = curWave * waveMulti;
        GenerateEnemies();
    }

    //Generates Enemys
    public void GenerateEnemies() {
        List<GameObject> generatedEnemies = new List<GameObject>();
        while (waveValue > 0) {

            //random because i thought i could add more than one enemy
            int randomEnemyId = Random.Range(0, enemies.Count);
            int randomEnemyCost = enemies[randomEnemyId].cost;

            //Cost would only be relevant with more enemy types
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

    //Spawns Enemies on Navmesh
    public void SpawnEnemy() {
        for (int i = 0; i < 1; i++) {
            NavMeshAgent agent = enemiesToSpawn[0].GetComponent<NavMeshAgent>();

            //Gets Random Point on Navmesh
            //if point is Valid then Sppawn enemy
            NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
            int vertexIndex = Random.Range(0, triangulation.vertices.Length);
            NavMeshHit hit;
            if (!NavMesh.SamplePosition(triangulation.vertices[vertexIndex], out hit, 2f, -1)) {
                i = 0;
                continue;
            }
            
            //Check min Spawn Height
            if (hit.position.y < minHeight) {
                i = 0;
                continue;
            }
            
            //Spawning Enemy
            agent.Warp(hit.position);
            agent.enabled = true;
            var obj =Instantiate(enemiesToSpawn[0], hit.position, Quaternion.identity);
            obj.transform.parent = transform;
            
            enemiesToSpawn.RemoveAt(0);

            if (enemiesToSpawn.Count == 0) {

                waveMulti += 2;

            }

        }
    }

    [System.Serializable]
    public class Enemy {
        public GameObject enemyPrefab;
        public int cost;
    }
}
