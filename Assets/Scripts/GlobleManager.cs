using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;

public class GlobleManager : MonoBehaviour
{
    public InputField inputField;
    
    public GameObject[] gameObjects;
    public Color mapColor;
    private GameObject newCountry = null;
    private int distanceKm;
    private int distanceMile;
    public GameObject bigMap; // Reference to the big map GameObject containing all country GameObjects
    private string targetGameObjectName;
    private GameObject targetGameObject;
    public Color guessColor;
    private float normalizedData;
    public Animator wrongInput;
    public List<CountryPair> countryPairs = new List<CountryPair>(); // List to hold country pairs
    public bool isBoarder = false;
    public Image guessedFlag1;
    public Image guessedFlag2;
    public Image guessedFlag3;
    public Image guessedFlag4;
    public Image guessedFlag5;
    public TextMeshProUGUI guessedCountry1;
    public TextMeshProUGUI guessedCountry2;
    public TextMeshProUGUI guessedCountry3;
    public TextMeshProUGUI guessedCountry4;
    public TextMeshProUGUI guessedCountry5;
    public TextMeshProUGUI guessedDistance;
    public TextMeshProUGUI closestBorderText;
    public TextMeshProUGUI closestBorderKm;
    private int guessedDistance1 = 50000;
    private int guessedDistance2 = 50000;
    private int guessedDistance3 = 50000;
    private int guessedDistance4 = 50000;
    private int guessedDistance5 = 50000;
    private int guessedDistanceNext = 50000;
    private int enterPressCount = 0;
    private bool valid = false;
    public static bool win = false;
    public static bool normalWin = false;
    public static bool normalLose = false;
    public GameObject maskNormal;
    public GameObject maskPractice;
    public static bool lose = false;
    public Image victory;
    public Image loseImage;
    public Image victoryBoard;
    public static bool normalLoaded = false;


    private void Start()
    {
        win = false;
        lose = false;
        if (MainCameraController.isNormal)
        {
            normalLoaded = true;
        }
        if (MainCameraController.isNormal)
        {
            maskPractice.SetActive(false);
            maskNormal.SetActive(true);
        }
        else if (MainCameraController.isPractice)
        {
            maskNormal.SetActive(false);
            maskPractice.SetActive(true);
        }
        SelecttargetCountry();
        Debug.Log(targetGameObjectName);
        LoadCountryPairsFromCSV();
        
        
    }

    private void LoadCountryPairsFromCSV()
    {
        // Read all lines from the CSV file
        TextAsset csvFile = Resources.Load<TextAsset>("country_pairs"); // Load without the extension
        if (csvFile != null)
        {
            string[] lines = csvFile.text.Split('\n');
            // Parse each line to create country pairs
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue; // Skip empty lines
                string[] fields = line.Split(';');
                // Create a new CountryPair object and add it to the list
                if (fields.Length == 2) // Ensure there are exactly two fields
                {
                    // Create a new CountryPair object and add it to the list
                    countryPairs.Add(new CountryPair(fields[0].Trim(), fields[1].Trim()));
                }
            }
        }
    }
    // Data structure to hold a pair of countries
    public class CountryPair
    {
        public string country1;
        public string country2;
        public CountryPair(string c1, string c2)
        {
            country1 = c1;
            country2 = c2;
        }
    }
    void SelecttargetCountry()
    {
        // Select a random index from the list
        int randomIndex = Random.Range(0, gameObjects.Length);    
        targetGameObject = gameObjects[randomIndex];
        // Store the name of the selected GameObject
        targetGameObjectName = targetGameObject.name;
    }

   
    private void Update()
    {

    }

    private IEnumerator SelectInputField()
    {
        yield return new WaitForSeconds(0.1f);
        inputField.text = "";
        if (win)
        {

            inputField.text = targetGameObjectName;
            inputField.interactable = false;

        }
        else if (lose)
        {

            inputField.text = targetGameObjectName;
            inputField.interactable = false;
        }
        
    }

    public void OnEnterClick()
    {
        CheckInput();
        if (valid)
        {
            StartCoroutine(EnterPressed());

        }
        if (win)
        {
            victoryBoard.gameObject.SetActive(true);
            victory.gameObject.SetActive(true);
            
            Debug.Log("You are winnig");
        }
        StartCoroutine(SelectInputField());
    }
    private void SortingDistanceValues()
    {
        int[] distanceValues = new int[] { guessedDistance1, guessedDistance2, guessedDistance3, guessedDistance4, guessedDistance5, guessedDistanceNext };
        System.Array.Sort(distanceValues);
        // Step 4: Assign the sorted values back to the variables
        guessedDistance1 = distanceValues[0];
        guessedDistance2 = distanceValues[1];
        guessedDistance3 = distanceValues[2];
        guessedDistance4 = distanceValues[3];
        guessedDistance5 = distanceValues[4];
        
        // Print the sorted values to verify
        Debug.Log($"a1: {guessedDistance1}, a2: {guessedDistance2}, a3: {guessedDistance3}, a4: {guessedDistance4}, a5: {guessedDistance5}");
    }
    private IEnumerator EnterPressed()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(0f);
        // The code here will be executed after 1 second
        enterPressCount++;
        if (win)
        {
            if (enterPressCount == 1)
            {
                guessedFlag1.gameObject.SetActive(true);
                guessedCountry1.gameObject.SetActive(true);                
                closestBorderText.gameObject.SetActive(true);
            }
            else if (enterPressCount == 2)
            {
                guessedFlag2.gameObject.SetActive(true);
                guessedCountry2.gameObject.SetActive(true);
            }
            else if (enterPressCount == 3)
            {
                guessedFlag3.gameObject.SetActive(true);
                guessedCountry3.gameObject.SetActive(true);
            }
            else if (enterPressCount == 4)
            {
                guessedFlag4.gameObject.SetActive(true);
                guessedCountry4.gameObject.SetActive(true);
            }
            else if (enterPressCount == 5)
            {
                guessedFlag5.gameObject.SetActive(true);
                guessedCountry5.gameObject.SetActive(true);
            }            
            guessedFlag5.sprite = guessedFlag4.sprite;
            guessedCountry5.text = guessedCountry4.text;
            guessedFlag4.sprite = guessedFlag3.sprite;
            guessedCountry4.text = guessedCountry3.text;
            guessedFlag3.sprite = guessedFlag2.sprite;
            guessedCountry3.text = guessedCountry2.text;
            guessedFlag2.sprite = guessedFlag1.sprite;
            guessedCountry2.text = guessedCountry1.text;
            guessedFlag1.sprite = Resources.Load<Sprite>(newCountry.name);
            guessedCountry1.text = Resources.Load<Sprite>(newCountry.name).name;            
            guessedDistance.gameObject.SetActive(false);
            closestBorderText.text = "You are Winning!";
            

        }
    
        else if (enterPressCount == 1)
        {
            guessedFlag1.gameObject.SetActive(true);
            guessedCountry1.gameObject.SetActive(true);
            guessedDistance.gameObject.SetActive(true);
            closestBorderText.gameObject.SetActive(true);            
            guessedFlag1.sprite = Resources.Load<Sprite>(newCountry.name);
            guessedCountry1.text = Resources.Load<Sprite>(newCountry.name).name;
            guessedDistance1 = distanceKm;
            if (isBoarder)
            {
                guessedDistance1 = 0;
            }
            guessedDistance.text = guessedDistance1.ToString() + " km";
            SortingDistanceValues();
            if (isBoarder)
            {
                guessedDistance1 = 0;
            }


        }
        else if (enterPressCount == 2)
        {
            guessedFlag2.gameObject.SetActive(true);
            guessedCountry2.gameObject.SetActive(true);
            guessedDistance2 = distanceKm;
            if (isBoarder)
            {
                guessedDistance2 = 0;
            }
            if (guessedDistance2 <= guessedDistance1)
            {
                guessedFlag2.sprite = guessedFlag1.sprite;
                guessedCountry2.text = guessedCountry1.text;                
                guessedFlag1.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedCountry1.text = Resources.Load<Sprite>(newCountry.name).name;
                guessedDistance.text = guessedDistance2.ToString() + " km";
                SortingDistanceValues();



            }
            else
            {
                guessedFlag2.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedCountry2.text = Resources.Load<Sprite>(newCountry.name).name;
                SortingDistanceValues();
            }
            
        }
        else if (enterPressCount == 3)
        {
            guessedFlag3.gameObject.SetActive(true);
            guessedCountry3.gameObject.SetActive(true);
            guessedDistance3 = distanceKm;
            if (isBoarder)
            {
                guessedDistance3 = 0;
            }
            if (guessedDistance3 <= guessedDistance1)
            {
                guessedFlag3.sprite = guessedFlag2.sprite;
                guessedCountry3.text = guessedCountry2.text;
                guessedFlag2.sprite = guessedFlag1.sprite;
                guessedCountry2.text = guessedCountry1.text;
                guessedFlag1.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedCountry1.text = Resources.Load<Sprite>(newCountry.name).name;
                guessedDistance.text = guessedDistance3.ToString() + " km";
                SortingDistanceValues();

            }
            else if (guessedDistance3 < guessedDistance2 && guessedDistance1 < guessedDistance3)
            {
                guessedFlag3.sprite = guessedFlag2.sprite;
                guessedCountry3.text = guessedCountry2.text;
                guessedFlag2.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedCountry2.text = Resources.Load<Sprite>(newCountry.name).name;
                SortingDistanceValues();
            }
            else
            {
                guessedFlag3.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedCountry3.text = Resources.Load<Sprite>(newCountry.name).name;
                SortingDistanceValues();
            }

        }
        else if (enterPressCount == 4)
        {
            guessedFlag4.gameObject.SetActive(true);
            guessedCountry4.gameObject.SetActive(true);
            guessedDistance4 = distanceKm;
            if (isBoarder)
            {
                guessedDistance4 = 0;
            }
            if (guessedDistance4 <= guessedDistance1)
            {
                guessedFlag4.sprite = guessedFlag3.sprite;
                guessedCountry4.text = guessedCountry3.text;
                guessedFlag3.sprite = guessedFlag2.sprite;
                guessedCountry3.text = guessedCountry2.text;
                guessedFlag2.sprite = guessedFlag1.sprite;
                guessedCountry2.text = guessedCountry1.text;
                guessedFlag1.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedCountry1.text = Resources.Load<Sprite>(newCountry.name).name;
                guessedDistance.text = guessedDistance4.ToString() + " km";
                SortingDistanceValues();
            }
            else if (guessedDistance4 < guessedDistance2 && guessedDistance1 < guessedDistance4)
            {
                guessedFlag4.sprite = guessedFlag3.sprite;
                guessedCountry4.text = guessedCountry3.text;
                guessedFlag3.sprite = guessedFlag2.sprite;
                guessedCountry3.text = guessedCountry2.text;
                guessedFlag2.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedCountry2.text = Resources.Load<Sprite>(newCountry.name).name;
                SortingDistanceValues();
            }
            else if (guessedDistance4 < guessedDistance3 && guessedDistance2 < guessedDistance4)
            {
                guessedFlag4.sprite = guessedFlag3.sprite;
                guessedCountry4.text = guessedCountry3.text;             
                guessedFlag3.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedCountry3.text = Resources.Load<Sprite>(newCountry.name).name;
                SortingDistanceValues();
            }
            
            else
            {
                guessedFlag4.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedCountry4.text = Resources.Load<Sprite>(newCountry.name).name;
                SortingDistanceValues();
            }
        }
        else if(enterPressCount == 5)
        {
            guessedFlag5.gameObject.SetActive(true);
            guessedCountry5.gameObject.SetActive(true);
            guessedDistance5 = distanceKm;
            if (isBoarder)
            {
                guessedDistance5 = 0;
            }
            if (guessedDistance5 <= guessedDistance1)
            {
                guessedFlag5.sprite = guessedFlag4.sprite;
                guessedCountry5.text = guessedCountry4.text;
                guessedFlag4.sprite = guessedFlag3.sprite;
                guessedCountry4.text = guessedCountry3.text;
                guessedFlag3.sprite = guessedFlag2.sprite;
                guessedCountry3.text = guessedCountry2.text;
                guessedFlag2.sprite = guessedFlag1.sprite;
                guessedCountry2.text = guessedCountry1.text;
                guessedFlag1.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedCountry1.text = Resources.Load<Sprite>(newCountry.name).name;
                guessedDistance.text = guessedDistance5.ToString() + " km";
                SortingDistanceValues();
            }
            else if (guessedDistance5 < guessedDistance2 && guessedDistance1 < guessedDistance5)
            {
                guessedFlag5.sprite = guessedFlag4.sprite;
                guessedCountry5.text = guessedCountry4.text;
                guessedFlag4.sprite = guessedFlag3.sprite;
                guessedCountry4.text = guessedCountry3.text;
                guessedFlag3.sprite = guessedFlag2.sprite;
                guessedCountry3.text = guessedCountry2.text;
                guessedFlag2.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedCountry2.text = Resources.Load<Sprite>(newCountry.name).name;
                SortingDistanceValues();
            }
            else if (guessedDistance5 < guessedDistance3 && guessedDistance2 < guessedDistance5)
            {
                guessedFlag5.sprite = guessedFlag4.sprite;
                guessedCountry5.text = guessedCountry4.text;
                guessedFlag4.sprite = guessedFlag3.sprite;
                guessedCountry4.text = guessedCountry3.text;
                guessedFlag3.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedCountry3.text = Resources.Load<Sprite>(newCountry.name).name;
                SortingDistanceValues();
            }
            else if (guessedDistance5 < guessedDistance4 && guessedDistance3 < guessedDistance5)
            {
                guessedFlag5.sprite = guessedFlag4.sprite;
                guessedCountry5.text = guessedCountry4.text;
                guessedFlag4.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedCountry4.text = Resources.Load<Sprite>(newCountry.name).name;
                SortingDistanceValues();
            }
            else
            {
                guessedFlag5.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedCountry5.text = Resources.Load<Sprite>(newCountry.name).name;
                SortingDistanceValues();
            }
        }
        else if (enterPressCount == 6 && !win)
        {
            lose = true;
            if (MainCameraController.isNormal)
            {
                normalLose = true;
            }
            victoryBoard.gameObject.SetActive(true);
            loseImage.gameObject.SetActive(true);
            
            guessedFlag5.gameObject.SetActive(true);
            guessedCountry5.gameObject.SetActive(true);
            guessedDistance5 = distanceKm;
            if (isBoarder)
            {
                guessedDistance5 = 0;
            }
            if (guessedDistance5 <= guessedDistance1)
            {
                guessedFlag5.sprite = guessedFlag4.sprite;
                guessedCountry5.text = guessedCountry4.text;
                guessedFlag4.sprite = guessedFlag3.sprite;
                guessedCountry4.text = guessedCountry3.text;
                guessedFlag3.sprite = guessedFlag2.sprite;
                guessedCountry3.text = guessedCountry2.text;
                guessedFlag2.sprite = guessedFlag1.sprite;
                guessedCountry2.text = guessedCountry1.text;
                guessedFlag1.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedCountry1.text = Resources.Load<Sprite>(newCountry.name).name;
                guessedDistance.text = guessedDistance5.ToString() + " km";
                SortingDistanceValues();
            }
            else if (guessedDistance5 < guessedDistance2 && guessedDistance1 < guessedDistance5)
            {
                guessedFlag5.sprite = guessedFlag4.sprite;
                guessedCountry5.text = guessedCountry4.text;
                guessedFlag4.sprite = guessedFlag3.sprite;
                guessedCountry4.text = guessedCountry3.text;
                guessedFlag3.sprite = guessedFlag2.sprite;
                guessedCountry3.text = guessedCountry2.text;
                guessedFlag2.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedCountry2.text = Resources.Load<Sprite>(newCountry.name).name;
                SortingDistanceValues();
            }
            else if (guessedDistance5 < guessedDistance3 && guessedDistance2 < guessedDistance5)
            {
                guessedFlag5.sprite = guessedFlag4.sprite;
                guessedCountry5.text = guessedCountry4.text;
                guessedFlag4.sprite = guessedFlag3.sprite;
                guessedCountry4.text = guessedCountry3.text;
                guessedFlag3.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedCountry3.text = Resources.Load<Sprite>(newCountry.name).name;
                SortingDistanceValues();
            }
            else if (guessedDistance5 < guessedDistance4 && guessedDistance3 < guessedDistance5)
            {
                guessedFlag5.sprite = guessedFlag4.sprite;
                guessedCountry5.text = guessedCountry4.text;
                guessedFlag4.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedCountry4.text = Resources.Load<Sprite>(newCountry.name).name;
                SortingDistanceValues();
            }
            else
            {
                guessedFlag5.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedCountry5.text = Resources.Load<Sprite>(newCountry.name).name;
                SortingDistanceValues();
            }
            FocusMapOnCountry(targetGameObject);
            targetGameObject.GetComponent<MeshRenderer>().material.color = Color.grey;

        }
        isBoarder = false;

    }
    public void CheckInput()
    {
        valid = false;
        
        string userInput = inputField.text.Trim();
        // Loop through the gameObjects array to find a match
        foreach (GameObject obj in gameObjects)
        {
            if (obj.name.Equals(userInput, System.StringComparison.OrdinalIgnoreCase))
            {
                // Country guessed correctly, now focus the camera on it
                valid = true;
                
                FocusMapOnCountry(obj);
                return;            
            }
            
        }
        if (!valid)
        {
            wrongInput.Play("NotValidWord");
        }
    }
    public void CalculateDistance()
    {
        if (targetGameObject != null && newCountry != null)
        {
            // Calculate the distance between the two GameObjects
            float distance = Vector3.Distance(targetGameObject.transform.localPosition, newCountry.transform.localPosition);
            distanceKm = Mathf.RoundToInt(distance) * (140 + Mathf.RoundToInt(distance) / 2); 
            if (distanceKm > 20000)
            {
                distanceKm = 40000 - distanceKm;
            }
            distanceMile = Mathf.RoundToInt(distanceKm * 0.62f);
            
            if (isBoarder && !win)
            {                
                newCountry.GetComponent<MeshRenderer>().material.color = Color.red;
                Debug.Log("Boarder!");
            }
            else if (!win)
            {
                if (distanceKm > 10000)
                {
                    guessColor.r = 1f;
                    guessColor.g = 1f;
                    guessColor.b = 0.5f;
                    
                }
                else
                {
                    normalizedData = (float)distanceKm / 5000f;
                    guessColor.r = 1;
                    guessColor.g = normalizedData;
                    guessColor.b = 0;
                    
                }
                //Apply the scale factor to the bigMap
                newCountry.GetComponent<MeshRenderer>().material.color = guessColor;
            }
            Debug.Log(distanceKm);
        }    
    }  
    public void CalculateDirection()
    {
        if (targetGameObject != null && newCountry != null)
        {
            // Calculate the direction vector from target to newCountry
            Vector3 direction = targetGameObject.transform.localPosition - newCountry.transform.localPosition;

            // Calculate the angle between the direction vector and the positive x-axis
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Normalize the angle to the range [0, 360)
            if (angle < 0f)
            {
                angle += 360f;
            }

            // Round the angle to the nearest cardinal direction
            string cardinalDirection = GetCardinalDirection(angle);
            Debug.Log(cardinalDirection);
        }        
    }
    string GetCardinalDirection(float angle)
    {
        // Define the threshold angles for each cardinal direction
        float[] thresholds = { 22.5f, 67.5f, 112.5f, 157.5f, 202.5f, 247.5f, 292.5f, 337.5f };
        string[] directions = { "E", "NE", "N", "NW", "W", "SW", "S", "SE" };

        // Find the closest cardinal direction based on the angle
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (angle < thresholds[i])
            {
                return directions[i];
            }
        }

        // If angle is greater than 337.5 degrees, return "E" (wrap around)
        return "E";
    }
    private void FocusMapOnCountry(GameObject country)
    {
        newCountry = country;
        if (country.name.ToLower() == targetGameObjectName.ToLower())
        {
            win = true;
            CurrencyManager.profileCurrency += 50;
            PlayerPrefs.SetInt("ProfileCurrency", CurrencyManager.profileCurrency);
            PlayerPrefs.Save();
            if (MainCameraController.isNormal)
            {
                normalWin = true;
            }
            newCountry.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            // Check if the guessed country shares a border with the target country
            foreach (CountryPair pair in countryPairs)
            {

                if ((pair.country1.ToLower() == targetGameObjectName.ToLower() && pair.country2.ToLower() == country.name.ToLower()) ||
                    (pair.country1.ToLower() == country.name.ToLower() && pair.country2.ToLower() == targetGameObjectName.ToLower()))
                {
                    isBoarder = true;
                    break;
                }
            }            
        }
        CalculateDistance();
        //CalculateDirection();
        Vector3 targetPosition = country.transform.localPosition;
        targetPosition.x = ((0 - country.transform.localPosition.x) * bigMap.transform.localScale.x);
        targetPosition.y = ((0 - country.transform.localPosition.y) * bigMap.transform.localScale.y);
        bigMap.transform.localPosition = targetPosition;   
    }
}
