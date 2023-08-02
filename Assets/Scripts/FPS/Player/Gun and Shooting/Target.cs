using System;
using UnityEngine;

public class Target : MonoBehaviour
{

    [SerializeField] private float _health;
    [SerializeField] private int scorePoints;
    private int curScore;
    public Score score;

    private void Awake() {
        score = GameObject.Find("Score").GetComponent<Score>();
    }

    public void PlayerDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
            score.SetScore(scorePoints);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
