using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gridtest : MonoBehaviour
{
    public GameObject prefabToPlace;
    public Grid grid;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // On left click
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = grid.WorldToCell(worldPosition);

            PlacePrefab(cellPosition);
        }
    }

    void PlacePrefab(Vector3Int cellPosition)
    {
        Vector3 worldPosition = grid.CellToWorld(cellPosition);
        Instantiate(prefabToPlace, worldPosition, Quaternion.identity);
    }
}
