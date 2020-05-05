using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    private Animator anim;
    private AudioSource audio;
    private RagdollSystem ragdoll;

    [Header("AudioClips")]
    [Tooltip("Audio clips for Boss step")]
    [SerializeField] private AudioClip[] clips;

    [Header("Player")]
    [SerializeField] private Transform target;
    private bool followHero = false;
    private float attackDistance = 1.2f;
    private float corrRatio = 0.1f;

    [Header("Enemy HP")]
    [SerializeField] private GameObject hpTextObj;
    private TextMesh HPtext;

    [Header("Colliders")]
    [SerializeField] private Renderer renderer;
    [SerializeField] private SphereCollider[] handColliders;
    
    private float hitPoint = 20f;
    private float speed = 0.01f;

    private void Start()
    {
        HPtext = hpTextObj.GetComponent<TextMesh>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        ragdoll = GetComponent<RagdollSystem>();
    }

    private void Update()
    {
        HPtext.text = ($"HP: {hitPoint}");
    }

    private void FixedUpdate()
    {
        Vector3 direction = target.position - transform.position;

        if (followHero)
        {
            if (Vector3.Magnitude(direction) > corrRatio)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                anim.SetBool("isIdle", false);
                anim.SetBool("isWalking", true);
                transform.Translate(0, 0, speed);
            }

            if (Vector3.Magnitude(direction) < attackDistance)
            {
                StartCoroutine(Attack(4.8f));
            }
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            anim.SetBool("isIdle", false);
            followHero = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            anim.SetBool("isIdle", true);
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", false);

            followHero = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "SpecialMagic")
        {
            GetDamage(8f);
        }
        else if (collision.gameObject.tag == "RegularMagic")
        {
            GetDamage(3f);
        }
        else if (collision.gameObject.tag == "Projectile")
        {
            GetDamage(1f);
        }
    }

    private void GetDamage(float damage)
    {
        hitPoint -= damage;
        if (hitPoint < 0) { hitPoint = 0; }
        followHero = true;
        CheckDeath();
    }

    private void CheckDeath()
    {
        if (hitPoint <= 0)
        {
            followHero = false;

            anim.SetBool("isDead", true);
            StartCoroutine(DestroyObject(4.0f));
        }
    }

    IEnumerator Attack(float time)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", true);

        yield return new WaitForSeconds(1.0f);

        followHero = false;

        anim.SetBool("isIdle", true);
        anim.SetBool("isAttacking", false);

        rb.useGravity = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        foreach(SphereCollider col in handColliders)
        {
            col.enabled = false;
        }



        while (time > 0)
        {
            renderer.enabled = !renderer.enabled;
            time -= 0.5f;
            yield return new WaitForSeconds(0.3f);
        }

        yield return null;

        renderer.enabled = true;
        rb.useGravity = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;

        followHero = true;

        StopCoroutine("Attack");
    }

    IEnumerator DestroyObject(float time)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        rb.useGravity = false;

        yield return new WaitForSeconds(time);
        Destroy(gameObject);
        StopCoroutine("DestroyObject");
    }

    private void Step()
    {
        audio.PlayOneShot(clips[Random.Range(0, clips.Length)], 0.75f);
    }
}
