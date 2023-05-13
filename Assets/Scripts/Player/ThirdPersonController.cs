using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Cotrolls")]
    [SerializeField] Camera playerCamera;
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float maxRunSpeed;
    [SerializeField] private float maxWalkSpeed;
    [SerializeField] private float rotateSpeed;

    public Rigidbody rb;
    public bool freeze;
    public bool activeGrapple;
    public bool enableMovementOnNextTouch;
    public Animator anim;

    public Transform backslot;
    public DroppedPart part;
    // Start is called before the first frame update
    void Start()
    {
        playerCamera= Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(!activeGrapple)
        {
            Movement();

        }

        if (freeze)
        {
            rb.velocity= Vector3.zero;
        }
    }
    void Movement()
    {
        Vector3 moveDirection = Vector3.zero;
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        if (xMove != 0 | zMove != 0)
        {
            moveDirection = (xMove * new Vector3(playerCamera.transform.right.x, transform.right.y, playerCamera.transform.right.z) + (zMove * new Vector3(playerCamera.transform.forward.x, transform.forward.y, playerCamera.transform.forward.z)));
            moveDirection.Normalize();
            transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(transform.forward), Quaternion.LookRotation(moveDirection), rotateSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.LeftShift) & speed < maxRunSpeed)
            {
                speed += acceleration;
            }
            else
            {
                if (speed > maxWalkSpeed)
                {
                    speed -= acceleration;
                }
                else
                {
                    speed += acceleration;
                }
            }
        }
        else if (xMove < 0)
        {
            speed = 1;
        }
        else
        {
            if (speed > 0)
            {
                speed -= acceleration * 2;
            }
        }
        if (speed <= 0)
        {
            anim.SetBool("Is Moving", false);
        }
        else
        {
            anim.SetBool("Is Moving", true);
        }


        rb.velocity = new Vector3(moveDirection.x * speed  , -9.81f, moveDirection.z * speed );
        anim.SetFloat("Speed", speed);

    }

    public void JumpToPosition(Vector3 targetPosition, float height)
    {
        activeGrapple = true;
        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, height);
        Invoke(nameof(SetVelocity), 0.1f);
    }
    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;
    }
    // equation to account for jump velocity so that player hits the target
    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) 
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    public void ResetRestrictions()
    {
        activeGrapple = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch= false;
            ResetRestrictions();
            GetComponent<Grappling>().StopGrapple();
        }
    }
}
