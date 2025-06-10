using UnityEngine;

public class GrappleSystem : MonoBehaviour
{
    public float maxGrappleDist;
    private Vector3 anchorPoint;
    public LineRenderer grappleRope;
    public float grappleAccel;
    public Rigidbody rb;
    private bool isGrappling;
    public LayerMask grappleLayer;
    public Transform grappleStart;
    public Transform Camera;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void StartGrapple()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(Camera.position, Camera.forward, out hit, maxGrappleDist, grappleLayer))
        {
            Debug.Log(hit.collider.name);

            anchorPoint = hit.point;
            isGrappling = true;
            grappleRope.positionCount = 2;
            rb.useGravity = false;
        }
    }

    void StopGrapple()
    {
        isGrappling = false;
        grappleRope.positionCount = 0;
        rb.useGravity = true;
    }

    void UpdateGrapple()
    {
        Vector3 dir = (anchorPoint - transform.position).normalized;
        rb.AddForce(dir * grappleAccel, ForceMode.Acceleration);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Q"))
        {
            StartGrapple();
        }
       

        if (Input.GetButtonUp("Q"))
        {
            StopGrapple();
        }
       

        if (isGrappling)
        {
            grappleRope.SetPositions(new Vector3[] { grappleStart.position, anchorPoint });
        }
      
    }
}
