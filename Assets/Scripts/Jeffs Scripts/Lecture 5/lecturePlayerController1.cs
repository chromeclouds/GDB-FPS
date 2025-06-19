using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

public class lecutrePlayerController : MonoBehaviour, IDamage, IPickup, IOpen
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpVel;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;
    [SerializeField] List<WeaponStats> weaponList = new List<WeaponStats>();
    [SerializeField] GameObject weaponModel;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;

    bool isSprinting;

    int jumpCount;
    int HPOrig;
    int weaponListPOS;
    float shootTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        spawnPlayer();
    }

    Vector3 moveDir;
    Vector3 playerVel;
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
        if (!gameManager.instance.isPaused)
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

        if (Input.GetButton("Fire1") && weaponList.Count > 0 && weaponList[weaponListPOS].ammoCur > 0 && shootTimer > shootRate)
            shoot();

        //transform.position += moveDir * speed * Time.deltaTime;
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
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
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
        if (Input.GetButtonDown("Reload"))
        {
            weaponList[weaponListPOS].ammoCur = weaponList[weaponListPOS].ammoMax;
        }
    }
    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();
        StartCoroutine(damageFlash());
        if (HP <= 0)
        {
            //Hey I lost
            gameManager.instance.youLose();
        }
    }

    void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
    }

    IEnumerator damageFlash()
    {
        gameManager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDamageScreen.SetActive(false);
    }

    public void getWeaponStats(WeaponStats weapon)
    {
        weaponList.Add(weapon); //Inventory
        weaponListPOS = weaponList.Count - 1;
        changeWeapon();
    }

    void changeWeapon()
    {
        shootDamage = weaponList[weaponListPOS].shootDamage;
        shootDist = weaponList[weaponListPOS].shootDist;
        shootRate = weaponList[weaponListPOS].shootRate;

        weaponModel.GetComponent<MeshFilter>().sharedMesh = weaponList[weaponListPOS].model.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weaponList[weaponListPOS].model.GetComponent<MeshRenderer>().sharedMaterial;
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

    public void spawnPlayer()
    {
        controller.transform.position = gameManager.instance.playerSpawnPos.transform.position;

        HP = HPOrig;
        updatePlayerUI();
    }
}
