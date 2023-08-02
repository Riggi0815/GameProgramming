using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour {

    //Variables for Health
    [SerializeField] private int maxHealth;
    private int curHealth;

    //Reference to the HealthBar
    public HealthBar healthBar;

    //on Start MaxHealth get assigned to curHealth
    private void Start() {
        curHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    //Update Health
    public void TakeDamage(int amount) {
        curHealth -= amount;
        healthBar.SetHealth(curHealth);
    }

}
