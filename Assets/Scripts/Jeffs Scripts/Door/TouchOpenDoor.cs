using UnityEngine;

public class TouchOpenDoor : MonoBehaviour
{

    [SerializeField] GameObject doorModel;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = doorModel.GetComponent<MeshRenderer>();
    }

   

    private void OnTriggerEnter(Collider other)
    {
        // Only open the door for the player
        if (!other.CompareTag("Player")) return;

        IOpen open = other.GetComponent<IOpen>();
        if (open != null && meshRenderer != null)
        {
            meshRenderer.enabled = false;
            doorModel.SetActive(false);
        }
        //commented out zacks edit just to get door to open for time being.
        //this needs to be set up so that doors are locked during rounds
        //unlocked after all enemies are dead and waiting for new round to start
        //should start new round after player enters castle and exits
        //doors lock behind, start round.

        /*
        if (gameManager.instance.gameGoalCount <= 0)
        {
            doorModel.SetActive(false);
        }
        */
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        IOpen open = other.GetComponent<IOpen>();
        if (open != null && meshRenderer != null)
        {
            meshRenderer.enabled = true;
            doorModel.SetActive(true);
        }
    }
}
