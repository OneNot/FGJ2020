using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float speed, lifetime, birthtime;

    public void SetParams(float _speed, float _lifetime)
    {
        speed = _speed;
        lifetime = _lifetime;
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
}
