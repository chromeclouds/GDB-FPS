using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject destructionEffect;

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            DestroyGate();
        }
    }

    void DestroyGate()
    {
        if (destructionEffect != null)
        {
            Instantiate(destructionEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}