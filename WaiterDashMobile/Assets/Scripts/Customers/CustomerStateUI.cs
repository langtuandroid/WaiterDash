using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static BaseDiningTable;

public class CustomerStateUI : MonoBehaviour
{
    [SerializeField] private Image customerStateIcon;
    [SerializeField] private Image iconBackground;
    [SerializeField] private DiningStateIconsSO diningStateIconSO;
    private void Start()
    {
        //hideIcons();
    }
    public void UpdateCustomerStateText(CustomerState? customerState)
    {

        switch (customerState)
        {
            case CustomerState.WatingAtTable:
                showIcons();
                customerStateIcon.sprite = diningStateIconSO.WaitingAtTable;
                break;
            case CustomerState.WaitingForMenuCard:
                customerStateIcon.sprite = diningStateIconSO.WaitingForMenuCard;
                break;
            case CustomerState.ReadingMenu:
                hideIcons();
                break;
            case CustomerState.ReadyToOrder:
                showIcons();
                customerStateIcon.sprite = diningStateIconSO.ReadyToOrder;
                break;
            case CustomerState.WaitingForFood:
                hideIcons();
                break;
            case CustomerState.EatingFoodAndDrinks:
                hideIcons();
                break;
            case CustomerState.WaitingToPayBills:
                showIcons();
                customerStateIcon.sprite = diningStateIconSO.WaitingToPayBills;
                break;
            case CustomerState.DoneDining:
                customerStateIcon.sprite = diningStateIconSO.DoneDining;
                FunctionTimer.Create(() =>
                {
                    hideIcons();
                }, 1);
                break;
            default:
                break;
        }
    }

    public void hideIcons()
    {
        if (customerStateIcon != null && iconBackground != null)
        {
            customerStateIcon.gameObject.SetActive(false);
            iconBackground.gameObject.SetActive(false);
        }
    }

    private void showIcons()
    {
        customerStateIcon.gameObject.SetActive(true);
        iconBackground.gameObject.SetActive(true);
    }
}
