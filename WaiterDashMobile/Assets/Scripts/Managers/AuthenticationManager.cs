using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class AuthenticationManager : MonoBehaviour
{
    public static AuthenticationManager Instance { get; set; }

    async void Awake()
    {
        Instance = this;
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private async void Start()
    {
        if (AuthenticationService.Instance.SessionTokenExists & !AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            SceneManager.LoadScene("RestaurantScene Dev");
        }
    }

    public async Task SignUpWithUsernamePasswordAsync(string username, string password, string gameTag)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            if (ES3.FileExists("GameSaveData.es3"))
            {
                ES3.DeleteFile("GameSaveData.es3");
            }
            ES3.Save("PlayerId", AuthenticationService.Instance.PlayerId);
            ES3.Save("Player_GameTag", gameTag);
            SceneManager.LoadScene("RestaurantScene Dev");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }
    public async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            Debug.Log("SignIn is successful.");
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            // load scene
            SceneManager.LoadScene("RestaurantScene Dev");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    public void SignOutAsync()
    {
        AuthenticationService.Instance.SignOut();
        AuthenticationService.Instance.ClearSessionToken();
        SceneManager.LoadScene("Main Menu");
    }

}
