using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float MaxHealth;
    private float CurrentHealth;

    private NavMeshAgent nma;
    private NavMeshObstacle navMeshObstacle;
    private GameObject playerGO;
    private Animator lb_animator, ub_animator;

    private void Awake()
    {
        nma = GetComponent<NavMeshAgent>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        navMeshObstacle.enabled = false;
        lb_animator = transform.Find("LowerBody").GetComponentInChildren<Animator>();
        ub_animator = transform.Find("UpperBody").GetComponentInChildren<Animator>();
        playerGO = GameObject.FindGameObjectWithTag("Player");
        lb_animator.SetBool("walking", false);
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //purkka koska stopping distance ei toimi >:C
        if(Vector3.Distance(transform.position, playerGO.transform.position) <= 15)
        {
            nma.enabled = false;
            navMeshObstacle.enabled = true;
        }
        else
        {
            navMeshObstacle.enabled = false;
            nma.enabled = true;
        }

        if(nma.enabled == true && !nma.pathPending)
            nma.SetDestination(playerGO.transform.position);

        
        if(nma.enabled == true && nma.velocity.magnitude > 0f)
        {
            lb_animator.SetBool("walking", true);
        }
        else
        {
            lb_animator.SetBool("walking", false);
        }

        if(nma.enabled == false || Vector3.Distance(transform.position, playerGO.transform.position) <= nma.stoppingDistance +2f)
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
        print("took " + dmg + "dmg");
        if(CurrentHealth <= 0f)
            Die();

    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
