using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class unifiedPlayerController : MonoBehaviour, IDamage, IPickup, IOpen
{
    [Header("Character Movement")]
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpVel;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    [Header("Player Stats")]
    [SerializeField] int HP;
    int HPOrig;

    [Header("UI")]
    [SerializeField] TMP_Text ammoCount;
    [SerializeField] float lookDistance;

    [Header("Weapon Handling")]
    public Transform weaponHolder;
    private List<GameObject> ownedWeapons = new List<GameObject>();
    private int currentWeaponIndex = 0;

    Vector3 moveDir;
    Vector3 playerVel;
    int jumpCount;
    bool isSprinting;

    void Start()
    {
        HPOrig = HP;
        spawnPlayer();
    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            movement();
            sprint();
            weaponSwap();
        }
    }

    void movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel.y = 0;
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);
        controller.Move(moveDir * speed * Time.deltaTime);

        jump();
        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;

        if (Input.GetButton("Interact"))
            interact();

        look();
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

    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();
        StartCoroutine(damageFlash());
        if (HP <= 0)
        {
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

    void look()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, lookDistance, ~ignoreLayer))
        {
            ICost cost = hit.collider.GetComponent<ICost>();
            if (cost != null)
            gameManager.instance.interactPromptPrice.text = cost.checkPrice().ToString("f0");
            gameManager.instance.interactPrompt.SetActive(cost != null && !hit.collider.CompareTag("Bought"));
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
                cost.buy();
        }
    }

    public void spawnPlayer()
    {
        controller.transform.position = gameManager.instance.playerSpawnPos.transform.position;
        HP = HPOrig;
        updatePlayerUI();
    }

    public void getWeaponStats(WeaponStats weapon) { } //lecture 
    public void getWeaponData(WeaponData data, GameObject heldPrefab)
    {
        GameObject spawned = Instantiate(heldPrefab, weaponHolder);
        spawned.transform.localPosition = Vector3.zero;
        spawned.transform.localRotation = Quaternion.identity;

        WeaponFire fire = spawned.GetComponent<WeaponFire>();
        if (fire != null)
        {
            fire.weaponData = data;
            fire.weaponHeldPrefab = heldPrefab;
            fire.weaponWorldPrefab = data.WeaponWorldPrefab;

            // Auto-assign bulletSpawnPoint
            if (fire.bulletSpawnPoint == null)
            {
                fire.bulletSpawnPoint = spawned.transform.Find("bulletSpawnPoint");
                if (fire.bulletSpawnPoint == null)
                    Debug.LogError("BulletSpawnPoint not found on: " + spawned.name);
            }

            // Manually re-run OnEnable logic if needed
            fire.enabled = false;
            fire.enabled = true;
        }

        var pickup = spawned.GetComponent<unifiedWeaponPickup>();
        if (pickup != null)
        {
            pickup.enabled = false;
        }

        foreach (Collider col in spawned.GetComponentsInChildren<Collider>())
            col.enabled = false;

        ownedWeapons.Add(spawned);
        currentWeaponIndex = ownedWeapons.Count - 1;

        for (int i = 0; i < ownedWeapons.Count; i++)
        {
            ownedWeapons[i].SetActive(i == currentWeaponIndex);
        }
    }



    void weaponSwap()
    {
        if (ownedWeapons.Count == 0)
            return;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % ownedWeapons.Count;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            currentWeaponIndex--;
            if (currentWeaponIndex < 0)
                currentWeaponIndex = ownedWeapons.Count - 1;
        }

        //manually activate only the current weapon
        for (int i = 0; i < ownedWeapons.Count; i++)
        {
            ownedWeapons[i].SetActive(i == currentWeaponIndex);
        }
    }
    public void AddExistingWeapon(GameObject weapon)
    {
        weapon.transform.SetParent(weaponHolder);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        ownedWeapons.Add(weapon);
        currentWeaponIndex = ownedWeapons.Count -1;
        for (int i = 0; i < ownedWeapons.Count; i++ )
        {
            ownedWeapons[i].SetActive(i == currentWeaponIndex);
        }
    }


    void switchTo(int index)
    {
        if (ownedWeapons.Count == 0) return;

        ownedWeapons[currentWeaponIndex].SetActive(false);
        currentWeaponIndex = index;
        ownedWeapons[currentWeaponIndex].SetActive(true);
    }

    public GameObject GetCurrentHeldWeapon()
    {
        return ownedWeapons.Count > 0 ? ownedWeapons[currentWeaponIndex] : null;
    }

    public GameObject RemoveCurrentHeldWeapon()
    {
        if (ownedWeapons.Count == 0) return null;

        GameObject weaponToDrop = ownedWeapons[currentWeaponIndex];
        ownedWeapons.RemoveAt(currentWeaponIndex);

        if (ownedWeapons.Count == 0)
        {
            currentWeaponIndex = 0;
        }
        else
        {
            currentWeaponIndex = Mathf.Clamp(currentWeaponIndex, 0, ownedWeapons.Count - 1);
            for (int i = 0; i < ownedWeapons.Count; i++)
            {
                ownedWeapons[i].SetActive(i == currentWeaponIndex);
            }
        }

        return weaponToDrop;
    }

}
