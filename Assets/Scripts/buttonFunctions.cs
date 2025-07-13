using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void newGame()
    {
        StartCoroutine(loadNewGame());
    }
    public void showCaseLevel()
    {
        StartCoroutine(loadShowCase());
    }
    public void resume()
    {
        gameManager.instance.stateUnpause();
    }

    public void restart()
    {
        SceneManager.MoveGameObjectToScene(gameManager.instance.player, SceneManager.GetActiveScene());
        SceneManager.MoveGameObjectToScene(gameManager.instance.transform.root.gameObject, SceneManager.GetActiveScene());
        gameManager.instance.resetTime();
        StartCoroutine(loadRestart());
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
        gameManager.instance.resetTime();
        StartCoroutine(quitGame());
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

    IEnumerator loadShowCase()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    IEnumerator loadNewGame()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    IEnumerator loadRestart()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(0);
    }
    IEnumerator quitGame()
    {
        yield return new WaitForSeconds(0.3f);
#if !UNITY_EDITOR
            Application.Quit();
#else
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
