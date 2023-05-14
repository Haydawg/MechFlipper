using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<MechController>(out MechController mech))
        {
            if(mech.GetTeam() == MechController.Team.Player)
            {
                GameManager.Instance.MechReachesEnd();
            }
        }
    }
}
