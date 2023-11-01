using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;


public class ServerHandler : MonoBehaviour
{
    private void Start()
    {
        
    }

    void RegisterPlayer()
    {

        //also i want to know if that name already exists.

        var request = new RegisterPlayFabUserRequest
        {
            DisplayName = "Thomas",
            Username = "Thomas",
            Email = "luke.and01@gmail.com",
            Password = "Potato"
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void LoginPlayer()
    {

    }


    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("it worked brother");
    }


    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Sucess " + result);
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error Server " + error.GenerateErrorReport());
    }

}
