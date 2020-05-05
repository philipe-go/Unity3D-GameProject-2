using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Password : MonoBehaviour
{
    private GameManager gameManager = GameManager.GetInstance();
    
    [SerializeField] private GameObject passwordInput;
    private String[] password = { " ", " ", " " };
    private int index = 0;
    [SerializeField] private Text text;
    
    [SerializeField] GameObject letter;
    [SerializeField] GameObject player;

    private void Start()
    {
        Cursor.visible = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Time.timeScale = 0.0f;
        Cursor.visible = true;
        passwordInput.SetActive(true);
    }

    public void Close()
    {
        letter.SetActive(false);
        Cursor.visible = false;
        player.SetActive(true);
    }

    public void EnterPassowrd()
    {
        if ((password[0] == "d") && (password[1] == "d") && (password[2] == "d"))
        {
            text.text = "your passworD is correct.";
            GameManager.currentStage++;
            gameManager.LoadStage();
        }
        else
        {
            text.text = "try again, your passworD is wrong.";
            ClearPassword();
        }
    }

    public void SetPassword(string letter)
    {
        password[index] = letter;
        index++;
        text.text = "";

        if (index == 3)
        {
            EnterPassowrd();
        }
    }

    public void ClearPassword()
    {
        Array.Clear(password, 0, 3);
        index = 0;
        text.text = "passworD cleareD";
    }
}
