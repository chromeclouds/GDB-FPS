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
    [SerializeField] int armor;
    [SerializeField] int armorValue;
    [SerializeField] int medArmorValue;
    [SerializeField] int heavyArmorValue;
    [SerializeField] int armorMax;
    int HPOrig;

    [Header("UI")]
    [SerializeField] TMP_Text ammoCount;
    [SerializeField] float lookDistance;
    [SerializeField] float interactRate;

    [Header("Weapon Handling")]
    public Transform weaponHolder;
    private List<GameObject> ownedWeapons = new List<GameObject>();
    private int currentWeaponIndex = 0;

    [Header("Melee Values")]
    [SerializeField] int meleeDist;
    [SerializeField] int meleeDmg;
    [SerializeField] float meleeCD;
    [SerializeField] GameObject pivotPoint;
    [SerializeField] private Transform meleeHolder;

    [Header("Animator")]
    [SerializeField] Animator anim;

    Vector3 moveDir;
    Vector3 playerVel;
    int jumpCount;
    float interactTime;
    float meleeCDTimer;
    bool isMeleeing;
    bool isSprinting;
    int remainingDamage;
    public bool hasTorch;

    private GameObject currentMeleeWeapon;

    void Start()
    {
        HPOrig = HP;
        armorValue = armor;
        spawnPlayer();
        hasTorch = false;
    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            movement();
            sprint();
            if (!isMeleeing)
            {
                weaponSwap();
            }
        }
    }


    void movement()
    {
        meleeCDTimer += Time.deltaTime;

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

        interactTime += Time.deltaTime;

        if (Input.GetButton("Interact") && interactTime >= interactRate)
            interact();

        look();

        if (Input.GetButtonDown("Melee") && meleeCDTimer > meleeCD)
        {
            melee();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            SetAiming(true);
        }

        if (Input.GetButtonUp("Fire2"))
        {
            SetAiming(false);
        }
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

    void melee()
    {
        pivotPoint.gameObject.SetActive(true);
        meleeCDTimer = 0;

        isMeleeing = true;
        weaponHolder.gameObject.SetActive(false);
        StartCoroutine(MeleeAnim());

       // RaycastHit hit;
        //if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, meleeDist, ~ignoreLayer))
        //{
          //  IDamage dmg = hit.collider.GetComponent<IDamage>();
          //  if (dmg != null)
           // {
            //    dmg.takeDamage(meleeDmg);
            //}
       // }
    }

    public void PickupMeleeWeapon(MeleeWeaponData data)
    {
        if (currentMeleeWeapon == null) Destroy(meleeHolder.gameObject.transform.GetChild(0)?.gameObject);
        else Debug.Log("Replacing existing melee weapon: " + currentMeleeWeapon.name);

        if (currentMeleeWeapon != null) Destroy(currentMeleeWeapon);

        currentMeleeWeapon = Instantiate(data.heldPrefab, meleeHolder.gameObject.transform);
        currentMeleeWeapon.transform.localPosition = data.heldPosition;
        currentMeleeWeapon.transform.localEulerAngles = data.heldRotation;

        //var heldScript = currentMeleeWeapon.GetComponent<MeleeWeaponHeld>();
        //heldScript.weaponData = data;
    }

    IEnumerator MeleeAnim()
    {
        float duration = 0.3f;
        float elapsed = 0f;

        float startAngle = -45f;
        float endAngle = 125f;

        Transform pivot = pivotPoint.transform;

        pivot.localRotation = Quaternion.Euler(0f, startAngle, 0f);


        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float angle = Mathf.Lerp(startAngle, endAngle, Mathf.SmoothStep(0f, 1f, t));
            pivot.localRotation = Quaternion.Euler(0f, angle, 0f);

            yield return null;
        }

        pivot.localRotation = Quaternion.Euler(0f, startAngle, 0f);
        weaponHolder.gameObject.SetActive(true);
        isMeleeing = false;
        pivotPoint.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Armor"))
        {
            armorValue++;
        }
        if (other.CompareTag("MedArmor"))
        {
            armorValue = armorValue + 2;
        }
        if (other.CompareTag("HeavyArmor"))
        {
            armorValue = armorValue + 3;
        }
        // Clamp to max armor value
        if (armorValue > armorMax)
        {
            armorValue = armorMax;
        }

        updatePlayerUI();

        // Destroy pickup object after collection
        if (other.CompareTag("Armor") || other.CompareTag("MedArmor") || other.CompareTag("HeavyArmor"))
        {
            Destroy(other.gameObject);
        }
    }
    public void takeDamage(int amount)
    {
        if (gameManager.instance.GetDifficulty())
            amount *= 2;
        int damageToHP = 0;

        if (armorValue > 0)
        {
            int overflow = amount - armorValue;
            armorValue -= amount;
            if (armorValue < 0) armorValue = 0;
            if (overflow > 0) damageToHP = overflow;
        }
        else
        {
            damageToHP = amount;

        }

        HP -= damageToHP;
        updatePlayerUI();
        StartCoroutine(damageFlash());
        if (HP <= 0)
        {
            gameManager.instance.youLose();
        }

        /*  //johns code remaining damage is used uninitialized unless armor value > 0
        if (gameManager.instance.GetDifficulty())
            amount *= 2;

        updatePlayerUI();
        StartCoroutine(damageFlash());
        if (armorValue <= 0)
        {
            HP -= amount;
        }
        if (armorValue > 0)
        {
            int remainingDamage = amount - armorValue;
            armorValue -= amount;
            updatePlayerUI();
            StartCoroutine(damageFlash());
        }
        if (remainingDamage > 0)
        {
            HP -= remainingDamage;
        }

        if (HP <= 0)
        {
            //oh no im dead
            gameManager.instance.youLose();
        }
        */

    }

    void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
        float armorPercent = (float)armorValue / armorMax;
        gameManager.instance.playerArmorBar.fillAmount = armorPercent;
    }

    public void resetHealth()
    {
        HP = HPOrig;
        updatePlayerUI();
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
            torchHolder holder = hit.collider.GetComponent<torchHolder>();
            if (cost != null)
            {
                gameManager.instance.interactPromptPrice.text = cost.checkPrice().ToString("f0");
                gameManager.instance.interactPrompt.SetActive(cost != null && !hit.collider.CompareTag("Bought"));
                gameManager.instance.interactTorchPrompt.SetActive(false);
                gameManager.instance.interactTorchPromptPlace.SetActive(false);
            }
            else if(holder != null && !hasTorch && holder.defaultTorch.activeSelf)
            {
                if(holder.GetComponent<torchHolder>().GetDifficulty())
                    gameManager.instance.interactTorchName.text = "Hard mode torch";
                else
                    gameManager.instance.interactTorchName.text = "Easy mode torch";

                gameManager.instance.interactTorchPrompt.SetActive(holder != null && !hasTorch);
                gameManager.instance.interactTorchPromptPlace.SetActive(false);
                gameManager.instance.interactPrompt.SetActive(false);
            }
            else if(holder != null && hasTorch && !holder.defaultTorch.activeSelf)
            {
                if(holder.GetComponent<torchHolder>().GetDifficulty() == gameManager.instance.GetDifficulty())
                    gameManager.instance.interactTorchPromptPlace.SetActive(holder != null && hasTorch);
                gameManager.instance.interactPrompt.SetActive(false);
                gameManager.instance.interactTorchPrompt.SetActive(false);
            }
        }
        else
        {
            gameManager.instance.interactPrompt.SetActive(false);
            gameManager.instance.interactTorchPrompt.SetActive(false);
            gameManager.instance.interactTorchPromptPlace.SetActive(false);
        }
    }

    void interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, lookDistance, ~ignoreLayer))
        {
            ICost cost = hit.collider.GetComponent<ICost>();
            torchHolder holder = hit.collider.GetComponent<torchHolder>();
            if (cost != null && !hit.collider.CompareTag("Bought"))
                cost.buy();
            else if (holder != null && !hasTorch && holder.defaultTorch.activeSelf)
                gameManager.instance.DifficultyChange(holder.GivePlayerTorch());
            else if (holder != null && hasTorch && holder.GetComponent<torchHolder>().GetDifficulty() == gameManager.instance.GetDifficulty() && !holder.defaultTorch.activeSelf)
                holder.RetrieveTorch();
                interactTime = 0;
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

        AmmoManager ammoManager = GetComponent<AmmoManager>();
        int reserve = ammoManager != null ? ammoManager.GetAmmoCount(data.AmmotType) : 0;
        WeaponUIManager.instance.UpdateWeaponUI(data, fire.CurrentAmmo, reserve);
    }



    void weaponSwap()
    {
        if (ownedWeapons.Count == 0)
            return;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % ownedWeapons.Count;
            switchTo(currentWeaponIndex);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            currentWeaponIndex--;
            if (currentWeaponIndex < 0)
                currentWeaponIndex = ownedWeapons.Count - 1;
            switchTo(currentWeaponIndex);
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
        currentWeaponIndex = ownedWeapons.Count - 1;
        for (int i = 0; i < ownedWeapons.Count; i++)
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

        UpdateWeaponAnimation();

        WeaponFire fire = ownedWeapons[currentWeaponIndex].GetComponent<WeaponFire>();
        if (fire != null && fire.weaponData != null)
        {
            AmmoManager ammoManager = GetComponent<AmmoManager>();
            int reserve = ammoManager != null ? ammoManager.GetAmmoCount(fire.weaponData.AmmotType) : 0;
            WeaponUIManager.instance.UpdateWeaponUI(fire.weaponData, fire.CurrentAmmo, reserve);
        }

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

    public void addArmor(int amount)
    {
        armorValue += amount;
        if (armorValue > armorMax)
            armorValue = armorMax;
        updatePlayerUI();
    }

    void UpdateWeaponAnimation()
    {
        GameObject weapon = GetCurrentHeldWeapon();

        int weaponType = 0;

        if (weapon != null)
        {
            string tag = weapon.tag;

            if (tag == "Pistol")
                weaponType = 1;

            else if (tag == "Shotgun")
                weaponType = 2;

            else if (tag == "Rifle")
                weaponType = 3;
        }
        anim.SetInteger("WeaponType", weaponType);
    }

    void SetAiming(bool aiming)
    {
        anim.SetBool("IsAiming", aiming);
    }

    void PlayMelee()
    {
        anim.SetTrigger("Melee");
    }
}
