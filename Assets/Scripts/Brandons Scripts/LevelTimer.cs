using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] TMP_Text timer;    // Place Timer Text here
    
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

        // Slowly increase time as the game runs
        levelTime += Time.deltaTime;

        // How many minutes have passed
        int minutes = Mathf.FloorToInt(levelTime / 60f);
        // How many seconds have passed
        int seconds = Mathf.FloorToInt(levelTime % 60f);

        // Update the UI text to show the current time in 00:00 format
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Stop the timer
    void StopTimer()
    { 
        isRunning = false;
    }

    // Reset the timer
    void ResetTimer()
    { 
        levelTime = 0f;
        isRunning = true;
    }
}
