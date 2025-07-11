using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;     // This is the enemy we're going to spawn
    [SerializeField] int spawnAmount;            // How many enemies we want to spawn total
    [SerializeField] float spawnIntreval;        // How long to wait between each spawn (in seconds)
    [SerializeField] bool triggerMode;           // Wait for trigger to be called
    [SerializeField] Transform[] spawnPOS;

    [SerializeField] GameObject lowlyDemonPrefab;
    [SerializeField] GameObject skullPrefab;
    [SerializeField] bool spawnSkullPairs;
    [SerializeField] Transform[] patrolPathForSkulls;

    // Keeps track of how many enemies we've spawned so far
    int spawnCount;

    // Prevents double starting on game launch
    bool isSpawning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // If not using tigger it starts automatically
        if (!triggerMode)
        {
            gameManager.instance.updateGameGoal(spawnAmount);
            StartCoroutine(SpawnEnemies(spawnAmount));
        }
    }

    // Create call method if gameManager is using trigger
    public void TriggerSpawn(int spawnMult)
    {
        int tempSpawnAmount = spawnAmount * spawnMult;

        if (!isSpawning)
        {
            gameManager.instance.updateGameGoal(tempSpawnAmount);
            StartCoroutine(SpawnEnemies(tempSpawnAmount));
        }
    }

    // Spawns enemies one at a time with a delay between each
    IEnumerator SpawnEnemies(int spawns)
    {
        // tells the script we're already spawning so it doesn t double up
        isSpawning = true;

        // Loop until we've spawned the amount we want
        while (spawnCount < spawns)
        {
            int arrayPOS = Random.Range(0, spawnPOS.Length);

            if (spawnSkullPairs && skullPrefab != null && lowlyDemonPrefab != null)
            {
                // Spawn Skull
                GameObject spawnedSkull = Instantiate(skullPrefab, spawnPOS[arrayPOS].position, spawnPOS[arrayPOS].rotation);

                // Assign patrol points to SkullEnemyAI
                SkullEnemyAI skullScript = spawnedSkull.GetComponent<SkullEnemyAI>();
                if (skullScript != null && patrolPathForSkulls.Length > 0)
                {
                    skullScript.patrolPoints = patrolPathForSkulls;
                }

                // Spawn Lowly Demon nearby
                Vector3 demonSpawnOffset = spawnPOS[arrayPOS].position + new Vector3(1.5f, 0, 0); // Offset a bit
                GameObject spawnedDemon = Instantiate(lowlyDemonPrefab, demonSpawnOffset, spawnPOS[arrayPOS].rotation
                );

                // Link Demon to Skull
                LectureEnemyAI demonScript = spawnedDemon.GetComponent<LectureEnemyAI>();
                if (demonScript != null)
                {
                    demonScript.skullTarget = spawnedSkull.transform;
                }
            }
            else
            {
                // Regular enemy spawn
                Instantiate(enemyPrefab, spawnPOS[arrayPOS].position, spawnPOS[arrayPOS].rotation);
            }

            spawnCount++;
            yield return new WaitForSeconds(spawnIntreval);
        }

        isSpawning = false;
        spawnCount = 0;
    }
}
