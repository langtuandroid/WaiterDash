using SingularityGroup.HotReload;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnReload : MonoBehaviour
{
    [InvokeOnHotReload]
    void Run()
    {
        Debug.Log("Hot Reloaded");
    }
}
