using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventUpward : MonoBehaviour
{
   public void Fire()
    {
        GetComponentInParent<MechController>().Fire();
    }
}
