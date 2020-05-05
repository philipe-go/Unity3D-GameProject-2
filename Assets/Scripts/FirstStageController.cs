using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstStageController : MonoBehaviour
{
    private Animator anim;

    [Header("Canvas")]
    [SerializeField] Text[] canvasText;

    [Header("Audios")]
    private AudioSource audio;
    [SerializeField] private AudioClip slideClip;
    [SerializeField] private AudioClip[] concreteStep;
    [SerializeField] private AudioClip[] woodStep;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        anim.SetFloat("verticalSpeed", Input.GetAxis("Horizontal"));

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
            }
        }
    }


    private void Step()
    {
        switch (PlayerHandler.groundType)
        {
            case "concrete":
                {
                    audio.PlayOneShot(concreteStep[Random.Range(0, concreteStep.Length - 1)], 0.3f);
                }
                break;
            case "wood":
                {
                    audio.PlayOneShot(woodStep[Random.Range(0, woodStep.Length - 1)], 0.3f);
                }
                break;
            default:
                {
                    audio.PlayOneShot(concreteStep[Random.Range(0, concreteStep.Length - 1)], 0.3f);
                }
                break;
        }
    }

}
