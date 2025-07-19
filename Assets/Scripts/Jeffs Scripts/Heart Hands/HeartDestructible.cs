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
            if (GlobalHeartManager.Instance != null)
            {
                GlobalHeartManager.Instance.RegisterHeartDestroyed(this);
            }
            else
            {
                Debug.LogWarning("global heart manager not set");
            }
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        StartCoroutine(WaitForGlobalHeartManager());
    }

    private System.Collections.IEnumerator WaitForGlobalHeartManager()
    {
        while (GlobalHeartManager.Instance == null)
        {
            yield return null;
        }

        GlobalHeartManager.Instance.RegisterHeart(this);
    }
}
