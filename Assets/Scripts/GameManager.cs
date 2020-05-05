using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    #region SingletonPattern GameManager
    private static GameManager instance = null;
    private GameManager() { }
    internal static GameManager GetInstance()
    {
        if (instance == null)
        {
            instance = new GameManager();
        }
        return instance;
    }

    #endregion

    [Header("Menus")]
    [SerializeField] internal GameObject pauseMenu;

    [Header("Collectables")]
    [SerializeField] internal GameObject[] collectables;

    [Header("Others")]
    [SerializeField] internal GameObject playerCharacter;

    private PlayerHandler playerInstance = PlayerHandler.GetInstance();

    [Header("Audios")]
    [SerializeField] private AudioClip[] menuClip;
    private AudioSource audio;

    #region SaveGameParameters
    internal static Vector3 playerPos;
    internal static Quaternion playerRot;
    internal static float playerHitPoint;
    internal static float playerMana;
    internal static int currentStage = 0;
    internal bool saveGame = false;
    private static bool gameSaved = false;
    #endregion

    #region CheckPoint
    private Vector3 cpPlayerPos;
    private Quaternion cpPlayerRot;
    private float cpPlayerHitPoint;
    private float cpPlayerMana;
    #endregion

    void Start()
    {
        Time.timeScale = 1f;
        audio = GetComponent<AudioSource>();
        if (gameSaved) { LoadGame(); }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Cursor.visible = false;
            PauseMenu();
        }

        if (saveGame)
        {
            SaveGame();
        }
    }

    internal void LoadNewScene(string name)
    {
        Loading.sceneName = name;
        SceneManager.LoadScene("LoadingScreen", LoadSceneMode.Single);
    }

    internal void LoadStage()
    {
        switch (currentStage)
        {
            case 0:
                {
                    LoadNewScene("TrainningStage");
                }
                break;
            case 1:
                {
                    LoadNewScene("FirstStage");
                }
                break;
            case (2):
                {
                    LoadNewScene("SecondStage");
                }
                break;
            case (3):
                {
                    LoadNewScene("ThirdStage");
                }
                break;
        }
    }

    public void PauseMenu()
    {

        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Cursor.visible = !Cursor.visible;

        if (pauseMenu.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void Quit()
    {
        audio.PlayOneShot(menuClip[0]);
        LoadNewScene("MainMenu");
    }

    private void SpawnOnce(ref bool spawn, GameObject obj, Vector3 pos, Quaternion rot)
    {
        if (spawn)
        {
            Instantiate(obj, pos, rot);
            spawn = false;
        }
    }

    public void SaveGame()
    {
        audio.PlayOneShot(menuClip[0]);

        PlayerPrefs.SetFloat("posX", playerCharacter.transform.position.x);
        PlayerPrefs.SetFloat("posY", playerCharacter.transform.position.y);
        PlayerPrefs.SetFloat("posZ", playerCharacter.transform.position.z);
        PlayerPrefs.SetFloat("rotX", playerCharacter.transform.rotation.x);
        PlayerPrefs.SetFloat("rotY", playerCharacter.transform.rotation.y);
        PlayerPrefs.SetFloat("rotZ", playerCharacter.transform.rotation.z);
        PlayerPrefs.SetFloat("rotW", playerCharacter.transform.rotation.w);
        PlayerPrefs.SetInt("Stage", currentStage);

        PlayerPrefs.SetFloat("hitPoint", playerHitPoint);
        PlayerPrefs.SetFloat("Mana", playerMana);

        gameSaved = true;
        saveGame = false;

        PauseMenu();
    }

    private void LoadGame()
    {
        playerPos = new Vector3(PlayerPrefs.GetFloat("posX"), PlayerPrefs.GetFloat("posY"), PlayerPrefs.GetFloat("posZ"));
        playerRot = new Quaternion(PlayerPrefs.GetFloat("rotX"), PlayerPrefs.GetFloat("rotY"), PlayerPrefs.GetFloat("rotZ"), PlayerPrefs.GetFloat("rotW"));
        currentStage = PlayerPrefs.GetInt("Stage");

        if (playerPos != playerCharacter.transform.position)
        {
            playerCharacter.transform.position = playerPos;
        }
        if (playerRot != playerCharacter.transform.rotation)
        {
            playerCharacter.transform.rotation = playerRot;
        }

        playerInstance.playerHitPoint = PlayerPrefs.GetFloat("hitPoint");
        playerInstance.playerMana = PlayerPrefs.GetFloat("Mana");
    }
}
