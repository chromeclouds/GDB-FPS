using UnityEngine;
using System.Collections;
using TMPro;
public class cjPlayerController : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] Animator anim;

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

    [SerializeField] GameObject knifeModel;
    [SerializeField] int meleeDist;
    [SerializeField] int meleeDmg;
    [SerializeField] float meleeCD;

    [SerializeField] float lookDistance;

    [SerializeField] GameObject ammoPickup;
    [SerializeField] GameObject light;
    [SerializeField] GameObject med;
    [SerializeField] GameObject heavy;
    [SerializeField] TMP_Text ammoCount;
     


    bool isSprinting;
    int jumpCount;
    int HPOrig;
    float shootTimer;
    float meleeCDTimer;
 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        updatePlayerUI();

    }

    Vector3 moveDir;
    Vector3 playerVel;


    // Update is called once per frame
    void Update()
    {
        setAnims();

        if (ammoCount != null)
        {
            ammoCount.text = "Ammo: " + ammo.ToString();
        }
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);

        movement();
        sprint();
    }

    void setAnims()
    {
        // Run
        if (controller.isGrounded) // setting up for jump
        {
            anim.SetFloat("Speed", controller.velocity.normalized.magnitude);
        }
    }

    void movement()
    {
        shootTimer += Time.deltaTime;
        meleeCDTimer += Time.deltaTime;

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

        //commented out because shoot is handled by the guns, you cant shoot if you dont have one
        //you are not a wizard
        //if (Input.GetButton("Fire1") && shootTimer > shootRate)
        //    shoot();

        look();

        if (Input.GetButton("Interact"))
            interact();

        //transform.position += moveDir * speed * Time.deltaTime;

        if (Input.GetButton("Melee") && meleeCDTimer > meleeCD) melee();
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

    void melee()
    {
        meleeCDTimer = 0;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, meleeDist, ~ignoreLayer))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(meleeDmg);
            }
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();
        StartCoroutine(damageFlash());
       
        if (HP <= 0)
        {
            //oh no im dead
            gameManager.instance.youLose();
        }
    }

    void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP/HPOrig;
    }

    IEnumerator damageFlash()
    {
        gameManager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDamageScreen.SetActive(false);
    }

    void look()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, lookDistance, ~ignoreLayer))
        {
            ICost cost = hit.collider.GetComponent<ICost>();
            if (cost != null && !hit.collider.CompareTag("Bought"))
            {
                gameManager.instance.interactPrompt.SetActive(true);
            }
            else
            {
                gameManager.instance.interactPrompt.SetActive(false);
            }
        }
        else
        {
            gameManager.instance.interactPrompt.SetActive(false);
        }
    }
    void interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, lookDistance, ~ignoreLayer))
        {

            ICost cost = hit.collider.GetComponent<ICost>();
            if (cost != null)
            {
                cost.buy();
            }
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
