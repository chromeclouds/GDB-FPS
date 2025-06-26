using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] TMP_Text timer;    // Place Timer Text here
    [SerializeField] bool isTimeLimit;    // Place Timer Text here
    [SerializeField] float startingTime;    // Place Timer Text here
    
    float levelTime;                    // Keeps track of how much time has passed during level
    bool isRunning;                     // Checks if the timer is running

    void Start()
    {
        ResetTimer(); // always start fresh when the scene loads
    }
    // Update is called once per frame
    void Update()
    {
        // If the game isn't running stop here
        if (!isRunning) return;

        if (isTimeLimit)
        {
            if(levelTime - Time.deltaTime > 0)
            levelTime -= Time.deltaTime;

            else
            {
                isRunning = false;
                levelTime = 0;
                gameManager.instance.endRound();
            }
        }

        // Slowly increase time as the game runs
        else levelTime += Time.deltaTime;

        // How many minutes have passed
        int minutes = Mathf.FloorToInt(levelTime / 60f);
        // How many seconds have passed
        int seconds = Mathf.FloorToInt(levelTime % 60f);

        // Update the UI text to show the current time in 00:00 format
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Start the timer
    public void StartTimer()
    { 
        isRunning = true;
    }

    // Reset the timer
    void ResetTimer()
    { 
        levelTime = startingTime;
        isRunning = false;
    }
}
