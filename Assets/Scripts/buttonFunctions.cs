using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
   public void resume()
    {
        gameManager.instance.stateUnpause();
    }

    public void restart()
    {
        SceneManager.MoveGameObjectToScene(gameManager.instance.player, SceneManager.GetActiveScene());
        SceneManager.MoveGameObjectToScene(gameManager.instance.transform.root.gameObject, SceneManager.GetActiveScene());
        SceneManager.LoadScene(0);
        gameManager.instance.stateUnpause();
    }
    public void restartRound()
    {
        //if(SceneManager.GetActiveScene().buildIndex == 0)
        //{

        //}
        //gameManager.instance.player.GetComponent<unifiedPlayerController>().resetHealth();
        //gameManager.instance.levelTimer.GetComponent<LevelTimer>().ResetTimer();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //gameManager.instance.stateUnpause();
    }
    public void Continue()
    {
        gameManager.instance.player.GetComponent<unifiedPlayerController>().resetHealth();
        gameManager.instance.stateUnpause();
    }

    public void quit()
    {
    #if !UNITY_EDITOR
            Application.Quit();
    #else 
            UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }

    public void respawnPlayer()
    {
       // gameManager.instance.playerScript.spawnPlayer();
        gameManager.instance.stateUnpause();
    }

    public void loadLevel(int lvl)
    {
        SceneManager.LoadScene(lvl);
        gameManager.instance.stateUnpause();
    }
}
