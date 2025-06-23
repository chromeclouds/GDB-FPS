using UnityEngine;
using System.Collections;

public class ArcDamage : MonoBehaviour
{
    enum damageType {moving, stationary, DOT, homing}
    [SerializeField] damageType type;
    [SerializeField] Rigidbody rb;

    [SerializeField] int damageAmount;
    [SerializeField] int damageRate;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;


    bool isDamaging;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {  
        if(type == damageType.moving || type == damageType.homing)
        {
            Destroy(gameObject, destroyTime);

            if(type == damageType.moving)
            {
                // Launch the projectile in a forward direction with a slight upward arc
                Vector3 launchDirection = (transform.forward + Vector3.up * 0.5f).normalized;
                rb.AddForce(launchDirection * speed, ForceMode.VelocityChange);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(type == damageType.homing)
        {
            // Continuously adjust the projectiles direction to home in on the player
            Vector3 homingDir = (gameManager.instance.player.transform.position - transform.position).normalized;
            rb.AddForce(homingDir * speed * Time.deltaTime, ForceMode.VelocityChange);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null && type != damageType.DOT) 
        {

            dmg.takeDamage(damageAmount);

        }
        if(type == damageType.homing || type == damageType.moving)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.isTrigger) return;

        IDamage dmg = other.GetComponent <IDamage>();
        if(dmg != null && type  == damageType.DOT & !isDamaging)
        {
            StartCoroutine(damageOther(dmg));
        }
    }

    IEnumerator damageOther(IDamage d)
    {
        isDamaging = true;
        d.takeDamage(damageAmount);
        yield return new WaitForSeconds(damageRate);
        isDamaging = false;

    }
}
