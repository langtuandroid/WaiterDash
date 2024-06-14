using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RestaurantObject : MonoBehaviour
{
    [SerializeField] private RestaurantObjectSO restaurantObjectSO;

    private IRestaurantObjectParent restaurantObjectParent;

    public RestaurantObjectSO GetRestaurantObjectSO()
    {
        return restaurantObjectSO;
    }


    public void SetRestaurantObjectParent(IRestaurantObjectParent restaurantObjectParent, Transform ObjectFollowTransform)
    {
        if(this.restaurantObjectParent != null)
        {
            this.restaurantObjectParent.ClearRestaurantObject();
        }
        this.restaurantObjectParent = restaurantObjectParent;

        if(restaurantObjectParent.HasRestaurantObject())
        {
            Debug.LogError("Counter already has a restaurant Object!");
        }
        restaurantObjectParent.SetRestaurantObject(this);
        transform.parent = ObjectFollowTransform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

    }

    public IRestaurantObjectParent GetClearCounter()
    {
        return restaurantObjectParent;
    }

    public void DestorySelf()
    {
        restaurantObjectParent.ClearRestaurantObject();
        Destroy(gameObject);

    }

    public static RestaurantObject SpawnRestaurantObject(RestaurantObjectSO restaurantObjectSO, IRestaurantObjectParent restaurantObjectParent)
    {
        Transform restaurantObjectTransform = Instantiate(restaurantObjectSO.prefab);
        RestaurantObject restaurantObject = restaurantObjectTransform.GetComponent<RestaurantObject>();
        restaurantObject.SetRestaurantObjectParent(restaurantObjectParent, restaurantObjectParent.GetRestaurantObjectFollowTransform());
        return restaurantObject;
    }
}
