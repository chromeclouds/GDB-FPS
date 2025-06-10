using UnityEngine;
using TMPro;
public class playerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpVel;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;
    [SerializeField] int ammoLight;
    [SerializeField] int ammoMed;
    [SerializeField] int ammoHeavy;
    //this int is for testing
    [SerializeField] int ammo;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] float shootRate;
    [SerializeField] GameObject ammoPickup;
    [SerializeField] GameObject light;
    [SerializeField] GameObject med;
    [SerializeField] GameObject heavy;
    [SerializeField] TMP_Text ammoCount;
     

    bool isSprinting;
    int jumpCount;

    float shootTimer;
 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    Vector3 moveDir;
    Vector3 playerVel;


    // Update is called once per frame
    void Update()
    {
        if (ammoCount != null)
        {
            ammoCount.text = "Ammo: " + ammo.ToString();
        }
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);

        movement();
        sprint();
    }

    void movement()
    {
        shootTimer += Time.deltaTime;

        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel.y = 0;
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right)
                + (Input.GetAxis("Vertical") * transform.forward);

        controller.Move(moveDir * speed * Time.deltaTime);

        //jump
        jump();

        controller.Move(playerVel * Time.deltaTime);

        playerVel.y -= gravity * Time.deltaTime;

        if (Input.GetButton("Fire1") && shootTimer > shootRate)
            shoot();

        //transform.position += moveDir * speed * Time.deltaTime;

    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
            isSprinting = true;

        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            isSprinting = false;
        }
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVel.y = jumpVel;
            jumpCount++;
        }
    }

    void shoot()
    {
        if (ammo > 0)
        {
            ammo--;
        }
        shootTimer = 0;
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

    public void takeDamage(int amount)
    {
        HP -= amount;
        if (HP <= 0)
        {
            //oh no im dead
            gameManager.instance.youLose();
        }
    }

    public void lowerAmmo()
    {
        ammo = 30;
        if (ammo > 0)
        {
            ammo--;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ammo Light"))
        {
            ammo++;
        }
        if (other.CompareTag("Ammo Med"))
        {
            ammo++;
        }
        if (other.CompareTag("Ammo Heavy"))
        {
            ammo++;
        }
    }
}
