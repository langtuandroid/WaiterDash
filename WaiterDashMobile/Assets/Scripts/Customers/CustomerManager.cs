using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using Assets.Scripts.Managers;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private Transform firstPosition;
    [SerializeField] private Transform exitPosition;
    [SerializeField] private GameObject cube;
    [SerializeField] private CustomerWaitingQueue customerWaitingQueue;
    [SerializeField] private List<CustomerObject> customers;
    [SerializeField] private ResturantDiningTable resturantDiningTable;
    [SerializeField] private List<BaseDiningTable> diningTables;
    [SerializeField] private Transform customerSpawnPosition;
    private List<CustomerObject> generatedCustomers = new List<CustomerObject>();
    private FunctionPeriodic customerSpanningPeriodic;
    int counter = 0;

    private void Update()
    {
    }
    private void Start()
    {
        RestaurantManager.OnRestaurantClosed += RestaurantManager_OnRestaurantClosed;
        RestaurantManager.OnRestaurantOpened += RestaurantManager_OnRestaurantOpened;
    }

    private void RestaurantManager_OnRestaurantOpened(object sender, System.EventArgs e)
    {
        generateCustomers();
    }

    private void RestaurantManager_OnRestaurantClosed(object sender, System.EventArgs e)
    {
        resetCustomers();
    }

    private void resetCustomers()
    {
        foreach (var customer in generatedCustomers)
        {
            if (customer != null)
            {
               // customer.removeNavMesh();
                Destroy(customer.gameObject);
            }
        }
        generatedCustomers.Clear();
        counter = 0;
        customerWaitingQueue.resetCustomers();
        if (customerSpanningPeriodic != null)
        {
            customerSpanningPeriodic.DestroySelf();
            customerSpanningPeriodic = null;
        }
    }
    private void generateCustomers()
    {
        List<Vector3> waitingQueuePositionList = new List<Vector3>();
        float positionSize = 2f;
        Vector3 firstPositionVector = firstPosition.position;
        firstPositionVector.z = firstPositionVector.z - 1f;
        firstPositionVector.x = firstPositionVector.x + 0.5f;
        for (int i = 0; i < 5; i++)
        {
            waitingQueuePositionList.Add(firstPositionVector + new Vector3(0, 0, -.7f) * positionSize * i);
        }
        // waitingQueuePositionList.Add(waitingQueuePositionList[waitingQueuePositionList.Count - 1] + new Vector3(0,0, -positionSize));
        customerWaitingQueue.customerWaitingQueue(waitingQueuePositionList, cube);
        customerSpanningPeriodic = FunctionPeriodic.Create(() =>
        {
            if (customerWaitingQueue.CanAddGuest())
            {
                counter++;
                CustomerObject newCustomer = Instantiate(GetRandomCustomer(), customerSpawnPosition.position, Quaternion.identity);
                newCustomer.name = "Customer_" + counter.ToString();
                customerWaitingQueue.AddCustomer(newCustomer);
                generatedCustomers.Add(newCustomer);
            }
        }, 5f);


        customerWaitingQueue.OnCustomerArrivedAtFrontOfQueue += CustomerWaitingQueue_OnCustomerArrivedAtFrontOfQueue;
        customerWaitingQueue.OnCustomerAdded += CustomerWaitingQueue_OnCustomerAdded;
        resturantDiningTable.ResturantDiningTables(customerWaitingQueue, diningTables, exitPosition.position);
    }

    private CustomerObject GetRandomCustomer()
    {
        int randomIndex = Random.Range(0, customers.Count);
        CustomerObject randomCustomer = customers[randomIndex];
        return randomCustomer;
    }

    private void CustomerWaitingQueue_OnCustomerAdded(object sender, System.EventArgs e)
    {
        Debug.Log("Customer Added.");

    }

    private void CustomerWaitingQueue_OnCustomerArrivedAtFrontOfQueue(object sender, System.EventArgs e)
    {
        Debug.Log("test");
    }
}
