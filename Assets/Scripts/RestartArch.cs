using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartArch : MonoBehaviour
{
    GameManager gameManager = GameManager.GetInstance();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.currentStage++;
            gameManager.LoadStage();
        }
    }
}
