using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterTableUI : MonoBehaviour
{
   public static CounterTableUI Instance;
    [SerializeField] private Button TableButton;
    [SerializeField] private BaseDiningTable diningTable;

    private void Awake()
    {
        TableButton.onClick.AddListener(() => {
                           
        });
    }

}
