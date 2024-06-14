using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Login : MonoBehaviour
{
    [SerializeField] private TMP_InputField userName;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private Button signInButton;
    [SerializeField] private Button registerUIButton;
    [SerializeField] private Transform registerUI;
    private string USERNAME;
    private string PASSWORD;
    private void Start()
    {
        signInButton.onClick.AddListener(async () =>
        {
            // Retrieve and trim the input values
            USERNAME = userName.text.ToString().Trim();
            PASSWORD = password.text.ToString().Trim();

            await AuthenticationManager.Instance.SignInWithUsernamePasswordAsync(USERNAME, PASSWORD);
        });

        registerUIButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            registerUI.gameObject.SetActive(true);
        });

    }

}
