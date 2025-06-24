using UnityEngine;

public class GrappleSystem : MonoBehaviour
{
    public float maxGrappleDist;
    private Vector3 anchorPoint;
    public LineRenderer grappleRope;
    public float grappleAccel;
    private bool isGrappling;
    public LayerMask grappleLayer;
    public Transform grappleStart;
    public Transform Camera;
    public float grappleSpeed;
    public float speed;
    public CharacterController controller;





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
            

            grappleRope.positionCount = 2;
        }
    }

    void StopGrapple()
    {
        isGrappling = false;
        grappleRope.positionCount = 0;
    }

    void UpdateGrapple()
    {
        Vector3 dir = anchorPoint - transform.position;
        float dist = dir.magnitude;
        Vector3 direction = dir.normalized;

        if (dist < 1f)
        {
            StopGrapple();
            return;
        }

        Vector3 move = dir.normalized * grappleSpeed * Time.deltaTime;
        controller.Move(move);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Grapple"))
        {
            StartGrapple();
        }
       

        if (Input.GetButtonUp("Grapple"))
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
