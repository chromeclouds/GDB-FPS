using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void newGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void showCaseLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
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
        //LectureEnemyAI[] enemies = FindObjectsByType<LectureEnemyAI>(FindObjectsSortMode.None);

        //foreach (var enemy in enemies)
        //{
        //    enemy.de
        //}

        //DemonAI[] demonEnemies = FindObjectsByType<DemonAI>(FindObjectsSortMode.None);

        //foreach (var singleDemon in demonEnemies)
        //{
        //    Destroy(singleDemon);
        //}
        gameManager.instance.levelTimer.GetComponent<LevelTimer>().ResetTimer();
        gameManager.instance.gameGoalCount = 0; 
        gameManager.instance.updateGameGoalText(0);
        gameManager.instance.player.GetComponent<unifiedPlayerController>().spawnPlayer();
        gameManager.instance.restartRound();
        gameManager.instance.playerIsOutside = false;
        gameManager.instance.stateUnpause();
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
