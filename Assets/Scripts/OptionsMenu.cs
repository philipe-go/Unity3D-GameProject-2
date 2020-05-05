using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    private Resolution[] resolutions;
    private string[] allQuality;
    
    [SerializeField] private AudioMixer audioMixer;

    [Header("Selections")]
    #region
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private Dropdown dropDownQuality;
    [SerializeField] private Dropdown dropDownResolution;
    [SerializeField] private Slider sliderScreenMode;
    #endregion

    [Header("Feed Backs")]
    #region
    [SerializeField] private Text feedbackMusic;
    [SerializeField] private Text feedbackSFX;
    [SerializeField] private Text feedBackQuality;
    [SerializeField] private Text feedBackResolution;
    [SerializeField] private Text feedBackFullScreen;
    #endregion

    private void Start()
    {
        LoadUserPrefs();
    }

    private void LoadUserPrefs()
    {
        if (Screen.fullScreen) { SetFullScreen(1f); }
        else if (!Screen.fullScreen) { SetFullScreen(0); }

        GetVolume();
        GetSFX();
        GetResolutions();
        GetQuality();
    }

    private void GetVolume()
    {
        float val = PlayerPrefs.GetFloat("masterChannel", 1.0f);
        sliderMusic.value = val;
        feedbackMusic.text = System.Convert.ToInt32(val + 80).ToString();
    }

    public void SetVolume(float vol)
    {
        audioMixer.SetFloat("masterChannel", vol);
        PlayerPrefs.SetFloat("masterChannel", vol);
        sliderMusic.value = vol;
        feedbackMusic.text = System.Convert.ToInt32(vol + 80).ToString();

        PlayerPrefs.Save();
    }

    private void GetSFX()
    {
        float val = PlayerPrefs.GetFloat("sfxChannel");
        sliderMusic.value = val;
        feedbackSFX.text = System.Convert.ToInt32(val + 80).ToString();
    }

    public void SetSFX(float vol)
    {
        audioMixer.SetFloat("sfxChannel", vol);
        PlayerPrefs.SetFloat("sfxChannel", vol);
        sliderSFX.value = vol;
        feedbackSFX.text = System.Convert.ToInt32(vol + 80).ToString();
        PlayerPrefs.Save();
    }

    private void GetQuality()
    {
        allQuality = QualitySettings.names;
        List<string> qualities = new List<string>();
        int curQuality = QualitySettings.GetQualityLevel();

        for (int i = 0; i < allQuality.Length; i++)
        {
            qualities.Add(allQuality[i]);
        }

        dropDownQuality.options.Clear();
        dropDownQuality.AddOptions(qualities);
        dropDownQuality.value = curQuality;
        feedBackQuality.text = allQuality[curQuality];
    }

    public void SetQuality(int set)
    {
        QualitySettings.SetQualityLevel(set, true);
        feedBackQuality.text = allQuality[set];
        PlayerPrefs.Save();
    }

    private void GetResolutions()
    {
        List<string> allResolutions = new List<string>();
        int currentRes = -1;

        resolutions = Screen.resolutions;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string selectable = $"{resolutions[i].width} x {resolutions[i].height}";
            allResolutions.Add(selectable);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentRes = i;
                feedBackResolution.text = $"{resolutions[i].width} x {resolutions[i].height}";
            }
        }

        dropDownResolution.options.Clear();
        dropDownResolution.AddOptions(allResolutions);
        dropDownResolution.value = currentRes;
        dropDownResolution.RefreshShownValue();
    }

    public void SetResolutions(int index)
    {
        Resolution setRes = resolutions[index];
        Screen.SetResolution(setRes.width, setRes.height, Screen.fullScreenMode);

        dropDownResolution.RefreshShownValue();
        feedBackResolution.text = System.Convert.ToString(resolutions[index]);

        PlayerPrefs.Save();
    }

    public void SetFullScreen()
    {
        float value = sliderScreenMode.value;
        Screen.fullScreen = System.Convert.ToBoolean(value);
        if (System.Convert.ToBoolean(value))
        {
            feedBackFullScreen.text = "Full Screen";
        }
        else { feedBackFullScreen.text = "Windowed"; }
    }

    public void SetFullScreen(float value)
    {
        Screen.fullScreen = System.Convert.ToBoolean(value);
        if (System.Convert.ToBoolean(value))
        {
            feedBackFullScreen.text = "Full Screen";
        }
        else { feedBackFullScreen.text = "Windowed"; }
    }
}
