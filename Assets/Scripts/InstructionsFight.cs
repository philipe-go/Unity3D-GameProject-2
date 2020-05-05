
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsFight : MonoBehaviour
{
    [SerializeField] private GameObject instruction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(Countdown(10));
            instruction.SetActive(true);
        }
    }

    IEnumerator Countdown(int time)
    {
        while (time > 0)
        {
            yield return new WaitForSeconds(1);
            time -= 1;
        }

        instruction.SetActive(false);

        yield return new WaitForSeconds(time + 1);

        Destroy(gameObject);

        StopCoroutine("Countdown");
    }
}

