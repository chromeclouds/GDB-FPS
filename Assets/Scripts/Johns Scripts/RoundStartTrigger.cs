using UnityEngine;

public class RoundStartTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        unifiedPlayerController player = other.GetComponent<unifiedPlayerController>();

        if(!gameManager.instance.playerIsOutside && player.hasTorch)
        {
            gameManager.instance.StartRound();
        }
        gameManager.instance.playerIsOutside = true;
    }
}
