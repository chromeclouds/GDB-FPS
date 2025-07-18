using UnityEngine;

public class insideDetector : MonoBehaviour
{

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager.instance.playerIsOutside = false;
        if (gameManager.instance.mainDoor != null)
        gameManager.instance.mainDoor.GetComponent<door>().Open();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger || !other.CompareTag("Player"))
            return;

        gameManager.instance.playerIsOutside = false;
        if (gameManager.instance.mainDoor != null)
            gameManager.instance.mainDoor.GetComponent<door>().Open();
    }
}
