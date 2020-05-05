﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainHandler : MonoBehaviour
{
    public GameObject terrain = new GameObject();

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<StepsSnow>().terrain = this.terrain;
    }
}
