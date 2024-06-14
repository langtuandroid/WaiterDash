using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CodeMonkey;
using CodeMonkey.Utils;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Assets.Scripts.Customers;

public class CustomerObject : MonoBehaviour
{
    private ICustomerObjectParent customerObjectParent;
    private RestaurantObject restaurantObject;
    private BaseDiningTable diningTable;
    private NavMeshAgent navMeshAgent;
    private bool isEating;
    private bool isWalking;
    private bool isSitting;
    private bool isWaitingForService;
    [SerializeField] private CustomerOrderVisualUI customerOrderVisualUI;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {

    }
    private void Update()
    {
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void MoveTo(Vector3 movePosition, Action onArrivedAtPosition = null)
    {
        // gameObject.transform.position = movePosition;
        navMeshAgent.destination = movePosition;
        bool hasArrived = false;
        isWalking = true;
        FunctionUpdater.Create(() =>
        {
            if (!navMeshAgent) return false;
            if (!navMeshAgent.pathPending && navMeshAgent.isActiveAndEnabled)
            {
                var remainingDistance = navMeshAgent.remainingDistance;
                if (remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        isWalking = false;
                        hasArrived = true;
                        isSitting = true;
                        onArrivedAtPosition();
                    }
                }
                else
                {
                    isSitting = false;
                    isWalking = true;
                }
            }
            return hasArrived;
        });


    }

    public void StandUp()
    {
        isSitting = false;
    }

    public void AskForService()
    {
        isWaitingForService = true;
    }

    public void ServiceReceived()
    {
        isWaitingForService = false;

    }

    public void StopMovement()
    {
        navMeshAgent.enabled = false;
    }
    public void StartMovement()
    {
        navMeshAgent.enabled = true;
    }

    public void PlayAnimationDining(Action onArrivedAtPosition = null)
    {
        FunctionTimer.Create(() =>
        {
            onArrivedAtPosition();
        }, 5f);
    }
    public void SetRestaurantObject(RestaurantObject restaurantObject)
    {
        if (!HasRestaurantObject())
        {
            this.restaurantObject = restaurantObject;
        }
    }

    public void SetDiningTable(BaseDiningTable diningTable)
    {
        this.diningTable = diningTable;
    }
    public bool HasRestaurantObject()
    {
        return restaurantObject != null;
    }
    public bool IsSittingOnATable()
    {
        return diningTable != null;
    }

    public void SetCustomerOrderIcon(CustomerOrder customerOrder)
    {
        customerOrderVisualUI.UpdateCustomerOrderIcon(customerOrder);
    }

    public void HideCustomerFoodOrderIcon()
    {
        customerOrderVisualUI.DisableCustomerFoodOrderIcon();
    }
    public void HideCustomerDrinkOrderIcon()
    {
        customerOrderVisualUI.DisableCustomerDinkOrderIcon();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public bool IsSitting()
    {
        if (IsSittingOnATable() && isSitting)
        {
            return true;

        }
        else
        {
            return false;
        }
    }

    public bool IsWaitingForService()
    {
        if (IsSittingOnATable() && isSitting && isWaitingForService)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void StartEating()
    {
        isEating = true;
    }

    public void StopEating()
    {
        isEating = false;
    }
    public bool IsEating()
    {
        return isEating;
    }

    public void removeNavMesh()
    {
        navMeshAgent.isStopped = true;
    }
}
