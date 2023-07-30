using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private GameObject enemy;

    [SerializeField] private int waveSize;
    private int enemyCount;

    [SerializeField] private float maxHeight;
    [SerializeField] private float minHeight;
    [SerializeField] private Vector2 xRange;
    [SerializeField] private Vector2 zRange;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(EnemyDrop());

    }
    
    IEnumerator EnemyDrop() {
        while (enemyCount < waveSize) {
            
            float sampleX = Random.Range(xRange.x, xRange.y);
            float sampleY = Random.Range(zRange.x, zRange.y);
            Vector3 raycastStart = new Vector3(sampleX, maxHeight, sampleY);

            if (!Physics.Raycast(raycastStart, Vector3.down, out RaycastHit hit, Mathf.Infinity)) {
                continue;
            }

            if (hit.point.y < minHeight) {
                continue;
            }

            Debug.Log(hit.point);
            Instantiate(enemy, hit.point, Quaternion.identity);
            yield return new WaitForSeconds(.1f);
            enemyCount++;
        }
    }
    
}
