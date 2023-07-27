using UnityEngine;

public class Target : MonoBehaviour
{

    [SerializeField] private float _health;

    public void TakeDamage(float amount)
    {
        _health -= amount;
        if (_health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
