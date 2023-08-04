using System;
using UnityEngine;

public class Target : MonoBehaviour
{

    //Variables Health and Score
    [SerializeField] private float _health;
    [SerializeField] private int scorePoints;
    public Score score;

    //Get the Score Script
    private void Awake() {
        score = GameObject.Find("Score").GetComponent<Score>();
    }

    //Applys Damage to Enemy
    public void PlayerDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
            score.AddScore(scorePoints);
        }
    }

    //Destroy when Death
    void Die()
    {
        Destroy(gameObject);
    }

}
