using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour {

    //Variables
    //For Movement
    public NavMeshAgent agent = null;
    [SerializeField] private Transform player;

    //For Attacks
    [SerializeField] private int damage;
    [SerializeField] private float attackSpeed;
    private float timeOfLastAttack = 0;
    
    //Gets relevant Components and Objects
    void Awake() {
        
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.Find("FirstPersonPlayer").transform;

    }

    //Updates the Enemy
    private void FixedUpdate() {
        MoveToTarget();
        RotateToTarget();
    }

    //Moves Enemy towards Player
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

    //look towards player
    private void RotateToTarget() {
        transform.LookAt(player);
    }

    //DoDamage to Player
    private void DoDamage() {
        PlayerDamage playerDamage = player.GetComponent<PlayerDamage>();
        playerDamage.TakeDamage(damage);
    }
    
}
