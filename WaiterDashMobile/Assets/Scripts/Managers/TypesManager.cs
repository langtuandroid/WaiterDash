using Assets.Scripts.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class TypesManager : MonoBehaviour
    {
        public static TypesManager Instance { get; private set; }
        [SerializeField] private List<StoreItemTypesSO> StoreItemTypesLists;


        private void Awake()
        {
            Instance = this;
        }

        public Sprite getTypeSprite(ItemType type)
        {
            Sprite typeIcon = null;
            foreach (var storeItem in StoreItemTypesLists)
            {
                if (storeItem.type == type)
                {
                    typeIcon = storeItem.typeIcon;
                }
            }
            return typeIcon;
        }
    }
}
