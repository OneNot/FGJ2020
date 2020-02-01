using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject BulletPrefab;
    public float bulletSpeed = 100f;

    public void Shoot()
    {
        if(PlayerController.UpperBodyMode == PlayerController.UpperBodyModes.Pistol)
        {
            Instantiate(BulletPrefab, transform.position, transform.rotation).GetComponent<BulletController>().SetParams(bulletSpeed, 5f);
        }
    }
}
