using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class EnemySpawn : MonoBehaviour
{ 
    [SerializeField] GameObject enemyPrefab;     // This is the enemy we're going to spawn
    [SerializeField] int spawnAmount;            // How many enemies we want to spawn total
    [SerializeField] float spawnIntreval;        // How long to wait between each spawn (in seconds)
    [SerializeField] Transform spawnPoint;       // Where to spawn the enemies from (can leave empty and it'll just use the spawner's position)
    [SerializeField] bool triggerMode;           // Wait for trigger to be called

    // Keeps track of how many enemies we've spawned so far
    int spawnCount;

    // Prevents double starting on game launch
    bool isSpawning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If we didn’t assign a spawn point in the inspector, just use this object’s position
        if (spawnPoint == null)
        {
            // If no spawn point was set, just use the position of this object
            spawnPoint = this.transform;
        }
        
        // If not using tigger it starts automatically
        if (!triggerMode)
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    // Create call method if gameManager is using trigger
    void TriggerSpawn()
    {
        if (!isSpawning)
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    // Spawns enemies one at a time with a delay between each
    IEnumerator SpawnEnemies()
    {
        // tells the script we're already spawning so it doesn’t double up
        isSpawning = true;

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

        // Turn off the spawning flag so this can restart next time
        isSpawning = false;
        spawnCount = 0;
    }
}
