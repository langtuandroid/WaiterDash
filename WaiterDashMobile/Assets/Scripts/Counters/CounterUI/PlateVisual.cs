using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlateVisual : MonoBehaviour
{
    [SerializeField] private PlateCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;
    [SerializeField] private TextMeshProUGUI CounterText;

    private List<GameObject> plateVisualGameObjectList;
    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();

    }

    private void OnEnable()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void OnDisable()
    {
        platesCounter.OnPlateSpawned -= PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved -= PlatesCounter_OnPlateRemoved;
    }
    private void Start()
    {

    }

    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        if (plateVisualGameObjectList.Count == 0) { return; }
        GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
        plateVisualGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
        CounterText.text = plateVisualGameObjectList.Count.ToString();
    }

    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        float plateOffsetY = 0.02f;
        plateVisualTransform.localPosition = new Vector3 (0, plateOffsetY * plateVisualGameObjectList.Count, 0);
        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
        CounterText.text = plateVisualGameObjectList.Count.ToString();
    }


}
