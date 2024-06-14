using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour, IRestaurantObjectParent
{
    public static Player Instance { get; private set; }
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMasks;
    [SerializeField] private LayerMask diningTableLayerMasks;
    [SerializeField] private Transform restaurantObjectHoldPoint;
    [SerializeField] private Transform playerVisuals;
    private Vector3 lastInteractDir;
    private bool isWalking;
    private BaseCounter selectedCounter;
    private BaseDiningTable selectedDiningTable;
    private BaseInteractablePrefab selectedInteractablePrefab;
    private RestaurantObject restaurantObject;
    private float interactStartTimer = 0.5f;
    private float interactTimer;
    public event EventHandler OnPickedSomething;
    public event EventHandler OnDroppedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public event EventHandler<OnSelectedDiningTableChangedEventArgs> OnSelectedDiningTableChanged;
    public event EventHandler<OnSelectedInteractablePrefabChangedEventArgs> OnSelectedInteractablePrefabChanged;
    private bool isInteracting;
    private Vector2 inputVector;
    private Vector3 moveDir;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    public class OnSelectedDiningTableChangedEventArgs : EventArgs
    {
        public BaseDiningTable selectedDiningTable;
    }

    public class OnSelectedInteractablePrefabChangedEventArgs : EventArgs
    {
        public BaseInteractablePrefab selectedInteractablePrefab;
    }

    private void Awake()
    {
        Instance = this;

    }

    private void OnEnable()
    {
        RestaurantManager.OnRestaurantClosed += RestaurantManager_OnRestaurantClosed;
        RestaurantManager.OnRestaurantOpened += RestaurantManager_OnRestaurantOpened;
    }

    private void OnDisable()
    {
        RestaurantManager.OnRestaurantClosed -= RestaurantManager_OnRestaurantClosed;
        RestaurantManager.OnRestaurantOpened -= RestaurantManager_OnRestaurantOpened;
    }
    private void Start()
    {

    }

    private void RestaurantManager_OnRestaurantOpened(object sender, EventArgs e)
    {
        playerVisuals.gameObject.SetActive(true);
    }

    private void RestaurantManager_OnRestaurantClosed(object sender, EventArgs e)
    {
        playerVisuals.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (RestaurantManager.Instance.IsRestaurantOpen())
        {
            HandleMovement();
            HandleInteractions();
        } else
        {
            isInteracting = false;
            SetSelectedCounter(null);
            SetSelectedDiningTable(null);
            SetSelectedInteractablePrefab(null);
            interactStartTimer = 0.5f;
        }
    }
    private void HandleMovement()
    {
        inputVector = gameInput.GetMomentVectorNormalized();
        moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.4f;
        float playerHeight = 2f;
        int layerMask = ~LayerMask.GetMask("TileLayer"); // Ignore only the TileLayer
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance, layerMask);

        // Rotate towards the direction of movement if there is any movement input
        if (inputVector != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        if (canMove)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        isWalking = moveDir != Vector3.zero;
    }

    private void HandleInteractions()
    {
        inputVector = gameInput.GetMomentVectorNormalized();
        // Converting from Vector 2 to Vector 3 because transform is vector 3 object.
        moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float interactDistance = 0.8f;

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHitCounter, interactDistance))
        {
            if (raycastHitCounter.transform.TryGetComponent(out IInteractable interactable))
            {

                interactStartTimer -= Time.deltaTime;
                if (interactStartTimer < 0f)
                {
                    if (!isInteracting)
                    {
                        isInteracting = true;
                        if (interactable is BaseCounter baseCounter)
                        {
                            if (baseCounter != this.selectedCounter)
                            {
                                SetSelectedCounter(baseCounter);
                                selectedCounter.Interact(this);
                            }
                        }
                        else if (interactable is BaseDiningTable baseDiningTable)
                        {
                            if (baseDiningTable != this.selectedDiningTable)
                            {
                                SetSelectedDiningTable(baseDiningTable);
                                selectedDiningTable.Interact(this);
                            }
                        }
                        else if (interactable is BaseInteractablePrefab baseInteractablePrefab)
                        {
                            if (baseInteractablePrefab != this.selectedInteractablePrefab)
                            {
                                SetSelectedInteractablePrefab(baseInteractablePrefab);
                                selectedInteractablePrefab.Interact(this);
                            }
                        }
                        else
                        {
                            interactable.Interact(this);
                        }
                    }

                }
            }
            else
            {
                isInteracting = false;
                SetSelectedCounter(null);
            }
        }
        else
        {
            isInteracting = false;
            SetSelectedCounter(null);
            SetSelectedDiningTable(null);
            SetSelectedInteractablePrefab(null);
            interactStartTimer = 0.5f;
        }
    }


    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public BaseCounter getSelectedCounter()
    {
        return this.selectedCounter;
    }

    private void SetSelectedDiningTable(BaseDiningTable selectedDiningTable)
    {
        this.selectedDiningTable = selectedDiningTable;

        OnSelectedDiningTableChanged?.Invoke(this, new OnSelectedDiningTableChangedEventArgs
        {
            selectedDiningTable = selectedDiningTable
        });
    }

    private void SetSelectedInteractablePrefab(BaseInteractablePrefab selectedInteractablePrefab)
    {
        this.selectedInteractablePrefab = selectedInteractablePrefab;
        OnSelectedInteractablePrefabChanged?.Invoke(this, new OnSelectedInteractablePrefabChangedEventArgs
        {
            selectedInteractablePrefab = selectedInteractablePrefab
        });
    }
    public bool IsWalking()
    {
        return isWalking;
    }

    public float GetInteractiveTimerNormalized()
    {
        return 1 - (interactStartTimer / .5f);
    }

    public Transform GetRestaurantObjectFollowTransform()
    {
        return restaurantObjectHoldPoint;
    }

    public void SetRestaurantObject(RestaurantObject restaurantObject)
    {
        this.restaurantObject = restaurantObject;
        if (restaurantObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public RestaurantObject GetRestaurantObject()
    {
        return restaurantObject;
    }

    public void ClearRestaurantObject()
    {
        restaurantObject = null;
        OnDroppedSomething?.Invoke(this, EventArgs.Empty);
    }

    public bool HasRestaurantObject()
    {
        return restaurantObject != null;
    }

    public bool isPlayerInteracting()
    {
        return isInteracting;
    }
}
