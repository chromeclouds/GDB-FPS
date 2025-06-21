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
    [SerializeField] GameObject startRoundPrompt;
    [SerializeField] TMP_Text gameGoalCountText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text scoreRound;
    [SerializeField] int wallet;
    [SerializeField] int rounds;
    [SerializeField] int roundValue;

    public Image playerHPBar;
    public GameObject playerDamageScreen;
 
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpawnPos;
    public GameObject interactPrompt;
    public TMP_Text interactPromptPrice;
    public GameObject checkpointPopup;

    public bool isPaused;

    float timescaleOrig;

    int gameGoalCount;
    int currRound;
    

   [SerializeField] TMP_Text ammo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        currRound = 0;
        player = GameObject.FindWithTag("Player");
        scoreText.text = wallet.ToString("f0");
        scoreRound.text = currRound.ToString("f0") + "/" + rounds.ToString("f0");
        playerScript = player.GetComponent<playerController>();
        timescaleOrig = Time.timeScale;
        startRoundPrompt.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //updateAmmoCount();


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
        if (startRoundPrompt.activeSelf && Input.GetButtonDown("Submit"))
        {
            startRoundPrompt.SetActive(false);
            wallet += roundValue;
            currRound++;
            scoreText.text = wallet.ToString("f0");
            scoreRound.text = currRound.ToString("f0") + "/" + rounds.ToString("f0");
            activateSpawners();
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

        if(gameGoalCount <= 0 && currRound == rounds)
        {
            //you win
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
        else if(gameGoalCount <= 0)
        {
            startRoundPrompt.SetActive(true);
        }
    }
    
    void activateSpawners()
    {
        EnemySpawn[] spawners = FindObjectsByType<EnemySpawn>(FindObjectsSortMode.None);

        foreach(var spawner in spawners)
        {
            spawner.TriggerSpawn();
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
        scoreText.text = wallet.ToString("f0");
    }
    public void increaseWallet(int amount)
    {
        wallet += amount;
        scoreText.text = wallet.ToString("f0");
    }

}
