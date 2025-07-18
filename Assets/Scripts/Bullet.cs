using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!gameObject.activeInHierarchy) return; //prevents double hits hopefully fixes shotguns

        IDamage dmg = collision.collider.GetComponent<IDamage>();
        if(dmg != null)
        {
            dmg.takeDamage(damage); //damage should come from weapondata
        }
        Destroy(gameObject);
    }

    
}
