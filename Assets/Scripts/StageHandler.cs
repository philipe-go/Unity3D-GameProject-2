using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageHandler : MonoBehaviour
{
    #region SingletronPattern
    private static StageHandler instance = null;
    private StageHandler() {     }
    public static StageHandler GetInstance()
    {
        if (instance == null)
        {
            instance = new StageHandler();
        }
        return instance;
    }
    #endregion


    [SerializeField] internal GameObject bossEnemy;
    [SerializeField] internal GameObject[] enemy;
    [SerializeField] internal GameObject portal;

    private void Start()
    {
        portal.SetActive(false);

        foreach (GameObject enemy in enemy)
        {
            enemy.SetActive(false);
        }

        bossEnemy.SetActive(false);
    }

    private void Update()
    {
        Cursor.visible = false;

        if (bossEnemy == null)
        { portal.SetActive(true); }
    }
}
