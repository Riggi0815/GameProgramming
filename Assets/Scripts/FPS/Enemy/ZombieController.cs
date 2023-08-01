using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour {

    public NavMeshAgent agent = null;
    private bool enemyOnGround;
    [SerializeField] private Transform player;

    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    private float timeOfLastAttack = 0;

    // Start is called before the first frame update
    void Awake() {
        
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.Find("FirstPersonPlayer").transform;

    }

    private void FixedUpdate() {
        MoveToTarget();
        RotateToTarget();
    }

    private void MoveToTarget() {
        
        agent.SetDestination(player.position);

        float distanceToTarget = Vector3.Distance(player.position, transform.position);
        if (distanceToTarget <= agent.stoppingDistance) {
            if (Time.time >= timeOfLastAttack + attackSpeed) {
                timeOfLastAttack = Time.time;
                DoDamage();
            }
        }

    }

    private void RotateToTarget() {
        transform.LookAt(player);
    }

    private void DoDamage() {
        Target target = player.GetComponent<Target>();
        target.EnemyDamage(damage);
    }
    
}
