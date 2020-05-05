using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    private GameManager gameManager;
    
    [Header("Root Menu")]
    [SerializeField] private GameObject[] menuItems;

    [Header("PopUp Menu")]
    [SerializeField] private GameObject[] subMenus;

    [Header("Background")]
    [SerializeField] private GameObject animatedBackground;
    private float screenWidth;
    private Vector3 initialScale;

    private void Start()
    {
        gameManager = GameManager.GetInstance();

        screenWidth = Screen.width;
        initialScale = animatedBackground.transform.localScale;

        if (GameManager.currentStage > 0)
        {
            menuItems[0].GetComponentInChildren<Text>().text = "Resume";
            menuItems[1].GetComponent<Button>().interactable = true;
        }
        else
        {
            menuItems[0].GetComponentInChildren<Text>().text = "Start";
            menuItems[1].GetComponent<Button>().interactable = false;

            if (gameManager.collectables != null)
            {
                menuItems[5].GetComponent<Button>().interactable = true;
            }
            else { menuItems[5].GetComponent<Button>().interactable = false; }
        }
    }


    private void Update()
    {
        Cursor.visible = true;
        HandleBackground();
    }

    public void StartButton()
    {
        gameManager.LoadStage();
        
    }

    public void TrainningButton()
    {
        gameManager.LoadNewScene("TrainningStage");
    }

    public void OptionsButton()
    {
        if (subMenus[0])
        {
            HandleMenuItems();
            subMenus[0].SetActive(!subMenus[0].activeSelf);
        }
    }

    public void CreditsButton()
    {
        if (subMenus[1])
        {
            HandleMenuItems();
            subMenus[1].SetActive(!subMenus[1].activeSelf);
        }
    }

    public void ControlButton()
    {
        if (subMenus[2])
        {
            HandleMenuItems();
            subMenus[2].SetActive(!subMenus[2].activeSelf);
        }
    }

    public void QuitButton()
    {
        if (subMenus[3])
        {
            HandleMenuItems();
            subMenus[3].SetActive(!subMenus[3].activeSelf);
        }
    }

    public void CollectablesButton()
    {
        if (subMenus[4])
        {
            HandleMenuItems();
            subMenus[4].SetActive(!subMenus[4].activeSelf);
        }
    }

    public void CloseGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void CloseMenu(int i)
    {
        subMenus[i].SetActive(false);
        HandleMenuItems();
    }

    private void HandleMenuItems()
    {
        foreach (GameObject obj in menuItems)
        {
            obj.GetComponent<Button>().interactable = !obj.GetComponent<Button>().interactable;
        }
    }

    private void HandleBackground()
    {
        if (screenWidth != Screen.width)
        {
            Vector3 temp = animatedBackground.transform.localScale;
            float resize = Screen.width / screenWidth;
            temp *= resize;
            if ((initialScale.x <= temp.x) && (initialScale.y <= temp.y))
            {
                animatedBackground.transform.localScale = initialScale;
            }
            else
            {
                animatedBackground.transform.localScale = temp;
            }
            screenWidth = Screen.width;
        }
    } 
}
