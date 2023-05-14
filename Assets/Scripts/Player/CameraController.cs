using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float turnspeed = 200.0f;
    [SerializeField] private float height = 3f;
    [SerializeField] private float distamce = 5f;

    [SerializeField] private float maxY;
    [SerializeField] private float minY;

    [SerializeField] private ThirdPersonController playerController;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0,height,distamce);
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion yaw = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnspeed * Time.deltaTime, Vector3.up);
        Quaternion pitch = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * turnspeed * Time.deltaTime, Vector3.right);

        

        offset = yaw * pitch * offset;;
        if (playerController)
        {
            
            transform.position = playerController.transform.position + offset;
            if (offset.y > maxY)
            {
                offset.y = maxY;
            }
            if (offset.y < minY)
            {
                offset.y = minY;
            }
            transform.position = playerController.transform.position + offset;

            transform.LookAt(playerController.transform.position + Vector3.up * 2);
        }
    }
}
