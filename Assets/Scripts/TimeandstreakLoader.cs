using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeandstreakLoader : MonoBehaviour
{
    public TextMeshProUGUI averageTimeW;
    public TextMeshProUGUI cstreakW;
    public TextMeshProUGUI hstreakW;
    public TextMeshProUGUI averageTimeM;
    public TextMeshProUGUI cstreakM;
    public TextMeshProUGUI hstreakM;
    public TextMeshProUGUI averageTimeP;
    public TextMeshProUGUI cstreakP;
    public TextMeshProUGUI hstreakP;
    public TextMeshProUGUI averageTimeF;
    public TextMeshProUGUI cstreakF;
    public TextMeshProUGUI hstreakF;
    public TextMeshProUGUI averageTimeG;
    public TextMeshProUGUI cstreakG;
    public TextMeshProUGUI hstreakG;
    public TextMeshProUGUI averageTimeS;
    public TextMeshProUGUI cstreakS;
    public TextMeshProUGUI hstreakS;

    



    // Start is called before the first frame update
    void Start()
    {
        TimeandStreakLoad();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TimeandStreakLoad()
    {
        float totalTimeW = 0;
        float totalTimeM = 0;
        float totalTimeP = 0;
        float totalTimeF = 0;
        float totalTimeG = 0;
        float totalTimeS = 0;
        int gamesCompletedW = PlayerPrefs.GetInt("GamesCompletedW", 0);
        int highStreakW = PlayerPrefs.GetInt("HighStreakW", 0);
        int currentStreakW = PlayerPrefs.GetInt("CurrentStreakW", 0);
        int gamesCompletedM = PlayerPrefs.GetInt("GamesCompletedM", 0);
        int highStreakM = PlayerPrefs.GetInt("HighStreakM", 0);
        int currentStreakM = PlayerPrefs.GetInt("CurrentStreakM", 0);
        int gamesCompletedP = PlayerPrefs.GetInt("GamesCompletedP", 0);
        int highStreakP = PlayerPrefs.GetInt("HighStreakP", 0);
        int currentStreakP = PlayerPrefs.GetInt("CurrentStreakP", 0);
        int gamesCompletedF = PlayerPrefs.GetInt("GamesCompletedF", 0);
        int highStreakF = PlayerPrefs.GetInt("HighStreakF", 0);
        int currentStreakF = PlayerPrefs.GetInt("CurrentStreakF", 0);
        int gamesCompletedG = PlayerPrefs.GetInt("GamesCompletedG", 0);
        int highStreakG = PlayerPrefs.GetInt("HighStreakG", 0);
        int currentStreakG = PlayerPrefs.GetInt("CurrentStreakG", 0);
        int gamesCompletedS = PlayerPrefs.GetInt("GamesCompletedS", 0);
        int highStreakS = PlayerPrefs.GetInt("HighStreakS", 0);
        int currentStreakS = PlayerPrefs.GetInt("CurrentStreakS", 0);

        

        if (gamesCompletedW == 0)
        {
            averageTimeW.text = "0";
            cstreakW.text = "0";
            hstreakW.text = "0";
            averageTimeM.text = "0";
            cstreakM.text = "0";
            hstreakM.text = "0";
            averageTimeP.text = "0";
            cstreakP.text = "0";
            hstreakP.text = "0";
            averageTimeF.text = "0";
            cstreakF.text = "0";
            hstreakF.text = "0";
            averageTimeG.text = "0";
            cstreakG.text = "0";
            hstreakG.text = "0";
            averageTimeS.text = "0";
            cstreakS.text = "0";
            hstreakS.text = "0";
            return;
        }

        for (int i = 1; i <= gamesCompletedW; i++)
        {
            totalTimeW += PlayerPrefs.GetFloat("GameTime_W" + i, 0);
        }
        float averageTime = totalTimeW / gamesCompletedW;
        averageTimeW.text = averageTime.ToString("F3") + " s";
        cstreakW.text = currentStreakW.ToString();
        hstreakW.text = highStreakW.ToString();

        for (int i = 1; i <= gamesCompletedM; i++)
        {
            totalTimeM += PlayerPrefs.GetFloat("GameTime_M" + i, 0);
        }
        float averageTime2 = totalTimeM / gamesCompletedM;
        averageTimeM.text = averageTime2.ToString("F3") + " s";
        cstreakM.text = currentStreakM.ToString();
        hstreakM.text = highStreakM.ToString();

        for (int i = 1; i <= gamesCompletedP; i++)
        {
            totalTimeP += PlayerPrefs.GetFloat("GameTime_P" + i, 0);
        }
        float averageTime3 = totalTimeP / gamesCompletedP;
        averageTimeP.text = averageTime3.ToString("F3") + " s";
        cstreakP.text = currentStreakP.ToString();
        hstreakP.text = highStreakP.ToString();

        for (int i = 1; i <= gamesCompletedF; i++)
        {
            totalTimeF += PlayerPrefs.GetFloat("GameTime_F" + i, 0);
        }
        float averageTime4 = totalTimeF / gamesCompletedF;
        averageTimeF.text = averageTime4.ToString("F3") + " s";
        cstreakF.text = currentStreakF.ToString();
        hstreakF.text = highStreakF.ToString();

        for (int i = 1; i <= gamesCompletedG; i++)
        {
            totalTimeG += PlayerPrefs.GetFloat("GameTime_G" + i, 0);
        }
        float averageTime5 = totalTimeG / gamesCompletedG;
        averageTimeG.text = averageTime5.ToString("F3") + " s";
        cstreakG.text = currentStreakG.ToString();
        hstreakG.text = highStreakG.ToString();

        for (int i = 1; i <= gamesCompletedS; i++)
        {
            totalTimeS += PlayerPrefs.GetFloat("GameTime_S" + i, 0);
        }
        float averageTime6 = totalTimeS / gamesCompletedS;
        averageTimeS.text = averageTime6.ToString("F3") + " s";
        cstreakS.text = currentStreakS.ToString();
        hstreakS.text = highStreakS.ToString();

    }
}
