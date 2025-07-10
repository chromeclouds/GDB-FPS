using UnityEngine;

public class torchHolder : MonoBehaviour
{
    [SerializeField] GameObject defaultTorch;
    [SerializeField] private bool isHardMode;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetDifficulty()
    {
        return isHardMode;
    }
    public bool GivePlayerTorch()
    {
        defaultTorch.SetActive(false);
        gameManager.instance.player.GetComponent<unifiedPlayerController>().hasTorch = true;
        return isHardMode;
    }

    public void RetrieveTorch()
    {
        gameManager.instance.player.GetComponent<unifiedPlayerController>().hasTorch = false;
        defaultTorch.SetActive(true);
    }
}
