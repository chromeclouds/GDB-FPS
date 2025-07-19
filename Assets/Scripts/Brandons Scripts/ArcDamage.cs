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

    [SerializeField] float homingDelay;     // Time before the projectile starts homing after being launched
    [SerializeField] float homingSpeed;     // Speed the projectile moves once homing starts
    [SerializeField] float turnRate;        // How quickly the projectile can rotate toward its target

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip hitSound;

    bool isDamaging;
    // See if the projectile is currently homing
    bool isHoming = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {  
        if(type == damageType.moving || type == damageType.homing)
        {
            Destroy(gameObject, destroyTime);

            // Enable gravity so the projectile follows arc trajectory
            rb.useGravity = true;
            // Launch the projectile in a forward direction with a slight upward arc
            Vector3 launchDirection = (transform.forward + Vector3.up * 1.0f).normalized;
            rb.AddForce(launchDirection * speed, ForceMode.VelocityChange);

            // If the projectile is a homing start delay
            if (type == damageType.homing)
            {
                StartCoroutine(StartHoming());
            }
            if (audioSource != null && shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isHoming)
        {
            // Continuously adjust the projectiles direction to home in on the player
            Vector3 toTarget = (gameManager.instance.player.transform.position - transform.position).normalized;
            Vector3 newDir = Vector3.RotateTowards(rb.linearVelocity.normalized, toTarget, turnRate * Time.deltaTime, 0f);
            rb.linearVelocity = newDir * homingSpeed;
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
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
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
        if(dmg != null && type  == damageType.DOT && !isDamaging)
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

    // Coroutine that enables homing behavior after a delay
    IEnumerator StartHoming()
    {
        yield return new WaitForSeconds(homingDelay);
        isHoming = true;
        rb.useGravity = false;
    }
}
