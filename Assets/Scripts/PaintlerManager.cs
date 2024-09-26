using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaintlerManager : MonoBehaviour
{
    
    public List<Image> colorList;
    public Image[] colors;
    private Color32 mix1;
    private Color32 mix2;
    private Color32 mix3;
    private Color32 mix4;
    public Image targetColor;
    public Image guessedColor1;
    public Image guessedColor2;
    public Image guessedColor3;
    public Image guessedColor4;
    public List<Image> guessedMixs;
    public List<GameObject> guessMixGO;
    public List<Image> guessedColors;
    public List<Image> guessedColorsOL;
    private int enterPressed = 0;
    public Image targetBackground;
    public List<TextMeshProUGUI> guessPercentage;
    private List<Color32> targetColors = new List<Color32>();
    // List of guessed colors arrays for different enterPressed values
    public Image victory;
    public Image loseImage;
    public Image victoryBoard;
    public Animator wrongInput;

    private Color baseGray = new Color(0.75f, 0.75f, 0.75f, 1f);
    private bool valid = false;
    public static bool win = false;
    public static bool normalWin = false;
    public static bool normalLose = false;
    public static bool lose = false;
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
        GetRandomMix();
    }

    // Update is called once per frame
    void Update()
    {
        if(enterPressed > 5 && !win)
        {
            lose = true;
            if (MainCameraController.isNormal)
            {
                normalLose = true;
            }
            victoryBoard.gameObject.SetActive(true);
            loseImage.gameObject.SetActive(true);
            guessedColor1.color = mix1;
            guessedColor2.color = mix2;
            guessedColor3.color = mix3;
            guessedColor4.color = mix4;
            targetBackground.color = Color.red;
        }
    }

    private IEnumerator EnterPressed()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(0f);
        MixGuessedColors();
        if (valid && !win)
        {
            CheckColorMatch();
        }
    }



    bool ColorsMatch(Color32 color1, Color32 color2)
    {
        return color1.r == color2.r && color1.g == color2.g && color1.b == color2.b && color1.a == color2.a;
    }

    public void OnDeleteClick()
    {
        if(!win && !lose)
        {
            if (guessedColor4.color != baseGray)
            {
                guessedColor4.color = baseGray;
            }
            else if (guessedColor3.color != baseGray)
            {
                guessedColor3.color = baseGray;
            }
            else if (guessedColor2.color != baseGray)
            {
                guessedColor2.color = baseGray;
            }
            else if (guessedColor1.color != baseGray)
            {
                guessedColor1.color = baseGray;
            }
        }
        
    }
    public void OnEnterClick()
    {
        if (!win && !lose)
        {
            StartCoroutine(EnterPressed());
        }
        
    }


    public void MixGuessedColors()
    {
        if (guessedColor1.color != baseGray && guessedColor2.color != baseGray && guessedColor3.color != baseGray && guessedColor4.color != baseGray)
        {
            valid = true;
            

            if (enterPressed >= 0 && enterPressed <= 5)
            {
                Color32 mixGuessed = MixColors(guessedColor1.color, guessedColor2.color, guessedColor3.color, guessedColor4.color);
                guessMixGO[enterPressed].gameObject.SetActive(true);               
                guessedMixs[enterPressed].color = mixGuessed;
                guessedColors[enterPressed * 4].color = guessedColor1.color;
                guessedColors[enterPressed * 4 + 1].color = guessedColor2.color;
                guessedColors[enterPressed * 4 + 2].color = guessedColor3.color;
                guessedColors[enterPressed * 4 + 3].color = guessedColor4.color;
                guessedColor1.color = baseGray;
                guessedColor2.color = baseGray;
                guessedColor3.color = baseGray;
                guessedColor4.color = baseGray;
                guessPercentage[enterPressed].text = CalculateSimilarity(mixGuessed, targetColor.color).ToString() + "%";

            }
        }
        else
        {
            Debug.Log("You need 4 colors!");
            valid = false;


        }
        if (!valid)
        {
            wrongInput.Play("NotValidWord");
        }
    }
    private void CheckColorMatch()
    {

        // Ensure targetColors is only filled once if applicable
        if (targetColors.Count == 0)
        {
            targetColors.Add(mix1);
            targetColors.Add(mix2);
            targetColors.Add(mix3);
            targetColors.Add(mix4);
        }

        for (int i = enterPressed * 4; i < enterPressed * 4 + 4; i++)
        {
            if (ColorsMatch(guessedColors[i].color, targetColors[i - enterPressed * 4]))
            {

                UpdateColor(guessedColors[i].color, Color.green);
                guessedColorsOL[i].color = Color.green;
            }
            else if (targetColors.Contains(guessedColors[i].color))
            {
                UpdateColor(guessedColors[i].color, Color.yellow);
                guessedColorsOL[i].color = Color.yellow;
            }
            else
            {
                UpdateColor(guessedColors[i].color, Color.grey);
                guessedColorsOL[i].color = Color.grey;
                foreach (Image color in colors)
                {
                    if (guessedColors[i].color == color.color)
                    {
                        Color alphaChange = color.color;
                        alphaChange.a = 0.3f;  // Alpha should be a float between 0 and 1
                        color.color = alphaChange;
                    }
                }
            }
            
        } 
        if (guessedColorsOL[enterPressed * 4].color == Color.green && 
            guessedColorsOL[enterPressed * 4 + 1].color == Color.green && 
            guessedColorsOL[enterPressed * 4 + 2].color == Color.green && 
            guessedColorsOL[enterPressed * 4 + 3].color == Color.green)
        {
            win = true;
            CurrencyManager.profileCurrency += 50;
            PlayerPrefs.SetInt("ProfileCurrency", CurrencyManager.profileCurrency);
            PlayerPrefs.Save();
            if (MainCameraController.isNormal)
            {
                normalWin = true;
            }
            guessedColor1.color = mix1;
            guessedColor2.color = mix2;
            guessedColor3.color = mix3;
            guessedColor4.color = mix4;
            targetBackground.color = Color.green;
            victoryBoard.gameObject.SetActive(true);
            victory.gameObject.SetActive(true);
        }
        


        enterPressed++;
    }

    float CalculateDistance(Color32 color1, Color32 color2)
    {
        float rDiff = color1.r - color2.r;
        float gDiff = color1.g - color2.g;
        float bDiff = color1.b - color2.b;

        return Mathf.Sqrt(rDiff * rDiff + gDiff * gDiff + bDiff * bDiff);
    }

    // Method to calculate percentage similarity
    float CalculateSimilarity(Color32 color1, Color32 color2)
    {
        float maxDistance = Mathf.Sqrt(255 * 255 * 3); // Maximum possible distance in RGB space
        float distance = CalculateDistance(color1, color2);
        Debug.Log(distance);
        if (distance != 0) 
        {
            float similarity = 100 - (distance / maxDistance * 100);
            return Mathf.Floor(similarity);
        }
        else
        {
            return 100f; // Return 100% similarity if the distance is zero (perfect match)
        }
        
    }

    void UpdateColor(Color32 colorTest, Color colorToSet)
    {
           foreach (Image color in colors)
           {
                if (colorTest == color.color)
                {
                    Image[] images = color.GetComponentsInChildren<Image>(true);
                    if (images.Length > 1)
                    {
                        Image childImage = images[1];
                        childImage.color = colorToSet;
                        
                    }
                }
           }
    }
    Color32 GetRandomColor(ref List<Image> colorList)
    {
        int index = Random.Range(0, colorList.Count);
        Color32 selectedColor = colorList[index].color;
        colorList.RemoveAt(index);
        return selectedColor;
    }
    public void GetRandomMix()
    {
        // First random color
        mix1 = GetRandomColor(ref colorList);
        // Second random color
        mix2 = GetRandomColor(ref colorList);
        // Third random color
        mix3 = GetRandomColor(ref colorList);
        // Fourth random color
        mix4 = GetRandomColor(ref colorList);
        Color32 mixedColor = MixColors(mix1, mix2, mix3, mix4);
        targetColor.color = mixedColor;
        
    }
    Color32 MixColors(Color32 c1, Color32 c2, Color32 c3, Color32 c4)
    {
        byte mixedR = (byte)((c1.r * 0.1f + c2.r * 0.2f + c3.r * 0.3f + c4.r * 0.4f) / 1f);
        byte mixedG = (byte)((c1.g * 0.1f + c2.g * 0.2f + c3.g * 0.3f + c4.g * 0.4f) / 1f);
        byte mixedB = (byte)((c1.b * 0.1f + c2.b * 0.2f + c3.b * 0.3f + c4.b * 0.4f) / 1f);
        byte mixedA = (byte)((c1.a * 0.1f + c2.a * 0.2f + c3.a * 0.3f + c4.a * 0.4f) / 1f);

        return new Color32(mixedR, mixedG, mixedB, mixedA);
    }
    public void GuessColorOnClick(Image buttonImage)
    {
        if (buttonImage != null)
        {
            if(guessedColor1.color == baseGray)
            {
                guessedColor1.color = buttonImage.color;
            }
            else if (guessedColor2.color == baseGray)
            {
                guessedColor2.color = buttonImage.color;
            }
            else if (guessedColor3.color == baseGray)
            {
                guessedColor3.color = buttonImage.color;
            }
            else if (guessedColor4.color == baseGray)
            {
                guessedColor4.color = buttonImage.color;
            }   
        }       
    }
    
    
}
