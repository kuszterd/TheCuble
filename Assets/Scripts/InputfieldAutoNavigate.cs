using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputfieldAutoNavigate : MonoBehaviour
{
    public InputField[] inputFields;
    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to the onValueChanged event of each input field
        for (int i = 0; i < inputFields.Length; i++)
        {
            int index = i; // Store current index in a local variable to avoid closure issues
            inputFields[i].onValueChanged.AddListener((value) => OnInputValueChanged(index));
        }
    }

    private void OnInputValueChanged(int currentIndex)
    {
        // If the current input field is not the last one and its text is not empty
        if (currentIndex < inputFields.Length - 1 && !string.IsNullOrEmpty(inputFields[currentIndex].text))
        {
            // Move focus to the next input field
            inputFields[currentIndex + 1].Select();
        }
    }
}
