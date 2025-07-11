using UnityEngine;

public class SkullRotate : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 100, 0);
    public float floatUpAndDown = 0.5f;
    public float frequency = 1f;

    private Vector3 startPos;
    public Transform player;
    public float faceTargetSpeed = 5f;
    private void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        FacePlayer();
        transform.Rotate(rotationSpeed * Time.deltaTime);
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * floatUpAndDown;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
    public void FacePlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir);
 
        rot *= Quaternion.Euler(0, 90f, 19f);  

        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed); ;
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }
}
