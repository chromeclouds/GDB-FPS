using UnityEngine;

public class door : MonoBehaviour 
{

    [SerializeField] GameObject doorModel;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        IOpen open = other.GetComponent<IOpen>();

        if (open != null)
        {
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
        

        IOpen open = other.GetComponent<IOpen>();
        if(open!= null)
        {
            doorModel.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
