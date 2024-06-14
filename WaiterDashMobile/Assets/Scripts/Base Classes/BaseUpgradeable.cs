using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UpgradeSO;

public abstract class BaseUpgradeable : MonoBehaviour, IUpgradeable
{
    [SerializeField] protected UpgradeSO upgradeDataSO;
    public static event Action<BaseUpgradeable> OnUpgradedToNewPrefab;
    protected int currentLevel = 1;
    public int CurrentLevel => currentLevel;
    public GameObject GameObject => gameObject;
    public int GetCurrentLevel()
    {
        return CurrentLevel;
    }

    public void SetCurrentLevel(int level)
    {
        currentLevel = level; 
    }
    private void Start()
    {
        
    }

    public UpgradeSO GetUpgradeSO()
    {
        return upgradeDataSO;
    }

    private void applyCurrentLevelOnLoad()
    {

    }

    public void Upgrade()
    {
        if (currentLevel < upgradeDataSO.levels.Count + 1)
        {
            var upgradeLevel = upgradeDataSO.GetUpgradeLevel(currentLevel);
            GameObject oldPrefabInstance = gameObject;
            GameObject newPrefabInstance = null;

            if (upgradeLevel.UpgradeablePrefab != null)
            {
                newPrefabInstance = ChangePrefab(upgradeLevel);
            }


            if (newPrefabInstance != null)
            {
                SetStateFrom(oldPrefabInstance, newPrefabInstance);
                OnUpgradedToNewPrefab?.Invoke(newPrefabInstance.GetComponent<BaseUpgradeable>());
                Destroy(oldPrefabInstance);
            }
            ApplyUpgrade(upgradeLevel);
            currentLevel++;
        }
    }

    protected abstract void ApplyUpgrade(UpgradeSO.UpgradeLevel upgradeLevel);

    private GameObject ChangePrefab(UpgradeSO.UpgradeLevel upgradeLevel)
    {
        if (upgradeLevel.UpgradeablePrefab != null)
        {
            // Instantiate the new prefab and replace the current one
            GameObject newPrefabInstance = Instantiate(upgradeLevel.UpgradeablePrefab, transform.position, transform.rotation);
            newPrefabInstance.transform.SetParent(transform.parent);

            // Return the new instance for further state transfer
            return newPrefabInstance;
        }
        return null;
    }
    protected virtual void SetStateFrom(GameObject oldInstance, GameObject newInstance)
    {
        var oldOutline = oldInstance.GetComponent<Outline>();
        var newOutline = newInstance.GetComponent<Outline>();

        if (oldOutline != null && newOutline != null)
        {
            newOutline.enabled = oldOutline.enabled;
        }

        var oldBaseUpgradeable = oldInstance.GetComponent<BaseUpgradeable>();
        var newBaseUpgradeable = newInstance.GetComponent<BaseUpgradeable>();

        if (oldBaseUpgradeable != null && newBaseUpgradeable != null)
        {
            newBaseUpgradeable.currentLevel = oldBaseUpgradeable.currentLevel;
        }
    }


    public void Select()
    {
        var outline = gameObject.GetComponent<Outline>();
        if (outline != null) outline.enabled = true;
    }

    public void Deselect()
    {
        var outline = gameObject.GetComponent<Outline>();
        if (outline != null) outline.enabled = false;
    }

    public void ResetOutlineEnabled()
    {
        var outline = gameObject.GetComponent<Outline>();
        if (outline != null)
        {
            Destroy(outline);
            StartCoroutine(AddOutlineEnabledNextFrame());
        }
        else
        {
            gameObject.AddComponent<Outline>();
        }
    }

    public void ResetOutlineDisabled()
    {
        var outline = gameObject.GetComponent<Outline>();
        if (outline != null)
        {
            Destroy(outline);
            StartCoroutine(AddOutlineDisabledNextFrame());
        }
        else
        {
            gameObject.AddComponent<Outline>();
        }
    }

    private IEnumerator AddOutlineDisabledNextFrame()
    {
        yield return null; // Wait until the next frame
        gameObject.AddComponent<Outline>().enabled = false;
    }

    private IEnumerator AddOutlineEnabledNextFrame()
    {
        yield return null; // Wait until the next frame
        gameObject.AddComponent<Outline>().enabled = true;
    }

}

