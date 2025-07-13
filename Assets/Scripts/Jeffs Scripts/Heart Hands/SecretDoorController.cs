using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SecretDoorController : MonoBehaviour
{
    public int heartsRequired = 2;
    public GameObject doorMesh;
    public GameObject popupUI;
    public float popupDuration = 3f;

    private bool isOpen = false;


    private void OnEnable()
    {
        HeartTrackerManager.Instance.HeartUpdate += CheckUnlock;

    }

    private void OnDisable()
    {
        if (HeartTrackerManager.Instance != null)
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
        if (popupUI != null)
            StartCoroutine(ShowPopup());
        if (doorMesh != null)
            doorMesh.SetActive(false);
        
    }

    private IEnumerator ShowPopup()
    {
        popupUI.SetActive(true);
        yield return new WaitForSeconds(popupDuration);
        popupUI.SetActive(false);
    }
}
