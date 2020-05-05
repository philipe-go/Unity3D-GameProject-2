using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalEnemy : MonoBehaviour
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
    private float attackDistance = 0.6f;
    private float corrRatio = 0.1f;
    

    [Header("Enemy HP")]
    [SerializeField] private GameObject hpTextObj;
    private TextMesh HPtext;

    [SerializeField] private Renderer renderer;

    private float hitPoint = 10f;
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
                StartCoroutine(Attack(3.0f));
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
            GetDamage(10f);
        }
        else if (collision.gameObject.tag == "RegularMagic")
        {
            GetDamage(5f);
        }
        else if (collision.gameObject.tag == "Projectile")
        {
            GetDamage(2f);
        }
    }

    private void GetDamage(float damage)
    {
        followHero = true;
        hitPoint -= damage;
        if (hitPoint < 0) { hitPoint = 0; }
        CheckDeath();
    }

    private void CheckDeath()
    {
        if (hitPoint <= 0)
        {
            followHero = false;
            StartCoroutine(DestroyObject(3.0f));
        }
    }

    IEnumerator Attack(float time)
    {
        
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", true);

        yield return new WaitForSeconds(2.0f);

        followHero = false;

        gameObject.GetComponent<CapsuleCollider>().enabled = false;

        anim.SetBool("isIdle", true);
        anim.SetBool("isAttacking", false);

        while (time > 0)
        {
            renderer.enabled = !renderer.enabled;
            time -= 0.5f;
            yield return new WaitForSeconds(0.3f);
        }

        yield return null;

        renderer.enabled = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;

        followHero = true;

        StopCoroutine("Attack");
    }

    IEnumerator DestroyObject(float time)
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        ragdoll.Die(1000, Vector3.forward);

        yield return new WaitForSeconds(time);
        Destroy(gameObject);
        StopCoroutine("DestroyObject");
    }

    private void Step()
    {
        audio.PlayOneShot(clips[Random.Range(0, clips.Length)], 0.35f);
    }
}
