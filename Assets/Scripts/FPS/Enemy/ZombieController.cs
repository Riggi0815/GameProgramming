using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour {

    public NavMeshAgent agent = null;
    private bool enemyOnGround;
    [SerializeField] private Transform target;

    // Start is called before the first frame update
    void Awake() {
        
        agent = GetComponent<NavMeshAgent>();

        target = GameObject.Find("FirstPersonPlayer").transform;

    }

    private void FixedUpdate() {
        MoveToTarget();
    }

    private void MoveToTarget() {
        
        agent.SetDestination(target.position);

    }

}
