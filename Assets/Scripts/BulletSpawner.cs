using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{
    public string name;
    public AudioClip audio;
    public GameObject bulletPrefab;
    public float maxFirerate;
    public float damage;
}

public class BulletSpawner : MonoBehaviour
{
    private AudioSource audioSource;
    public float bulletSpeed = 100f;
    [SerializeField]
    public List<Weapon> weapons;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Shoot(PlayerController.UpperBodyModes mode)
    {
        if(mode == PlayerController.UpperBodyModes.Pistol)
        {
            Weapon w = weapons.Find(x => x.name == "pistol");
            Instantiate(w.bulletPrefab, transform.position, transform.rotation).GetComponent<BulletController>().SetParams(bulletSpeed, 5f, w.damage);
            audioSource.PlayOneShot(w.audio);
        }
        else if(mode == PlayerController.UpperBodyModes.Shotgun)
        {
            Weapon w = weapons.Find(x => x.name == "shotgun");
            for(int i = 0; i < 5; i++)
            {
                Quaternion rot = transform.rotation;
                rot = Quaternion.Euler(rot.eulerAngles.x, Random.Range(rot.eulerAngles.y - 10f, rot.eulerAngles.y + 10f), rot.eulerAngles.z);
                Instantiate(w.bulletPrefab, transform.position, rot).GetComponent<BulletController>().SetParams(bulletSpeed, 5f, w.damage);
            }  
            audioSource.PlayOneShot(w.audio);
        }
        if(mode == PlayerController.UpperBodyModes.Rifle)
        {
            Weapon w = weapons.Find(x => x.name == "rifle");
            Instantiate(w.bulletPrefab, transform.position, transform.rotation).GetComponent<BulletController>().SetParams(bulletSpeed, 5f, w.damage);
            audioSource.PlayOneShot(w.audio);
        }
    }

    public void StartRepeatingFire()
    {
        StopAllCoroutines();
        StartCoroutine(RepeatingFire());
    }
    public void EndRepeatingFire()
    {
        StopAllCoroutines();
    }
    
    IEnumerator RepeatingFire()
    {
        float waitTime = 1f / weapons.Find(x => x.name == "rifle").maxFirerate;
        while(true)
        {
            Shoot(PlayerController.UpperBodyModes.Rifle);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
