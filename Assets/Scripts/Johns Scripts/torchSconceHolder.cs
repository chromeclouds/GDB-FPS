using UnityEngine;

public class torchHolder : MonoBehaviour
{
    [SerializeField] private GameObject defaultTorch;
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
    bool GivePlayerTorch()
    {
        defaultTorch.SetActive(false);
        return isHardMode;
    }

    void RetrieveTorch()
    {
        defaultTorch.SetActive(true);
    }
}
