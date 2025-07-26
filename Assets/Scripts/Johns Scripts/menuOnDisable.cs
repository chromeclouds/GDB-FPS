using UnityEngine;

public class menuOnDisable : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] bool isActive;
    void OnDisable()
    {
        if(isActive)
            menu.SetActive(true);
        else
            menu.SetActive(false);
    }
}
