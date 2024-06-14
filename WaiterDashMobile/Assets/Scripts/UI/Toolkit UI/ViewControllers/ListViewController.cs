using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class ListViewController<T, U> : MonoBehaviour where T : MonoBehaviour where U : ScriptableObject
{
    public Transform itemContainer;
    public Transform itemTemplate;

    // List of items
    public List<U> items;

    private void Awake()
    {
        itemTemplate.gameObject.SetActive(false);
        PopulateItemList();
    }

    private void Start()
    {
    }

    // Abstract method to instantiate UI elements
    protected abstract void InstantiateUIElements(Transform item, U itemData);

    public void PopulateItemList()
    {
        // Clear any existing items in the container
        foreach (Transform child in itemContainer)
        {
            if (child == itemTemplate) continue;
            Destroy(child.gameObject);
        }
        // Populate the item list
        foreach (var itemData in items)
        {
            // Instantiate item template
            Transform item = Instantiate(itemTemplate, itemContainer);
            // Instantiate UI elements specific to derived class
            item.gameObject.SetActive(true);
            InstantiateUIElements(item, itemData);
        }
    }

}
