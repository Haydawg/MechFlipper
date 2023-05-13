using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedPart : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] GameObject sphere;
    private bool onFloor;

    public bool repaired;
    private void Start()
    {
        onFloor = true;
    }
    private void Update()
    {
        if (onFloor)
        {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.Self);
        }
        else
        {

        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<ThirdPersonController>(out ThirdPersonController player))
        {
            onFloor = false;
            sphere.SetActive(false);
            transform.parent = player.backslot;
            GetComponent<Rigidbody>().isKinematic= true;
            transform.localPosition = Vector3.zero;
            player.part = this;
        }
    }
}
