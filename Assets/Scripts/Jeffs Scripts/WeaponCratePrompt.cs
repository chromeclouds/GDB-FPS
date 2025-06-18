using UnityEngine;

public class WeaponCratePrompt : MonoBehaviour
{
    public GameObject promptUI;
    private Transform player;
    public float displayRange = 3f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        promptUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        if (dist < displayRange)
        {
            promptUI.SetActive(true);
            promptUI.transform.LookAt(Camera.main.transform);
        }
        else
        {
            promptUI.SetActive(false);
        }
    }
}
