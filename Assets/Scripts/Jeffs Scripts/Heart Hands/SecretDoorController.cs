using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SecretDoorController : MonoBehaviour
{
    public int heartsRequired = 2;
    public GameObject doorMesh;
    private GameObject popupUI;
    public float popupDuration = 3f;

    private bool isOpen = false;

    private IEnumerator WaitForGlobalHeartManager()
    {
        while (GlobalHeartManager.Instance == null) yield return null;
        GlobalHeartManager.Instance.HeartDestroyedEvent += CheckUnlock;
        CheckUnlock();
    }

    private void OnEnable()
    {
        popupUI = gameManager.instance?.secretDoorPopupUI;

        StartCoroutine(WaitForGlobalHeartManager());

    }

    private void OnDisable()
    {
        if (GlobalHeartManager.Instance != null) 
            GlobalHeartManager.Instance.HeartDestroyedEvent -= CheckUnlock;
    }

    private void CheckUnlock()
    {
        if(!isOpen && GlobalHeartManager.Instance.GetDestroyedCount() >= heartsRequired)
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
