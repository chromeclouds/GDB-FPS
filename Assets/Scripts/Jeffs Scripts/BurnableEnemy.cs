using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;




public class BurnableEnemy : MonoBehaviour, IBurnable, IDamage
{

    private bool isBurning = false;
    private float burnTimeLeft = 0f;
    private float burnDamagePerSecond = 0;

    private int health = 100;

    public void ApplyBurn(float duration, float damagePerSecond)
    {
        if (!isBurning)
        {
            isBurning = true;
            burnTimeLeft = duration;
            burnDamagePerSecond = damagePerSecond;
            StartCoroutine(Burn());
        }
    }

    private IEnumerator Burn()
    {
        while (burnTimeLeft > 0)
        {
            takeDamage(Mathf.RoundToInt(burnDamagePerSecond));
            burnTimeLeft -= 1f;
            yield return new WaitForSeconds(1f);

            TrySpreadFire();
        }

        isBurning = false;
    }

    void TrySpreadFire()
    {
        Collider[] nearby = Physics.OverlapSphere(transform.position, 3f);

        foreach (Collider col in nearby)
        {
            IBurnable burnable = col.GetComponent<IBurnable>();

            if (burnable != null && burnable != this)
            {
                burnable.ApplyBurn(2f, burnDamagePerSecond); //spread lighter
            }
        }
    }

    public void takeDamage(int amount)
    {
        health -= amount;
        Debug.Log($"{gameObject.name} took {amount} burn damage. Health = {health}");
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
