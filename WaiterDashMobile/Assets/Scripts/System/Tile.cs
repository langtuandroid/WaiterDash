using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private void Start()
    {
        GridSystem.OnGridSystemEditEnable += OnGridSystemEditEnable;
        GridSystem.OnGridSystemEditDisable += OnGridSystemEditDisable;
        hideTile();
    }

    private void OnGridSystemEditDisable(object sender, System.EventArgs e)
    {
        if (this)
        {
            hideTile();
        }
    }

    private void OnGridSystemEditEnable(object sender, System.EventArgs e)
    {
        if(this)
        {
            showTile();
        }
    }

    public void setTileOccupiedColor()
    {
        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public void setTileAvailable()
    {
        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public void showTile()
    {
        this.gameObject.SetActive(true);
    }

    public void hideTile()
    {
        this.gameObject.SetActive(false);
    }
}
