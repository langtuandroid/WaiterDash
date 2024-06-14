using Assets.Scripts.Customers;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Managers;
using CodeMonkey;
using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseDiningTable : BaseUpgradeable, ICustomerObjectParent, IRestaurantObjectParent, IInteractable, IPlacementObject, IPrefabSelectable, IPointerClickHandler
{
    // public static event EventHandler OnAnyCustomerPlacedHere;
    public static event Action<IPrefabSelectable> OnSelectableSelected;
    [SerializeField] private Transform[] diningTableChairSitPoint;
    [SerializeField] private Transform diningTableFoodPoint;
    [SerializeField] private Transform diningTableDrinkPoint;
    [SerializeField] private Transform exitGatePosition;
    [SerializeField] private Transform prefabVisual;
    [SerializeField] private MenuCardSO[] menuCardSOArray;
    [SerializeField] private CustomerStateUI customerStateUI;
    [SerializeField] public RestaurantObject orderNote;
    [SerializeField] public CustomerOrder orderedFoodAndDrinks;
    [SerializeField] public DiningCustomerOrderManager customerOrderManager;
    [SerializeField] public RestaurantObjectSO dirtyPlate;
    [SerializeField] private GameObject visualsParent;

    private RestaurantObject restaurantObject;
    public static EventHandler OnCustomerPayment;
    private List<RestaurantObjectSO> casaRestaurantObjects;
    public float rotationSpeed = 5f;
    public float moveSpeed = 5f;
    public Vector2Int prefabSize = new Vector2Int();
    protected CustomerObject customersObject;
    protected RestaurantObject firstRestaurantObject;
    public float OrderedAmount;
    private bool hasFoodOnTheTable;
    private bool hasDrinkOnTheTable;
    public enum CustomerState
    {
        WatingAtTable,
        WaitingForMenuCard,
        ReadingMenu,
        ReadyToOrder,
        WaitingForFood,
        EatingFoodAndDrinks,
        WaitingToPayBills,
        DoneDining
    }
    public CustomerState? customerState;
    private CustomerState? previousState;

    private void Start()
    {
        previousState = null;
        RestaurantManager.OnRestaurantClosed += RestaurantManager_OnRestaurantClosed;
        RestaurantManager.OnRestaurantOpened += RestaurantManager_OnRestaurantOpened;
    }

    private void RestaurantManager_OnRestaurantOpened(object sender, EventArgs e)
    {
        // on restaurant opened
    }

    private void RestaurantManager_OnRestaurantClosed(object sender, EventArgs e)
    {
        resetDiningTable();
    }

    public void resetDiningTable()
    {
        customerState = CustomerState.WatingAtTable;
        previousState = null;
        hasFoodOnTheTable = false;
        hasDrinkOnTheTable = false;
        ClearCasaRestaurantObject();
        ClearCustomerObject();
        ClearCasaRestaurantObject();
        customerStateUI.hideIcons();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!RestaurantManager.Instance.IsRestaurantOpen() && !CameraTouchMovementManager.Instance.isDragging)
        {
            OnSelectableSelected?.Invoke(this);
        }
    }
    protected override void ApplyUpgrade(UpgradeSO.UpgradeLevel upgradeLevel)
    {
        // Handle common upgrade logic for all counters, if any
    }

    public new virtual void Upgrade()
    {
        base.Upgrade();
        UpdateVisuals();
        ResetOutlineEnabled();
    }

    private void UpdateVisuals()
    {
        int currentLevel = GetCurrentLevel();
        Transform lastVisual = null;

        // Loop through all child GameObjects
        foreach (Transform visual in visualsParent.transform)
        {
            int level = GetLevelFromGameObject(visual.gameObject);
            if (level <= currentLevel)
            {
                visual.gameObject.SetActive(true);
                lastVisual = visual;
            }
        }

        foreach (Transform visual in visualsParent.transform)
        {
            if (visual != lastVisual)
            {
                visual.gameObject.SetActive(false);
            }
        }
    }
    private IEnumerator AddOutlineNextFrame()
    {
        yield return null; // Wait until the next frame
        gameObject.AddComponent<Outline>();
    }

    private int GetLevelFromGameObject(GameObject obj)
    {
        // Get the name of the GameObject
        string name = obj.name;

        // Remove any non-numeric characters from the name
        string levelString = Regex.Replace(name, "[^0-9]", "");

        // Parse the remaining string as an integer
        int level;
        if (int.TryParse(levelString, out level))
        {
            // Return the parsed level
            return level;
        }
        else
        {
            // If parsing fails, return a default value or handle the error as needed
            return 0;
        }
    }

    protected virtual void ApplyUpgrade()
    {
        // Override this in derived classes to apply specific upgrade effects
    }

    private void Update()
    {
        if (HasCustomerObject())
        {
            if (customerState != previousState)
            {
                switch (customerState)
                {
                    case CustomerState.WatingAtTable:
                        Debug.Log("Waiting");
                        break;
                    case CustomerState.WaitingForMenuCard:
                        Debug.Log("Wait for Menu Card");
                        break;
                    case CustomerState.ReadingMenu:
                        FunctionTimer.Create(() =>
                        {
                            Debug.Log("Is ready to order");
                            customerState = CustomerState.ReadyToOrder;
                            GetCustomersObject().AskForService();
                        }, 1);
                        break;
                    case CustomerState.ReadyToOrder:
                        Debug.Log("Waiting for waiter");
                        break;
                    case CustomerState.WaitingForFood:
                        break;
                    case CustomerState.EatingFoodAndDrinks:
                        GetCustomersObject().StartEating();
                        FunctionTimer.Create(() =>
                        {
                            GetCustomersObject().StopEating();
                            restaurantObject.DestorySelf();
                            if(firstRestaurantObject)
                            {
                                firstRestaurantObject.DestorySelf();
                            }
                            ClearRestaurantObject();
                            customerState = CustomerState.WaitingToPayBills;
                            GetCustomersObject().AskForService();
                            RestaurantObject.SpawnRestaurantObject(dirtyPlate, this);
                        }, 1);
                        break;
                    case CustomerState.WaitingToPayBills:
                        break;
                    default:
                        break;
                }
                customerStateUI.UpdateCustomerStateText(customerState);
                previousState = customerState;
            }
        }
        else
        {
            if (customerState != previousState)
            {
                if (customerState == CustomerState.DoneDining)
                {
                    customerStateUI.UpdateCustomerStateText(customerState);
                    previousState = customerState;
                }
            }

        }
    }
    public virtual void Interact(Player player)
    {

    }
    public void ClearCustomerObject()
    {
        customersObject = null;
    }

    public CustomerObject GetCustomersObject()
    {
        return customersObject;
    }

    public Transform[] GetCustomerObjectFollowTransform()
    {
        return diningTableChairSitPoint;
    }


    public bool HasCustomerObject()
    {
        return customersObject != null;
    }

    public bool IsEmpty()
    {
        return customersObject == null && restaurantObject == null;
    }

    // This function starts the smooth rotation towards the target rotation
    // This function starts the smooth movement and rotation towards the target position and rotation
    private void StartSmoothMovementAndRotation(Vector3 targetPosition, Quaternion targetRotation)
    {
        // Start the coroutine for smooth movement and rotation
        StartCoroutine(SmoothMoveAndRotate(targetPosition, targetRotation));
    }

    private IEnumerator SmoothMoveAndRotate(Vector3 targetPosition, Quaternion targetRotation)
    {
        while (Vector3.Distance(customersObject.transform.position, targetPosition) > 0.01f ||
               Quaternion.Angle(customersObject.transform.rotation, targetRotation) > 0.01f)
        {
            // Smoothly move towards the target position
            customersObject.transform.position = Vector3.Lerp(customersObject.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Smoothly rotate towards the target rotation
            customersObject.transform.rotation = Quaternion.Lerp(customersObject.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            yield return null;
        }

        // Ensure the final position and rotation are set exactly to the target values
        customersObject.transform.position = targetPosition;
        customersObject.transform.rotation = targetRotation;
    }

    public void SetCustomerObject(CustomerObject customersObject)
    {
        this.customersObject = customersObject;
        Debug.Log(customersObject.name);
        if (HasCustomerObject())
        {
            customersObject.MoveTo(diningTableChairSitPoint[0].position, () =>
            {
                customersObject.StopMovement();
                var desiredRotation = diningTableChairSitPoint[0].transform.rotation;
                var desiredPosition = diningTableChairSitPoint[0].transform.position;
                StartSmoothMovementAndRotation(desiredPosition, desiredRotation);

                customerState = CustomerState.WatingAtTable;
                customersObject.SetDiningTable(this);
                FunctionTimer.Create(() =>
                {
                    customerState = CustomerState.WaitingForMenuCard;
                }, 5);
            });

        }
    }

    public void SetRestaurantObject(RestaurantObject restaurantObject)
    {
        this.restaurantObject = restaurantObject;
    }

    public string GetInteractText()
    {
        throw new NotImplementedException();
    }

    public Transform GetRestaurantObjectFollowTransform()
    {
        return diningTableFoodPoint;
    }
    public Transform GetRestaurantDrinkTransform()
    {
        return diningTableDrinkPoint;
    }

    public Transform GetRestaurantFoodTransform()
    {
        return diningTableFoodPoint;
    }

    public RestaurantObject GetRestaurantObject()
    {
        return restaurantObject;
    }

    public void ClearRestaurantObject()
    {
        restaurantObject = null;
    }

    public bool HasRestaurantObject()
    {
        return restaurantObject != null;
    }

    public void SetCasaRestaurantObject(List<RestaurantObjectSO> restaurantObject)
    {
        this.casaRestaurantObjects = restaurantObject;
    }
    public List<RestaurantObjectSO> GetCasaRestaurantObjectSO()
    {
        return casaRestaurantObjects;
    }

    public void SetCustomerOrder(CustomerOrder customerOrder)
    {
        this.orderedFoodAndDrinks = customerOrder;
        this.OrderedAmount += customerOrder.FoodOrder.GetRestaurantObjectSO().price;
        if(customerOrder.DrinkOrder)
        {
            this.OrderedAmount += customerOrder.DrinkOrder.GetRestaurantObjectSO().price;
        }
    }
    public RestaurantObject CustomerFoodOrder()
    {
         return this.orderedFoodAndDrinks.FoodOrder ? this.orderedFoodAndDrinks.FoodOrder : null;
    }

    public RestaurantObject CustomerDrinkOrder()
    {
        return this.orderedFoodAndDrinks.DrinkOrder ? this.orderedFoodAndDrinks.DrinkOrder : null;
    }

    public void CustomerReceivedFood()
    {
        this.hasFoodOnTheTable = true;
        this.orderedFoodAndDrinks.FoodOrder = null;
    }

    public void CustomerReceivedDrink()
    {
        this.hasDrinkOnTheTable = true;
        this.orderedFoodAndDrinks.DrinkOrder = null;
    }

    public bool HasFoodOnTheTable()
    {
        return this.hasFoodOnTheTable;
    }

    public bool HasDrinkOnTheTable()
    {
        return this.hasDrinkOnTheTable;
    }
    public float GetOrderedAmount()
    {
        return OrderedAmount;
    }

    public void ClearCasaRestaurantObject()
    {
        this.casaRestaurantObjects = new List<RestaurantObjectSO>();
    }
    

    public Vector3 getExitGatePosition()
    {
        return exitGatePosition.position;
    }

    public Transform GetPrefab()
    {
        return this.transform;
    }

    public Vector2Int GetPrefabSize()
    {
        return prefabSize;
    }

    public Transform GetPrefabVisual()
    {
        return prefabVisual;
    }
}
