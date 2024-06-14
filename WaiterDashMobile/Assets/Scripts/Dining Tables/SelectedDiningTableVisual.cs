using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedDiningTableVisual : MonoBehaviour
{
    [SerializeField] private BaseDiningTable baseDiningTable;
    [SerializeField] private GameObject[] visualGameObjectArray;
    private void Start()
    {
        Player.Instance.OnSelectedDiningTableChanged += Player_OnSelectedDiningTableChanged;
    
    }

    private void Player_OnSelectedDiningTableChanged(object sender, Player.OnSelectedDiningTableChangedEventArgs e)
    {
        if (e.selectedDiningTable == baseDiningTable)
        {
            ShowVisualGameObject();
        }
        else
        {
            HideVisualGameObject();
        }
    }

    private void ShowVisualGameObject()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
    }

    private void HideVisualGameObject()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
