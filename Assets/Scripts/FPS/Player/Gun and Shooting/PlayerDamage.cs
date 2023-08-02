using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour {

    [SerializeField] private int maxHealth;
    private int curHealth;

    public HealthBar healthBar;

    private void Start() {
        curHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int amount) {
        curHealth -= amount;
        healthBar.SetHealth(curHealth);
    }

}
