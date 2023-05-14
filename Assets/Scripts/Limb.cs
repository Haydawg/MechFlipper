using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Limb : MonoBehaviour
{
    [SerializeField] private DroppedPart droppedPart;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;

    bool limbMissing;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<ThirdPersonController>(out ThirdPersonController player))
        {
            Instantiate(droppedPart, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
    }

    public void ChangeMesh(Mesh mesh)
    {
        meshRenderer.sharedMesh = mesh;
    }
}
