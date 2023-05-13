using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    private ThirdPersonController player;
    public Transform cam;
    public Transform grappleTip;
    public LayerMask grappleable;
    public LineRenderer lineRenderer;

    public float maxGrappleDistance;
    private Vector3 grapplePoint;

    public float grappleTime;
    public float grappleDelayTime;
    public float overshootYAxis;

    private float grappleCooldown;
    private float grappleCooldownTimer;

    private bool isGrappling;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<ThirdPersonController>();
        isGrappling = false;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartGrapple();
        }
        if(grappleCooldownTimer > 0)
        {
            grappleCooldownTimer -= Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        if(isGrappling)
        {
            lineRenderer.SetPosition(0, grappleTip.position);
        }
    }

    public void StartGrapple()
    {
        if (grappleCooldownTimer > 0)
            return;
        isGrappling = true;
        player.freeze = true;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, maxGrappleDistance, grappleable))
        {
            grapplePoint= hit.point;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + ray.direction * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(1, grapplePoint);
    }

    public void ExecuteGrapple()
    {
        player.freeze = false;
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        float grapplePointRelativeYPos = grapplePoint.y + lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        player.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 3f);
    }
    public void StopGrapple()
    {
        isGrappling= false;
        player.freeze = false;
        grappleCooldownTimer = grappleCooldown;
        lineRenderer.enabled = false;

    }

}
