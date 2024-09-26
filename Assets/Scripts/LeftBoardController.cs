using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeftBoardController : MonoBehaviour
{
    public Image leftBoard; // Reference to the Image GameObject
    public Vector3 inPosition; // Starting position of the image when it's hidden
    public Vector3 outPosition; // End position of the image when it's visible
    public float inScale = 1;
    public float outScale = 0.8f;
    public float animationDuration = 1f; // Duration of the slide animation
    private bool isIn = true;
    private bool isOut = false;
    public static int enterHelp = 0;


    void Start()
    {
        
    }

    private void Update()
    {
     
    }

    private IEnumerator SlideCoroutine(Vector3 startPosition, Vector3 targetPosition, float outScale, float inScale)
    {
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / animationDuration);
            leftBoard.rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, t);
            float currentScale = Mathf.Lerp(outScale, inScale, t);
            leftBoard.rectTransform.localScale = new Vector3(currentScale, currentScale, currentScale);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }   
    public void Slide()
    {
        if (isOut)
        {
            StartCoroutine(SlideCoroutine(outPosition, inPosition, outScale, inScale));
            isOut = false;
            isIn = true;
        }
        else if (isIn)
        {
            StartCoroutine(SlideCoroutine(inPosition, outPosition, inScale, outScale));
            isOut = true;
            isIn = false;
        }
    }
}
