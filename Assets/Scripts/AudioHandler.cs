using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    [Tooltip("Enter the type of ground to be parsed to the player's step method - ie: grass")]
    public string groundType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHandler.groundType = this.groundType;
        }
    }
}

