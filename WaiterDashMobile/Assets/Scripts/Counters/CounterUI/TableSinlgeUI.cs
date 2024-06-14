using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TableSinlgeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tableNumberText;
    [SerializeField] private TextMeshProUGUI currentLevel;
    [SerializeField] private Sprite red;
    [SerializeField] private Sprite green;
    [SerializeField] private Image fillImage;
    private BaseDiningTable diningTable;
    

    public void SetDiningTable(BaseDiningTable diningTable)
    {
        tableNumberText.text = diningTable.name.ToString();
        currentLevel.text = diningTable.GetCurrentLevel().ToString();
        this.diningTable = diningTable;
        if(!this.diningTable.IsEmpty())
        {
            fillImage.sprite = red;
        }
    }

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            if(diningTable.IsEmpty())
            {
                CustomerObject customer = CustomerWaitingQueue.Instance.GetFirstInQueue();
                if (customer != null)
                {
                    diningTable.SetCustomerObject(customer);
                    fillImage.sprite = red;
                }
            } else
            {
                CasaTableUI.Instance.loadFoodInCasaCounter(diningTable);
            }
        });
    }
    private void Update()
    {

    }

}
