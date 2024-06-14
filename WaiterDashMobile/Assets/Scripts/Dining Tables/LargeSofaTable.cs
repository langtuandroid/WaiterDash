using CodeMonkey;
using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeSofaTable : BaseDiningTable
{
    public override void Interact(Player player)
    {
        if (HasCustomerObject())
        {
            if (!HasRestaurantObject())
            {
                if (player.HasRestaurantObject())
                {
                    var restaurantObject = player.GetRestaurantObject();
                    var objectType = restaurantObject.GetRestaurantObjectSO().objectType;
                    switch (objectType)
                    {
                        case ObjectType.MenuCard:
                            if (customerState == CustomerState.WaitingForMenuCard)
                            {
                                player.GetRestaurantObject().SetRestaurantObjectParent(this, GetRestaurantObjectFollowTransform());

                                customerState = CustomerState.ReadingMenu;
                            }
                            break;
                        case ObjectType.Food:
                            if (customerState == CustomerState.WaitingForFood)
                            {
                                if (player.GetRestaurantObject() is PlateRestaurantObject)
                                {

                                    PlateRestaurantObject plate = player.GetRestaurantObject() as PlateRestaurantObject;
                                    var food = plate.GetRestaurantObject();
                                    var customerFoodOrder = CustomerFoodOrder();
                                    if (customerFoodOrder)
                                    {
                                        if (customerFoodOrder.GetRestaurantObjectSO() == food.GetRestaurantObjectSO())
                                        {
                                            customersObject.HideCustomerFoodOrderIcon();
                                            player.GetRestaurantObject().SetRestaurantObjectParent(this, GetRestaurantFoodTransform());
                                            CustomerReceivedFood();
                                            if (CustomerDrinkOrder() == null)
                                            {
                                                CustomerOrdersUI.Instance.RemoveCustomerOrderUI(this);
                                                customerState = CustomerState.EatingFoodAndDrinks;
                                            }
                                            else
                                            {
                                                firstRestaurantObject = GetRestaurantObject();
                                                ClearRestaurantObject();
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case ObjectType.Drinks:
                            if (customerState == CustomerState.WaitingForFood)
                            {
                                if (player.GetRestaurantObject() is not FruitObject)
                                {
                                    var drink = player.GetRestaurantObject();
                                    var customerDrinkOrder = CustomerDrinkOrder();
                                    if (customerDrinkOrder)
                                    {
                                        if (customerDrinkOrder.GetRestaurantObjectSO() == drink.GetRestaurantObjectSO())
                                        {
                                            customersObject.HideCustomerDrinkOrderIcon();
                                            player.GetRestaurantObject().SetRestaurantObjectParent(this, GetRestaurantDrinkTransform());
                                            CustomerReceivedDrink();
                                            if (CustomerFoodOrder() == null)
                                            {
                                                CustomerOrdersUI.Instance.RemoveCustomerOrderUI(this);
                                                customerState = CustomerState.EatingFoodAndDrinks;
                                            }
                                            else
                                            {
                                                firstRestaurantObject = GetRestaurantObject();
                                                ClearRestaurantObject();
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    SoundManager.Instance.TaskUnsuccessFull(this.transform.position, 0.7f);
                }
            }
            else
            {
                if (!player.HasRestaurantObject() && customerState == CustomerState.ReadyToOrder)
                {
                    this.GetRestaurantObject().DestorySelf();
                    ClearRestaurantObject();
                    RestaurantObject.SpawnRestaurantObject(this.orderNote.GetRestaurantObjectSO(), player);
                    customerOrderManager.GenerateCustomerOrders();
                    var customerOrder = customerOrderManager.GetCustomerOrders();
                    customersObject.SetCustomerOrderIcon(customerOrder);
                    OrderNoteCustomerOrderVisualUI.Instance.UpdateCustomerOrderIcon(customerOrder, this);
                    SetCustomerOrder(customerOrder);
                    customersObject.ServiceReceived();
                    customerState = CustomerState.WaitingForFood;
                    // this.GetRestaurantObject().SetRestaurantObjectParent(player);
                }
                else if (player.GetRestaurantObject().GetRestaurantObjectSO().objectType == ObjectType.PaymentMachine && customerState == CustomerState.WaitingToPayBills)
                {
                    // player.GetRestaurantObject().SetRestaurantObjectParent(this, GetRestaurantObjectFollowTransform());
                    customersObject.ServiceReceived();
                    player.GetRestaurantObject().DestorySelf();
                    FunctionTimer.Create(() =>
                    {
                        customerState = CustomerState.DoneDining;
                        CurrencyManager.Instance.addGoldBalance(GetOrderedAmount());
                        customersObject.StandUp();
                        var currentCustomerObject = customersObject;
                        FunctionTimer.Create(() =>
                        {
                            currentCustomerObject.StartMovement();
                            currentCustomerObject.MoveTo(getExitGatePosition(), () =>
                            {
                                currentCustomerObject.DestroySelf();
                            });
                        }, 1.7f);

                        ClearCustomerObject();
                        //ClearRestaurantObject();
                        OnCustomerPayment?.Invoke(this, EventArgs.Empty);
                    }, 5);
                }
                else
                {
                    SoundManager.Instance.TaskUnsuccessFull(this.transform.position, 0.7f);
                }
            }
        }
        else
        {
            if (GetRestaurantObject().GetRestaurantObjectSO() == dirtyPlate)
            {
                GetRestaurantObject().SetRestaurantObjectParent(player, player.GetRestaurantObjectFollowTransform());
                CasaTableUI.Instance.UpdateVisual();
                ClearRestaurantObject();
            } else
            {
                SoundManager.Instance.TaskUnsuccessFull(this.transform.position, 0.7f);
            }
        }
    }
}
