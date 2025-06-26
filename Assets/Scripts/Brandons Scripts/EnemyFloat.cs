using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    // Controls how high the floating effect goes
    public float floatHeight = 0.5f;
    private Vector3 startPos;

    void Start()
    {
        // Records the objects initial local position
        startPos = transform.localPosition;
    }

    void Update()
    {
        // Makes the object float up and down smoothly over time
        transform.localPosition = startPos + new Vector3(0, Mathf.Sin(Time.time * 2f) * floatHeight, 0);
    }
}