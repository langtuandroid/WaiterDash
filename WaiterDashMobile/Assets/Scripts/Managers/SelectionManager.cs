using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    private IPrefabSelectable currentSelected;
    [SerializeField] private SelectionUI selectionUI;
    private void OnEnable()
    {
        BaseCounter.OnSelectableSelected += HandleSelectableSelected;
        BaseDiningTable.OnSelectableSelected += HandleSelectableSelected;
    }

    private void OnDisable()
    {
        BaseCounter.OnSelectableSelected -= HandleSelectableSelected;
        BaseDiningTable.OnSelectableSelected -= HandleSelectableSelected;
    }

    private void HandleSelectableSelected(IPrefabSelectable selected)
    {
        if (currentSelected != null)
        {
            //currentSelected.Deselect();
            
            selectionUI.Hide(); // hide will deselect the selected
        }

        currentSelected = selected;
        currentSelected.Select();
        if (selected is IUpgradeable upgradeable)
        {
            selectionUI.Show(currentSelected, upgradeable, upgradeable.GetUpgradeSO());
        }

    }

}
