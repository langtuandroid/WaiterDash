using Assets.Scripts.UI.Item_Single_UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StoreView : ListViewController<StoreView, StoreItemSO>
{
    protected override void InstantiateUIElements(Transform storeItem, StoreItemSO itemData)
    {
        storeItem.GetComponent<StoreSingleUI>().setStoreItem(itemData);
    }
}