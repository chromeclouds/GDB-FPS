using UnityEngine;

public class nextLevelPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        unifiedPlayerController player = other.GetComponent<unifiedPlayerController>();

        if (player != null)
        {
            gameManager.instance.LoadNextLevel();
        }
    }
}
