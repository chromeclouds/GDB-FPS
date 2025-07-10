using UnityEngine;
using UnityEngine.AI;

public class Companion : MonoBehaviour, IOpen
{
    public NavMeshAgent agent;
    public Transform player;
    public Transform shootPos;
    public GameObject bullet;

    Vector3 dest;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       dest = player.position;
       agent.destination = dest;
    }


}
