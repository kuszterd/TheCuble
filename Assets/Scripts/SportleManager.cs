using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

public class SportleManager : MonoBehaviour
{
    public TextMeshProUGUI correction;
    private string actualClosestSport;
    private int enterKeyPressCount = 0;
    //win variable
    public static bool win = false;
    public static bool normalWin = false;
    public static bool lose = false;
    public static bool normalLose = false;
    // Valid word check
    private bool valid = false;
    private Sport targetSport;
    private string targetSportName;
    public Animator wrongInput;
    public List<Sport> sports = new List<Sport>();    
    public List<TMP_InputField> sportGuess1;
    public List<TMP_InputField> sportGuess2;
    public List<TMP_InputField> sportGuess3;
    public List<TMP_InputField> sportGuess4;
    public List<TMP_InputField> sportGuess5;
    public List<TMP_InputField> sportGuess6;
    public List<List<TMP_InputField>> allInputFieldLists;
    public List<Image> sportImage;
    public List<Image> sportImageBackgound;
    public Image winSportImage;
    public InputField userInputField;
    public TMP_InputField guessedSeason;
    public TMP_InputField guessedTypeOfCompetition;
    public TMP_InputField guessedVenue;
    public TMP_InputField guessedEquipment;
    public TMP_InputField guessedSkills;
    public TMP_InputField guessedEnvironmentTerrain;    
    public TMP_InputField guessedScoringMethod;
    private List<string> partialElements = new List<string>();
    public Image victory;
    public Image loseImage;
    public Image victoryBoard;
    public Image guessedImageBackground;
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
        LoadSportsFromCsv();
        SelectRandomSport();
        allInputFieldLists = new List<List<TMP_InputField>>() { sportGuess1, sportGuess2, sportGuess3, sportGuess4, sportGuess5, sportGuess6 };
        

    }
    private void Update()
    { 
        if (enterKeyPressCount > 5 && !win)
        {
            lose = true;
            if (MainCameraController.isNormal)
            {
                normalLose = true;
            }
            victoryBoard.gameObject.SetActive(true);
            loseImage.gameObject.SetActive(true);
            guessedSeason.text = string.Join(", ", targetSport.Season);
            guessedTypeOfCompetition.text = string.Join(", ", targetSport.TypeOfCompetition);
            guessedVenue.text = string.Join(", ", targetSport.Venue);
            guessedEquipment.text = string.Join(", ", targetSport.Equipment);
            guessedSkills.text = string.Join(", ", targetSport.Skills);
            guessedEnvironmentTerrain.text = string.Join(", ", targetSport.EnvironmentTerrain);
            guessedScoringMethod.text = string.Join(", ", targetSport.ScoringMethod);
            guessedImageBackground.color = Color.red;
            winSportImage.sprite = targetSport.sportImage;

        }
    }
    private IEnumerator SelectInputField()
    {
        yield return new WaitForSeconds(0f);
        userInputField.text = "";
        if (win)
        {

            userInputField.text = targetSportName;
            userInputField.interactable = false;
            guessedImageBackground.color = Color.green;

        }
        else if (lose)
        {

            userInputField.text = targetSportName;
            userInputField.interactable = false;
        }
     
    }

    public void OnEnterClick()
    {
        CheckInput();
        StartCoroutine(SelectInputField());
        
    }
    void LoadSportsFromCsv()
    {
        TextAsset csvData = Resources.Load<TextAsset>("Sportle,database");
        if (csvData != null)
        {
            using (StringReader reader = new StringReader(csvData.text))
            {
                string headerLine = reader.ReadLine(); // Skip the header line
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] fields = line.Split(';');
                    Sport sport = new Sport
                    {
                        Name = fields[0],
                        Season = new List<string>(fields[1].Split('/')),
                        TypeOfCompetition = new List<string>(fields[2].Split('/')),
                        Venue = new List<string>(fields[3].Split('/')),
                        Equipment = new List<string>(fields[4].Split('/')),
                        Skills = new List<string>(fields[5].Split('/')),
                        EnvironmentTerrain = new List<string>(fields[6].Split('/')),                        
                        ScoringMethod = new List<string>(fields[7].Split('/')),
                        sportImage = Resources.Load<Sprite>("Sports/" + fields[0]),

                    };
                    sports.Add(sport);
                }
            }
        }
        else
        {
            Debug.LogError("sports.csv not found in Resources folder.");
        }
    }
    void SelectRandomSport()
    {
        // Select a random index from the list
        int randomIndex = UnityEngine.Random.Range(0, sports.Count);
        targetSport = sports[randomIndex];
        // Store the name of the selected GameObject
        targetSportName = targetSport.Name;
        Debug.Log(targetSportName);
    }
    public void RevealLists()
    {
        foreach (var inputField in allInputFieldLists[enterKeyPressCount])
        {
            inputField.gameObject.SetActive(true);
        }
        sportImage[enterKeyPressCount].gameObject.SetActive(true);
        sportImageBackgound[enterKeyPressCount].gameObject.SetActive(true);
        if (sportImage.Count > 0)
        {
            sportImage[enterKeyPressCount].gameObject.SetActive(true);
        }
            
    }   

    public void CheckInput()
    {
        string userInput = userInputField.text.Trim().ToLower();
        // Loop through the gameObjects array to find a match
        if(userInput == targetSportName.ToLower())
        {
            win = true;
            CurrencyManager.profileCurrency += 50;
            PlayerPrefs.SetInt("ProfileCurrency", CurrencyManager.profileCurrency);
            PlayerPrefs.Save();
            if (MainCameraController.isNormal)
            {
                normalWin = true;
                MainCameraController.playedTodayS = 1;
                PlayerPrefs.SetInt("PlayedTodayS", MainCameraController.playedTodayS);
                PlayerPrefs.Save();
            }
            victoryBoard.gameObject.SetActive(true);
            victory.gameObject.SetActive(true);
            if (win)
            {                            
                winSportImage.sprite = targetSport.sportImage;
                
                
            }
            
        }

        

        foreach (Sport obj in sports)
        {
            if (obj.Name.ToLower().Trim() == userInput.ToLower().Trim())
            {
                valid = true;
                
                if (enterKeyPressCount < 6 && valid)
                {
                    List<TMP_InputField> currentInputFieldList = allInputFieldLists[enterKeyPressCount];                                       
                    sportImage[enterKeyPressCount].sprite = obj.sportImage;
                    currentInputFieldList[0].text = string.Join(", ", obj.Season);
                    currentInputFieldList[1].text = string.Join(", ", obj.TypeOfCompetition);
                    currentInputFieldList[2].text = string.Join(", ", obj.Venue);
                    currentInputFieldList[3].text = string.Join(", ", obj.Equipment);
                    currentInputFieldList[4].text = string.Join(", ", obj.Skills);
                    currentInputFieldList[5].text = string.Join(", ", obj.EnvironmentTerrain);                    
                    currentInputFieldList[6].text = string.Join(", ", obj.ScoringMethod);                   
                    RevealLists();
                    enterKeyPressCount++;
                    if (AreListsEqual(targetSport.Season, obj.Season))
                    {
                        Animator animator = currentInputFieldList[0].GetComponent<Animator>();
                        animator.Play("SportleGreen");
                        animator = guessedSeason.GetComponent<Animator>();
                        animator.Play("SportleGreen");
                        guessedSeason.text = string.Join(", ", targetSport.Season);
                    }
                    else if (HasPartialElement(targetSport.Season, obj.Season))
                    {
                        Animator animator = currentInputFieldList[0].GetComponent<Animator>();
                        animator.Play("SportleYellow");
                        if (guessedSeason.image.color != Color.green)
                        {
                            animator = guessedSeason.GetComponent<Animator>();
                            animator.Play("SportleYellow");
                        }
                        List<String> gSeasonList = guessedSeason.text.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                        foreach (string skill in partialElements)
                        {
                            if (!gSeasonList.Contains(skill))
                            {
                                gSeasonList.Add(skill);
                                if (AreListsEqual(gSeasonList, targetSport.Season))
                                {         
                                    animator = guessedSeason.GetComponent<Animator>();
                                    animator.Play("SportleGreen");
                                }
                            }
                        }
                        guessedSeason.text = string.Join(", ", gSeasonList);
                        
                    }
                    else
                    {
                        Animator animator = currentInputFieldList[0].GetComponent<Animator>();
                        animator.Play("SportleGrey");
                        if (guessedSeason.image.color != Color.green && guessedSeason.image.color != Color.yellow)
                        {
                            animator = guessedSeason.GetComponent<Animator>();
                            animator.Play("SportleGrey");
                        }
                    }

                    if (AreListsEqual(targetSport.TypeOfCompetition, obj.TypeOfCompetition))
                    {
                        Animator animator2 = currentInputFieldList[1].GetComponent<Animator>();
                        animator2.Play("SportleGreen");
                        animator2 = guessedTypeOfCompetition.GetComponent<Animator>();
                        animator2.Play("SportleGreen");
                        guessedTypeOfCompetition.text = string.Join(", ", targetSport.TypeOfCompetition);



                    }
                    else if (HasPartialElement(targetSport.TypeOfCompetition, obj.TypeOfCompetition))
                    {
                        Animator animator2 = currentInputFieldList[1].GetComponent<Animator>();
                        animator2.Play("SportleYellow");
                        if (guessedTypeOfCompetition.image.color != Color.green)
                        {
                            animator2 = guessedTypeOfCompetition.GetComponent<Animator>();
                            animator2.Play("SportleYellow");
                        }
                        List<String> gTypeOfCompetitionList = guessedTypeOfCompetition.text.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                        foreach (string skill in partialElements)
                        {
                            if (!gTypeOfCompetitionList.Contains(skill))
                            {
                                gTypeOfCompetitionList.Add(skill);
                                if (AreListsEqual(gTypeOfCompetitionList, targetSport.TypeOfCompetition))
                                {       
                                    animator2 = guessedTypeOfCompetition.GetComponent<Animator>();
                                    animator2.Play("SportleGreen");
                                }
                            }
                        }
                        guessedTypeOfCompetition.text = string.Join(", ", gTypeOfCompetitionList);
                        
                    }
                    else
                    {
                        Animator animator2 = currentInputFieldList[1].GetComponent<Animator>();
                        animator2.Play("SportleGrey");
                        if (guessedTypeOfCompetition.image.color != Color.green && guessedTypeOfCompetition.image.color != Color.yellow)
                        {
                            animator2 = guessedTypeOfCompetition.GetComponent<Animator>();
                            animator2.Play("SportleGrey");
                        }
                    }


                    if (AreListsEqual(targetSport.Venue, obj.Venue))
                    {
                        Animator animator3 = currentInputFieldList[2].GetComponent<Animator>();
                        animator3.Play("SportleGreen");
                        animator3 = guessedVenue.GetComponent<Animator>();
                        animator3.Play("SportleGreen");
                        guessedVenue.text = string.Join(", ", targetSport.Venue);

                    }
                    else if (HasPartialElement(targetSport.Venue, obj.Venue))
                    {
                        Animator animator3 = currentInputFieldList[2].GetComponent<Animator>();
                        animator3.Play("SportleYellow");
                        if (guessedVenue.image.color != Color.green)
                        {
                            animator3 = guessedVenue.GetComponent<Animator>();
                            animator3.Play("SportleYellow");
                        }
                        List<String> gVenueList = guessedVenue.text.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                        foreach (string skill in partialElements)
                        {
                            if (!gVenueList.Contains(skill))
                            {
                                gVenueList.Add(skill);
                                if (AreListsEqual(gVenueList, targetSport.Venue))
                                {
                                    animator3 = guessedVenue.GetComponent<Animator>();
                                    animator3.Play("SportleGreen");
                                }
                            }
                        }
                        guessedVenue.text = string.Join(", ", gVenueList);
                        
                    }
                    else
                    {
                        Animator animator3 = currentInputFieldList[2].GetComponent<Animator>();
                        animator3.Play("SportleGrey");
                        if (guessedVenue.image.color != Color.green && guessedVenue.image.color != Color.yellow)
                        {
                            animator3 = guessedVenue.GetComponent<Animator>();
                            animator3.Play("SportleGrey");
                        }
                    }

                    if (AreListsEqual(targetSport.Equipment, obj.Equipment))
                    {
                        Animator animator4 = currentInputFieldList[3].GetComponent<Animator>();
                        animator4.Play("SportleGreen");
                        animator4 = guessedEquipment.GetComponent<Animator>();
                        animator4.Play("SportleGreen");
                        guessedEquipment.text = string.Join(", ", targetSport.Equipment);
                    }
                    else if (HasPartialElement(targetSport.Equipment, obj.Equipment))
                    {
                        Animator animator4 = currentInputFieldList[3].GetComponent<Animator>();
                        animator4.Play("SportleYellow");
                        if (guessedEquipment.image.color != Color.green)
                        {
                            animator4 = guessedEquipment.GetComponent<Animator>();
                            animator4.Play("SportleYellow");
                        }
                        List<String> gEquipmentList = guessedEquipment.text.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                        foreach (string skill in partialElements)
                        {
                            if (!gEquipmentList.Contains(skill))
                            {
                                gEquipmentList.Add(skill);
                                if (AreListsEqual(gEquipmentList, targetSport.Equipment))
                                {
                                    animator4 = guessedEquipment.GetComponent<Animator>();
                                    animator4.Play("SportleGreen");
                                }
                            }
                        }
                        guessedEquipment.text = string.Join(", ", gEquipmentList);
                        
                    }
                    else
                    {
                        Animator animator4 = currentInputFieldList[3].GetComponent<Animator>();
                        animator4.Play("SportleGrey");
                        if (guessedEquipment.image.color != Color.green && guessedEquipment.image.color != Color.yellow)
                        {
                            animator4 = guessedEquipment.GetComponent<Animator>();
                            animator4.Play("SportleGrey");
                        }
                    }

                    if (AreListsEqual(targetSport.Skills, obj.Skills))
                    {
                        Animator animator5 = currentInputFieldList[4].GetComponent<Animator>();
                        animator5.Play("SportleGreen");
                        animator5 = guessedSkills.GetComponent<Animator>();
                        animator5.Play("SportleGreen");
                        guessedSkills.text = string.Join(", ", targetSport.Skills);
                    }
                    else if (HasPartialElement(targetSport.Skills, obj.Skills))
                    {
                        Animator animator5 = currentInputFieldList[4].GetComponent<Animator>();
                        animator5.Play("SportleYellow");
                        if (guessedSkills.image.color != Color.green)
                        {
                            animator5 = guessedSkills.GetComponent<Animator>();
                            animator5.Play("SportleYellow");
                        }
                        List<String> gSkillsList = guessedSkills.text.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                        foreach (string skill in partialElements)
                        {
                            if (!gSkillsList.Contains(skill))
                            {
                                gSkillsList.Add(skill);
                                if (AreListsEqual(gSkillsList, targetSport.Skills))
                                {
                                    animator5 = guessedSkills.GetComponent<Animator>();
                                    animator5.Play("SportleGreen");
                                }
                            }
                        }
                        guessedSkills.text = string.Join(", ", gSkillsList);
                        
                    }
                    else
                    {
                        Animator animator5 = currentInputFieldList[4].GetComponent<Animator>();
                        animator5.Play("SportleGrey");
                        if (guessedSkills.image.color != Color.green && guessedSkills.image.color != Color.yellow)
                        {
                            animator5 = guessedSkills.GetComponent<Animator>();
                            animator5.Play("SportleGrey");
                        }
                    }

                    if (AreListsEqual(targetSport.EnvironmentTerrain, obj.EnvironmentTerrain))
                    {
                        Animator animator6 = currentInputFieldList[5].GetComponent<Animator>();
                        animator6.Play("SportleGreen");
                        animator6 = guessedEnvironmentTerrain.GetComponent<Animator>();
                        animator6.Play("SportleGreen");
                        guessedEnvironmentTerrain.text = string.Join(", ", targetSport.EnvironmentTerrain);
                    }
                    else if (HasPartialElement(targetSport.EnvironmentTerrain, obj.EnvironmentTerrain))
                    {
                        Animator animator6 = currentInputFieldList[5].GetComponent<Animator>();
                        animator6.Play("SportleYellow");
                        if (guessedEnvironmentTerrain.image.color != Color.green)
                        {
                            animator6 = guessedEnvironmentTerrain.GetComponent<Animator>();
                            animator6.Play("SportleYellow");
                        }
                        List<String> gEnvironmentTerrainList = guessedEnvironmentTerrain.text.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                        foreach (string skill in partialElements)
                        {
                            if (!gEnvironmentTerrainList.Contains(skill))
                            {
                                
                                gEnvironmentTerrainList.Add(skill);
                                if (AreListsEqual(gEnvironmentTerrainList, targetSport.EnvironmentTerrain))
                                {
                                    animator6 = guessedEnvironmentTerrain.GetComponent<Animator>();
                                    animator6.Play("SportleGreen");
                                }
                            }
                        }
                        guessedEnvironmentTerrain.text = string.Join(", ", gEnvironmentTerrainList);
                        
                    }
                    else
                    {
                        Animator animator6 = currentInputFieldList[5].GetComponent<Animator>();
                        animator6.Play("SportleGrey");
                        if (guessedEnvironmentTerrain.image.color != Color.green && guessedEnvironmentTerrain.image.color != Color.yellow)
                        {
                            animator6 = guessedEnvironmentTerrain.GetComponent<Animator>();
                            animator6.Play("SportleGrey");
                        }
                    }

                    //if (targetSport.OlympicDebut == obj.OlympicDebut)
                    //{
                    //    Animator animator7 = currentInputFieldList[6].GetComponent<Animator>();
                    //    animator7.Play("SportleGreen");
                    //    animator7 = guessedOlympicDebut.GetComponent<Animator>();
                    //    animator7.Play("SportleGreen");
                    //    guessedOlympicDebut.text = targetSport.OlympicDebut.ToString();
                    //}
                    //else
                    //{
                    //    Animator animator7 = currentInputFieldList[6].GetComponent<Animator>();
                    //    animator7.Play("SportleGrey");
                    //    if (guessedOlympicDebut.image.color != Color.green)
                    //    {
                    //        animator7 = guessedOlympicDebut.GetComponent<Animator>();
                    //        animator7.Play("SportleGrey");
                    //    }

                    //}

                    if (AreListsEqual(targetSport.ScoringMethod, obj.ScoringMethod))
                    {
                        Animator animator8 = currentInputFieldList[6].GetComponent<Animator>();
                        animator8.Play("SportleGreen");
                        animator8 = guessedScoringMethod.GetComponent<Animator>();
                        animator8.Play("SportleGreen");
                        guessedScoringMethod.text = string.Join(", ", targetSport.ScoringMethod);
                    }
                    else if (HasPartialElement(targetSport.ScoringMethod, obj.ScoringMethod))
                    {
                        Animator animator8 = currentInputFieldList[6].GetComponent<Animator>();
                        animator8.Play("SportleYellow");
                        if (guessedScoringMethod.image.color != Color.green)
                        {
                            animator8 = guessedScoringMethod.GetComponent<Animator>();
                            animator8.Play("SportleYellow");

                        }
                        List<String> gScoringMethodList = guessedScoringMethod.text.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                        foreach (string skill in partialElements)
                        {
                            if (!gScoringMethodList.Contains(skill))
                            {
                                gScoringMethodList.Add(skill);
                                if (AreListsEqual(gScoringMethodList, targetSport.ScoringMethod))
                                {
                                    animator8 = guessedScoringMethod.GetComponent<Animator>();
                                    animator8.Play("SportleGreen");
                                }
                            }
                        }
                        guessedScoringMethod.text = string.Join(", ", gScoringMethodList);
                        
                    }
                    else
                    {
                        Animator animator8 = currentInputFieldList[6].GetComponent<Animator>();
                        animator8.Play("SportleGrey");
                        if (guessedScoringMethod.image.color != Color.green && guessedScoringMethod.image.color != Color.yellow)
                        {
                            animator8 = guessedScoringMethod.GetComponent<Animator>();
                            animator8.Play("SportleGrey");
                        }
                    }
                }
                return;
            }
            else
            {
                valid = false;   
                
            }
        }
        if (!valid)
        {
            string closestSport = GetClosestCountry(userInput, sports, 5);  // 2 a határérték
            if (closestSport != null)
            {
                correction.gameObject.SetActive(true);
                correction.text = "Did you mean " + closestSport + "?";
                actualClosestSport = closestSport;
            }


            Debug.Log("Wrong input!");          
            wrongInput.Play("NotValidWord");
            
        }
    }

    public int DamerauLevenshteinDistance(string s, string t)
    {
        int n = s.Length;
        int m = t.Length;
        int[,] dp = new int[n + 1, m + 1];

        for (int i = 0; i <= n; i++)
            dp[i, 0] = i;
        for (int j = 0; j <= m; j++)
            dp[0, j] = j;

        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;

                dp[i, j] = Mathf.Min(
                    dp[i - 1, j] + 1,
                    dp[i, j - 1] + 1,
                    dp[i - 1, j - 1] + cost
                );

                if (i > 1 && j > 1 && s[i - 1] == t[j - 2] && s[i - 2] == t[j - 1])
                {
                    dp[i, j] = Mathf.Min(dp[i, j], dp[i - 2, j - 2] + cost);
                }
            }
        }
        return dp[n, m];
    }

    public string GetClosestCountry(string input, List<Sport> sports, int threshold)
    {
        string closestSport = null;
        int minDistance = int.MaxValue;

        foreach (Sport obj in sports)
        {
            string sportName= obj.Name;  // GameObject name gets the country name
            int distance = DamerauLevenshteinDistance(input.ToLower(), sportName.ToLower());

            if (distance < minDistance && distance <= threshold)
            {
                minDistance = distance;
                closestSport = sportName;
            }
        }

        return closestSport;
    }

    public void GetCorrection()
    {
        if (!valid)
        {
            userInputField.text = "";
            userInputField.text = actualClosestSport;
            correction.gameObject.SetActive(false);
        }
    }



    private bool AreListsEqual(List<string> list1, List<string> list2)
    {
        if (list1.Count != list2.Count)
        {
            return false;
        }

        var sortedList1 = list1.OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();
        var sortedList2 = list2.OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();

        for (int i = 0; i < sortedList1.Count; i++)
        {
            if (!string.Equals(sortedList1[i], sortedList2[i], StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        return true;
    }

    private bool HasPartialElement(List<string> list1, List<string> list2)
    {
        partialElements.Clear();

        foreach (string element1 in list1)
        {
            foreach (string element2 in list2)
            {
                if (element2 == element1 || element1 == element2)
                {
                    partialElements.Add(element1);
                    break;
                }
            }
        }
        return partialElements.Count > 0;

    }
}
[System.Serializable]
public class Sport
{
    public string Name;
    public List<string> Season;
    public List<string> TypeOfCompetition;
    public List<string> Venue;
    public List<string> Equipment;
    public List<string> Skills;
    public List<string> EnvironmentTerrain;    
    public List<string> ScoringMethod;
    public Sprite sportImage;
}




