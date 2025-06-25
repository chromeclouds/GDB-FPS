using UnityEngine;

public class RopeSwing : MonoBehaviour
{
    public LineRenderer swingRope;
    private Vector3 swingPoint;
    public LayerMask whatIsSwingable;
    public Transform swingStartPoint, Camera;
    public float maxDistance;
    

    private void Awake()
    {
        swingRope = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartSwing();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            StopSwing();
        }
    }

    void StartSwing()
    {
        //RaycastHit hit;
        //if (Physics.Raycast(Camera.position, Camera.forward, out hit, maxDistance, whatIsSwingable));
    }

    void StopSwing()
    {

    }
}
