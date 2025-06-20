using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class SkullEnemyAI : MonoBehaviour, IDamage, IOpen
{
    [Header("General")]
    [SerializeField] Renderer model;
    [SerializeField] Transform headPOS;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float floatHeight = 2f;
    [SerializeField] int HP;
    [SerializeField] int roamRadius = 10;
    [SerializeField] int roamStopTime = 3;
    [SerializeField] float FOV = 60f;
    

    private Color colorOrig;
    private Vector3 targetPOS;
    private float shootTimer;
    private float roamTimer;
    private bool playerInRange;
    private Vector3 playerDir;
    private Vector3 startingPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        startingPos = transform.position;
        PickNewRoamTarget();
    }

    // Update is called once per frame
    void Update()
    {
        roamTimer += Time.deltaTime;

        if (!playerInRange)
        {
            if (roamTimer >= roamStopTime)
                PickNewRoamTarget();
            FloatMoveTowards(targetPOS);
        }
        if (playerInRange && canSeePlayer())
        {
            shootTimer += Time.deltaTime;
            FloatMoveTowards(gameManager.instance.player.transform.position);

            if (shootTimer > shootRate)
            {
                shootTimer = 0;
                shoot();
            }
        }
    }

    void FloatMoveTowards(Vector3 target)
    {
        Vector3 desiredPOS = target;
        desiredPOS.y = startingPos.y + Mathf.Sin(Time.time * 2f) * floatHeight;
        transform.position = Vector3.Lerp(transform.position, desiredPOS, moveSpeed * Time.deltaTime);

        Vector3 dir = target - transform.position;
        dir.y = 0;
        if (dir.magnitude > 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 2f);
        }
    }



    public void createBullet()
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }



    bool canSeePlayer()
    {
        Vector3 dirToPlayer = gameManager.instance.player.transform.position - headPOS.position;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (angle < FOV)
        {
            RaycastHit hit;
            if (Physics.Raycast(headPOS.position, dirToPlayer.normalized, out hit, 100))
            {
                return hit.collider.CompareTag("Player");
            }
        }
        return false;
    }

    void PickNewRoamTarget()
    {
        roamTimer = 0;
        Vector3 randomOffset = Random.insideUnitSphere * roamRadius;
        randomOffset.y = 0;
        targetPOS = startingPos + randomOffset;
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            
        }
    }
  

    IEnumerator flashRed() //Timer
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }

    

    void shoot()
    {
        shootTimer = 0f;
        createBullet();
        
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        if (HP <= 0)
        {
            Destroy(gameObject);
            gameManager.instance.updateGameGoal(-1);
        }
        else
        {
            StartCoroutine(flashRed());
        }
    }

    
}
