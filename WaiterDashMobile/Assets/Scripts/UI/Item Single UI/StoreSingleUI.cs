using Assets.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Item_Single_UI
{
    public class StoreSingleUI : MonoBehaviour
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI price;
        [SerializeField] private TextMeshProUGUI ItemName;
        [SerializeField] private TextMeshProUGUI level;
        [SerializeField] private Image type;

        private StoreItemSO storeItem;
        public void setStoreItem(StoreItemSO itemData)
        {
            storeItem = itemData;
            itemIcon.sprite = storeItem.icon;
            ItemName.text = storeItem.ItemName;
            price.text = storeItem.price.ToString();
            level.text = storeItem.level.ToString();
            type.sprite = TypesManager.Instance.getTypeSprite(storeItem.type);

            this.gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                GridSystem.Instance.SetOriginalPrefabSize(itemData.prefab.gameObject);
                OnScreenUI.Instance.hideStoreUI();
            });
        }

    }
}
