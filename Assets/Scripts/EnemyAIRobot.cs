using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIRobot : MonoBehaviour
{
    public float MaxHealth;
    private float CurrentHealth;

    private NavMeshAgent nma;
    private NavMeshObstacle navMeshObstacle;
    private GameObject playerGO;
    private ParticleSystem particleSystem;
    private float timer;

    private void Awake()
    {
        nma = GetComponent<NavMeshAgent>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        navMeshObstacle.enabled = false;
        playerGO = GameObject.FindGameObjectWithTag("Player");
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

        if(nma.enabled == false || Vector3.Distance(transform.position, playerGO.transform.position) <= nma.stoppingDistance +2f)
            Shoot();
    }

    private void Shoot()
    {
        if(Time.realtimeSinceStartup - timer > 0.6f)
        {
            particleSystem.Play();
            timer = Time.realtimeSinceStartup;
        }
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
