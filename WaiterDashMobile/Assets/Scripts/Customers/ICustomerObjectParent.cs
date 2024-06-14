using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ICustomerObjectParent {

    public Transform[] GetCustomerObjectFollowTransform();

    public void SetCustomerObject(CustomerObject customerObject);
    public CustomerObject GetCustomersObject();

    public void ClearCustomerObject();

    public bool HasCustomerObject();
}
