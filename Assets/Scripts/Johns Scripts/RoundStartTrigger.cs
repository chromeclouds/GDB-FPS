using UnityEngine;

public class RoundStartTrigger : MonoBehaviour
{
    bool playerIsOutside;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerIsOutside = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        unifiedPlayerController player = other.GetComponent<unifiedPlayerController>();

        if(!playerIsOutside && player.hasTorch)
        {
            gameManager.instance.StartRound();
        }
        playerIsOutside = !playerIsOutside;
    }
}
