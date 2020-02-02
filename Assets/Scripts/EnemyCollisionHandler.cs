using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionHandler : MonoBehaviour
{
    public bool criticalArea;
    private EnemyAI parent;
    private EnemyAIRobot parentR;

    private void Awake()
    {
        parent = GetComponentInParent<EnemyAI>();
        parentR = GetComponentInParent<EnemyAIRobot>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if(parent)
            parent.TakeDamage(other.gameObject.GetComponent<BulletController>().dmg * (criticalArea ? 2 : 1));
        else
            parentR.TakeDamage(other.gameObject.GetComponent<BulletController>().dmg * (criticalArea ? 2 : 1));

        Destroy(other.gameObject);
    }
}
