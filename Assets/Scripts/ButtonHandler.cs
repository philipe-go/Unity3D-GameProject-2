using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private AudioSource audio;
    [Header("Audios")]
    [SerializeField] private AudioClip highlight;
    [SerializeField] private AudioClip selection;

    public void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audio.PlayOneShot(highlight);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        audio.PlayOneShot(selection);
    }
}
