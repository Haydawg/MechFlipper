using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionRadius = 0.0f;
    [SerializeField] private LayerMask interactableMask;

    private readonly Collider[] colliders = new Collider[3];
    [SerializeField] private int numFound;
    [SerializeField] private InteractionPromptUI interactionPromptUI;
    private IInteractable interactable;


    private void Update()
    {
        numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionRadius, colliders, interactableMask);

        if(numFound > 0)
        {
            interactable = colliders[0].GetComponent<IInteractable>();
            if(!interactionPromptUI.isDisplayed)
            {
                interactionPromptUI.SetUp(interactable.InteractionPromt);
            }

            if(interactable != null && Input.GetKeyDown(KeyCode.F))
            {
                interactable.Interact(this);
            }
        }
        else
        {
            if(interactable != null)
                interactable = null;
            if (interactionPromptUI.isDisplayed)
                interactionPromptUI.Close();
        }
    }
}

