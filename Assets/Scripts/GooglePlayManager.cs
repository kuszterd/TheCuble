using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class GooglePlayManager : MonoBehaviour
{
    public TextMeshProUGUI googlePlayDetails;
    
    // Start is called before the first frame update
    void Start()
    {
        SignIn();
    }

    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            string name = PlayGamesPlatform.Instance.GetUserDisplayName();
            

            googlePlayDetails.text = "Signed in as: " + name;

            // Continue with Play Games Services
        }
        else
        {

            googlePlayDetails.text = "Not connected!";
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }

        
    }
    
}
