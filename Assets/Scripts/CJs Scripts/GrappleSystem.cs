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
    public float grappleSpeed;





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

            grappleRope.positionCount = 2;

            
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
        //rb.AddForce(dir * grappleAccel, ForceMode.Acceleration);

        //attempt at overriding playerController during grapple
        float dist = dir.magnitude;
        if (dist < 1f)
        {
            StopGrapple();
            return;
        }

        Vector3 move = dir * grappleSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + move);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartGrapple();
        }
       

        if (Input.GetKeyUp(KeyCode.Q))
        {
            StopGrapple();
        }
       

        if (isGrappling)
        {
            grappleRope.SetPositions(new Vector3[] { grappleStart.position, anchorPoint });
            UpdateGrapple();
        }
      
    }
}
