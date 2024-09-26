using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SideSelecterController : MonoBehaviour
{
    [SerializeField]
    GameObject cube;
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    CubeRotationUI cubeRotationUI;
    private string currentSide;
    public Animator transition;
    
    public float tolerance = 0.5f;
    Quaternion[] WordleRotations = { Quaternion.Euler(0f, 0f, 90f), Quaternion.Euler(0f, 0f, 0f), Quaternion.Euler(0f, 0f, -90f), Quaternion.Euler(0f, 0f, -180f) };
    Quaternion[] SportleRotations = { Quaternion.Euler(0f, 90f, -90f), Quaternion.Euler(-90f, 0f, 0f), Quaternion.Euler(0f, -90f, 90f), Quaternion.Euler(90f, 0f, -180f) };
    Quaternion[] FlagleRotations = { Quaternion.Euler(0f, 180f, 0f), Quaternion.Euler(0f, -180f, -90f), Quaternion.Euler(180f, 0f, 0f), Quaternion.Euler(0f, 180f, 90f) };
    Quaternion[] PaintleRotations = { Quaternion.Euler(0f, 90f, 90f), Quaternion.Euler(-90f, 0f, 180f), Quaternion.Euler(0f, -90f, -90f), Quaternion.Euler(90f, 0f, 0f) };
    Quaternion[] GlobleRotations = { Quaternion.Euler(-90f, 0f, -90f), Quaternion.Euler(0f, -90f, 0f), Quaternion.Euler(90f, 0f, 90f), Quaternion.Euler(180f, -90f, 0f) };
    Quaternion[] MathleRotations = { Quaternion.Euler(180f, 90f, 0f), Quaternion.Euler(90f, 0f, -90f), Quaternion.Euler(0f, 90f, 0f), Quaternion.Euler(-90f, 0f, 90f) };

    public Image titleW;
    public Image titleM;
    public Image titleP;
    public Image titleF;
    public Image titleG;
    public Image titleS;
    public List<Image> titles;
    Quaternion cubeRotation;

    public void Start()
    {

        LoadPosition();
        
    }

    public void SavePosition()
    {
        Quaternion cubeRotationSave = cube.transform.rotation;
        PlayerPrefs.SetFloat("CubeRotationX", cubeRotationSave.x);
        PlayerPrefs.SetFloat("CubeRotationY", cubeRotationSave.y);
        PlayerPrefs.SetFloat("CubeRotationZ", cubeRotationSave.z);
        PlayerPrefs.SetFloat("CubeRotationW", cubeRotationSave.w);
        PlayerPrefs.Save();
        
    }

    // Load the position of the cube
    public void LoadPosition()
    {
        if (PlayerPrefs.HasKey("CubeRotationX") && PlayerPrefs.HasKey("CubeRotationY") && PlayerPrefs.HasKey("CubeRotationZ") && PlayerPrefs.HasKey("CubeRotationW"))
        {
            float x = PlayerPrefs.GetFloat("CubeRotationX");
            float y = PlayerPrefs.GetFloat("CubeRotationY");
            float z = PlayerPrefs.GetFloat("CubeRotationZ");
            float w = PlayerPrefs.GetFloat("CubeRotationW");
            cube.transform.rotation = new Quaternion(x, y, z, w);       
        }
        else
        {
            Debug.Log("No saved Rotation found");
        }
    }
    void Update()
    {
        cubeRotation = cube.transform.rotation;
    }
   
    public void TitleShow()
    {
        
        if (IsSameRotation(cubeRotation, WordleRotations))
        {
            Debug.Log("Wordle"); 
            titleW.gameObject.SetActive(true);

        }
        else if (IsSameRotation(cubeRotation, SportleRotations))
        {
            Debug.Log("Sportle");
            titleS.gameObject.SetActive(true);

        }
        else if (IsSameRotation(cubeRotation, FlagleRotations))
        {
            Debug.Log("Flagle");
            titleF.gameObject.SetActive(true);

        }
        else if (IsSameRotation(cubeRotation, PaintleRotations))
        {
            Debug.Log("Paintle");
            titleP.gameObject.SetActive(true);

        }
        else if (IsSameRotation(cubeRotation, GlobleRotations))
        {
            Debug.Log("Globle");
            titleG.gameObject.SetActive(true);

        }
        else if (IsSameRotation(cubeRotation, MathleRotations))
        {
            Debug.Log("Mathle");
            titleM.gameObject.SetActive(true);

        }
    }

    bool IsSameRotation(Quaternion cubeRotation, Quaternion[] rotations)
    {
        // Iterate over each predefined rotation for the color
        foreach (Quaternion rotation in rotations)
        {
            // Check if the dot product of the cube's rotation and the predefined rotation is close to 1
            if (Mathf.Abs(Quaternion.Dot(cubeRotation, rotation)) > 0.99f)
            {
                 return true; // Cube's rotation matches one of the predefined rotations
            }
        }
            return false; // Cube's rotation does not match any of the predefined rotations
    }

    public void TapSelect()
    {
        SavePosition();
        Quaternion cubeRotation = cube.transform.rotation;
        
        transition.SetTrigger("Start");
        if (IsSameRotation(cubeRotation, WordleRotations))
        {
            Debug.Log("Wordle");
            currentSide = "WordleScene";
            StartCoroutine(LoadSideScene());
        }
        else if (IsSameRotation(cubeRotation, SportleRotations))
        {
            Debug.Log("Sportle");

            currentSide = "SportleScene";
            StartCoroutine(LoadSideScene());
        }
        else if (IsSameRotation(cubeRotation, FlagleRotations))
        {
            Debug.Log("Flagle");

            currentSide = "FlagleScene";
            StartCoroutine(LoadSideScene());
        }
        else if (IsSameRotation(cubeRotation, PaintleRotations))
        {
            Debug.Log("Paintle");

            currentSide = "PaintleScene";
            StartCoroutine(LoadSideScene());
        }
        else if (IsSameRotation(cubeRotation, GlobleRotations))
        {
            Debug.Log("Globle");

            currentSide = "GlobleScene";
            StartCoroutine(LoadSideScene());
        }
        else if (IsSameRotation(cubeRotation, MathleRotations))
        {
            Debug.Log("Mathle");

            currentSide = "MathleScene";
            StartCoroutine(LoadSideScene());
        }
    }

    private IEnumerator LoadSideScene()
    {
        yield return new WaitForSeconds(1f);
        if (currentSide == "WordleScene")
        {
            if(MainCameraController.isNormal && MainCameraController.playedTodayW == 1)
            {
                SceneManager.LoadScene("SideSelecter");       
            }
            else if (MainCameraController.isPractice)
            {
                if (CurrencyManager.profileCurrency >= 100)
                {
                    CurrencyManager.profileCurrency -= 100;
                    PlayerPrefs.SetInt("ProfileCurrency", CurrencyManager.profileCurrency);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene(currentSide);
                }
                else
                {
                    SceneManager.LoadScene("SideSelecter");      
                }
            }
            else
            {
                SceneManager.LoadScene(currentSide);
            }      
        }
        else if (currentSide == "SportleScene")
        {
            if (MainCameraController.isNormal && MainCameraController.playedTodayS == 1)
            {
                SceneManager.LoadScene("SideSelecter");
                
            }
            else if (MainCameraController.isPractice)
            {
                if (CurrencyManager.profileCurrency >= 100)
                {
                    CurrencyManager.profileCurrency -= 100;
                    PlayerPrefs.SetInt("ProfileCurrency", CurrencyManager.profileCurrency);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene(currentSide);
                }
                else
                {
                    SceneManager.LoadScene("SideSelecter");
                }
            }
            else
            {
                SceneManager.LoadScene(currentSide);
            }
        }
        else if (currentSide == "PaintleScene")
        {
            if (MainCameraController.isNormal && MainCameraController.playedTodayP == 1)
            {
                SceneManager.LoadScene("SideSelecter");
                
            }
            else if (MainCameraController.isPractice)
            {
                if (CurrencyManager.profileCurrency >= 100)
                {
                    CurrencyManager.profileCurrency -= 100;
                    PlayerPrefs.SetInt("ProfileCurrency", CurrencyManager.profileCurrency);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene(currentSide);
                }
                else
                {
                    SceneManager.LoadScene("SideSelecter");
                }
            }
            else
            {
                SceneManager.LoadScene(currentSide);
            }
        }
        else if (currentSide == "FlagleScene")
        {
            if (MainCameraController.isNormal && MainCameraController.playedTodayF == 1)
            {
                SceneManager.LoadScene("SideSelecter");
                
            }
            else if (MainCameraController.isPractice)
            {
                if (CurrencyManager.profileCurrency >= 100)
                {
                    CurrencyManager.profileCurrency -= 100;
                    PlayerPrefs.SetInt("ProfileCurrency", CurrencyManager.profileCurrency);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene(currentSide);
                }
                else
                {
                    SceneManager.LoadScene("SideSelecter");
                }
            }
            else
            {
                SceneManager.LoadScene(currentSide);
            }
        }
        else if (currentSide == "GlobleScene")
        {
            if (MainCameraController.isNormal && MainCameraController.playedTodayG == 1)
            {
                SceneManager.LoadScene("SideSelecter");
                
            }
            else if (MainCameraController.isPractice)
            {
                if (CurrencyManager.profileCurrency >= 100)
                {
                    CurrencyManager.profileCurrency -= 100;
                    PlayerPrefs.SetInt("ProfileCurrency", CurrencyManager.profileCurrency);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene(currentSide);
                }
                else
                {
                    SceneManager.LoadScene("SideSelecter");
                }
            }
            else
            {
                SceneManager.LoadScene(currentSide);
            }
        }
        else if (currentSide == "MathleScene")
        {
            if (MainCameraController.isNormal && MainCameraController.playedTodayM == 1)
            {
                SceneManager.LoadScene("SideSelecter");
                
            }
            else if (MainCameraController.isPractice)
            {
                if (CurrencyManager.profileCurrency >= 100)
                {
                    CurrencyManager.profileCurrency -= 100;
                    PlayerPrefs.SetInt("ProfileCurrency", CurrencyManager.profileCurrency);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene(currentSide);
                }
                else
                {
                    SceneManager.LoadScene("SideSelecter");
                }
            }
            else
            {
                SceneManager.LoadScene(currentSide);
            }
        }
    }
 }

