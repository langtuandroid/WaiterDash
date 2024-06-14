using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CasaCounter : BaseCounter
{
    [SerializeField] private List<CustomerObject> customer;
    [SerializeField] private List<BaseDiningTable> diningTables;
    public static event EventHandler OnCasaInteract;
    public static CasaCounter Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
    }

    private void Update()
    {
    }
    public override void Interact(Player player)
    {
        Debug.Log("Interact");
        OnCasaInteract?.Invoke(player, EventArgs.Empty);
    }
}
