using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapHandler : MonoBehaviour
{
    public Canvas canvas; // Reference to your canvas
    public GameObject targetObject;
    public GameObject objectToDrag;
    private bool mouseOn = false;
    private bool isDragging = false;
    private Vector3 offset;
    public float zoomSpeed = 1f; // Adjust this value to control the zoom speed
    public float minScale = 0.1f; // Minimum scale allowed
    public float maxScale = 10f; // Maximum scale allowed

    void Update()
    {
        
        // Check if the mouse is over the target object
        if (IsMouseOverObject(targetObject))
        {
            mouseOn = true;
            
        }
        else
        {
            mouseOn = false;
        }
        // Check for mouse button down to start dragging
        if (Input.GetMouseButtonDown(0) && mouseOn)
        {
            isDragging = true;
            offset = objectToDrag.transform.position - GetMouseWorldPosition();
        }
        // Check for mouse button up to stop dragging
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
        // If dragging, move the object with the mouse
        if (isDragging)
        {
            objectToDrag.transform.position = GetMouseWorldPosition() + offset;
        }
        // Zoom in/out with mouse scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f && mouseOn)
        {
            float newScale = Mathf.Clamp(objectToDrag.transform.localScale.x + scroll * zoomSpeed, minScale, maxScale);
            objectToDrag.transform.localScale = new Vector3(newScale, newScale, 1f);
        }

    }

    // Function to check if the mouse is over a specific object
    private bool IsMouseOverObject(GameObject obj)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        // Create a list to store the raycast results
        List<RaycastResult> results = new List<RaycastResult>();

        // Raycast from the mouse position and check if the hit object is the target object
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == obj)
            {
                return true;
            }
        }

        return false;
    }
    // Function to get mouse position in world coordinates
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.WorldToScreenPoint(objectToDrag.transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
