using UnityEngine;

public class Target : MonoBehaviour
{

    [SerializeField] private float _health;

    public void PlayerDamage(float amount)
    {
        _health -= amount;
        if (_health <= 0)
        {
            Die();
        }
    }

    public void EnemyDamage(float amount) {
        _health -= amount;
        Debug.Log(_health);
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
