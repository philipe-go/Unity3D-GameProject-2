using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class Loading : MonoBehaviour
{
    [SerializeField] private Image loadingBar;
    [SerializeField] private Text loadingText;

    internal static string sceneName;
    private AsyncOperation async;

    private void Start()
    {
        if (sceneName == null)
        {
            sceneName = "MainMenu";
        }

        Time.timeScale = 1f;
        Input.ResetInputAxes();
        Scene currentScene = SceneManager.GetActiveScene();
        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        System.GC.Collect();
    }

    private void Update()
    {
        if (loadingBar)
        {
            loadingBar.fillAmount = async.progress + 0.1f;
        }

        if (loadingText)
        {
            loadingText.text = $"{(async.progress + 0.1f) * 100} %";
        }

        if (async.progress > 0.89f)
        {
            if (SplashScreen.isFinished) { async.allowSceneActivation = true; }
        }
    }
}
