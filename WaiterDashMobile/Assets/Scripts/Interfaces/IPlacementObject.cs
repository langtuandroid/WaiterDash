using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlacementObject
{
    Transform GetPrefab();
    Transform GetPrefabVisual();

    Vector2Int GetPrefabSize();

}
