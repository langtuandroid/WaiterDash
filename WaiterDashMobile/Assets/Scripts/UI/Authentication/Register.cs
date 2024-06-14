using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Register : MonoBehaviour
{
    [SerializeField] private TMP_InputField userName;
    [SerializeField] private TMP_InputField gamerTag;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private Button signUpButton;
    [SerializeField] private Button loginUIButton;
    [SerializeField] private Transform loginUI;
    private string USERNAME;
    private string PASSWORD;
    private string GAMETAG;
    private void Start()
    {
        signUpButton.onClick.AddListener(async () =>
        {
            // Retrieve and trim the input values
            USERNAME = userName.text.ToString().Trim();
            PASSWORD = password.text.ToString().Trim();
            GAMETAG = gamerTag.text.ToString().Trim();
            await AuthenticationManager.Instance.SignUpWithUsernamePasswordAsync(USERNAME, PASSWORD, GAMETAG);
            showLoginUI();
        });

        loginUIButton.onClick.AddListener(() =>
        {
            showLoginUI();
        });
    }

    void showLoginUI ()
    {
        gameObject.SetActive(false);
        loginUI.gameObject.SetActive(true);
    }

}
