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
       // Destroy the bullet after it hits something
       Destroy(gameObject);
    }
}
