using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;


    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && Input.GetKeyDown(KeyCode.Return))
        {
            
            transition.SetTrigger("Start");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Escape();
            
        }

    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadPrieviusLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }

    public void Escape()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            LoadPrieviusLevel();
        }
        else if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            StartCoroutine(LoadLevel(1));
        }
        else
        {
            return;
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelIndex);
    }


}
