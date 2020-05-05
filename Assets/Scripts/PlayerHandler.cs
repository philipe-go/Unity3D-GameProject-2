using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    #region SingletonPattern Character
    private static PlayerHandler instance = null;
    private PlayerHandler() { }
    public static PlayerHandler GetInstance()
    {
        if (instance == null)
        {
            instance = new PlayerHandler();
        }

        return instance;
    }
    #endregion

    private float isGroundedDist = 0.2f;

    internal static string groundType;

    private CharacterController controller;
    private Animator anim;
    internal float playerMana;
    internal float playerHitPoint;

    private float magicRange = 10f;

    private float initialHitPoints = 15f;
    private float initialMana = 10f;
    internal Vector3 initialPos;

    private float projectileForce = 5000f;

    private float characterScale;
    private CapsuleCollider characterCollider;

    [SerializeField] private AudioClip hpPotionClip;
    [SerializeField] private AudioClip manaPotionClip;
    [SerializeField] private AudioClip slideClip;

    [Header("Atacks")]
    [SerializeField] private GameObject[] magicList;
    [SerializeField] private GameObject[] magicThrow;
    [SerializeField] private GameObject magicCastPoint;
    [SerializeField] private GameObject[] projectile;

    [Header("Canvas")]
    [SerializeField] Text[] canvasText;

    [Header("Audios")]
    private AudioSource audio;
    [SerializeField] private AudioClip[] magicCastClips;
    [SerializeField] private AudioClip[] magicProjectileClips;
    [SerializeField] private AudioClip projectileClip;
    [SerializeField] private AudioClip[] concreteStep;
    [SerializeField] private AudioClip[] grassStep;
    [SerializeField] private AudioClip[] woodStep;


    private void Awake()
    {
        initialPos = gameObject.transform.position;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        characterCollider = GetComponent<CapsuleCollider>();
        characterScale = characterCollider.height;

        playerHitPoint = initialHitPoints;
        playerMana = initialMana;

        controller = GetComponent<CharacterController>();
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        canvasText[0].text = ($"HitPoints: {playerHitPoint}");
        canvasText[1].text = ($"Mana: {playerMana}");

        if (controller.isGrounded)
        {
            if (Input.GetButtonDown("SpecialMagic"))
            {
                if (playerMana >= 5.0f) { CastMagic(2); }
                else { AlterManaText("NO MANA", 0); }
            }
            if (Input.GetButtonDown("RegularMagic"))
            {
                if (playerMana >= 2.0f) { CastMagic(1); }
                else { AlterManaText("NO MANA", 0); }
            }
            if (Input.GetButtonDown("RecoverMagic"))
            {
                if (playerMana >= 5.0f) { CastMagic(0); }
                else { AlterManaText("NO MANA", 0); }
            }
            //if (Input.GetButtonDown("Projectile"))
            //{
            //    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("StandingAttack"))
            //    {
            //        anim.Play("StandingAttack");
            //        audio.PlayOneShot(projectileClip, 0.5f);
            //        StartCoroutine(ThrowProjectile(.4f));
            //    }
            //}
        }
        if (!controller.isGrounded)
        {

        }
    }

    private void FixedUpdate()
    {
        if (controller.isGrounded)
        {
            anim.SetFloat("horizontalSpeed", Input.GetAxis("Horizontal"));
            anim.SetFloat("verticalSpeed", Input.GetAxis("Vertical"));

            //if ((Input.GetAxis("Vertical") > 0) && (Input.GetAxis("Horizontal") == 0))
            //{
            //    Vector3 direction = transform.position - Input.mousePosition;
            //    Quaternion target = Quaternion.Euler(0, -Vector3.Angle(Vector3.up, direction), 0);
            //    transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 2f);
            //}

            if (Input.GetButton("Jump"))
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("RunningJump"))
                {
                    anim.Play("RunningJump");
                }
            }

            if (Input.GetButton("Dodge"))
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("RunningSlide"))
                {
                    anim.Play("RunningSlide");
                    audio.PlayOneShot(slideClip);
                    StartCoroutine(ColliderScale(1, 1.0f));
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Death")
        {
            anim.SetBool("isDead", true);
            StartCoroutine(ReloadScene());
        }

        if (other.gameObject.tag == "HealthPotion")
        {
            audio.PlayOneShot(hpPotionClip);
            playerHitPoint += 3f;
            if (playerHitPoint > initialHitPoints) { playerHitPoint = initialHitPoints; }
            Destroy(other.gameObject);
            AlterHPtext("+", 3);
        }

        if (other.gameObject.tag == "ManaPotion")
        {
            audio.PlayOneShot(manaPotionClip);
            playerMana += 3f;
            if (playerMana > initialMana) { playerMana = initialMana; }
            Destroy(other.gameObject);
            AlterManaText("+", 3f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "MovingPlattform")
        {
            gameObject.transform.parent = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "MovingPlattform")
        {
            gameObject.transform.parent = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BossEnemy")
        {
            GetDamage(5f);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            GetDamage(2f);
        }
    }

    private void CastMagic(int i)
    {
        switch (i)
        {
            case (2):
                {
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("SpecialMagic"))
                    {
                        anim.Play("SpecialMagic");
                        audio.PlayOneShot(magicCastClips[i], 0.2f);
                        GameObject aMagic = Instantiate(magicList[i], gameObject.transform) as GameObject;
                        StartCoroutine(CallMagic(i, 2.0f));
                        Destroy(aMagic, 3f);
                        playerMana -= 5f;
                        AlterManaText("-", 5f);
                    }
                }
                break;
            case (1):
                {
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("RegularMagic"))
                    {
                        anim.Play("RegularMagic");
                        GameObject aMagic = Instantiate(magicList[i], gameObject.transform) as GameObject;
                        audio.PlayOneShot(magicCastClips[i], 0.2f);
                        StartCoroutine(CallMagic(i, 1.0f));
                        StartCoroutine(CallMagic(i, 1.5f));
                        StartCoroutine(CallMagic(i, 2.0f));
                        Destroy(aMagic, 3f);
                        playerMana -= 2f;
                        AlterManaText("-", 2f);
                    }
                }
                break;
            case (0):
                {
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("RecoverMagic"))
                    {
                        anim.Play("RecoverMagic");
                        audio.PlayOneShot(magicCastClips[i], 0.2f);
                        GameObject aMagic = Instantiate(magicList[i], gameObject.transform) as GameObject;
                        Destroy(aMagic, 3f);
                        playerMana -= 5f;
                        playerHitPoint += 5f;
                        if (playerHitPoint > initialHitPoints) { playerHitPoint = initialHitPoints; }
                        AlterManaText("-", 5f);
                    }
                }
                break;
            default:
                {
                    Debug.Log("this spell does not exist");
                }
                break;
        }
    }

    IEnumerator CallMagic(int i, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject aMagic = Instantiate(magicThrow[i - 1], magicCastPoint.transform.position, transform.rotation) as GameObject;
        audio.PlayOneShot(magicProjectileClips[i - 1], 0.2f);
        Destroy(aMagic, magicRange);
        StopCoroutine("CallMagic");
    }

    IEnumerator ThrowProjectile(float time)
    {
        yield return new WaitForSeconds(time);
        int i = Random.Range(0, 6);
        GameObject aThrow = Instantiate(projectile[i], magicCastPoint.transform.position, transform.rotation) as GameObject;
        aThrow.GetComponent<Rigidbody>().AddForce(transform.forward * projectileForce * Time.deltaTime, ForceMode.Impulse);
        StopCoroutine("ThrowProjectile");

    }

    IEnumerator ColliderScale(int i, float time)
    {
        characterCollider.height = characterScale * 0.5f;
        controller.height = characterScale * 0.5f;
        if (i == 1)
        {
            characterCollider.center = new Vector3(0, 0.25f, 0);
            controller.center = new Vector3(0, 0.25f, 0);
        }
        if (i == 0)
        {
            characterCollider.center = new Vector3(0, 1.15f, 0);
            controller.center = new Vector3(0, 0.25f, 0);
        }

        yield return new WaitForSeconds(time);
        characterCollider.height = characterScale;
        controller.height = characterScale;
        characterCollider.center = new Vector3(0, 0.7f, 0);
        controller.center = new Vector3(0, 0.7f, 0);
        StopCoroutine("ColliderScale");
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StopCoroutine("ReloadScene");
    }

    IEnumerator CantBeDamaged(float time)
    {
        yield return new WaitForSeconds(0.8f);
        anim.Play("Hit");

        gameObject.GetComponent<CapsuleCollider>().enabled = false;

        yield return new WaitForSeconds(time);

        gameObject.GetComponent<CapsuleCollider>().enabled = true;

        StopCoroutine("CantBeDamaged");
    }

    void GetDamage(float damage)
    {
        playerHitPoint -= damage;
        if (playerHitPoint <= 0) { playerHitPoint = 0; }
        AlterHPtext("-", damage);
        StartCoroutine(CantBeDamaged(3.0f));
        DeathCheck();
    }

    void AlterHPtext(string signal, float damage)
    {
        canvasText[2].enabled = true;
        canvasText[2].text = ($"{signal} {damage}");
        canvasText[2].GetComponent<Animation>().Play();
    }

    void AlterManaText(string signal, float mana)
    {
        Color alpha = canvasText[3].color;
        alpha.a = 1f;
        canvasText[3].text = ($"{signal} {mana}");
        canvasText[3].GetComponent<Animation>().Play();
    }

    void DeathCheck()
    {
        if (playerHitPoint <= 0)
        {
            anim.SetBool("isDead", true);
            StartCoroutine(ReloadScene());
        }
    }

    private void Step()
    {
        switch (groundType)
        {
            case "concrete": 
                {
                    audio.PlayOneShot(concreteStep[Random.Range(0, concreteStep.Length - 1)], 0.3f);
                }
                break;
            case "grass":
                {
                    audio.PlayOneShot(grassStep[Random.Range(0, grassStep.Length - 1)], 0.3f);
                }
                break;
            case "wood":
                {
                    audio.PlayOneShot(woodStep[Random.Range(0, woodStep.Length - 1)], 0.3f);
                }
                break;
            default:
                {
                    audio.PlayOneShot(grassStep[Random.Range(0, grassStep.Length - 1)], 0.3f);
                } break;
        }
    }
}
