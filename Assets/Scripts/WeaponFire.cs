using System.Threading;
using UnityEngine;

public class WeaponFire : MonoBehaviour
{
    public Animator animator; //trigger firing animation with this
    public float fireRate = 0.1f; //how fast gun can fire (seconds between shots)
    private float fireTimer = 0f; //timer to control fire rate

    public float shootDistance = 100f;
    public int shootDamage = 10;
    public LayerMask ignoreLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.deltaTime;

        if(Input.GetButton("Fire1") && fireTimer >= fireRate)
        {
            Fire();
            fireTimer = 0f;
        }
        
    }

    void Fire()
    {
        if (animator != null)
        {
            animator.SetTrigger("Fire"); //play firing anim
        }
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);
            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }
    }
}
