using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float speed, lifetime, birthtime;
    public float dmg {get; private set;}

    public void SetParams(float _speed, float _lifetime, float _dmg)
    {
        speed = _speed;
        lifetime = _lifetime;
        dmg = _dmg;
    }

    private void Start()
    {
        birthtime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;

        if(Time.realtimeSinceStartup - birthtime > lifetime)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        transform.forward = Vector3.Reflect(transform.forward, other.GetContact(0).normal);
    }
}
