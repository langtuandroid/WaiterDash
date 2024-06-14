using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BaseInteractablePrefab : MonoBehaviour, IInteractable
{
    private void Start()
    {

    }
    public virtual void Interact(Player player)
    {

    }

    public string GetInteractText()
    {
        throw new NotImplementedException();
    }
}
