using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Limb : MonoBehaviour
{
    [SerializeField] private DroppedPart droppedPart;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<ThirdPersonController>(out ThirdPersonController player))
        {
            Instantiate(droppedPart, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
