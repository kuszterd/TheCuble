using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public List<Image> gameList;
    public Image back;
    public TMP_InputField userNameIF;
    public string urlPP;
    public string urlT;
    public string emailAddress = "qstr.tv@gmail.com";
    public string subject = "Bug Report";


    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("UserName"))
        {
            string savedUsername = PlayerPrefs.GetString("UserName");
            userNameIF.text = savedUsername;

        }
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();

    }

    public void OpenPrivacyPolicy()
    {
        Application.OpenURL(urlPP);
    }

    public void ReportBug()
    {
        string url = $"mailto:{emailAddress}?subject={Uri.EscapeDataString(subject)}";
        Application.OpenURL(url);
    }

    public void OpenTerms()
    {
        Application.OpenURL(urlT);
    }
    public void SaveUserName()
    {
        string userName = userNameIF.text;
        PlayerPrefs.SetString("UserName", userName);
        PlayerPrefs.Save();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            EscGame();
        }
    }

    public void MenuLoader(Image game)
    {
        foreach (Image g in gameList)
        {
            g.gameObject.SetActive(false);
        }
        back.gameObject.SetActive(true);
        game.gameObject.SetActive(true);
        if (PlayerPrefs.HasKey("UserName"))
        {
            string savedUsername = PlayerPrefs.GetString("UserName");
            userNameIF.text = savedUsername;

        }
    }

    public void Back(Image main)
    {
        foreach (Image g in gameList)
        {
            g.gameObject.SetActive(false);
        }
        main.gameObject.SetActive(true);
        back.gameObject.SetActive(false);
    }

    public void EscGame()
    {
        // If we are running in a standalone build of the game
#if UNITY_ANDROID
        Application.Quit();
#endif

        // If we are running in the editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }


}
