using UnityEngine;
using System.Collections;

public class TreePatrolSpawner : MonoBehaviour
{
    
    [SerializeField] private GameObject skullPrefab;
    [SerializeField] private GameObject lowlyDemonPrefab;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private int spawnLimit = 10;
    [SerializeField] private bool infiniteSpawning = true;

    [SerializeField] private float initialSpawnDelayMin = 10f;
    [SerializeField] private float initialSpawnDelayMax = 25f;
    [SerializeField] private float spawnIntervalMin = 10f;
    [SerializeField] private float spawnIntervalMax = 14f;

    [SerializeField] private Transform gateTarget;

    private bool isTreeAlive = true;
    private int spawnCount = 0;
 
    private void Start()
    {
        // Validate required references
        if (skullPrefab == null || patrolPoints == null || patrolPoints.Length == 0)
        {
            return;
        }

        // Randomized interval per tree to add variation
        // spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);

        // Random initial delay before the first patrol
        float randomDelay = Random.Range(initialSpawnDelayMin, initialSpawnDelayMax);
        StartCoroutine(DelayedSpawnStart(randomDelay));
    }

    private IEnumerator DelayedSpawnStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(SpawnPatrols());
    }

    private IEnumerator SpawnPatrols()
    {
        while (isTreeAlive && (infiniteSpawning || spawnCount < spawnLimit))
        {
            SpawnSkullAndFollowers();
            spawnCount++;

            // Randomize delay for the next spawn
            float randomDelay = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(randomDelay);
        }
    }

    private void SpawnSkullAndFollowers()
    {
        Vector3 spawnPos = transform.position + Vector3.up * 2f; // Slightly above tree base
        Quaternion spawnRot = transform.rotation;

        // Spawn Skull
        GameObject skull = Instantiate(skullPrefab, spawnPos, spawnRot);
        SkullEnemyAI skullAI = skull.GetComponent<SkullEnemyAI>();
        if (skullAI != null)
        {
            skullAI.AssignGateTarget(gateTarget);
        }

        // Spawn Lowly Demon followers
        if (lowlyDemonPrefab != null)
        {
            // Spawn 2 lowly demons with different offsets
            Vector3[] positionOffsets = { Vector3.right * 2f, Vector3.left * 2f };
            Vector3[] followOffsets = { new Vector3(0.75f, 0, -0.5f), new Vector3(-0.75f, 0, -0.5f) };

            for (int i = 0; i < 2; i++)
            {
                Vector3 spawnPosition = spawnPos + positionOffsets[i];
                StartCoroutine(SpawnFollowerWithDelay(skull.transform, skullAI, spawnPosition, spawnRot, followOffsets[i]));
            }
        }
    }

    IEnumerator SpawnFollowerWithDelay(Transform skullTransform, SkullEnemyAI skullAI, Vector3 pos, Quaternion rot, Vector3 followOffset)
    {
        yield return new WaitForSeconds(0.05f);

        GameObject demon = Instantiate(lowlyDemonPrefab, pos, rot);
        LowlyDemonAI demonAI = demon.GetComponent<LowlyDemonAI>();
        if (demonAI != null)
        {
            demonAI.skullTarget = skullTransform;
            demonAI.SendMessage("SetFollowOffset", followOffset, SendMessageOptions.DontRequireReceiver);

            if (skullAI != null)
                skullAI.lowlyDemons.Add(demonAI);
        }
    }

    // Called when the tree dies
    public void StopSpawning()
    {
        isTreeAlive = false;
    }
}
