using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float MaxHealth;
    private float CurrentHealth;

    private NavMeshAgent nma;
    private GameObject playerGO;
    private Animator lb_animator, ub_animator;

    private void Awake()
    {
        nma = GetComponent<NavMeshAgent>();
        lb_animator = transform.Find("LowerBody").GetComponentInChildren<Animator>();
        ub_animator = transform.Find("UpperBody").GetComponentInChildren<Animator>();
        playerGO = GameObject.FindGameObjectWithTag("Player");
        lb_animator.SetBool("walking", false);
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(!nma.pathPending)
            nma.SetDestination(playerGO.transform.position);

        
        if(nma.velocity.magnitude > 0f)
        {
            lb_animator.SetBool("walking", true);
        }
        else
        {
            lb_animator.SetBool("walking", false);
        }

        if(Vector3.Distance(transform.position, playerGO.transform.position) <= nma.stoppingDistance +2f)
            Bite();
    }

    private void Bite()
    {
        //if not already palying bite anim
        if(!ub_animator.GetCurrentAnimatorStateInfo(0).IsName("bite"))
            ub_animator.SetTrigger("bite");
    }

    public void TakeDamage(float dmg)
    {
        CurrentHealth -= dmg;
        if(CurrentHealth <= 0f)
            Die();

    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
