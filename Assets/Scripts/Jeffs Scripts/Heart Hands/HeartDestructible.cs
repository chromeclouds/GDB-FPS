using UnityEngine;

public class HeartDestructible : MonoBehaviour, IDamage
{
    public int heartIndex;
    public int health = 5;
    private bool destroyed = false;

    public void takeDamage(int damage)
    {
        if (destroyed) return;
        health -= damage;
        if (health <= 0)
        {
            destroyed = true;
            HeartTrackerManager.Instance.RegisterHeartDestroyed(heartIndex);
            Destroy(gameObject);
        }
    }
    
}
