using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI timerText2;
    private bool timerRunning = false; // Check if the timer is running
    private float startTime; // Start time of the timer
    private bool timerstopped = false;    
    private int gamesCompletedW;
    private int gamesCompletedM;
    private int gamesCompletedP;
    private int gamesCompletedG;
    private int gamesCompletedS;
    private int gamesCompletedF;
    private int highStreakW;
    private int highStreakM;
    private int highStreakP;
    private int highStreakG;
    private int highStreakS;
    private int highStreakF;
    private int currentStreakW;
    private int currentStreakM;
    private int currentStreakP;
    private int currentStreakG;
    private int currentStreakF;
    private int currentStreakS;
    public Image timer;
    public TextMeshProUGUI actualTime;
    public TimeandstreakLoader timeandstreakLoader;
    private float elapsedTime;




    //Reference to the GameManager script


    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
        timerRunning = false;
        timerstopped = false;
        if (MainCameraController.isPractice)
        {
            timer.gameObject.SetActive(false);           
        }


        gamesCompletedW = PlayerPrefs.GetInt("GamesCompletedW", 0);
        gamesCompletedM= PlayerPrefs.GetInt("GamesCompletedM", 0);
        gamesCompletedP = PlayerPrefs.GetInt("GamesCompletedP", 0);
        gamesCompletedG = PlayerPrefs.GetInt("GamesCompletedG", 0);
        gamesCompletedS = PlayerPrefs.GetInt("GamesCompletedS", 0);
        gamesCompletedF = PlayerPrefs.GetInt("GamesCompletedF", 0);
        highStreakW = PlayerPrefs.GetInt("HighStreakW", 0);
        highStreakM = PlayerPrefs.GetInt("HighStreakM", 0);
        highStreakP = PlayerPrefs.GetInt("HighStreakP", 0);
        highStreakG = PlayerPrefs.GetInt("HighStreakG", 0);
        highStreakS = PlayerPrefs.GetInt("HighStreakS", 0);
        highStreakF = PlayerPrefs.GetInt("HighStreakF", 0);
        currentStreakW = PlayerPrefs.GetInt("CurrentStreakW", 0);
        currentStreakM = PlayerPrefs.GetInt("CurrentStreakM", 0);
        currentStreakP = PlayerPrefs.GetInt("CurrentStreakP", 0);
        currentStreakG = PlayerPrefs.GetInt("CurrentStreakG", 0);
        currentStreakS = PlayerPrefs.GetInt("CurrentStreakS", 0);
        currentStreakF = PlayerPrefs.GetInt("CurrentStreakF", 0);

    }
    // Function to start the timer, called by the first user interaction
    public void StartTimer()
    {
        if (!timerRunning && !timerstopped)
        {
            timerRunning = true;
            startTime = Time.time;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!timerRunning && Input.anyKeyDown && MainCameraController.isNormal)
        { 
            StartTimer();
        }

        if (timerRunning)
        {
            // Calculate the time elapsed since the start time
            elapsedTime = Time.time - startTime;

            // Convert the elapsed time to milliseconds
            int seconds = Mathf.FloorToInt(elapsedTime);
            int milliseconds = Mathf.FloorToInt((elapsedTime - seconds) * 1000);

            // Update the timer text
            timerText.text = seconds.ToString();
            timerText2.text = milliseconds.ToString("D3");

            if (WordleGameManager.normalWin && SceneManager.GetActiveScene().name == "WordleScene")
            {
                timerRunning = false;
                timerstopped = true;
                //Calculate the final elapsed time when stopping
                float finalElapsedTime = Time.time - startTime;
                actualTime.text = finalElapsedTime.ToString("F3") + " s";
                // Separate the seconds and milliseconds parts
                int finalSeconds = Mathf.FloorToInt(finalElapsedTime);
                int finalMilliseconds = Mathf.FloorToInt((finalElapsedTime - finalSeconds) * 1000);

                // Update the Text elements one last time
                timerText.text = finalSeconds.ToString();
                timerText2.text = finalMilliseconds.ToString("D3");

                gamesCompletedW++;
                currentStreakW++;
                if (currentStreakW > highStreakW)
                {
                    highStreakW++;
                }
                PlayerPrefs.SetFloat("GameTime_W" + gamesCompletedW, finalElapsedTime);
                PlayerPrefs.SetInt("GamesCompletedW", gamesCompletedW);
                PlayerPrefs.SetInt("HighStreakW", highStreakW);
                PlayerPrefs.SetInt("CurrentStreakW", currentStreakW);
                PlayerPrefs.Save();
                timeandstreakLoader.TimeandStreakLoad();
                
                



            }
            else if (PaintlerManager.normalWin && SceneManager.GetActiveScene().name == "PaintleScene")
            {
                timerRunning = false;
                timerstopped = true;
                //Calculate the final elapsed time when stopping
                float finalElapsedTime = Time.time - startTime;
                actualTime.text = finalElapsedTime.ToString("F3") + " s";
                // Separate the seconds and milliseconds parts
                int finalSeconds = Mathf.FloorToInt(finalElapsedTime);
                int finalMilliseconds = Mathf.FloorToInt((finalElapsedTime - finalSeconds) * 1000);

                // Update the Text elements one last time
                timerText.text = finalSeconds.ToString();
                timerText2.text = finalMilliseconds.ToString("D3");

                gamesCompletedP++;
                currentStreakP++;
                if (currentStreakP > highStreakP)
                {
                    highStreakP++;
                }
                PlayerPrefs.SetFloat("GameTime_P" + gamesCompletedP, finalElapsedTime);
                PlayerPrefs.SetInt("GamesCompletedP", gamesCompletedP);
                PlayerPrefs.SetInt("HighStreakP", highStreakP);
                PlayerPrefs.SetInt("CurrentStreakP", currentStreakP);
                PlayerPrefs.Save();
                timeandstreakLoader.TimeandStreakLoad();


            }
            else if (FlagleManager.normalWin && SceneManager.GetActiveScene().name == "FlagleScene")
            {
                timerRunning = false;
                timerstopped = true;
                //Calculate the final elapsed time when stopping
                float finalElapsedTime = Time.time - startTime;
                actualTime.text = finalElapsedTime.ToString("F3") + " s";
                // Separate the seconds and milliseconds parts
                int finalSeconds = Mathf.FloorToInt(finalElapsedTime);
                int finalMilliseconds = Mathf.FloorToInt((finalElapsedTime - finalSeconds) * 1000);

                // Update the Text elements one last time
                timerText.text = finalSeconds.ToString();
                timerText2.text = finalMilliseconds.ToString("D3");

                gamesCompletedF++;
                currentStreakF++;
                if (currentStreakF > highStreakF)
                {
                    highStreakF++;
                }
                PlayerPrefs.SetFloat("GameTime_F" + gamesCompletedF, finalElapsedTime);
                PlayerPrefs.SetInt("GamesCompletedF", gamesCompletedF);
                PlayerPrefs.SetInt("HighStreakF", highStreakF);
                PlayerPrefs.SetInt("CurrentStreakF", currentStreakF);
                PlayerPrefs.Save();
                timeandstreakLoader.TimeandStreakLoad();
            }
            else if (SportleManager.normalWin && SceneManager.GetActiveScene().name == "SportleScene")
            {
                timerRunning = false;
                timerstopped = true;
                //Calculate the final elapsed time when stopping
                float finalElapsedTime = Time.time - startTime;
                actualTime.text = finalElapsedTime.ToString("F3") + " s";
                // Separate the seconds and milliseconds parts
                int finalSeconds = Mathf.FloorToInt(finalElapsedTime);
                int finalMilliseconds = Mathf.FloorToInt((finalElapsedTime - finalSeconds) * 1000);

                // Update the Text elements one last time
                timerText.text = finalSeconds.ToString();
                timerText2.text = finalMilliseconds.ToString("D3");

                gamesCompletedS++;
                currentStreakS++;
                if (currentStreakS > highStreakS)
                {
                    highStreakS++;
                }
                PlayerPrefs.SetFloat("GameTime_S" + gamesCompletedS, finalElapsedTime);
                PlayerPrefs.SetInt("GamesCompletedS", gamesCompletedS);
                PlayerPrefs.SetInt("HighStreakS", highStreakS);
                PlayerPrefs.SetInt("CurrentStreakS", currentStreakS);
                PlayerPrefs.Save();
                timeandstreakLoader.TimeandStreakLoad();

            }
            else if (GlobleManager.normalWin && SceneManager.GetActiveScene().name == "GlobleScene")
            {
                timerRunning = false;
                timerstopped = true;
                //Calculate the final elapsed time when stopping
                float finalElapsedTime = Time.time - startTime;
                actualTime.text = finalElapsedTime.ToString("F3") + " s";
                // Separate the seconds and milliseconds parts
                int finalSeconds = Mathf.FloorToInt(finalElapsedTime);
                int finalMilliseconds = Mathf.FloorToInt((finalElapsedTime - finalSeconds) * 1000);

                // Update the Text elements one last time
                timerText.text = finalSeconds.ToString();
                timerText2.text = finalMilliseconds.ToString("D3");

                gamesCompletedG++;
                currentStreakG++;
                if (currentStreakG > highStreakG)
                {
                    highStreakG++;
                }
                PlayerPrefs.SetFloat("GameTime_G" + gamesCompletedG, finalElapsedTime);
                PlayerPrefs.SetInt("GamesCompletedG", gamesCompletedG);
                PlayerPrefs.SetInt("HighStreakG", highStreakG);
                PlayerPrefs.SetInt("CurrentStreakG", currentStreakG);
                PlayerPrefs.Save();
                timeandstreakLoader.TimeandStreakLoad();

            }
            else if (MathleManager.normalWin && SceneManager.GetActiveScene().name == "MathleScene")
            {
                timerRunning = false;
                timerstopped = true;
                //Calculate the final elapsed time when stopping
                float finalElapsedTime = Time.time - startTime;
                actualTime.text = finalElapsedTime.ToString("F3") + " s";
                // Separate the seconds and milliseconds parts
                int finalSeconds = Mathf.FloorToInt(finalElapsedTime);
                int finalMilliseconds = Mathf.FloorToInt((finalElapsedTime - finalSeconds) * 1000);

                // Update the Text elements one last time
                timerText.text = finalSeconds.ToString();
                timerText2.text = finalMilliseconds.ToString("D3");

                gamesCompletedM++;
                currentStreakM++;
                if (currentStreakM > highStreakM)
                {
                    highStreakM++;
                }
                PlayerPrefs.SetFloat("GameTime_M" + gamesCompletedM, finalElapsedTime);
                PlayerPrefs.SetInt("GamesCompletedM", gamesCompletedM);
                PlayerPrefs.SetInt("HighStreakM", highStreakM);
                PlayerPrefs.SetInt("CurrentStreakM", currentStreakM);
                PlayerPrefs.Save();
                timeandstreakLoader.TimeandStreakLoad();

            }

            if(WordleGameManager.lose && SceneManager.GetActiveScene().name == "WordleScene") 
            {
                timerRunning = false;
                timerstopped = true;
                //Calculate the final elapsed time when stopping
                float finalElapsedTime = Time.time - startTime;
                actualTime.text = finalElapsedTime.ToString("F3") + " s";
                // Separate the seconds and milliseconds parts
                int finalSeconds = Mathf.FloorToInt(finalElapsedTime);
                int finalMilliseconds = Mathf.FloorToInt((finalElapsedTime - finalSeconds) * 1000);
                

                // Update the Text elements one last time
                timerText.text = finalSeconds.ToString();
                timerText2.text = finalMilliseconds.ToString("D3");
                gamesCompletedW++;
                currentStreakW = 0;
                PlayerPrefs.SetFloat("GameTime_W" + gamesCompletedW, finalElapsedTime);
                PlayerPrefs.SetInt("GamesCompletedW", gamesCompletedW);
                PlayerPrefs.SetInt("HighStreakW", highStreakW);
                PlayerPrefs.SetInt("CurrentStreakW", currentStreakW);                
                PlayerPrefs.Save();
                timeandstreakLoader.TimeandStreakLoad();

            }
            else if (MathleManager.lose && SceneManager.GetActiveScene().name == "MathleScene")
            {
                timerRunning = false;
                timerstopped = true;
                //Calculate the final elapsed time when stopping
                float finalElapsedTime = Time.time - startTime;
                actualTime.text = finalElapsedTime.ToString("F3") + " s";
                // Separate the seconds and milliseconds parts
                int finalSeconds = Mathf.FloorToInt(finalElapsedTime);
                int finalMilliseconds = Mathf.FloorToInt((finalElapsedTime - finalSeconds) * 1000);


                // Update the Text elements one last time
                timerText.text = finalSeconds.ToString();
                timerText2.text = finalMilliseconds.ToString("D3");
                gamesCompletedM++;
                currentStreakM = 0;
                PlayerPrefs.SetFloat("GameTime_M" + gamesCompletedM, finalElapsedTime);
                PlayerPrefs.SetInt("GamesCompletedM", gamesCompletedM);
                PlayerPrefs.SetInt("HighStreakM", highStreakM);
                PlayerPrefs.SetInt("CurrentStreakM", currentStreakM);
                PlayerPrefs.Save();
                timeandstreakLoader.TimeandStreakLoad();
            }
            else if (PaintlerManager.lose && SceneManager.GetActiveScene().name == "PaintleScene")
            {
                timerRunning = false;
                timerstopped = true;
                //Calculate the final elapsed time when stopping
                float finalElapsedTime = Time.time - startTime;
                actualTime.text = finalElapsedTime.ToString("F3") + " s";
                // Separate the seconds and milliseconds parts
                int finalSeconds = Mathf.FloorToInt(finalElapsedTime);
                int finalMilliseconds = Mathf.FloorToInt((finalElapsedTime - finalSeconds) * 1000);


                // Update the Text elements one last time
                timerText.text = finalSeconds.ToString();
                timerText2.text = finalMilliseconds.ToString("D3");
                gamesCompletedP++;
                currentStreakP = 0;
                PlayerPrefs.SetFloat("GameTime_P" + gamesCompletedP, finalElapsedTime);
                PlayerPrefs.SetInt("GamesCompletedP", gamesCompletedP);
                PlayerPrefs.SetInt("HighStreakP", highStreakP);
                PlayerPrefs.SetInt("CurrentStreakP", currentStreakP);
                PlayerPrefs.Save();
                timeandstreakLoader.TimeandStreakLoad();
            }
            else if (FlagleManager.lose && SceneManager.GetActiveScene().name == "FlagleScene")
            {
                timerRunning = false;
                timerstopped = true;
                //Calculate the final elapsed time when stopping
                float finalElapsedTime = Time.time - startTime;
                actualTime.text = finalElapsedTime.ToString("F3") + " s";
                // Separate the seconds and milliseconds parts
                int finalSeconds = Mathf.FloorToInt(finalElapsedTime);
                int finalMilliseconds = Mathf.FloorToInt((finalElapsedTime - finalSeconds) * 1000);


                // Update the Text elements one last time
                timerText.text = finalSeconds.ToString();
                timerText2.text = finalMilliseconds.ToString("D3");
                gamesCompletedF++;
                currentStreakF = 0;
                PlayerPrefs.SetFloat("GameTime_F" + gamesCompletedF, finalElapsedTime);
                PlayerPrefs.SetInt("GamesCompletedF", gamesCompletedF);
                PlayerPrefs.SetInt("HighStreakF", highStreakF);
                PlayerPrefs.SetInt("CurrentStreakF", currentStreakF);
                PlayerPrefs.Save();
                timeandstreakLoader.TimeandStreakLoad();
            }
            else if (GlobleManager.lose && SceneManager.GetActiveScene().name == "GlobleScene")
            {
                timerRunning = false;
                timerstopped = true;
                //Calculate the final elapsed time when stopping
                float finalElapsedTime = Time.time - startTime;
                actualTime.text = finalElapsedTime.ToString("F3") + " s";
                // Separate the seconds and milliseconds parts
                int finalSeconds = Mathf.FloorToInt(finalElapsedTime);
                int finalMilliseconds = Mathf.FloorToInt((finalElapsedTime - finalSeconds) * 1000);


                // Update the Text elements one last time
                timerText.text = finalSeconds.ToString();
                timerText2.text = finalMilliseconds.ToString("D3");
                gamesCompletedG++;
                currentStreakG = 0;
                PlayerPrefs.SetFloat("GameTime_G" + gamesCompletedG, finalElapsedTime);
                PlayerPrefs.SetInt("GamesCompletedG", gamesCompletedG);
                PlayerPrefs.SetInt("HighStreakG", highStreakG);
                PlayerPrefs.SetInt("CurrentStreakG", currentStreakG);
                PlayerPrefs.Save();
                timeandstreakLoader.TimeandStreakLoad();
            }
            else if (SportleManager.lose && SceneManager.GetActiveScene().name == "SportleScene")
            {
                timerRunning = false;
                timerstopped = true;
                //Calculate the final elapsed time when stopping
                float finalElapsedTime = Time.time - startTime;
                actualTime.text = finalElapsedTime.ToString("F3") + " s";
                // Separate the seconds and milliseconds parts
                int finalSeconds = Mathf.FloorToInt(finalElapsedTime);
                int finalMilliseconds = Mathf.FloorToInt((finalElapsedTime - finalSeconds) * 1000);


                // Update the Text elements one last time
                timerText.text = finalSeconds.ToString();
                timerText2.text = finalMilliseconds.ToString("D3");
                gamesCompletedS++;
                currentStreakS = 0;
                PlayerPrefs.SetFloat("GameTime_S" + gamesCompletedS, finalElapsedTime);
                PlayerPrefs.SetInt("GamesCompletedS", gamesCompletedS);
                PlayerPrefs.SetInt("HighStreakS", highStreakS);
                PlayerPrefs.SetInt("CurrentStreakS", currentStreakS);
                PlayerPrefs.Save();
                timeandstreakLoader.TimeandStreakLoad();
            }

        }
        

    }

    
}
