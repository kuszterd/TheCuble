using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.EventSystems;

public class FlagleManager : MonoBehaviour
{
    public InputField inputField;
    public List<GameObject> gameObjects;
    //public GameObject[] gameObjects;
    private GameObject targetFlag;
    public Image targetFlagImage;
    private GameObject newCountry = null;
    private int distanceKm;
    private int distanceMile;    
    private string targetFlagName;
    public Animator wrongInput;
    public List<CountryPair> countryPairs = new List<CountryPair>(); // List to hold country pairs
    public bool isBorder = false;
    public Image guessedFlag1;
    public Image guessedFlag2;
    public Image guessedFlag3;
    public Image guessedFlag4;
    public Image guessedFlag5;
    public Image guessedDirection1;
    public Image guessedDirection2;
    public Image guessedDirection3;
    public Image guessedDirection4;
    public Image guessedDirection5;   
    public TextMeshProUGUI guessedDistance1;
    public TextMeshProUGUI guessedDistance2;
    public TextMeshProUGUI guessedDistance3;
    public TextMeshProUGUI guessedDistance4;
    public TextMeshProUGUI guessedDistance5;
    private int guessedKm1 = 50000;
    private int guessedKm2 = 50000;
    private int guessedKm3 = 50000;
    private int guessedKm4 = 50000;
    private int guessedKm5 = 50000;
    public Image Cover1;
    public Image Cover2;
    public Image Cover3;
    public Image Cover4;
    public Image Cover5; 
    public Image Cover6;
    private string actualDirection;
    private int guessedKmNext = 50000;
    private int enterPressCount = 0;
    private bool valid = false;
    public static bool win = false;
    public static bool normalWin = false;
    public static bool normalLose = false;
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
        SelecttargetCountry();
        Debug.Log(targetFlagName);
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
    //Data structure to hold a pair of countries
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
        int randomIndex = Random.Range(0, gameObjects.Count);
        targetFlag = gameObjects[randomIndex];
        // Store the name of the selected GameObject
        targetFlagName = targetFlag.name;
        targetFlagImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(targetFlagName); ;
    }

    private IEnumerator SelectInputField()
    {
        yield return new WaitForSeconds(0.1f);
        inputField.text = "";
        if (win)
        {

            inputField.text = targetFlagName;
            inputField.interactable = false;

        }
        else if (lose)
        {

            inputField.text = targetFlagName;
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
        StartCoroutine(SelectInputField());
    }
    private void Update()
    {
        

    }
    private void SortingDistanceValues()
    {
        int[] distanceValues = new int[] { guessedKm1, guessedKm2, guessedKm3, guessedKm4, guessedKm5, guessedKmNext };
        System.Array.Sort(distanceValues);
        // Step 4: Assign the sorted values back to the variables
        guessedKm1 = distanceValues[0];
        guessedKm2 = distanceValues[1];
        guessedKm3 = distanceValues[2];
        guessedKm4 = distanceValues[3];
        guessedKm5 = distanceValues[4];

        // Print the sorted values to verify
        Debug.Log($"a1: {guessedKm1}, a2: {guessedKm2}, a3: {guessedKm3}, a4: {guessedKm4}, a5: {guessedKm5}");
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
                guessedDistance1.gameObject.SetActive(true);
                guessedDirection1.gameObject.SetActive(true);

            }
            else if (enterPressCount == 2)
            {
                guessedFlag2.gameObject.SetActive(true);
                guessedDistance2.gameObject.SetActive(true);
                guessedDirection2.gameObject.SetActive(true);
            }
            else if (enterPressCount == 3)
            {
                guessedFlag3.gameObject.SetActive(true);
                guessedDistance3.gameObject.SetActive(true);
                guessedDirection3.gameObject.SetActive(true);
            }
            else if (enterPressCount == 4)
            {
                guessedFlag4.gameObject.SetActive(true);
                guessedDistance4.gameObject.SetActive(true);
                guessedDirection4.gameObject.SetActive(true);
            }
            else if (enterPressCount == 5)
            {
                guessedFlag5.gameObject.SetActive(true);
                guessedDistance5.gameObject.SetActive(true);
                guessedDirection5.gameObject.SetActive(true);
            }
            guessedFlag5.sprite = guessedFlag4.sprite;
            guessedDistance5.text = guessedDistance4.text;
            guessedDirection5.sprite = guessedDirection4.sprite;
            guessedFlag4.sprite = guessedFlag3.sprite;
            guessedDistance4.text = guessedDistance3.text;
            guessedDirection4.sprite = guessedDirection3.sprite;
            guessedFlag3.sprite = guessedFlag2.sprite;
            guessedDistance3.text = guessedDistance2.text;
            guessedDirection3.sprite = guessedDirection2.sprite;
            guessedFlag2.sprite = guessedFlag1.sprite;
            guessedDistance2.text = guessedDistance1.text;
            guessedDirection2.sprite = guessedDirection1.sprite;
            guessedFlag1.sprite = Resources.Load<Sprite>(newCountry.name);
            guessedDistance1.text = "0 km";
            guessedDirection1.gameObject.SetActive(false);
            victoryBoard.gameObject.SetActive(true);
            victory.gameObject.SetActive(true);
            
            Debug.Log("WIN!");
        }

        else if (enterPressCount == 1)
        {
            
            guessedFlag1.gameObject.SetActive(true);
            guessedDistance1.gameObject.SetActive(true);
            guessedDirection1.gameObject.SetActive(true);
            Animator animator = Cover1.GetComponent<Animator>();
            animator.SetBool("Flip", true);
            guessedKm1 = distanceKm;
            if (isBorder)
            {
                guessedKm1 = 0;
            }
            guessedFlag1.sprite = Resources.Load<Sprite>(newCountry.name);
            guessedDistance1.text = guessedKm1.ToString() + " km";
            guessedDirection1.sprite = Resources.Load<Sprite>(actualDirection);
            
                     
            SortingDistanceValues();
            
        }
        else if (enterPressCount == 2)
        {
            guessedFlag2.gameObject.SetActive(true);
            guessedDistance2.gameObject.SetActive(true);
            guessedDirection2.gameObject.SetActive(true);
            Animator animator = Cover2.GetComponent<Animator>();
            animator.SetBool("Flip", true);
            guessedKm2 = distanceKm;
            if (isBorder)
            {
                guessedKm2 = 0;
            }
            if (guessedKm2 <= guessedKm1)
            {
                guessedFlag2.sprite = guessedFlag1.sprite;
                guessedDistance2.text = guessedDistance1.text;
                guessedDirection2.sprite = guessedDirection1.sprite;
                guessedFlag1.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedDistance1.text = guessedKm2.ToString() + " km";
                guessedDirection1.sprite = Resources.Load<Sprite>(actualDirection);    
                SortingDistanceValues();
            }
            else
            {
                guessedFlag2.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedDistance2.text = guessedKm2.ToString() + " km";
                guessedDirection2.sprite = Resources.Load<Sprite>(actualDirection);
                SortingDistanceValues();
            }

        }
        else if (enterPressCount == 3)
        {
            guessedFlag3.gameObject.SetActive(true);
            guessedDistance3.gameObject.SetActive(true);
            guessedDirection3.gameObject.SetActive(true);
            Animator animator = Cover3.GetComponent<Animator>();
            animator.SetBool("Flip", true);
            guessedKm3 = distanceKm;
            if (isBorder)
            {
                guessedKm3 = 0;
            }
            if (guessedKm3 <= guessedKm1)
            {
                guessedFlag3.sprite = guessedFlag2.sprite;
                guessedDistance3.text = guessedDistance2.text;
                guessedDirection3.sprite = guessedDirection2.sprite;
                guessedFlag2.sprite = guessedFlag1.sprite;
                guessedDistance2.text = guessedDistance1.text;
                guessedDirection2.sprite = guessedDirection1.sprite;
                guessedFlag1.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedDistance1.text = guessedKm3.ToString() + " km";
                guessedDirection1.sprite = Resources.Load<Sprite>(actualDirection);
                SortingDistanceValues();

            }
            else if (guessedKm3 < guessedKm2 && guessedKm1 < guessedKm3)
            {
                guessedFlag3.sprite = guessedFlag2.sprite;
                guessedDistance3.text = guessedDistance2.text;
                guessedDirection3.sprite = guessedDirection2.sprite;
                guessedFlag2.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedDistance2.text = guessedKm3.ToString() + " km";
                guessedDirection2.sprite = Resources.Load<Sprite>(actualDirection);
                SortingDistanceValues();
            }
            else
            {
                guessedFlag3.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedDistance3.text = guessedKm3.ToString() + " km";
                guessedDirection3.sprite = Resources.Load<Sprite>(actualDirection);
                SortingDistanceValues();
            }

        }
        else if (enterPressCount == 4)
        {
            guessedFlag4.gameObject.SetActive(true);
            guessedDistance4.gameObject.SetActive(true);
            guessedDirection4.gameObject.SetActive(true);
            Animator animator = Cover4.GetComponent<Animator>();
            animator.SetBool("Flip", true);
            guessedKm4 = distanceKm;
            if (isBorder)
            {
                guessedKm4 = 0;
            }
            if (guessedKm4 <= guessedKm1)
            {
                guessedFlag4.sprite = guessedFlag3.sprite;
                guessedDistance4.text = guessedDistance3.text;
                guessedDirection4.sprite = guessedDirection3.sprite;
                guessedFlag3.sprite = guessedFlag2.sprite;
                guessedDistance3.text = guessedDistance2.text;
                guessedDirection3.sprite = guessedDirection2.sprite;
                guessedFlag2.sprite = guessedFlag1.sprite;
                guessedDistance2.text = guessedDistance1.text;
                guessedDirection2.sprite = guessedDirection1.sprite;
                guessedFlag1.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedDistance1.text = guessedKm4.ToString() + " km";
                guessedDirection1.sprite = Resources.Load<Sprite>(actualDirection);
                SortingDistanceValues();
            }
            else if (guessedKm4 < guessedKm2 && guessedKm1 < guessedKm4)
            {
                guessedFlag4.sprite = guessedFlag3.sprite;
                guessedDistance4.text = guessedDistance3.text;
                guessedDirection4.sprite = guessedDirection3.sprite;
                guessedFlag3.sprite = guessedFlag2.sprite;
                guessedDistance3.text = guessedDistance2.text;
                guessedDirection3.sprite = guessedDirection2.sprite;
                guessedFlag2.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedDistance2.text = guessedKm4.ToString() + " km";
                guessedDirection2.sprite = Resources.Load<Sprite>(actualDirection);
                SortingDistanceValues();
            }
            else if (guessedKm4 < guessedKm3 && guessedKm2 < guessedKm4)
            {
                guessedFlag4.sprite = guessedFlag3.sprite;
                guessedDistance4.text = guessedDistance3.text;
                guessedDirection4.sprite = guessedDirection3.sprite;
                guessedFlag3.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedDistance3.text = guessedKm4.ToString() + " km";
                guessedDirection3.sprite = Resources.Load<Sprite>(actualDirection);
                SortingDistanceValues();
            }

            else
            {
                guessedFlag4.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedDistance4.text = guessedKm4.ToString() + " km";
                guessedDirection4.sprite = Resources.Load<Sprite>(actualDirection);
                SortingDistanceValues();
            }
        }
        else if (enterPressCount == 5)
        {
            guessedFlag5.gameObject.SetActive(true);
            guessedDistance5.gameObject.SetActive(true);
            guessedDirection5.gameObject.SetActive(true);
            Animator animator = Cover5.GetComponent<Animator>();
            animator.SetBool("Flip", true);
            guessedKm5 = distanceKm;
            if (isBorder)
            {
                guessedKm5 = 0;
            }
            if (guessedKm5 <= guessedKm1)
            {
                guessedFlag5.sprite = guessedFlag4.sprite;
                guessedDistance5.text = guessedDistance4.text;
                guessedDirection5.sprite = guessedDirection4.sprite;
                guessedFlag4.sprite = guessedFlag3.sprite;
                guessedDistance4.text = guessedDistance3.text;
                guessedDirection4.sprite = guessedDirection3.sprite;
                guessedFlag3.sprite = guessedFlag2.sprite;
                guessedDistance3.text = guessedDistance2.text;
                guessedDirection3.sprite = guessedDirection2.sprite;
                guessedFlag2.sprite = guessedFlag1.sprite;
                guessedDistance2.text = guessedDistance1.text;
                guessedDirection2.sprite = guessedDirection1.sprite;
                guessedFlag1.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedDistance1.text = guessedKm5.ToString() + " km";
                guessedDirection1.sprite = Resources.Load<Sprite>(actualDirection);
                SortingDistanceValues();
            }
            else if (guessedKm5 < guessedKm2 && guessedKm1 < guessedKm5)
            {
                guessedFlag5.sprite = guessedFlag4.sprite;
                guessedDistance5.text = guessedDistance4.text;
                guessedDirection5.sprite = guessedDirection4.sprite;
                guessedFlag4.sprite = guessedFlag3.sprite;
                guessedDistance4.text = guessedDistance3.text;
                guessedDirection4.sprite = guessedDirection3.sprite;
                guessedFlag3.sprite = guessedFlag2.sprite;
                guessedDistance3.text = guessedDistance2.text;
                guessedDirection3.sprite = guessedDirection2.sprite;
                guessedFlag2.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedDistance2.text = guessedKm5.ToString() + " km";
                guessedDirection2.sprite = Resources.Load<Sprite>(actualDirection);
                SortingDistanceValues();
            }
            else if (guessedKm5 < guessedKm3 && guessedKm2 < guessedKm5)
            {
                guessedFlag5.sprite = guessedFlag4.sprite;
                guessedDistance5.text = guessedDistance4.text;
                guessedDirection5.sprite = guessedDirection4.sprite;
                guessedFlag4.sprite = guessedFlag3.sprite;
                guessedDistance4.text = guessedDistance3.text;
                guessedDirection4.sprite = guessedDirection3.sprite;
                guessedFlag3.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedDistance3.text = guessedKm5.ToString() + " km";
                guessedDirection3.sprite = Resources.Load<Sprite>(actualDirection);
                SortingDistanceValues();
            }
            else if (guessedKm5 < guessedKm4 && guessedKm3 < guessedKm5)
            {
                guessedFlag5.sprite = guessedFlag4.sprite;
                guessedDistance5.text = guessedDistance4.text;
                guessedDirection5.sprite = guessedDirection4.sprite;
                guessedFlag4.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedDistance4.text = guessedKm5.ToString() + " km";
                guessedDirection4.sprite = Resources.Load<Sprite>(actualDirection);
                SortingDistanceValues();
            }
            else
            {
                guessedFlag5.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedDistance5.text = guessedKm5.ToString() + " km";
                guessedDirection5.sprite = Resources.Load<Sprite>(actualDirection);
                SortingDistanceValues();
            }
        }
        else if (enterPressCount > 5)
        {
            lose = true;
            if (MainCameraController.isNormal)
            {
                normalLose = true;
            }
            victoryBoard.gameObject.SetActive(true);
            loseImage.gameObject.SetActive(true);
            guessedKmNext = distanceKm;
            Cover6.gameObject.SetActive(false);
            
            if (isBorder)
            {
                guessedKmNext = 0;
            }
            if (guessedKmNext <= guessedKm1)
            {
                guessedFlag5.sprite = guessedFlag4.sprite;
                guessedDistance5.text = guessedDistance4.text;
                guessedDirection5.sprite = guessedDirection4.sprite;
                guessedFlag4.sprite = guessedFlag3.sprite;
                guessedDistance4.text = guessedDistance3.text;
                guessedDirection4.sprite = guessedDirection3.sprite;
                guessedFlag3.sprite = guessedFlag2.sprite;
                guessedDistance3.text = guessedDistance2.text;
                guessedDirection3.sprite = guessedDirection2.sprite;
                guessedFlag2.sprite = guessedFlag1.sprite;
                guessedDistance2.text = guessedDistance1.text;
                guessedDirection2.sprite = guessedDirection1.sprite;
                guessedFlag1.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedDistance1.text = guessedKmNext.ToString() + " km";
                guessedDirection1.sprite = Resources.Load<Sprite>(actualDirection);
                SortingDistanceValues();
            }
            else if (guessedKmNext < guessedKm2 && guessedKm1 < guessedKmNext)
            {
                guessedFlag5.sprite = guessedFlag4.sprite;
                guessedDistance5.text = guessedDistance4.text;
                guessedDirection5.sprite = guessedDirection4.sprite;
                guessedFlag4.sprite = guessedFlag3.sprite;
                guessedDistance4.text = guessedDistance3.text;
                guessedDirection4.sprite = guessedDirection3.sprite;
                guessedFlag3.sprite = guessedFlag2.sprite;
                guessedDistance3.text = guessedDistance2.text;
                guessedDirection3.sprite = guessedDirection2.sprite;
                guessedFlag2.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedDistance2.text = guessedKmNext.ToString() + " km";
                guessedDirection2.sprite = Resources.Load<Sprite>(actualDirection);
                SortingDistanceValues();
            }
            else if (guessedKmNext < guessedKm3 && guessedKm2 < guessedKmNext)
            {
                guessedFlag5.sprite = guessedFlag4.sprite;
                guessedDistance5.text = guessedDistance4.text;
                guessedDirection5.sprite = guessedDirection4.sprite;
                guessedFlag4.sprite = guessedFlag3.sprite;
                guessedDistance4.text = guessedDistance3.text;
                guessedDirection4.sprite = guessedDirection3.sprite;
                guessedFlag3.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedDistance3.text = guessedKmNext.ToString() + " km";
                guessedDirection3.sprite = Resources.Load<Sprite>(actualDirection);
                SortingDistanceValues();
            }
            else if (guessedKmNext < guessedKm4 && guessedKm3 < guessedKmNext)
            {
                guessedFlag5.sprite = guessedFlag4.sprite;
                guessedDistance5.text = guessedDistance4.text;
                guessedDirection5.sprite = guessedDirection4.sprite;
                guessedFlag4.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedDistance4.text = guessedKmNext.ToString() + " km";
                guessedDirection4.sprite = Resources.Load<Sprite>(actualDirection);
                SortingDistanceValues();
            }
            else if (guessedKmNext < guessedKm5 && guessedKm4 < guessedKmNext)
            {
                guessedFlag5.sprite = Resources.Load<Sprite>(newCountry.name);
                guessedDistance5.text = guessedKmNext.ToString() + " km";
                guessedDirection5.sprite = Resources.Load<Sprite>(actualDirection);
                SortingDistanceValues();
            }
            else
            {
                Debug.Log("Not in the top 5!");
            }
        }
        isBorder = false;
        

    }
    public void CheckInput()
    {
        string userInput = inputField.text;
        valid = false;
        // Loop through the gameObjects array to find a match
        foreach (GameObject obj in gameObjects)
        {
            if (obj.name.Equals(userInput, System.StringComparison.OrdinalIgnoreCase))
            { 
                    // Country guessed correctly, now focus the camera on it
                    valid = true;

                    CheckWinOrBorder(obj);
                
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
        if (targetFlag != null && newCountry != null)
        {
            // Calculate the distance between the two GameObjects
            float distance = Vector3.Distance(targetFlag.transform.localPosition, newCountry.transform.localPosition);
            distanceKm = Mathf.RoundToInt(distance) * (140 + Mathf.RoundToInt(distance) / 2);
            distanceMile = Mathf.RoundToInt(distanceKm * 0.62f);    
            Debug.Log(distanceKm);
        }
    }
    public void CalculateDirection()
    {
        if (targetFlag != null && newCountry != null)
        {
            // Calculate the direction vector from target to newCountry
            Vector3 direction = targetFlag.transform.localPosition - newCountry.transform.localPosition;

            // Calculate the angle between the direction vector and the positive x-axis
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Normalize the angle to the range [0, 360)
            if (angle < 0f)
            {
                angle += 360f;
            }

            // Round the angle to the nearest cardinal direction
            string cardinalDirection = GetCardinalDirection(angle);
            actualDirection = cardinalDirection;
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
    private void CheckWinOrBorder(GameObject country)
    {
        newCountry = country;
        
        if (country.name.ToLower() == targetFlagName.ToLower())
        {
            win = true;
            CurrencyManager.profileCurrency += 50;
            PlayerPrefs.SetInt("ProfileCurrency", CurrencyManager.profileCurrency);
            PlayerPrefs.Save();
            if (MainCameraController.isNormal)
            {
                normalWin = true;
            }
            Animator animator = Cover1.GetComponent<Animator>();
            animator.SetBool("Flip", true);
            Animator animator2 = Cover2.GetComponent<Animator>();
            animator2.SetBool("Flip", true);
            Animator animator3 = Cover3.GetComponent<Animator>();
            animator3.SetBool("Flip", true);
            Animator animator4 = Cover4.GetComponent<Animator>();
            animator4.SetBool("Flip", true);
            Animator animator5 = Cover5.GetComponent<Animator>();
            animator5.SetBool("Flip", true);
            Animator animator6 = Cover6.GetComponent<Animator>();
            animator6.SetBool("Flip", true);
        }
        else
        {
            // Check if the guessed country shares a border with the target country
            foreach (CountryPair pair in countryPairs)
            {

                if ((pair.country1.ToLower() == targetFlagName.ToLower() && pair.country2.ToLower() == country.name.ToLower()) ||
                    (pair.country1.ToLower() == country.name.ToLower() && pair.country2.ToLower() == targetFlagName.ToLower()))
                {
                    isBorder = true;
                    break;
                }
                else
                {
                    isBorder = false;

                }
            }
        }
        CalculateDistance();
        CalculateDirection();        
    }
}

