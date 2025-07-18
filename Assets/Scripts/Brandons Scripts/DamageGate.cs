using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

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
        else
        {
           gameManager.instance.gateDamaged();
        }
    }

    void DestroyGate()
    {

        //Destroy(gameObject);
        gameObject.GetComponent<door>().Open();
        gameManager.instance.youLose();

    }
}