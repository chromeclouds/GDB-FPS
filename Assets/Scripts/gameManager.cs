using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    public playerController playerController;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] TMP_Text gameGoalCountText;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text scoreWinText;
    [SerializeField] TMP_Text scoreLoseText;
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
    public GameObject playerPortal;
    public GameObject interactPrompt;
    public TMP_Text interactPromptPrice;
    public GameObject interactTorchPrompt;
    public TMP_Text interactTorchName;
    public GameObject interactTorchPromptPlace;
    public GameObject checkpointPopup;
    public GameObject levelTimer;

    public bool isPaused;

    float timescaleOrig;

    public bool playerIsOutside;
    public int gameGoalCount;
    int currRound;
    int currLevel;
    int scoreMult;
    int spawnMult;
    bool isHardMode;
    bool isLoading;
    

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        isLoading = false;
        currLevel = SceneManager.GetActiveScene().buildIndex;
        instance = this;
        currRound = 0;
        spawnMult = 1;
        player = GameObject.FindWithTag("Player");
        scoreText.text = wallet.ToString("f0");
        scoreWinText.text = wallet.ToString("f0");
        scoreLoseText.text = wallet.ToString("f0");
        scoreRound.text = currRound.ToString("f0") + "/" + rounds.ToString("f0");
        playerScript = player.GetComponent<playerController>();
        timescaleOrig = Time.timeScale;
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
        playerPortal = GameObject.FindWithTag("Portal");
        playerPortal.SetActive(false);
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(transform.root.gameObject);
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
        //if (startRoundPrompt.activeSelf && Input.GetButtonDown("Submit") && !isPaused)
        //{
        //    startRoundPrompt.SetActive(false);
        //    currRound++;
        //    scoreRound.text = currRound.ToString("f0") + "/" + rounds.ToString("f0");
        //    activateSpawners();
        //    menuActive = null;
        //    roundPaused = false;
        //    levelTimer.GetComponent<LevelTimer>().ResetTimer();
        //    levelTimer.GetComponent<LevelTimer>().StartTimer();
        //}
    }

    //IEnumerator welcomeMessage()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    difficultySelection();
    //}
    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void DifficultyChange(bool difficulty)
    {
        isHardMode = difficulty;
    }
    public bool GetDifficulty()
    {
        return isHardMode;
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

    public void updateGameGoalText(int count)
    {
        gameGoalCountText.text = count.ToString("f0");
    }

    public void restartRound()
    {
        currRound -= 1;
        if (currLevel != SceneManager.sceneCountInBuildSettings - 1)
            scoreRound.text = currRound.ToString("f0") + "/" + rounds.ToString("f0");
        else scoreRound.text = "Final";
    }
    public void updateGameGoal(int amount)
    {
        gameGoalCount += amount;
        updateGameGoalText(gameGoalCount);

        if (gameGoalCount <= 0 && currRound == rounds)
        {
            //you win
            //statePause();
            //menuActive = menuWin;
            //menuActive.SetActive(true);
            levelTimer.GetComponent<LevelTimer>().ResetTimer();
            playerPortal.SetActive(true);
        }
        else if(gameGoalCount <= 0)
        {
            player.GetComponent<unifiedPlayerController>().resetHealth();
            levelTimer.GetComponent<LevelTimer>().ResetTimer();
        }
    }

    public void LoadNextLevel()
    {
        if (!isLoading)
        {
            isLoading = !isLoading;
            player.GetComponent<unifiedPlayerController>().resetHealth();
            levelTimer.GetComponent<LevelTimer>().ResetTimer();
            currRound = 0;
            currLevel += 1;
            SceneManager.LoadScene(currLevel);
            StartCoroutine(newScene());
        }
    }
    IEnumerator newScene()
    {
        yield return new WaitForSeconds(0.3f);
        if (currLevel != SceneManager.sceneCountInBuildSettings - 1)
            scoreRound.text = currRound.ToString("f0") + "/" + rounds.ToString("f0");
        else scoreRound.text = "Final";
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
        player.GetComponent<unifiedPlayerController>().spawnPlayer();
        player.GetComponent<unifiedPlayerController>().hasTorch = false;
        playerPortal = null;
        if (currLevel < SceneManager.sceneCountInBuildSettings - 1)
        {
            playerPortal = GameObject.FindWithTag("Portal");
            playerPortal.SetActive(false);
        }
        isLoading = !isLoading;
    }

    public void WinGame()
    {
        //you win
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }
    public void StartRound()
    {
        if (currRound == rounds || gameGoalCount > 0)
            return;
        if (isHardMode) 
            scoreMult = 2;
        else scoreMult = 1;
        activateSpawners();
        currRound++;
        increaseWallet(roundValue);
        if (currLevel != SceneManager.sceneCountInBuildSettings - 1)
            scoreRound.text = currRound.ToString("f0") + "/" + rounds.ToString("f0");
        else scoreRound.text = "Final";
        levelTimer.GetComponent<LevelTimer>().ResetTimer();
        levelTimer.GetComponent<LevelTimer>().StartTimer();
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

    //public void difficultySelection()
    //{
    //    statePause();
    //    menuActive = difficultyPrompt;
    //    difficultyPrompt.SetActive(true);
    //}
    //public void yes()
    //{
    //    difficultyPrompt.SetActive(false);
    //    scoreMult = 2;
    //    spawnMult = 2;
    //    wallet += (roundValue * scoreMult);
    //    scoreText.text = wallet.ToString("f0");
    //    scoreWinText.text = wallet.ToString("f0");
    //    scoreLoseText.text = wallet.ToString("f0");
    //    stateUnpause();
    //    startRoundPrompt.SetActive(true);
    //}

    //public void no()
    //{
    //    difficultyPrompt.SetActive(false);
    //    scoreMult = 1;
    //    spawnMult = 1;
    //    wallet += (roundValue * scoreMult);
    //    scoreText.text = wallet.ToString("f0");
    //    scoreWinText.text = wallet.ToString("f0");
    //    scoreLoseText.text = wallet.ToString("f0");
    //    stateUnpause();
    //    startRoundPrompt.SetActive(true);
    //}

    public void endRound()
    {
        LectureEnemyAI[] enemies = FindObjectsByType<LectureEnemyAI>(FindObjectsSortMode.None);

        foreach (var enemy in enemies)
        {
            enemy.endRound();
        }

        DemonAI[] demonEnemies = FindObjectsByType<DemonAI>(FindObjectsSortMode.None);

        foreach (var singleDemon in demonEnemies)
        {
            singleDemon.endRound();
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
        scoreWinText.text = wallet.ToString("f0");
        scoreLoseText.text = wallet.ToString("f0");
    }
    public void increaseWallet(int amount)
    {
        wallet += (amount * scoreMult);
        scoreText.text = wallet.ToString("f0");
        scoreWinText.text = wallet.ToString("f0");
        scoreLoseText.text = wallet.ToString("f0");
    }
}
