using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class mainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject Options;
    [SerializeField] GameObject Credits;

    GameObject currMenu;

    void Start()
    {
        currMenu = mainMenu;
        currMenu.SetActive(true);
    }

    public void main()
    {
        currMenu.SetActive(false);
        currMenu = mainMenu;
        currMenu.SetActive(true);
    }

    public void newGame()
    {
        StartCoroutine(loadNewGame());
    }
    public void options()
    {
        currMenu.SetActive(false);
        currMenu = Options;
        currMenu.SetActive(true);
    }
    public void credits()
    {
        currMenu.SetActive(false);
        currMenu = Credits;
        currMenu.SetActive(true);
    }
    public void quit()
    {
#if !UNITY_EDITOR
            Application.Quit();
#else
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    
    public void showCaseLevel()
    {
        StartCoroutine(loadShowCase());
    }

    IEnumerator loadShowCase()
    {
        yield return new WaitForSeconds(100f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    IEnumerator loadNewGame()
    {
        yield return new WaitForSeconds(100f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
