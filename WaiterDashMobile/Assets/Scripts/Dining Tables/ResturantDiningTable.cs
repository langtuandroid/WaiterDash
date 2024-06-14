using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ResturantDiningTable : MonoBehaviour
{
    public static ResturantDiningTable Instance { get; private set; }

    private CustomerWaitingQueue customerWaitingQueue;
    private List<BaseDiningTable> diningTableList;
    private Vector3 exitPosition;

    private void Awake()
    {
        Instance = this;
    }
    public void ResturantDiningTables(CustomerWaitingQueue customerWaitingQueue, List<BaseDiningTable> diningTableList, Vector3 exitPosition)
    {
        this.customerWaitingQueue = customerWaitingQueue;
        this.diningTableList = diningTableList;
        this.exitPosition = exitPosition;
        // customerWaitingQueue.OnCustomerArrivedAtFrontOfQueue += CustomerWaitingQueue_OnCustomerArrivedAtFrontOfQueue;
    }

    private void CustomerWaitingQueue_OnCustomerArrivedAtFrontOfQueue(object sender, System.EventArgs e)
    {
        Debug.Log("Customer at front of queue");

        //TrySendCustomerToTable();
    }

    private void TrySendCustomerToTable()
    {
        //BaseDiningTable emptyDiningTable = GetEmptyDiningTable();
        //if (emptyDiningTable != null)
        //{
        //    List<CustomerObject> customerObjectLists = new List<CustomerObject>();
        //    CustomerObject customerObject = customerWaitingQueue.GetFirstInQueue();
        //    if (customerObject != null)
        //    {
        //        customerObjectLists.Add(customerObject);
        //        //emptyDiningTable.SetCustomerObject(customerObjectLists, () =>
        //        //{
        //        //    customerObject.PlayAnimationDining(() =>
        //        //    {
        //        //        emptyDiningTable.ClearCustomerObject();
        //        //        customerObject.MoveTo(exitPosition, () =>
        //        //        {
        //        //            TrySendCustomerToTable();
        //        //            Debug.Log("Clear and send customer to table");

        //        //        });
        //        //    });
        //        //});
        //    }
        //}
    }

    private BaseDiningTable GetEmptyDiningTable()
    {
        foreach (BaseDiningTable diningTable in diningTableList)
        {
            if (!diningTable.HasCustomerObject())
            {
                Debug.Log("Dining is Free");
                return diningTable;
            }
        }
        return null;
    }


    public List<BaseDiningTable> GetDiningTables()
    {
        return diningTableList;
    }
}
