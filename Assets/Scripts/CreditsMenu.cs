using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : MonoBehaviour
{
    public void OpenWebSite(string webSite)
    {
        Application.OpenURL(webSite);
    }
}
