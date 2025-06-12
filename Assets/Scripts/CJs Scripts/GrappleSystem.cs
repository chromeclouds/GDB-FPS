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

    //private SpringJoint spring;




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

            // testing springjoint
            //spring = gameObject.AddComponent<SpringJoint>();
            //spring.autoConfigureConnectedAnchor = false;
            //spring.connectedAnchor = anchorPoint;

            //float distanceFromPoint = Vector3.Distance(transform.position, anchorPoint);

            //spring.maxDistance = distanceFromPoint * 0.8f;
            //spring.minDistance = distanceFromPoint * 0.25f;

            //spring.spring = 100f;
            //spring.damper = 5f;
            //spring.massScale = 1f;

            grappleRope.positionCount = 2;
            //rb.useGravity = false;
        }
    }

    void StopGrapple()
    {
        isGrappling = false;
        grappleRope.positionCount = 0;

        // destroy springjoint
        //Destroy(spring);

        rb.useGravity = true;
    }

    void UpdateGrapple()
    {
        Vector3 dir = (anchorPoint - transform.position).normalized;
        //rb.AddForce(dir * grappleAccel, ForceMode.Acceleration);

        //attempt at fixing using "MoveTowards"
        transform.position = Vector3.MoveTowards(transform.position, anchorPoint, grappleSpeed * Time.deltaTime);
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
