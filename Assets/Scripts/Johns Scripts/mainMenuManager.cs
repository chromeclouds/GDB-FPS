using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
