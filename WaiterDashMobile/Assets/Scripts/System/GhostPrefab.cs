using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GhostPrefab : MonoBehaviour
{
    [SerializeField] public List<Tile> ghostPrefabTiles;
    [SerializeField] public Button placePrefab;
    [SerializeField] public Button cancelPrefab;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material greenMaterial;
    public bool canPlace = false;
    private Renderer[] renderers;

    private void Awake()
    {
        // Cache renderers
    }

    public void getRenders()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }
    public void OnCanPlace()
    {
        canPlace = true;
        //ghostPrefabTiles.ForEach(tile =>
        //{
        //    tile.setTileAvailable();
        //});
        foreach (Renderer renderer in renderers)
        {
            var materials = renderer.sharedMaterials.ToList();
            materials.Clear();
            materials.Add(greenMaterial);
            renderer.materials = materials.ToArray();
        }

    }

    public void OnCannotPlace()
    {
        canPlace = false;
        //ghostPrefabTiles.ForEach(tile =>
        //{
        //    tile.setTileOccupiedColor();
        //});
        foreach (Renderer renderer in renderers)
        {
            var materials = renderer.sharedMaterials.ToList();
            materials.Clear();
            materials.Add(redMaterial);
            renderer.materials = materials.ToArray();
        }
    }

    public bool canPlacePrefab()
    {
        return canPlace;
    }
}
