using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class GlobalHeartManager : MonoBehaviour
{
    public static GlobalHeartManager Instance;

    private int totalHearts = 0;
    private int destroyedHearts = 0;

    private List<HeartDestructible> registeredHearts = new List<HeartDestructible>();

    public delegate void OnHeartDestroyed();
    public event OnHeartDestroyed HeartDestroyedEvent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    public void RegisterHeart(HeartDestructible heart)
    {
        if (!registeredHearts.Contains(heart))
        {
            registeredHearts.Add(heart);
            totalHearts++;
            UpdateUI();
        }
    }


    public void RegisterHeartDestroyed(HeartDestructible heart)
    {
        if (registeredHearts.Contains(heart))
        {
            destroyedHearts++;
            UpdateUI();
            HeartDestroyedEvent?.Invoke();
        }
    }

    public int GetDestroyedCount() => destroyedHearts;
    public int GetTotalHearts() => totalHearts;

    private void UpdateUI()
    {
        if (gameManager.instance != null && gameManager.instance.heartCountText != null)
        {
            gameManager.instance.heartCountText.text = $"{destroyedHearts}/{totalHearts}";
        }
    }
   
}
