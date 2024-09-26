using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class WordleGameManager : MonoBehaviour
{
    // List of valid words
    private List<string> validWords;
    // List of valid words
    private string targetWord;
    // List od input fields
    public List<InputField> inputFields1;
    public List<InputField> alphabet;
    // Index of the currently focused input field
    private int currentInputFieldIndex = 0;
    // Counter for Enter key presses
    private int enterKeyPressCount = 0;
    //win variable
    public static bool win = false;
    public static bool normalWin = false;
    public static bool normalLose = false;

    // Valid word check
    private bool valid = false;
    public static bool lose = false;
    // Duration of the scaling animation
    public float scaleDuration = 0.2f;    
    public Camera mainCamera;
    public Image victory;
    public Image loseImage;
    public Image victoryBoard;
    public Button[] buttons;
    private char[] letters = new char[26];
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
        

        // Load words from the text file
        LoadWordsFromFile("validwords");
        // Set the targetWord to be a random word from the 'words' list
        SetRandomTargetWord();
        //Subscribe to the onValueChanged event of each input field
        for (int i = 0; i < inputFields1.Count; i++)
        {
            int index = i; // Store current index in a local variable to avoid closure issues
            inputFields1[i].onValueChanged.AddListener((value) => OnInputValueChanged(index));
        }
        StartCoroutine(ScaleInputFieldsOverTime());
        
        foreach (InputField field in alphabet)
        {
            field.text = field.placeholder.GetComponent<Text>().text;
        }

        char letter = 'a';
        for (int i = 0; i < 26; i++)
        {
            letters[i] = letter;
            buttons[i].GetComponentInChildren<Text>().text = letter.ToString();
            letter++;
        }




    }  

    IEnumerator ScaleInputFieldsOverTime()
        {
            for (int i = enterKeyPressCount * 5; i < 5 + enterKeyPressCount * 5; i++)
            {
                inputFields1[i].gameObject.SetActive(true);
                RectTransform rectTransform = inputFields1[i].GetComponent<RectTransform>();
                rectTransform.localScale = Vector3.zero;
                // Gradually increase scale to 1 over time
                float elapsedTime = 0f;
                while (elapsedTime < scaleDuration)
                {
                    elapsedTime += Time.deltaTime;
                    rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsedTime / scaleDuration);
                    yield return null; // Wait for the next frame
                }
                // Ensure final scale is exactly 1
                rectTransform.localScale = Vector3.one;
            }
            SelectInputField(enterKeyPressCount * 5);
        }
        // Function to load words from a text file
        
    void LoadWordsFromFile(string validwords)
        {
            validWords = new List<string>();
            TextAsset wordListFile = Resources.Load<TextAsset>(validwords);
            if (wordListFile != null)
            {
                string[] words = wordListFile.text.Split('\n');
                foreach (string word in words)
                {
                    validWords.Add(word.Trim().ToLower()); // Trim whitespace and convert to lowercase
                }
            }
            else
            {
                Debug.LogError("Word list file not found: " + validwords);
            }
        }
        // Function to set the targetWord to be a random word from the validWords
        
    void SetRandomTargetWord()
        {
            // Generate a random index to select a word from the 'words' list
            int randomIndex = UnityEngine.Random.Range(0, validWords.Count);
            // Assign the randomly selected word as the targetWord
            targetWord = validWords[randomIndex];
            Debug.Log("Target word: " + targetWord);
        }
        // Function to check if a word is valid
        
    public bool IsWordValid(string word)
        {
            return validWords.Contains(word.ToLower()); // Convert to lowercase for case-insensitive comparison
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
                if (currentInputFieldIndex == enterKeyPressCount * 5)
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
        if(!win && !lose)
        {
            CheckWord();
            // Start a coroutine to set up new row from imputfields, when not wining, and word is valid
            if (!win && valid)
            {
                StartCoroutine(EnterPressed());
            }
        }
        
    }
    public void CheckWord()
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
                string enteredWord = GetWordFromInputFields(activeInputFields);
                // Check if the entered word is valid
                Debug.Log(enteredWord);
                if (IsWordValid(enteredWord))
                {

                        // Check if the entered word lenght is correct
                    CheckWordMatch(enteredWord);
                    valid = true;
                    Debug.Log(enteredWord + " is valid");                        
                    if (enteredWord == targetWord)
                    {                            
                        if (MainCameraController.isNormal)
                        {
                            normalWin = true;                                
                            CurrencyManager.profileCurrency += 50;                                
                            PlayerPrefs.SetInt("ProfileCurrency", CurrencyManager.profileCurrency);                                                     
                        }                            
                        win = true;                            
                        victoryBoard.gameObject.SetActive(true);                            
                        victory.gameObject.SetActive(true);
                    }

                }                    
                else                    
                {                        
                    //Error                        
                    Debug.Log(enteredWord + " is not a valid word.");                        
                    valid = false;                        
                    foreach (InputField field in activeInputFields)                        
                    {                           
                        Animator animator = field.GetComponent<Animator>();                            
                        //animator.SetBool("NotValid", true);                            
                        animator.Play("NotValidWord");                        
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
                    animator.Play("NotValidWord");                  
                }                   
                SelectInputField(currentInputFieldIndex);                
            }
        }
    }
        
    // Function to get active input fields
       
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
         //Getting a 5 letter word from the fields
        
    string GetWordFromInputFields(List<InputField> inputFields1)
    {
        string word = "";
        //Get the text from each input field and concatenate it to form the word
        foreach (InputField inputField in inputFields1)
        {
            word += inputField.text.Trim(); // Trim any leading or trailing whitespace
        }
        return word;
    }
    //Function to check if all active input fields are filled
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
    //Function to handle automatic navigation between input fields
    private void OnInputValueChanged(int currentIndex)
    {
        //If the current input field is not the last one and its text is not empty
        if (!string.IsNullOrEmpty(inputFields1[currentIndex].text))
        {
            //Move focus to the next input field
            SelectInputField(currentIndex + 1);
        }
    }
    //Function to select the specified input field and set caret position to the end
    private void SelectInputField(int index)
    {            
        inputFields1[index].Select();
        inputFields1[index].ActivateInputField();
        currentInputFieldIndex = index;
    }

    private void Update()
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
            loseSolution.text = "The target word was: " + targetWord;
        }
    }

    // Coroutine to delay the execution of code inside the if block
    private IEnumerator EnterPressed()        
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);
        // The code here will be executed after 1 second
        enterKeyPressCount++;
        if (enterKeyPressCount == 1)
        {
            for (int i = enterKeyPressCount * 5; i < 5 + enterKeyPressCount * 5; i++)
            {
                inputFields1[i].gameObject.SetActive(true);
                RectTransform rectTransform = inputFields1[i].GetComponent<RectTransform>();
                rectTransform.localScale = Vector3.zero;
                // Gradually increase scale to 1 over time
                float elapsedTime = 0f;
                while (elapsedTime < scaleDuration)
                {
                    elapsedTime += Time.deltaTime;
                    rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsedTime / scaleDuration);
                    yield return null; // Wait for the next frame
                }
                // Ensure final scale is exactly 1
                rectTransform.localScale = Vector3.one;
            }
            SelectInputField(enterKeyPressCount * 5);
        }
        else if (enterKeyPressCount == 2)
        {
            for (int i = enterKeyPressCount * 5; i < 5 + enterKeyPressCount * 5; i++)
            {
                inputFields1[i].gameObject.SetActive(true);
                RectTransform rectTransform = inputFields1[i].GetComponent<RectTransform>();
                rectTransform.localScale = Vector3.zero;
                // Gradually increase scale to 1 over time
                float elapsedTime = 0f;
                while (elapsedTime < scaleDuration)
                {
                    elapsedTime += Time.deltaTime;
                    rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsedTime / scaleDuration);
                    yield return null; // Wait for the next frame
                }
                // Ensure final scale is exactly 1
                rectTransform.localScale = Vector3.one;
            }
            SelectInputField(enterKeyPressCount * 5);
        }
        else if (enterKeyPressCount == 3)
        {
            for (int i = enterKeyPressCount * 5; i < 5 + enterKeyPressCount * 5; i++)
            {
                inputFields1[i].gameObject.SetActive(true);
                RectTransform rectTransform = inputFields1[i].GetComponent<RectTransform>();
                rectTransform.localScale = Vector3.zero;
                // Gradually increase scale to 1 over time
                float elapsedTime = 0f;
                while (elapsedTime < scaleDuration)
                {
                    elapsedTime += Time.deltaTime;
                    rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsedTime / scaleDuration);
                    yield return null; // Wait for the next frame
                }
                // Ensure final scale is exactly 1
                rectTransform.localScale = Vector3.one;
            }
            SelectInputField(enterKeyPressCount * 5);
        }
        else if (enterKeyPressCount == 4)
        {
            for (int i = enterKeyPressCount * 5; i < 5 + enterKeyPressCount * 5; i++)
            {
                inputFields1[i].gameObject.SetActive(true);
                RectTransform rectTransform = inputFields1[i].GetComponent<RectTransform>();
                rectTransform.localScale = Vector3.zero;
                // Gradually increase scale to 1 over time
                float elapsedTime = 0f;
                while (elapsedTime < scaleDuration)
                {
                    elapsedTime += Time.deltaTime;
                    rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsedTime / scaleDuration);
                    yield return null; // Wait for the next frame
                }
                // Ensure final scale is exactly 1
                rectTransform.localScale = Vector3.one;
            }
            SelectInputField(enterKeyPressCount * 5);
        }
    }
    // Function to check if the entered word matches the target word
    private void CheckWordMatch(string enteredWord)
    {
        List<char> targetLetters = new List<char>();            
        for (int i = 0; i < targetWord.Length; i++)
        {
            targetLetters.Add(targetWord[i]);
        }
        // Compare each letter in the entered word with the corresponding letter in the target word
        for (int i = 0; i < targetWord.Length; i++)
        {
            // Get the Animator component attached to the input field GameObject
            Animator animator = inputFields1[i + enterKeyPressCount * 5].GetComponent<Animator>();
            // If the letters match, continue to the next letter
            if (enteredWord[i] == targetLetters[i])
            {            
                // Trigger the animation by setting a trigger parameter
                animator.SetBool("Green", true);
                targetLetters[i] = '.';
                foreach (InputField alphabetCheck in alphabet)
                {
                    if (enteredWord[i].ToString().ToLower() == alphabetCheck.text.ToLower())
                    {

                        Animator animator2 = alphabetCheck.GetComponent<Animator>();
                        animator2.SetBool("Yellow", true);
                        if (!targetLetters.Contains(enteredWord[i]))
                        {
                            Animator animator3 = alphabetCheck.GetComponent<Animator>();
                            animator3.SetBool("YellowToGreen", true);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < targetWord.Length; i++)
            {
            Debug.Log(string.Join(", ", targetLetters));
            // If the letter is in the word but not in the correct position, change the background color to mismatched color
            if (targetLetters.Contains(enteredWord[i]) && targetLetters.IndexOf(enteredWord[i]) != '.')
            {
                // Get the Animator component attached to the input field GameObject
                Animator animator = inputFields1[i + enterKeyPressCount * 5].GetComponent<Animator>();
                // Trigger the animation by setting a trigger parameter                    
                animator.SetBool("Yellow", true);                
                Debug.Log(string.Join(", ", targetLetters));               
                int index = targetLetters.IndexOf(enteredWord[i]);               
                targetLetters[index] = '.';                
                foreach (InputField alphabetCheck in alphabet)
                {
                    if (alphabetCheck.text.ToLower() == enteredWord[i].ToString())
                    {
                        Animator animator2 = alphabetCheck.GetComponent<Animator>();
                        animator2.SetBool("Yellow", true);
                    }
                }
            }                
            else
            {                    
                // Get the Animator component attached to the input field GameObject
                Animator animator = inputFields1[i + enterKeyPressCount * 5].GetComponent<Animator>();       
                foreach (InputField alphabetCheck in alphabet)         
                {         
                    if (alphabetCheck.text.ToLower() == enteredWord[i].ToString())                   
                    {                    
                        Animator animator2 = alphabetCheck.GetComponent<Animator>();                   
                        animator2.SetBool("Grey", true);                    
                    }                 
                }                 
                // Trigger the animation by setting a trigger parameter             
                animator.SetBool("Grey", true);            
            }  
            inputFields1[i+ enterKeyPressCount * 5].interactable = false;          
        }       
    }
}




