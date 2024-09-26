using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class RightBoardController : MonoBehaviour
{
    public Image rightBoard; // Reference to the Image GameObject
    public Vector3 inPosition; // Starting position of the image when it's hidden
    public Vector3 outPosition; // End position of the image when it's visible
    public float animationDuration = 1f; // Duration of the slide animation
    private bool isIn = false;
    private bool isOut = true;


    void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private IEnumerator SlideCoroutine(Vector3 startPosition, Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            float t = Mathf.Clamp01(elapsedTime / animationDuration);
            rightBoard.rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    public void Slide()
    {
        if (isOut)
        {
            StartCoroutine(SlideCoroutine(outPosition, inPosition));
            isOut = false;
            isIn = true;
        }
        else if (isIn)
        {
            StartCoroutine(SlideCoroutine(inPosition, outPosition));
            isOut = true;
            isIn = false;
        }
    }
}    

