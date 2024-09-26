using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;



public class MainCameraController : MonoBehaviour
{


    private Color32 practiceMode = new Color32(200, 200, 200, 255);
    private Color32 normalMode = new Color32(250, 220, 130, 255);
    public Image practiceBoard;
    public Image normalBoard;
    public static bool isPractice = false;
    public static bool isNormal = true;
    public TextMeshProUGUI practiceOrNormal;
    public Camera mainCamera;

    public static int playedTodayW = 0;
    public static int playedTodayM = 0;
    public static int playedTodayP = 0;
    public static int playedTodayG = 0;
    public static int playedTodayF = 0;
    public static int playedTodayS = 0;

    private string checkedDay;
    private string today;




    // Start is called before the first frame update
    void Start()
    {
        playedTodayW = PlayerPrefs.GetInt("PlayedTodayW", playedTodayW);
        playedTodayM = PlayerPrefs.GetInt("PlayedTodayM", playedTodayM);
        playedTodayP = PlayerPrefs.GetInt("PlayedTodayP", playedTodayP);
        playedTodayG = PlayerPrefs.GetInt("PlayedTodayG", playedTodayG);
        playedTodayF = PlayerPrefs.GetInt("PlayedTodayF", playedTodayF);
        playedTodayS = PlayerPrefs.GetInt("PlayedTodayS", playedTodayS);

        checkedDay = PlayerPrefs.GetString("CheckedDay", checkedDay);

        today = DateTime.Now.Date.ToString();

        CheckMiniGamePlayed();

        if (checkedDay != today)
        {
            PlayerPrefs.SetInt("PlayedTodayW", 0);
            PlayerPrefs.SetInt("PlayedTodayM", 0);
            PlayerPrefs.SetInt("PlayedTodayP", 0);
            PlayerPrefs.SetInt("PlayedTodayG", 0);
            PlayerPrefs.SetInt("PlayedTodayF", 0);
            PlayerPrefs.SetInt("PlayedTodayS", 0);

            PlayerPrefs.SetString("CheckedDay", today);
            PlayerPrefs.Save();
        }

        Debug.Log(today);
        Debug.Log(checkedDay);
        Debug.Log(playedTodayW);



        if (isNormal)
        {
            mainCamera.backgroundColor = normalMode;
            practiceOrNormal.text = "Daily mode";
            practiceBoard.gameObject.SetActive(false);
            normalBoard.gameObject.SetActive(true);
            isNormal = true;
            isPractice = false;
            return;
        }
        else if (isPractice)
        {
            mainCamera.backgroundColor = practiceMode;
            practiceOrNormal.text = "Practice mode";
            normalBoard.gameObject.SetActive(false);
            practiceBoard.gameObject.SetActive(true);
            isPractice = true;
            isNormal = false;
            return;
        }

    }


    public void CheckMiniGamePlayed()
    {
        if (WordleGameManager.normalLoaded)
        {
            playedTodayW = 1;
            PlayerPrefs.SetInt("PlayedTodayW", playedTodayW);
            PlayerPrefs.Save();
        }
        if (SportleManager.normalLoaded)
        {
            playedTodayS = 1;
            PlayerPrefs.SetInt("PlayedTodayS", playedTodayS);
            PlayerPrefs.Save();
        }
        if (PaintlerManager.normalLoaded)
        {
            playedTodayP = 1;
            PlayerPrefs.SetInt("PlayedTodayP", playedTodayP);
            PlayerPrefs.Save();
        }
        if (FlagleManager.normalLoaded)
        {
            playedTodayF = 1;
            PlayerPrefs.SetInt("PlayedTodayF", playedTodayF);
            PlayerPrefs.Save();
        }
        if (GlobleManager.normalLoaded)
        {
            playedTodayG = 1;
            PlayerPrefs.SetInt("PlayedTodayG", playedTodayG);
            PlayerPrefs.Save();
        }
        if (MathleManager.normalLoaded)
        {
            playedTodayM = 1;
            PlayerPrefs.SetInt("PlayedTodayM", playedTodayM);
            PlayerPrefs.Save();
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    

    public void SwitchMode()
    {
        if (isNormal)
        {
            practiceOrNormal.text = "Practice mode";
            mainCamera.backgroundColor = practiceMode;
            practiceBoard.gameObject.SetActive(true);
            normalBoard.gameObject.SetActive(false);
            isNormal = false;
            isPractice = true;
            return;
        }
        else if (isPractice)
        {
            practiceOrNormal.text = "Daily mode";
            mainCamera.backgroundColor = normalMode;
            normalBoard.gameObject.SetActive(true);
            practiceBoard.gameObject.SetActive(false);
            isPractice = false;
            isNormal = true;
            return;
        }
    }
}
