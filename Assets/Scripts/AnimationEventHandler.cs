using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{

    private Animator ub_animator;

    private void Awake()
    {
        ub_animator = GetComponent<Animator>();
    }

    public void MeleeStart()
    {
        ub_animator.SetBool("ActionInProgress", true);
    }
    public void MeleeEnd()
    {
        ub_animator.ResetTrigger("melee_attack");
        ub_animator.SetBool("ActionInProgress", false);
    }

    public void ClearBiteTrigger()
    {
        ub_animator.ResetTrigger("bite");
    }
}
