using UnityEngine;

public class insideDetector : MonoBehaviour
{

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager.instance.playerIsOutside = false;
        gameManager.instance.mainDoor.GetComponent<door>().Open();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        gameManager.instance.playerIsOutside = false;
        gameManager.instance.mainDoor.GetComponent<door>().Open();
    }
}
