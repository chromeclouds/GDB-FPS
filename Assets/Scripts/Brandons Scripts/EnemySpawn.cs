using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;        // This is the enemy we're going to spawn
    [SerializeField] int spawnAmount;               // How many enemies we want to spawn total
    [SerializeField] float spawnIntreval;           // How long to wait between each spawn (in seconds)

    [SerializeField] Transform spawnPoint;          // Where to spawn the enemies from (can leave empty and it'll just use the spawner's position

    int spawnCount;                                 // Keeps track of how many enemies we've spawned so far

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If we didn’t assign a spawn point in the inspector, just use this object’s position
        if (spawnPoint == null)
        {
            // If no spawn point was set, just use the position of this object
            spawnPoint = this.transform;
        }

        // Start the spawn process
        StartCoroutine(SpawnEnemies());
    }

    // Spawns enemies one at a time with a delay between each
    IEnumerator SpawnEnemies()
    {
        // Loop until we've spawned the amount we want
        while (spawnCount < spawnAmount)
        {
            // Make a new enemy at the spawn point
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            // Keep track of how many we've made
            spawnCount++;
            // Wait a few seconds before spawning the next one
            yield return new WaitForSeconds(spawnIntreval);
        }
    }
}
