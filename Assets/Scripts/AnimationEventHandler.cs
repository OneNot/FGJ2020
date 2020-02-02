using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    private Animator ub_animator;
    private EnemyAI enemyAI;
    private PlayerController playerController;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponentInParent<AudioSource>();
        playerController = FindObjectOfType<PlayerController>();
        ub_animator = GetComponent<Animator>();
        enemyAI = GetComponentInParent<EnemyAI>();
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
    public void MeleeSound()
    {
        playerController.PlayMeleeSound();
    }
    public void TryToDoMeleeDamage()
    {
        playerController.TryToDoMeleeDamage();
    }

    public void ClearBiteTrigger()
    {
        ub_animator.ResetTrigger("bite");
    }
    public void BiteAudio()
    {
        audioSource.PlayOneShot(enemyAI.BiteSounds[Random.Range(0, enemyAI.BiteSounds.Length-1)], 0.3f);
    }

    public void TryToDoDamage()
    {
        enemyAI.TryToDoDamage();
    }
}
