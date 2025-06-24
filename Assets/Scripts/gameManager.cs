using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    public playerController playerController;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject startRoundPrompt;
    [SerializeField] GameObject difficultyPrompt;
    [SerializeField] TMP_Text gameGoalCountText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text scoreRound;
    [SerializeField] int wallet;
    [SerializeField] int rounds;
    [SerializeField] int roundValue;

    public Image playerHPBar;
    public Image playerArmorBar;
    public GameObject playerDamageScreen;
 
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpawnPos;
    public GameObject interactPrompt;
    public TMP_Text interactPromptPrice;
    public GameObject checkpointPopup;
    public GameObject levelTimer;

    public bool isPaused;
    bool roundPaused;

    float timescaleOrig;

    public int gameGoalCount;
    int currRound;
    int scoreMult;
    int spawnMult;
    

   

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
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
        StartCoroutine(welcomeMessage());
    }

    // Update is called once per frame
    void Update()
    {
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
        if (startRoundPrompt.activeSelf && Input.GetButtonDown("Submit") && !isPaused)
        {
            startRoundPrompt.SetActive(false);
            currRound++;
            scoreRound.text = currRound.ToString("f0") + "/" + rounds.ToString("f0");
            activateSpawners();
            menuActive = null;
            roundPaused = false;
            levelTimer.GetComponent<LevelTimer>().StartTimer();
        }
    }

    IEnumerator welcomeMessage()
    {
        yield return new WaitForSeconds(0.1f);
        difficultySelection();
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
        if(roundPaused)
            startRoundPrompt.SetActive(false);
        isPaused = !isPaused;
        Time.timeScale = timescaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
        if (roundPaused)
            startRoundPrompt.SetActive(true);
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
            roundPaused = true;
            difficultySelection();
        }
    }
    
    void activateSpawners()
    {
        EnemySpawn[] spawners = FindObjectsByType<EnemySpawn>(FindObjectsSortMode.None);

        foreach(var spawner in spawners)
        {
            spawner.TriggerSpawn(spawnMult);
        }

    }


    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public void difficultySelection()
    {
        statePause();
        menuActive = difficultyPrompt;
        difficultyPrompt.SetActive(true);
    }
    public void yes()
    {
        difficultyPrompt.SetActive(false);
        scoreMult = 2;
        spawnMult = 2;
        wallet += (roundValue * scoreMult);
        scoreText.text = wallet.ToString("f0");
        stateUnpause();
        startRoundPrompt.SetActive(true);
    }

    public void no()
    {
        difficultyPrompt.SetActive(false);
        scoreMult = 1;
        spawnMult = 1;
        wallet += (roundValue * scoreMult);
        scoreText.text = wallet.ToString("f0");
        stateUnpause();
        startRoundPrompt.SetActive(true);
    }

    public void endRound()
    {
        LectureEnemyAI[] enemies = FindObjectsByType<LectureEnemyAI>(FindObjectsSortMode.None);

        foreach (var enemy in enemies)
        {
            enemy.endRound();
        }
        updateGameGoal(-gameGoalCount);
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
        wallet += (amount * scoreMult);
        scoreText.text = wallet.ToString("f0");
    }

    public void openDoor()
    {

    }
}
