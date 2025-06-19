using UnityEngine;
using System.Collections;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
public class playerController : MonoBehaviour, IDamage, IPickup, IOpen
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;

    [Header("Player Stats")]
    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpVel;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    [Header("Weapon & Shooting")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] float shootRate;
    //from lecture 5 change this to work with weapondata system
    [SerializeField] List<WeaponStats> weaponList = new List<WeaponStats>();
    [SerializeField] GameObject weaponModel;

    [Header("Ammo & UI")]
    [SerializeField] int ammoLight;
    [SerializeField] int ammoMed;
    [SerializeField] int ammoHeavy;
    //this int is for testing
    [SerializeField] int ammo;
    [SerializeField] TMP_Text ammoCount;

    [Header("Raycasting")]
    [SerializeField] float lookDistance;

    [Header("Pickup Prefabs")]
    [SerializeField] GameObject ammoPickup;
    [SerializeField] GameObject light;
    [SerializeField] GameObject med;
    [SerializeField] GameObject heavy;
    
    bool isSprinting;
    int jumpCount;
    int HPOrig;
    int weaponListPOS;
    float shootTimer;

    Vector3 moveDir;
    Vector3 playerVel;

    void Start()
    {
        HPOrig = HP;
        spawnPlayer();
        updatePlayerUI();

    }

    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);
        if (!gameManager.instance.isPaused)
        {
            movement();
            sprint();
        }

        if (ammoCount != null)
        {
            ammoCount.text = "Ammo: " + ammo.ToString();
        }

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

        if (Input.GetButton("Interact"))
            interact();

        look();
        selectWeapon();
        reload();

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
        shootTimer = 0;
        weaponList[weaponListPOS].ammoCur--;
     
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance, ~ignoreLayer))
        {
            Instantiate(weaponList[weaponListPOS].hitEffect, hit.point, Quaternion.identity);

            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }

    }

    void reload()
    {
        if (Input.GetButtonDown("Reload") && weaponList.Count > 0)
        {
            weaponList[weaponListPOS].ammoCur = weaponList[weaponListPOS].ammoMax;
        }
    }

    void selectWeapon()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && weaponListPOS < weaponList.Count - 1)
        {
            weaponListPOS++;
            changeWeapon();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && weaponListPOS > 0)
        {
            weaponListPOS--;
            changeWeapon();
        }
    }

    void changeWeapon()
    {
        shootDamage = weaponList[weaponListPOS].shootDamage;
        shootDistance = weaponList[weaponListPOS].shootDist;
        shootRate = weaponList[weaponListPOS].shootRate;

        weaponModel.GetComponent<MeshFilter>().sharedMesh = weaponList[weaponListPOS].model.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weaponList[weaponListPOS].model.GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void spawnPlayer()
    {
        controller.transform.position = gameManager.instance.playerSpawnPos.transform.position;
        HP = HPOrig;
        updatePlayerUI();
    }

    public void getWeaponStats(WeaponStats weapon)
    {
        weaponList.Add(weapon);
        weaponListPOS = weaponList.Count - 1;
        changeWeapon();
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
