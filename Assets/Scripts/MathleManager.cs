using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MathleManager : MonoBehaviour
{   

    // List od input fields
    public List<InputField> inputFields1;
    public List<InputField> calculator;
    public List<InputField> equalsign;
    public List<InputField> results;
    List<string> numbers = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
    List<string> operators = new List<string>() { "0", "+", "-", "*", "/" };
    private string randomEquasion;
    // Index of the currently focused input field
    private int currentInputFieldIndex = 0;
    // Counter for Enter key presses
    private int enterKeyPressCount = 0;
    //win variable
    public static bool win = false;
    public static bool normalWin = false;
    public static bool normalLose = false;
    // Valid number check
    private bool valid = false;
    public static bool lose = false;
    public Image victory;
    public Image loseImage;
    public Image victoryBoard;
    public TextMeshProUGUI loseSolution;
    public static bool normalLoaded = false;


    // Start is called before the first frame update
    void Start()
    {
        win = false;
        lose = false;
        if (MainCameraController.isNormal)
        {
            normalLoaded = true;
        }
        foreach (InputField fields in inputFields1)
        {
            fields.keyboardType = (TouchScreenKeyboardType)(-1);
        }

        GenerateRandomEquasion();
        Debug.Log(randomEquasion);
        int result = EvaluateEquation(randomEquasion);
        while (result < 0)
        {
            Debug.Log("Equation is invalid. Regenerating...");
            randomEquasion = GenerateRandomEquasion();
            Debug.Log("Generated Equation: " + randomEquasion);
            result = EvaluateEquation(randomEquasion);
        }
        
        for (int i = 0; i < results.Count; i++)
        {
            results[i].text = result.ToString();
        }
        for (int i = 0; i < equalsign.Count; i++)
        {
            equalsign[i].text = "=";
        }
        foreach (InputField fields in calculator)
        {
            // Access the text property of the placeholder object
            fields.text = fields.placeholder.GetComponent<Text>().text;
        }

        //Subscribe to the onValueChanged event of each input field
        for (int i = 0; i < inputFields1.Count; i++)
        {
            int index = i; // Store current index in a local variable to avoid closure issues
            inputFields1[i].onValueChanged.AddListener((value) => OnInputValueChanged(index));
        }
        StartCoroutine(SetActiveInputFieldsOverTime());


    }

    IEnumerator SetActiveInputFieldsOverTime()
    {
        for (int i = enterKeyPressCount * 6; i < 6 + enterKeyPressCount * 6; i++)
        {
            inputFields1[i].gameObject.SetActive(true);
            yield return null;
        }
        SelectInputField(enterKeyPressCount * 6);
    }

    public void OnButtonClick(Button clickedButton)
    {

        inputFields1[currentInputFieldIndex].text = clickedButton.GetComponentInChildren<Text>().text;
    }

    public void OnDeleteClick()
    {
        if(!win && !lose)
        {
            if (string.IsNullOrEmpty(inputFields1[currentInputFieldIndex].text))
            {
                if (currentInputFieldIndex == enterKeyPressCount * 6)
                {
                    return;
                }
                else
                {
                    SelectInputField(currentInputFieldIndex - 1);
                    inputFields1[currentInputFieldIndex].text = "";
                }
            }
        }
        
    }
    public void OnEnterClick()
    {
        CheckEquasion();
        // Start a coroutine to set up new row from imputfields, when not wining, and word is valid
        if (!win && valid)
        {
            StartCoroutine(EnterPressed());
        }
    }
    private List<InputField> GetActiveInputFields()
    {
        List<InputField> activeInputFields = new List<InputField>();
        foreach (InputField inputField in inputFields1)
        {
            if (inputField.gameObject.activeInHierarchy && inputField.interactable)
            {
                activeInputFields.Add(inputField);
            }
        }
        return activeInputFields;
    }
    string GetWordFromInputFields(List<InputField> inputFields1)
    {
        string word = "";
        // Get the text from each input field and concatenate it to form the word
        foreach (InputField inputField in inputFields1)
        {
            word += inputField.text.Trim(); // Trim any leading or trailing whitespace
        }
        return word;
    }
    private bool AreAllInputFieldsFilled(List<InputField> inputFields)
    {
        foreach (InputField inputField in inputFields)
        {
            if (string.IsNullOrEmpty(inputField.text))
            {
                return false;
            }
        }
        return true;
    }
    private IEnumerator EnterPressed()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);
        // The code here will be executed after 1 second
        enterKeyPressCount++;
        if (enterKeyPressCount < 5 && !win)
        {
            equalsign[enterKeyPressCount].gameObject.SetActive(true);
            results[enterKeyPressCount].gameObject.SetActive(true);
        }
        
        if (enterKeyPressCount == 1 && !win)
        {
            for (int i = enterKeyPressCount * 6; i < 6 + enterKeyPressCount * 6; i++)
            {
                inputFields1[i].gameObject.SetActive(true);
                
            }
            SelectInputField(enterKeyPressCount * 6);
        }
        else if (enterKeyPressCount == 2 && !win)
        {
            for (int i = enterKeyPressCount * 6; i < 6 + enterKeyPressCount * 6; i++)
            {
                inputFields1[i].gameObject.SetActive(true);
                
                
            }
            SelectInputField(enterKeyPressCount * 6);
        }
        else if (enterKeyPressCount == 3 && !win)
        {
            for (int i = enterKeyPressCount * 6; i < 6 + enterKeyPressCount * 6; i++)
            {
                inputFields1[i].gameObject.SetActive(true);
                
            }
            SelectInputField(enterKeyPressCount * 6);
        }
        else if (enterKeyPressCount == 4 && !win)
        {
            for (int i = enterKeyPressCount * 6; i < 6 + enterKeyPressCount * 6; i++)
            {
                inputFields1[i].gameObject.SetActive(true);
                
            }
            SelectInputField(enterKeyPressCount * 6);
        }
    }
    private void OnInputValueChanged(int currentIndex)
    {

        // If the current input field is not the last one and its text is not empty
        if (!string.IsNullOrEmpty(inputFields1[currentIndex].text))
        {
            // Move focus to the next input field
            SelectInputField(currentIndex + 1);
        }

    }
    private void SelectInputField(int index)
    {
        inputFields1[index].Select();
        inputFields1[index].ActivateInputField();
        currentInputFieldIndex = index;
    }

    string GenerateRandomEquasion()
    {
        randomEquasion = "";

        // Generate the first character from the numbers list
        randomEquasion += numbers[UnityEngine.Random.Range(0, numbers.Count)];

        for (int i = 1; i < 5; i++)
        {
            // If the previous character is an operator, choose from numbers list
            if (operators.Contains(randomEquasion[i - 1].ToString()))
            {
                randomEquasion += numbers[UnityEngine.Random.Range(0, numbers.Count)];
            }
            // Otherwise, choose from operators list
            else
            {
                randomEquasion += operators[UnityEngine.Random.Range(0, operators.Count)];
            }

        }
        // Add the last character from the numbers list
        randomEquasion += numbers[UnityEngine.Random.Range(0, numbers.Count)];
        return randomEquasion;
    }
    int EvaluateEquation(string equation)
    {
        try
        {
            int result = (int)new System.Data.DataTable().Compute(equation, null);
            return result <= 999 ? result : -1;

        }
        catch (Exception)
        {
            return -1; // Return -1 for invalid equations
        }
    }


    public void CheckEquasion()
    {
        // Get the active input fields
        List<InputField> activeInputFields = GetActiveInputFields();
        
        // Check if there are active input fields
        if (activeInputFields.Count > 0)
        {
            // Check if all active input fields are filled
            if (AreAllInputFieldsFilled(activeInputFields))
            {
                
                // Get the word from active input fields
                string enteredEquasion = GetWordFromInputFields(activeInputFields);

                // Check if the entered word is valid
                Debug.Log(enteredEquasion);                
                if (EvaluateEquation(randomEquasion) == EvaluateEquation(enteredEquasion))
                {
                    // Check if the entered word lenght is correct
                    CheckEquasionMatch(enteredEquasion);
                    valid = true;
                    Debug.Log(enteredEquasion + " is valid");
                    if (enteredEquasion == randomEquasion)
                    {
                        if (MainCameraController.isNormal)
                        {
                            normalWin = true;
                        }
                        win = true;
                        CurrencyManager.profileCurrency += 50;
                        PlayerPrefs.SetInt("ProfileCurrency", CurrencyManager.profileCurrency);
                        PlayerPrefs.Save();
                        victoryBoard.gameObject.SetActive(true);
                        victory.gameObject.SetActive(true);
                        loseSolution.gameObject.SetActive(true);
                        loseSolution.text = "You are winning!";
                        Debug.Log("You are wining!");
                    }
                }
                else
                {
                    valid = false;
                    foreach (InputField field in activeInputFields)
                    {
                        Animator animator = field.GetComponent<Animator>();
                        
                        animator.Play("NotValidText");

                    }
                    
                    SelectInputField(currentInputFieldIndex);
                }
                

            }
            else
            {
                valid = false;
                foreach (InputField field in activeInputFields)
                {
                    Animator animator = field.GetComponent<Animator>();
                    //animator.SetBool("NotValid", true);
                    animator.Play("NotValidText");

                }
                SelectInputField(currentInputFieldIndex);
            }

        }
    }
    private void CheckEquasionMatch(string enteredEquasion)
    {
        List<char> targetEquasion = new List<char>();
        for (int i = 0; i < randomEquasion.Length; i++)
        {
            targetEquasion.Add(randomEquasion[i]);
        }
        // Compare each letter in the entered word with the corresponding letter in the target word
        for (int i = 0; i < randomEquasion.Length; i++)
        {
            // Get the Animator component attached to the input field GameObject
            Animator animator = inputFields1[i + enterKeyPressCount * 6].GetComponent<Animator>();
            // If the letters match, continue to the next letter
            if (enteredEquasion[i] == targetEquasion[i])
            {
                // Trigger the animation by setting a trigger parameter
                animator.SetBool("GreenText", true);
                targetEquasion[i] = '.';

                foreach (InputField calculatorCheck in calculator)
                {
                    if (calculatorCheck.text == enteredEquasion[i].ToString())
                    {
                        Animator animator2 = calculatorCheck.GetComponent<Animator>();
                        animator2.SetBool("YellowText", true);
                        if (!targetEquasion.Contains(enteredEquasion[i]))
                        {
                            Animator animator3 = calculatorCheck.GetComponent<Animator>();
                            animator3.SetBool("YellowToGreen", true);
                        }
                    }

                }
                

            }

        }
        for (int j = 0; j < randomEquasion.Length; j++)
        {
            // If the letter is in the word but not in the correct position, change the background color to mismatched color
            if (targetEquasion.Contains(enteredEquasion[j]))
            {
                // Get the Animator component attached to the input field GameObject
                Animator animator = inputFields1[j + enterKeyPressCount * 6].GetComponent<Animator>();
                
                // Trigger the animation by setting a trigger parameter
                animator.SetBool("YellowText", true);
                int index = targetEquasion.IndexOf(enteredEquasion[j]);
                targetEquasion[index] = '.';
                foreach (InputField calculatorCheck in calculator)
                {
                    if (calculatorCheck.text.ToLower() == enteredEquasion[j].ToString())
                    {
                        Animator animator2 = calculatorCheck.GetComponent<Animator>();
                        animator2.SetBool("YellowText", true);
                    }

                }
            }
            else
            {
                // Get the Animator component attached to the input field GameObject
                Animator animator = inputFields1[j + enterKeyPressCount * 6].GetComponent<Animator>();
                foreach (InputField calculatorCheck in calculator)
                {
                    if (calculatorCheck.text.ToLower() == enteredEquasion[j].ToString())
                    {
                        Animator animator2 = calculatorCheck.GetComponent<Animator>();
                        animator2.SetBool("GreyText", true);
                    }

                }
                // Trigger the animation by setting a trigger parameter
                animator.SetBool("GreyText", true);
            }
            inputFields1[j + enterKeyPressCount * 6].interactable = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        if (enterKeyPressCount > 4 && !win)
        {
            lose = true;
            if (MainCameraController.isNormal)
            {
                normalLose = true;
            }
            victoryBoard.gameObject.SetActive(true);
            loseImage.gameObject.SetActive(true);
            loseSolution.gameObject.SetActive(true);
            int result = EvaluateEquation(randomEquasion);
            loseSolution.text = randomEquasion + " = " + result;

        }
    }
}
