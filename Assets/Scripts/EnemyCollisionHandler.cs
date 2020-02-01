using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionHandler : MonoBehaviour
{
    private EnemyAI parent;

    private void Awake()
    {
        parent = GetComponentInParent<EnemyAI>();
    }

    private void OnCollisionEnter(Collision other)
    {
        parent.TakeDamage(other.gameObject.GetComponent<BulletController>().dmg);
        Destroy(other.gameObject);
    }
}
