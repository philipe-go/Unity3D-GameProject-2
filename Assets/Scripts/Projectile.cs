using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    private Rigidbody rb;
    private float alpha;
    private float startTime;
    private float liveTime = 30f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        alpha = GetComponent<Renderer>().material.color.a;
        startTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - startTime > liveTime)
        {
            alpha -= Time.deltaTime;
            if (alpha <= 0) { Destroy(gameObject); }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Enemy") || (collision.gameObject.tag == "BossEnemy"))
        {
            Destroy(gameObject);
        }
    }
}
