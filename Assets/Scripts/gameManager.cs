using UnityEngine;
using UnityEngine.UI;
using TMPro;

 
public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    public playerController playerController;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] TMP_Text gameGoalCountText;
    [SerializeField] int wallet;

    public Image playerHPBar;
    public GameObject playerDamageScreen;
 
    public GameObject player;
    public playerController playerScript;
    public GameObject interactPrompt;

    public bool isPaused;

    float timescaleOrig;

    int gameGoalCount;
    

   [SerializeField] TMP_Text ammo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();

        timescaleOrig = Time.timeScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        updateAmmoCount();


        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null) 
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause) 
            { 
                stateUnpause(); 
            }
        }
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void stateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = timescaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void updateGameGoal(int amount)
    {
        gameGoalCount += amount;
        gameGoalCountText.text = gameGoalCount.ToString("f0");

        if(gameGoalCount <= 0)
        {
            //you win
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
    }
    public void updateAmmoCount(int amount=1)
    {
 
 
        //|| Input.GetButton("Fire1")
        //the code above will subtract 1 from the ammo every frame of the game if put into the if statement below

        //Checking to see if the Fire1 button is pressed
        if (Input.GetButtonDown("Fire1"))
        {
 
            //if so, turn ammo.text into an int named ammoBase
            if (int.TryParse(ammo.text, out int ammoBase) )
            {
                //checking to see if ammoBase is not equal to 0, if so, continue with the method.
                if (ammoBase != 0)
                {
                    //create a new int called newAmmo and have it equal ammoBase - amount (1)
                    int newAmmo = ammoBase - amount;
                    //update the string
                    ammo.text = newAmmo.ToString();
                    //debug
                    //Debug.Log("Ammo updated to: " + newAmmo);
                }

            }
        }
 
    }
    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public int walletAmount()
    {
        return wallet;
    }
    public void reduceWallet(int amount)
    {
        wallet -= amount;
    }
 
}
