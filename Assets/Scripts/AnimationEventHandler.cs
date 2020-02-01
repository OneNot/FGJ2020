using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public void MeleeStart()
    {
        PlayerController.ub_animator.SetBool("ActionInProgress", true);
    }
    public void MeleeEnd()
    {
        PlayerController.ub_animator.ResetTrigger("melee_attack");
        PlayerController.ub_animator.SetBool("ActionInProgress", false);
    }
}
