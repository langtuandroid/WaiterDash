using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameTag;
    private void OnEnable()
    {
        UnityCloudSave.onSavedCloudFileLoaded += OnSavedCloudFileLoaded;
    }

    private void OnDisable()
    {
        UnityCloudSave.onSavedCloudFileLoaded -= OnSavedCloudFileLoaded;
    }


    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            UnityCloudSave.Instance.saveCloudSavedFile();
            CurrencyManager.Instance.saveGoldBalanceOnCloud();
            AuthenticationManager.Instance.SignOutAsync();
        });

    }
    private void OnSavedCloudFileLoaded(object sender, EventArgs e)
    {
        var savedGameTag = ES3.Load<string>("Player_GameTag");
        gameTag.text = savedGameTag;
    }

}
