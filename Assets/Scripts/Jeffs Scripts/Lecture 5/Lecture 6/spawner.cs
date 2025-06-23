using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] int numToSpawn;
    [SerializeField] int spawnRate;
    [SerializeField] Transform[] spawnPOS;

    float spawnTimer;
    int spawnCount;
    bool startSpawning;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager.instance.updateGameGoal(numToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning)
        {
            spawnTimer += Time.deltaTime;
            if(spawnTimer >= spawnRate && spawnCount < numToSpawn)
            {
                spawn();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }

    void spawn()
    {
        int arrayPOS = Random.Range(0, spawnPOS.Length);

        Instantiate(objectToSpawn, spawnPOS[arrayPOS].transform.position, spawnPOS[arrayPOS].transform.rotation);
        spawnCount++;
        spawnTimer = 0;
    }
}
