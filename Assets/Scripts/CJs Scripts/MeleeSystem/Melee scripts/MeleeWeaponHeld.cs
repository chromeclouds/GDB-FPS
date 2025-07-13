using UnityEngine;

public class MeleeWeaponHeld : MonoBehaviour
{
    public MeleeWeaponData weaponData;
    public GameObject AttackPoint;
    public float attackCooldown = 1f;
    [SerializeField] GameObject pivotPoint;
    public Transform weaponHolder;

    private bool isMeleeing;
    private float nextAttackTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Melee") && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            Attack();
        }
    }

    void Attack()
    {

        // TODO: Add attack animation here
        Collider[] hitEnemies = Physics.OverlapSphere(AttackPoint.transform.position, 1.5f);
        foreach (var enemy in hitEnemies)
        {
            if (enemy.TryGetComponent<IDamage>(out var damageable))
            {
                damageable.takeDamage((int)weaponData.damage);
            } 
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (AttackPoint != null)
        Gizmos.DrawWireSphere(AttackPoint.transform.position, 1.5f);
    }
}
