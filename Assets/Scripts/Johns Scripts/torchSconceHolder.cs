using UnityEngine;

public class torchHolder : MonoBehaviour
{
    [SerializeField] private GameObject defaultTorch;

    public Transform torchLocation;
    private GameObject currentTorch;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (defaultTorch != null)
        {
            GameObject spawnedTorch = Instantiate(defaultTorch);
            placeTorch(spawnedTorch);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void placeTorch(GameObject torch)
    {
        if (currentTorch != null)
        {
            Destroy(currentTorch);
        }

        currentTorch = torch;

        currentTorch.transform.SetParent(torchLocation);
        currentTorch.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}
