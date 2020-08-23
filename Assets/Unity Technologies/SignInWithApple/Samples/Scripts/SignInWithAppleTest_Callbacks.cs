using System;
using UnityEngine;
using UnityEngine.SignInWithApple;
using UnityEngine.UI;

public class SignInWithAppleTest_Callbacks : MonoBehaviour
{
    private string userId;
    public Text callback;
    public void ButtonPress()
    {
        var siwa = gameObject.GetComponent<SignInWithApple>();
        siwa.Login(OnLogin);
    }

    public void CredentialButton()
    {
        // User id that was obtained from the user signed into your app for the first time.
        var siwa = gameObject.GetComponent<SignInWithApple>();
        siwa.GetCredentialState(userId, OnCredentialState);
    }

    private void OnCredentialState(SignInWithApple.CallbackArgs args)
    {
        Debug.Log(string.Format("User credential state is: {0}", args.credentialState));
        callback.text = "User credential state is:" + args.credentialState;
        if (args.error != null)
        {
            Debug.Log(string.Format("Errors occurred: {0}", args.error));
            callback.text = "Errors occurred:" +  args.error;
        }
           
    }

    private void OnLogin(SignInWithApple.CallbackArgs args)
    {
        if (args.error != null)
        {
            Debug.Log("Errors occurred: " + args.error);
            callback.text = "errors : " + args.error;
            return;
        }

        UserInfo userInfo = args.userInfo;

        // Save the userId so we can use it later for other operations.
        userId = userInfo.userId;
        
        // Print out information about the user who logged in.
        Debug.Log(
            string.Format("Display Name: {0}\nEmail: {1}\nUser ID: {2}\nID Token: {3}", userInfo.displayName ?? "",
                userInfo.email ?? "", userInfo.userId ?? "", userInfo.idToken ?? ""));

        callback.text = "Display Name: " + userInfo.displayName + "email :" + userInfo.email
            + "user id :" + userInfo.userId + "idtoken : " + userInfo.idToken;
     }
}
