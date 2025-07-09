using UnityEngine;

public class VerticalSlash : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 15;

    public float lifetime = 3f;
    public float aimSlightlyTowardPlayer = 0.1f; //slight homing correction

    private Vector3 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 toPlayer = (player.transform.position - transform.position).normalized;
            direction = Vector3.Lerp(transform.forward, toPlayer, aimSlightlyTowardPlayer);

        }
        else
        {
            direction = transform.forward;

        }
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDamage dmg = other.GetComponent<IDamage>();
            if (dmg != null) dmg.takeDamage(damage);
            Destroy(gameObject);
        }

    }
}
