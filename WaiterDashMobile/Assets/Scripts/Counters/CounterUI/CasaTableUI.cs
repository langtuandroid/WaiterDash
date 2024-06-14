using Assets.Scripts.Counters.CounterUI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CasaTableUI : MonoBehaviour
{
    [SerializeField] private Transform tableContainer;
    [SerializeField] private Transform tableTemplate;
    [SerializeField] private Transform restaurantObjectContainer;
    [SerializeField] private Transform foodTemplate;
    [SerializeField] private Transform foodContainer;
    [SerializeField] private Transform drinkContainer;
    [SerializeField] private Transform drinkTemplate;
    [SerializeField] private Transform closeBtn;
    [SerializeField] private TextMeshProUGUI diningTableName;
    [SerializeField] private OrderNoteOverview orderNote;
    public static CasaTableUI Instance { get; private set; }
    private BaseDiningTable diningTable;
    private RestaurantObject OrderNoteObject;
    private void Awake()
    {
        tableTemplate.gameObject.SetActive(false);
        //tableContainer.gameObject.SetActive(true);
        foodTemplate.gameObject.SetActive(false);
        drinkTemplate.gameObject.SetActive(false);
        //restaurantObjectContainer.gameObject.SetActive(false);
        //closeBtn.gameObject.SetActive(true);
    }


    private void Start()
    {
        Instance = this;
        CasaCounter.OnCasaInteract += CasaCounter_OnCasaInteract;
        OrdersOverview.OnOrderConfirmed += OnOrderConfirmed;
    }

    private void OnOrderConfirmed(object sender, System.EventArgs e)
    {
        if (orderNote != null && OrderNoteObject != null)
        {
            orderNote.clearOrderIconTemplates();
            OrderNoteObject.DestorySelf();
        }
    }

    private void Update()
    {
    }

    private void CasaCounter_OnCasaInteract(object sender, System.EventArgs e)
    {
        orderNote.clearOrderIconTemplates();
        var player = (Player)sender;
        if (player.HasRestaurantObject())
        {
            var restaurantObject = player.GetRestaurantObject();
            OrderNoteObject = restaurantObject;
            if (restaurantObject.GetRestaurantObjectSO().objectType == ObjectType.OrderNote)
            {
                OrderNoteCustomerOrderVisualUI orderNoteCustomer = restaurantObject.GetComponentInChildren<OrderNoteCustomerOrderVisualUI>();
                orderNote.AddOrderNote(orderNoteCustomer.GetCustomerOrder(), orderNoteCustomer.GetCustomerDiningTable());
            } else
            {
                orderNote.hideOrderNoteOverview();
            }
        }
        polluteTableLists();
        restaurantObjectContainer.gameObject.SetActive(false);
        tableContainer.gameObject.SetActive(true);
        closeBtn.gameObject.SetActive(true);
    }

    public void UpdateVisual()
    {
        polluteTableLists();
        polluteFoodLists();
    }

    private void polluteTableLists()
    {
        Debug.Log("polluteTableLists");
        foreach (Transform child in tableContainer)
        {
            if (child == tableTemplate) continue;
            Destroy(child.gameObject);
        }
        int index = 0;
        foreach (BaseDiningTable diningTable in ResturantDiningTable.Instance.GetDiningTables())
        {
            index++;
            Transform diningTableTransform = Instantiate(tableTemplate, tableContainer);
            diningTableTransform.gameObject.SetActive(true);
            diningTable.name = "T " + index;
            diningTableTransform.GetComponent<TableSinlgeUI>().SetDiningTable(diningTable);
        }
    }

    private void polluteFoodLists()
    {
        foreach (Transform child in foodContainer)
        {
            if (child == foodTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (RestaurantObject restaurantObject in FoodManager.Instance.GetAvailableFoodObjectLists())
        {
            Transform restaurantObjectTransform = Instantiate(foodTemplate, foodContainer);
            restaurantObjectTransform.gameObject.SetActive(true);
            restaurantObjectTransform.GetComponent<FoodSingleUI>().SetRestaurantObjectSO(restaurantObject);

        }
    }

    private void polluteDrinkLists()
    {
        foreach (Transform child in drinkContainer)
        {
            if (child == drinkTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (RestaurantObject restaurantObject in FoodManager.Instance.GetAvailableDrinkObjectLists())
        {
            Transform restaurantObjectTransform = Instantiate(drinkTemplate, drinkContainer);
            restaurantObjectTransform.gameObject.SetActive(true);
            restaurantObjectTransform.GetComponent<FoodSingleUI>().SetRestaurantObjectSO(restaurantObject);
        }
    }
    public void loadFoodInCasaCounter(BaseDiningTable diningTable)
    {
        OrdersOverview.Instance.clearIcons();
        setDiningTable(diningTable);
        diningTableName.text = getDiningTable().name;
        polluteFoodLists();
        polluteDrinkLists();
        restaurantObjectContainer.gameObject.SetActive(true);
        tableContainer.gameObject.SetActive(false);
    }

    public void loadTableInCasaCounter()
    {
        polluteTableLists();
        restaurantObjectContainer.gameObject.SetActive(false);
        tableContainer.gameObject.SetActive(true);
    }
    private void setDiningTable(BaseDiningTable diningTable)
    {
        this.diningTable = diningTable;
    }

    public BaseDiningTable getDiningTable()
    {
        return this.diningTable;
    }

    private void removeDiningTable()
    {
        this.diningTable = null;
    }

    public void closeCasaTableUI()
    {
        //tableTemplate.gameObject.SetActive(false);
        //tableContainer.gameObject.SetActive(false);
        //foodTemplate.gameObject.SetActive(false);
        //drinkTemplate.gameObject.SetActive(false);
        //restaurantObjectContainer.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
        closeBtn.gameObject.SetActive(false);
    }
}
