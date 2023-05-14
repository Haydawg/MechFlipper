using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class RepairBench : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform repairSpot;
    [SerializeField] private float repairTime;
    DroppedPart part;
    private bool hasPart;

    [SerializeField] private string prompt;
    [SerializeField] private string repairPrompt;
    [SerializeField] private string placePrompt;
    public string InteractionPromt => prompt;

    public bool CanInteract => true;

    bool repairing;

    // Start is called before the first frame update
    void Start()
    {


    }


    public bool Interact(Interactor interactor)
    {
        interactor.TryGetComponent<ThirdPersonController>(out ThirdPersonController player);

        if (!repairing)
        {

            if (hasPart)
            {
                if (player.part != null) { return false; }
                player.part = part;
                part.transform.parent = player.backslot;
                part.transform.localPosition = Vector3.zero;
                part = null;
                hasPart = false;
                return true;
            }
            else
            {
                if (player.part == null) { return false; }
                part = player.part;
                part.transform.parent = repairSpot;
                part.transform.localPosition = Vector3.zero;
                part.repaired = true;
                player.part = null;
                hasPart = true;
                StartCoroutine(Repairing(repairTime));
                return true;
            }
        }
        else
        {
            
            return false;
        }
    }

    IEnumerator Repairing(float repairTime)
    {
        repairing = true;
        prompt = repairPrompt;
        yield return new WaitForSeconds(repairTime);
        prompt = placePrompt;
        repairing = false;
    }
}
