using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    private float magicForce = 30f;
    private float startTime;
    private float liveTime = 10f;
    [SerializeField] private GameObject endMagicParticle;

    private void Start()
    {
        startTime = Time.time;
        transform.parent = null;
    }

    void Update()
    {
        gameObject.transform.position += transform.forward * (magicForce * Time.deltaTime);
        if (Time.time - startTime > liveTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject endMagic = Instantiate(endMagicParticle, gameObject.transform.position - Vector3.forward, gameObject.transform.rotation) as GameObject;
        Destroy(endMagic, 3.0f);
        Destroy(gameObject);
    }
}
