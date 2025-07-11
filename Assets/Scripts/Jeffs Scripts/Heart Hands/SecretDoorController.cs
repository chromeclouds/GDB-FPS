using UnityEngine;

public class SecretDoorController : MonoBehaviour
{
    public int heartsRequired = 2;
    private bool isOpen = false;

    private void OnEnable()
    {
        HeartTrackerManager.Instance.HeartUpdate += CheckUnlock;

    }

    private void OnDisable()
    {
        HeartTrackerManager.Instance.HeartUpdate -= CheckUnlock;

    }

    private void CheckUnlock()
    {
        if(!isOpen && HeartTrackerManager.Instance.GetDestroyedCount() >= heartsRequired)
        {
            OpenDoor();
            isOpen = true;

        }
    }
    private void OpenDoor()
    {
        gameObject.SetActive(false);
    }
}
