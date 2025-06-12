using UnityEngine;

public class BulletHitDestroy : MonoBehaviour
{
   public void OnTriggerEnter(Collider other)
   {
       // Check if the collided object has an IDamage component
       IDamage dmg = other.GetComponent<IDamage>();
       if (dmg != null)
       {
           // Apply damage to the object
           dmg.takeDamage(10); // Assuming a fixed damage value of 10
       }
        // Destroy After the object has beent it
        // This will destroy the bullet itself
        Destroy(gameObject);
    }
}
