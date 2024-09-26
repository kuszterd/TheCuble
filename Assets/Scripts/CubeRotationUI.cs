using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CubeRotationUI : MonoBehaviour
{

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Vector2 currentSwipe;
    // Define the minimum swipe distance for a swipe to be recognized
    private float minSwipeLength = 20f;
    private float maxTapTime = 0.1f;
    private float tapStartTime;
    [SerializeField] private float _rollSpeed = 5;
    public SideSelecterController controller;
    private bool canSwipeOrTap = true; // Flag to control swipe/tap detection cooldown



    private void Update()
    {
        if (canSwipeOrTap)
        {
            DetectSwipeOrTap();
        }


    }
    private void Start()
    {
        StartCoroutine(TitleTimer());
    }
    private IEnumerator Roll(Vector3 anchor, Vector3 axis) {

        for (var i = 0; i < 90 / _rollSpeed; i++) 
        {
            transform.RotateAround(anchor, axis, _rollSpeed);
            yield return new WaitForSeconds(0.01f);
        }

    }
    void Assemble(Vector3 dir)
    {
        var anchor = transform.position;
        var axis = Vector3.Cross(Vector3.back, dir);
        StartCoroutine(Roll(anchor, axis));
    }

    private void DetectSwipeOrTap()
    {

        // Check if there is at least one touch
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Record the start position of the touch
                    startTouchPosition = touch.position;
                    tapStartTime = Time.time;
                    break;

                case TouchPhase.Ended:
                    // Record the end position of the touch
                    endTouchPosition = touch.position;
                    currentSwipe = endTouchPosition - startTouchPosition;

                    // Only consider the swipe if it exceeds the minimum swipe length
                    if (currentSwipe.magnitude > minSwipeLength)
                    {
                        currentSwipe.Normalize();

                        // Determine the direction of the swipe
                        if (IsSwipeUp(currentSwipe))
                        {
                            Assemble(Vector3.up);
                            // Handle swipe up
                        }
                        else if (IsSwipeDown(currentSwipe))
                        {
                            Assemble(Vector3.down);
                            // Handle swipe down
                        }
                        else if (IsSwipeLeft(currentSwipe))
                        {
                            Assemble(Vector3.left);
                            // Handle swipe left
                        }
                        else if (IsSwipeRight(currentSwipe))
                        {
                            Assemble(Vector3.right);
                            // Handle swipe right
                        }
                        StartCoroutine(TitleTimer());
                        StartCoroutine(SwipeOrTapCooldown()); // Start cooldown after swipe
                    }
                    else if (Time.time - tapStartTime <= maxTapTime)
                    {
                        if (IsTapOnCube(touch))
                        {
                            HandleTap();
                            StartCoroutine(SwipeOrTapCooldown()); // Start cooldown after tap
                        }
                    }
                    break;
            }
            
        }
    }

    IEnumerator TitleTimer()
    {
        foreach (Image title in controller.titles)
        {
            title.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(1f);
        controller.TitleShow();
    }

    private bool IsSwipeUp(Vector2 swipe)
    {
        return swipe.y > 0 && Mathf.Abs(swipe.x) < Mathf.Abs(swipe.y);
    }

    private bool IsSwipeDown(Vector2 swipe)
    {
        return swipe.y < 0 && Mathf.Abs(swipe.x) < Mathf.Abs(swipe.y);
    }

    private bool IsSwipeLeft(Vector2 swipe)
    {
        return swipe.x < 0 && Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y);
    }

    private bool IsSwipeRight(Vector2 swipe)
    {
        return swipe.x > 0 && Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y);
    }

    public void HandleTap()
    {
        controller.TapSelect();       
    }
    private bool IsTapOnCube(Touch touch)
    {
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator SwipeOrTapCooldown()
    {
        canSwipeOrTap = false; // Disable further swipes/taps
        yield return new WaitForSeconds(1f); // Wait for 1 second
        canSwipeOrTap = true; // Re-enable swipes/taps
    }



}

