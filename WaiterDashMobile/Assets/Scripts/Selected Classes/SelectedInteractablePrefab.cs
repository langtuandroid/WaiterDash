using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SelectedInteractablePrefab : MonoBehaviour
{
    [SerializeField] private BaseInteractablePrefab baseInteractablePrefab;
    [SerializeField] private GameObject[] visualGameObjectArray;
    private void Start()
    {
        Player.Instance.OnSelectedInteractablePrefabChanged += Player_OnSelectedInteractablePrefabChanged;
    }

    private void Player_OnSelectedInteractablePrefabChanged(object sender, Player.OnSelectedInteractablePrefabChangedEventArgs e)
    {
        if (e.selectedInteractablePrefab == baseInteractablePrefab)
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
