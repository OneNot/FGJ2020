using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionHandler : MonoBehaviour
{
    public bool criticalArea;
    private EnemyAI parent;

    private void Awake()
    {
        parent = GetComponentInParent<EnemyAI>();
    }

    private void OnCollisionEnter(Collision other)
    {
        parent.TakeDamage(other.gameObject.GetComponent<BulletController>().dmg * (criticalArea ? 2 : 1));
        Destroy(other.gameObject);
    }
}
