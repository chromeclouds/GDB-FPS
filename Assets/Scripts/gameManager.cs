using System;
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
    [SerializeField] GameObject levelMusic;
    [SerializeField] GameObject menuMusic;
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
    public GameObject mainDoor;
    public GameObject interactPrompt;
    public TMP_Text interactPromptPrice;
    public GameObject interactTorchPrompt;
    public TMP_Text interactTorchName;
    public GameObject interactTorchPromptPlace;
    public GameObject checkpointPopup;
    public GameObject preFirstRoundPopup;
    public GameObject roundEndPopup;
    public GameObject finalRoundEndPopup;
    public GameObject finalLevelPopup;
    public GameObject gateAttackedPopup;
    public GameObject tutorialPopup;
    public GameObject levelTimer;

    public TMP_Text heartCountText;
    [SerializeField] public GameObject secretDoorPopupUI;

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
    bool gatePromptIsRunning;
    

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        menuMusic.GetComponent<AudioSource>().Play();
        menuMusic.GetComponent<AudioSource>().Pause();
        isLoading = false;
        currLevel = SceneManager.GetActiveScene().buildIndex;
        instance = this;
        currRound = 0;
        spawnMult = 1;
        player = GameObject.FindWithTag("Player");
        mainDoor = GameObject.FindWithTag("Gate Door");
        scoreText.text = wallet.ToString("f0");
        scoreWinText.text = wallet.ToString("f0");
        scoreLoseText.text = wallet.ToString("f0");
        scoreRound.text = currRound.ToString("f0") + "/" + rounds.ToString("f0");
        playerScript = player.GetComponent<playerController>();
        timescaleOrig = Time.timeScale;
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
        playerPortal = GameObject.FindWithTag("Portal");
        levelMusic = GameObject.FindWithTag("Level Music");
        playerPortal.SetActive(false);
        //if(SceneManager.GetActiveScene().buildIndex != 1)
        //{
            DontDestroyOnLoad(player);
            DontDestroyOnLoad(transform.root.gameObject);
        //}
        StartCoroutine(startUpMenu());
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
        levelMusic.GetComponent<AudioSource>().Pause();
        menuMusic.GetComponent<AudioSource>().UnPause();
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
        menuMusic.GetComponent<AudioSource>().Pause();
        levelMusic.GetComponent<AudioSource>().UnPause();
    }

    public void resetTime()
    {
        Time.timeScale = timescaleOrig;
    }

    public void updateGameGoalText(int count)
    {
        gameGoalCountText.text = count.ToString("f0");
    }

    public void gateDamaged()
    {
        if (gatePromptIsRunning)
            return;
        StartCoroutine(flashGatePrompt());
    }

    public void restartRound()
    {
        currRound -= 1;
        if (currLevel != SceneManager.sceneCountInBuildSettings - 1)
        {
            scoreRound.text = currRound.ToString("f0") + "/" + rounds.ToString("f0");
            StartCoroutine(flashPrompt(preFirstRoundPopup));
        }
        else
        {
            scoreRound.text = "Final";
            StartCoroutine(flashPrompt(finalLevelPopup));
        }
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
            mainDoor.GetComponent<door>().Open();
            ClearLevel();
            StartCoroutine(flashPrompt(finalRoundEndPopup));
        }
        else if(gameGoalCount <= 0)
        {
            player.GetComponent<unifiedPlayerController>().resetHealth();
            levelTimer.GetComponent<LevelTimer>().ResetTimer();
            mainDoor.GetComponent<door>().Open();
            ClearLevel();
            StartCoroutine(flashPrompt(roundEndPopup));
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
            levelMusic.SetActive(false);
            SceneManager.LoadScene(currLevel);
            StartCoroutine(newScene());
        }
    }

    public void ClearLevel()
    {
        LectureEnemyAI[] enemies = FindObjectsByType<LectureEnemyAI>(FindObjectsSortMode.None);

        foreach (var enemy in enemies)
        {
            enemy.kill();
        }

        DemonAI[] demonEnemies = FindObjectsByType<DemonAI>(FindObjectsSortMode.None);

        foreach (var singleDemon in demonEnemies)
        {
            singleDemon.kill();
        }

        HellBornDemonAI[] hellBornEnemies = FindObjectsByType<HellBornDemonAI>(FindObjectsSortMode.None);

        foreach (var singleHellBorn in hellBornEnemies)
        {
            singleHellBorn.kill();
        }

        SkullEnemyAI[] skullEnemies = FindObjectsByType<SkullEnemyAI>(FindObjectsSortMode.None);

        foreach (var singleSkull in skullEnemies)
        {
            singleSkull.kill();
        }

        enemyAI1[] Bosses = FindObjectsByType<enemyAI1>(FindObjectsSortMode.None);

        foreach (var singleboss in Bosses)
        {
            singleboss.kill();
        }
    }
    IEnumerator newScene()
    {
        yield return new WaitForSeconds(0.1f);
        playerPortal = null;
        // Checks if the new level is the final level
        if (currLevel != SceneManager.sceneCountInBuildSettings - 1)
        {
            scoreRound.text = currRound.ToString("f0") + "/" + rounds.ToString("f0");
            StartCoroutine(flashPrompt(preFirstRoundPopup));
            playerPortal = GameObject.FindWithTag("Portal");
            playerPortal.SetActive(false);
        }
        else
        {
            scoreRound.text = "Final";
            StartCoroutine(flashPrompt(finalLevelPopup));
        }
        // Get's components placed in each level
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
        levelMusic = GameObject.FindWithTag("Level Music");
        mainDoor = GameObject.FindWithTag("Gate Door");
        // Sets the player as being inside when they spawn
        player.GetComponent<unifiedPlayerController>().spawnPlayer();
        player.GetComponent<unifiedPlayerController>().hasTorch = false;
        levelMusic.SetActive(true);
        isLoading = !isLoading;
    }

    IEnumerator flashPrompt(GameObject prompt)
    {
        prompt.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        prompt.SetActive(false);
    }

    IEnumerator startUpMenu()
    {
        //yield return new WaitForSeconds(0.001f);
        yield return null;
        statePause();
        menuActive = tutorialPopup;
        menuActive.SetActive(true);
    }
    IEnumerator flashGatePrompt()
    {
        gatePromptIsRunning = true;
        gateAttackedPopup.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        gateAttackedPopup.SetActive(false);
        gatePromptIsRunning = false;
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
        increaseWallet(roundValue);
        gameManager.instance.mainDoor.GetComponent<door>().Close();
        if (currLevel != SceneManager.sceneCountInBuildSettings - 1)
        {
            currRound++;
            scoreRound.text = currRound.ToString("f0") + "/" + rounds.ToString("f0");
        }
        else
        {
            scoreRound.text = "Final";
        }
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
        if (currLevel != SceneManager.sceneCountInBuildSettings - 1)
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

            HellBornDemonAI[] hellBornEnemies = FindObjectsByType<HellBornDemonAI>(FindObjectsSortMode.None);

            foreach (var singleHellBorn in hellBornEnemies)
            {
                singleHellBorn.endRound();
            }

            SkullEnemyAI[] skullEnemies = FindObjectsByType<SkullEnemyAI>(FindObjectsSortMode.None);

            foreach (var singleSkull in skullEnemies)
            {
                singleSkull.kill();
            }
            updateGameGoal(-gameGoalCount);
            mainDoor.GetComponent<door>().Open();
            if (wallet < 0)
                youLose();
        }
        else
        {
            youLose();
        }
        
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
