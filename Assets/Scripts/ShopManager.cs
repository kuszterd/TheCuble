using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Purchasing;
using System.Collections.Generic;
using UnityEngine.Purchasing.Extension;

public class ShopManager : MonoBehaviour
{
    private string cube600 = "com.qstr.thecuble.cube_600";
    private string noAds = "com.qstr.thecuble.noads";
    private string unlimitedCube = "com.qstr.thecuble.unlimitedcube";

    public TextMeshProUGUI noAdText;
    public Button noAdButton;
    public TextMeshProUGUI subsText;
    public Button subsButton;

    public static int noAdHelp = 0;
    public static int subsHelp = 0;
    

    void Start()
    {
        noAdHelp = PlayerPrefs.GetInt("NoAdHelp", noAdHelp);
        if(noAdHelp == 0)
        {
            ShowAds();
        }
        else
        {
            NoAds();
        }
        subsHelp = PlayerPrefs.GetInt("SubsHelp", subsHelp);
        if (subsHelp == 0)
        {
            DeSubs();
        }
        else
        {
            Subs();
        }
    }

    public PayloadData payloadData;
    public class PayloadData
    {
        public int quantity;
    }

    public void OnPurchaseComplete(Product product)
    {
        if(product.definition.id == cube600)
        {          
                Add600();        
        }
        if (product.definition.id == noAds)
        {
            NoAds();
            noAdHelp = 1;
            PlayerPrefs.SetInt("NoAdHelp", noAdHelp);
            PlayerPrefs.Save();
        }
        if (product.definition.id == unlimitedCube)
        {
            Subs();
            subsHelp = 1;
            PlayerPrefs.SetInt("SubsHelp", subsHelp);
            PlayerPrefs.Save();
            
        }

    }
    public static void LoadNoAdHelp()
    {
        noAdHelp = PlayerPrefs.GetInt("NoAdHelp", noAdHelp);
    }
    public static void LoadNoAdSubs()
    {
        noAdHelp = PlayerPrefs.GetInt("SubsHelp", subsHelp);
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureDescription purchaseFailure)
    {
        Debug.Log(product.definition.id + "failed because" + purchaseFailure);
    }

    
    

    public void Add600()
    {
        int currentCube = PlayerPrefs.GetInt("ProfileCurrency");

        currentCube += 600;
        PlayerPrefs.SetInt("ProfileCurrency", currentCube);
        PlayerPrefs.Save();
        CurrencyManager.Load();
    }




    public void NoAds()
    {
        noAdText.color = Color.green;
        noAdButton.image.color = Color.grey;
        noAdButton.interactable = false;
    }

    public void ShowAds()
    {
        noAdText.color = Color.black;
        noAdButton.image.color = new Color32(255, 150, 0, 255);
        noAdButton.interactable = true;
    }

    public void Subs()
    {
        subsText.color = Color.green;
        subsButton.image.color = Color.grey;
        subsButton.interactable = false;
        int currentCube = PlayerPrefs.GetInt("ProfileCurrency");

        currentCube += 999999999;

        PlayerPrefs.SetInt("ProfileCurrency", currentCube);
        PlayerPrefs.Save();
        CurrencyManager.Load();

    }

    public void DeSubs()
    {
        subsText.color = Color.black;
        subsButton.image.color = new Color32(255, 150, 0, 255);
        subsButton.interactable = true;
        


    }

}


