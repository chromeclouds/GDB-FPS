using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 100, 0);  
    public float floatUpAndDown = 0.5f;  
    public float frequency = 1f;    

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * floatUpAndDown;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
