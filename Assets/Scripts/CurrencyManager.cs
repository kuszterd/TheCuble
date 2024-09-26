using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{

    public static int profileCurrency;
    public Text currencyText;
    // Start is called before the first frame update
    public void Start()
    {
        profileCurrency = PlayerPrefs.GetInt("ProfileCurrency", profileCurrency);
        ShopManager.LoadNoAdHelp();
        ShopManager.LoadNoAdSubs();
        if(ShopManager.subsHelp == 1)
        {
            currencyText.text = "Unlimited";
        }


    }

    // Update is called once per frame
    public void Update()
    {
        if (ShopManager.subsHelp == 1)
        {
            currencyText.text = "Unlimited";
        }
        else
        {
            currencyText.text = profileCurrency.ToString();
        }
        

    }

    public static void Load()
    {
        profileCurrency = PlayerPrefs.GetInt("ProfileCurrency", profileCurrency);
        
    }
}
