using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GlobalHeartManager : MonoBehaviour
{
    public static GlobalHeartManager Instance;

    private int totalHearts = 0;
    private int destroyedHearts = 0;

    private List<HeartDestructible> registeredHearts = new List<HeartDestructible>();
    private TMP_Text uiText;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        //should find ui text dynamically
     //   uiText = gameManager.instance?.gameGoalCount;
     //   UpdateUIText();
    }

    public void RegisterHeart(HeartDestructible heart)
    {
        if (!registeredHearts.Contains(heart))
        {
            registeredHearts.Add(heart);
            totalHearts++;
     //       UpdateUIText();
        }
    }

    public void RegisterHeartDestroyed(HeartDestructible heart)
    {
        if (registeredHearts.Contains(heart))
        {
            destroyedHearts++;
       //     UpdateUIText();
        }
    }

    public int getDestroyedCount() => destroyedHearts;
    public int GetTotalHearts() => totalHearts;

   /* private void UpdateUIText()
    {
        if (uiText == null)
            uiText = gameManager.instance?.gameGoalCount;
        if (uiText != null)
            uiText.text = $"{destroyedHearts}/{totalHearts}";
    }
   */
}
