using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject ghostPrefabTemplate;
    public static GridSystem Instance { get; set; }
    public static event EventHandler OnGridSystemEditEnable;
    public static event EventHandler OnGridSystemEditDisable;
    public int width = 10;
    public int depth = 10;
    public float tileSize = 1.0f;
    public bool[,] occupied;
    private bool placingGhost = false;
    private Quaternion currentRotation = Quaternion.identity;
    private Vector2Int currentPrefabSize;
    private Vector2Int originalPrefabSize;
    private Transform currentGhostPrefab;
    private Transform currentPrefab;
    private Vector3 currentPrefabGridPosition;

    private bool isDraggingGhost = false;
    private bool editState = false;
    private void Awake()
    {
        Instance = this;
        generateTiles();

    }
    private void Start()
    {
        //disableGridSystem();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            enableGridSystem();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            disableGridSystem();
        }
        if (editState)
        {
            //if (Input.GetMouseButtonDown(0) && currentPrefab != null) // Check for left mouse click
            //{
            //    RaycastHit hit;
            //    Debug.Log("hit");
            //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //    int layerMask = 1 << LayerMask.NameToLayer("TileLayer"); // Only hit the tile layer
            //    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            //    {
            //        Vector3 gridPosition = WorldToGridPosition(hit.point);
            //        PlaceObjectOnGrid(gridPosition, currentPrefabSize);
            //    }
            //}
            // Check if the mouse clicks on the ghost prefab
            if (Input.GetMouseButtonDown(0) && currentGhostPrefab != null)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("Hit object name: " + hit.collider.gameObject.name);
                    if (hit.collider.gameObject.transform == currentGhostPrefab)
                    {
                        isDraggingGhost = true; // Start dragging the ghost prefab
                    }
                }
            }
            // Check if the mouse is released
            if (Input.GetMouseButtonUp(0))
            {
                isDraggingGhost = false; // Stop dragging the ghost prefab
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                //ToggleGhostPlacement();
            }

            if (placingGhost)
            {
                UpdateGhostPosition();
            }

            // Rotate the placement prefab when the R key is pressed
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotatePlacementPrefab();
            }
        } else
        {

        }

    }

    public void enableGridSystem()
    {
        editState = true;
        OnGridSystemEditEnable?.Invoke(this, EventArgs.Empty);
    }
    public void disableGridSystem()
    {
        editState = false;
        OnGridSystemEditDisable?.Invoke(this, EventArgs.Empty);
    }

    private void generateTiles()
    {
        occupied = new bool[width, depth];
        InitializeWithExistingObjects();
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                Vector3 tilePosition = new Vector3(x * tileSize, 0, z * tileSize);
                GameObject tile = Instantiate(_tilePrefab, tilePosition, Quaternion.identity);
                if (occupied[x, z]) // Check if the current tile is occupied
                {
                    tile.name = x + " x " + z;
                    // If occupied, change the tile color or call the method to update the tile
                    var tilePrefab = tile.GetComponent<Tile>();
                    tilePrefab.setTileOccupiedColor();
                }
            }
        }
    }

    private void InitializeWithExistingObjects()
    {
        // Calculate the bounds of the grid
        Vector3 gridMin = new Vector3(0, 0, 0);
        Vector3 gridMax = new Vector3(width * tileSize, 0, depth * tileSize);

        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (var obj in allObjects)
        {
            // Check if the object has a collider (you may need to adjust this based on your requirements)
            Collider objCollider = obj.GetComponent<Collider>();
            if (objCollider == null)
                continue;

            // Calculate the bounds of the object
            Bounds objBounds = objCollider.bounds;

            // Check if the object is within the grid bounds
            if (IsWithinBounds(objBounds.min, gridMin, gridMax) && IsWithinBounds(objBounds.max, gridMin, gridMax))
            {
                // Iterate over the grid cells covered by the object
                for (float x = objBounds.min.x; x <= objBounds.max.x; x += tileSize)
                {
                    for (float z = objBounds.min.z; z <= objBounds.max.z; z += tileSize)
                    {
                        int gridX = Mathf.RoundToInt(x / tileSize);
                        int gridZ = Mathf.RoundToInt(z / tileSize);

                        if (IsWithinGridBounds(gridX, gridZ))
                        {
                            occupied[gridX, gridZ] = true; // Marking the position as occupied
                        }
                    }
                }
            }
        }
    }

    public bool gridEditState()
    {
        return editState;
    }

    private bool IsWithinBounds(Vector3 position, Vector3 min, Vector3 max)
    {
        return position.x >= min.x && position.x <= max.x &&
               position.z >= min.z && position.z <= max.z;
    }

    private bool IsWithinGridBounds(int x, int z)
    {
        return x >= 0 && x < width && z >= 0 && z < depth;
    }

    void RotatePlacementPrefab()
    {
        if (currentGhostPrefab != null)
        {
            // Rotate the prefab by 90 degrees around the Y axis
            currentGhostPrefab.transform.Rotate(Vector3.up, 90f);
            currentRotation = currentGhostPrefab.transform.rotation;
            Vector3 verticalRotation = new Vector3(0, 90, 0);
            Vector3 verticalRotation2 = new Vector3(0, 270, 0);
            // Update currentPrefabSize based on rotation
            if (currentRotation.eulerAngles == verticalRotation || currentRotation.eulerAngles == verticalRotation2)
            {
                // Swap the dimensions
                int temp = currentPrefabSize.x;
                currentPrefabSize.x = currentPrefabSize.y;
                currentPrefabSize.y = temp;
            }
            else
            {
                currentPrefabSize = GetOriginalPrefabSize();
            }
        }
    }

    Vector2Int GetOriginalPrefabSize()
    {
        return originalPrefabSize;
    }

    public void SetOriginalPrefabSize(GameObject placementGameObject)
    {
        enableGridSystem();
        var placementObject = placementGameObject.GetComponent<IPlacementObject>();
        currentPrefab = placementObject.GetPrefab();
        currentPrefabSize = placementObject.GetPrefabSize();
        originalPrefabSize = placementObject.GetPrefabSize();
        currentGhostPrefab = generateGhostPrefab(placementObject);
        placingGhost = true;
    }

    private Transform generateGhostPrefab(IPlacementObject originalPrefab)
    {
        GameObject prefab = Instantiate(ghostPrefabTemplate, Vector3.zero, Quaternion.identity, null);
        Instantiate(originalPrefab.GetPrefabVisual().transform.gameObject, Vector3.zero, Quaternion.identity, prefab.transform);
        prefab.name = "GhostPrefab";
        var ghostPrefab = prefab.GetComponent<GhostPrefab>();
        ghostPrefab.getRenders();
        // Check if GhostPrefab component already exists, if not, add it
        if (!ghostPrefab)
        {
            ghostPrefab = prefab.AddComponent<GhostPrefab>();
        }

        var ghostPrefabTiles = new List<Tile>();

        Vector2Int prefabSize = originalPrefab.GetPrefabSize();

        // Calculate the size of the prefab visually
        Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();
        Bounds bounds = renderers[0].bounds;
        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }
        Vector3 prefabSizeVector = bounds.size;

        // Add Box Collider to the prefab and adjust its size
        BoxCollider boxCollider = prefab.AddComponent<BoxCollider>();
        boxCollider.size = prefabSizeVector;

        for (int x = 0; x < prefabSize.x; x++)
        {
            for (int y = 0; y < prefabSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(x - (prefabSize.x - 1) * 0.5f, 0, y);
                Tile tile = Instantiate(_tilePrefab, tilePosition, Quaternion.identity, ghostPrefab.transform).GetComponent<Tile>();
                ghostPrefabTiles.Add(tile);
                tile.gameObject.SetActive(true);
            }
        }

        ghostPrefab.placePrefab.onClick.AddListener(() =>
        {
            if (ghostPrefab.canPlacePrefab())
            {
                PlaceObjectOnGrid(currentPrefabGridPosition, prefabSize);
            }
        });
        ghostPrefab.cancelPrefab.onClick.AddListener(() =>
        {
            placingGhost = false;
            resetPlacementPrefab();
        });

        ghostPrefab.ghostPrefabTiles = ghostPrefabTiles;
        return ghostPrefab.gameObject.transform;
    }

    void PlaceObjectAndDisableGhost()
    {
        // Call the method to place the object on the grid and disable the ghost prefab
        // PlaceObjectOnGrid(worldPosition, currentPrefabSize);
        currentGhostPrefab.gameObject.SetActive(false);
        placingGhost = false;
        resetPlacementPrefab();
    }

    void CancelPlacementAndDisableGhost()
    {
        // Cancel the placement and disable the ghost prefab
        Destroy(currentGhostPrefab.gameObject); // Destroy the ghost prefab
        placingGhost = false;
        resetPlacementPrefab();
    }


    //void ToggleGhostPlacement()
    //{
    //    placingGhost = !placingGhost;

    //    if (placingGhost)
    //    {
    //        // Instantiate the ghost prefab
    //        currentGhostPrefab = Instantiate(currentGhostPrefab.gameObject,Vector3.zero, currentRotation);
    //        currentGhostPrefab.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        // If the user stops ghost placement, instantiate the actual object
    //        if (ghostObject != null)
    //        {
    //            Destroy(ghostObject);
    //            // PlaceObjectOnGrid(ghostObject.transform.position, currentPrefabSize);
    //        }
    //    }
    //}
    void UpdateGhostPosition()
    {
        if (isDraggingGhost) // Check if the left mouse button is held down
        {
            // Raycast to determine the position on the grid
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << LayerMask.NameToLayer("TileLayer");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Vector3 gridPosition = WorldToGridPosition(hit.point);
                currentPrefabGridPosition = gridPosition;
                Vector3Int gridPositionInt = new Vector3Int(Mathf.RoundToInt(gridPosition.x), 0, Mathf.RoundToInt(gridPosition.z));
                var prefabSize1x1 = new Vector2Int(1, 1);
                Vector3 verticalRotation = new Vector3(0, 90, 0);
                Vector3 verticalRotation2 = new Vector3(0, 270, 0);
                if (currentPrefabSize != prefabSize1x1)
                {
                    if (currentRotation.eulerAngles == verticalRotation || currentRotation.eulerAngles == verticalRotation2)
                    {
                        gridPosition.z = gridPosition.z + 0.5f;
                    }
                    else
                    {
                        gridPosition.x = gridPosition.x + 0.5f;
                    }
                }
                currentGhostPrefab.gameObject.transform.position = gridPosition;

                // Check if the placement is valid and update color accordingly
                bool isValidPlacement = CanPlace(gridPositionInt);
                GhostPrefab ghostPrefab = currentGhostPrefab.gameObject.GetComponent<GhostPrefab>();
                if (isValidPlacement)
                {
                    ghostPrefab.OnCanPlace();
                }
                else
                {
                    ghostPrefab.OnCannotPlace();

                }
            }
        }
    }


    //Vector2Int CalculatePrefabSize(GameObject prefab)
    //{
    //    // Replace this with actual logic to calculate prefab size
    //    return new Vector2Int(2, 1);
    //}

    public void PlaceObjectOnGrid(Vector3 worldPosition, Vector2Int prefabSize)
    {
        Vector3Int gridPosition = WorldToGridPosition(worldPosition);
        // Adjust the grid position to place the object in the middle
        if (CanPlace(gridPosition))
        {
            for (int x = 0; x < prefabSize.x; x++)
            {
                for (int z = 0; z < prefabSize.y; z++)
                {
                    int checkX = gridPosition.x + x;
                    int checkZ = gridPosition.z + z;

                    occupied[checkX, checkZ] = true;
                    UpdateTileBorder(new Vector3Int(checkX, 0, checkZ)); // Update border color to red
                }
            }
            var prefabSize1x1 = new Vector2Int(1, 1);
            Vector3 verticalRotation = new Vector3(0, 90, 0);
            Vector3 verticalRotation2 = new Vector3(0, 270, 0);
            if (currentPrefabSize != prefabSize1x1)
            {
                if (currentRotation.eulerAngles == verticalRotation || currentRotation.eulerAngles == verticalRotation2)
                {
                    worldPosition.z = worldPosition.z + 0.5f;
                }
                else
                {
                    worldPosition.x = worldPosition.x + 0.5f;
                }
            }
            Instantiate(currentPrefab, worldPosition, currentRotation);
            placingGhost = false;
            resetPlacementPrefab();
        }
    }

    private bool CanPlace(Vector3Int gridPosition)
    {
        for (int x = 0; x < currentPrefabSize.x; x++)
        {
            for (int z = 0; z < currentPrefabSize.y; z++)
            {
                int checkX = gridPosition.x + x;
                int checkZ = gridPosition.z + z;

                // Check if checkX and checkZ are within bounds
                if (checkX < 0 || checkX >= width || checkZ < 0 || checkZ >= depth)
                {
                    // If out of bounds, return false
                    return false;
                }

                var isOccupied = occupied[checkX, checkZ];
                if (isOccupied)
                {
                    return false;
                }
            }
        }
        return true;
    }


    private Vector3Int WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / tileSize);
        int z = Mathf.RoundToInt(worldPosition.z / tileSize);
        return new Vector3Int(x, 0, z);
    }

    private Vector3 GridToWorldPosition(Vector3Int gridPosition)
    {
        return new Vector3(gridPosition.x * tileSize, 0, gridPosition.z * tileSize);
    }

    private void UpdateTileBorder(Vector3Int gridPosition)
    {
        Vector3 worldPosition = GridToWorldPosition(gridPosition);
        RaycastHit hit;

        if (Physics.Raycast(worldPosition + Vector3.up, Vector3.down, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Tile")) // Assuming each tile has a "Tile" tag
            {
                var tile = hit.collider.gameObject.GetComponent<Tile>();
                tile.setTileOccupiedColor();
            }
        }
    }

   private void resetPlacementPrefab()
    {
        currentGhostPrefab.gameObject.SetActive(false);
        currentPrefab = null;
        currentGhostPrefab = null;
        disableGridSystem();
    }
}
