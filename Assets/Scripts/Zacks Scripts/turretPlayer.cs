using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class turretPlayer : MonoBehaviour 
{
    public LayerMask enemyLayerMask;
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & enemyLayerMask) != 0)
        {
            Debug.Log("Enemy entered detection range: " + other.name);
            Destroy(this.gameObject); // Example action
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TurretEnemy"))
        {
            Debug.Log("Enemy exited detection range: " + other.name);
            // Optional: Remove target or stop firing
        }
    }
}
