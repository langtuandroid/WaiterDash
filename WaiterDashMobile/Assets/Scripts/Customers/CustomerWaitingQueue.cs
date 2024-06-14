using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomerWaitingQueue : MonoBehaviour
{
    public event EventHandler OnCustomerArrivedAtFrontOfQueue;
    public event EventHandler OnCustomerAdded;
    private const float POSITION_SIZE = 8f;
    private List<CustomerObject> customerLists;
    private List<Vector3> positionList;
    private Vector3 entrancePosition;
    private bool customerWalking;
    public static CustomerWaitingQueue Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        customerLists = new List<CustomerObject>();
    }
    public void customerWaitingQueue(List<Vector3> positionList, GameObject cube)
    {
        this.positionList = positionList;
        CalculateEntrancePosition();
        foreach (var position in positionList)
        {
            Instantiate(cube, position, new Quaternion());
        }
        Instantiate(cube, entrancePosition, new Quaternion());

    }

    private void CalculateEntrancePosition()
    {
        if (positionList.Count <= 1)
        {
            entrancePosition = positionList[positionList.Count - 1];
        }
        else
        {
            Vector3 dir = positionList[positionList.Count - 1] - positionList[positionList.Count - 2];
            entrancePosition = positionList[positionList.Count - 1] + dir;
        }
    }

    public void AddPosition(Vector3 position)
    {
        positionList.Add(position);
        World_Sprite.Create(position, new Vector3(1, 0, 1), Color.green);
        CalculateEntrancePosition();
    }

    public void AddPosition_Down()
    {
        AddPosition(positionList[positionList.Count - 1] + new Vector3(0, 0, -1) * POSITION_SIZE);
    }

    public void AddPosition_Up()
    {
        AddPosition(positionList[positionList.Count - 1] + new Vector3(0, 0, +1) * POSITION_SIZE);
    }

    public void AddPosition_Left()
    {
        AddPosition(positionList[positionList.Count - 1] + new Vector3(-1, 0, 0) * POSITION_SIZE);
    }

    public void AddPosition_Right()
    {
        AddPosition(positionList[positionList.Count - 1] + new Vector3(+1, 0, 0) * POSITION_SIZE);
    }

    public bool CanAddGuest()
    {
        return customerLists.Count < positionList.Count && customerWalking == false;
    }

    public void AddCustomer(CustomerObject customer)
    {
        customerWalking = true;
        customerLists.Add(customer);
        var customerNewIndex = customerLists.IndexOf(customer);
        //customerLists.RemoveAt(customerLists.Count - 1);
        customer.MoveTo(positionList[customerNewIndex], () =>
        {
            //customerLists.Add(customer);
            CustomerArrivedAtQueuePosition(customer);
            customerWalking = false;
        });
        OnCustomerAdded?.Invoke(this, EventArgs.Empty);
    }



    public CustomerObject GetFirstInQueue()
    {
        if (customerLists.Count == 0)
        {
            return null;
        }
        else
        {
            CustomerObject customer = customerLists[0];
            if(customer.IsWalking() == false)
            {
                customerLists.RemoveAt(0);
                RelocateAllCustomers();
                return customer;
            } else
            {
                return null;
            }

        }
    }

    private void RelocateAllCustomers()
    {
        for (int i = 0; i < customerLists.Count; i++)
        {
            CustomerObject customer = customerLists[i];
            customer.MoveTo(positionList[i], () =>
            {
                Debug.Log("Relocate customer");
                CustomerArrivedAtQueuePosition(customer);
            });
        }
    }

    private void CustomerArrivedAtQueuePosition(CustomerObject customer)
    {
        if (customer == customerLists[0])
        {
            Debug.Log("If customer at front.");

            OnCustomerArrivedAtFrontOfQueue?.Invoke(this, EventArgs.Empty);
        }
    }

    public void resetCustomers()
    {
        if (customerLists != null)
        {
            customerLists.Clear();
            customerWalking = false;
        }

    }

}
