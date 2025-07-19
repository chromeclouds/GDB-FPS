using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{
    public float scrollSpeed = 30f;

    void Update()
    {
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene(0); 
        }
    }
}