using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawn;
    private StageHandler stageHandler = StageHandler.GetInstance();


    private void OnTriggerEnter(Collider other)
    {
        spawn.SetActive(true);
    }

}
