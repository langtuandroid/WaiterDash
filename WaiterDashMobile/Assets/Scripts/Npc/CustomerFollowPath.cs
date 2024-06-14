using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerFollowPath : MonoBehaviour
{
    public GameObject[] customers;
    public GameObject[] pathPoints;
    public int numberOfPoints;
    public float speed;

    private bool isFollowingPath = true;
    private int[] customerIndices;
    private Vector3[] customerPositions;

    private void Start()
    {
        customerIndices = new int[customers.Length];
        customerPositions = new Vector3[customers.Length];
    }

    private void Update()
    {
        if (isFollowingPath)
        {
            for (int i = 0; i < customers.Length; i++)
            {
                MoveCustomer(i);
            }
        }
    }

    private void MoveCustomer(int index)
    {
        var customer = customers[index];
        var currentPosition = customer.transform.position;

        if (customerIndices[index] == numberOfPoints - 1)
        {
            // Customer has reached the last point, skip movement
            return;
        }

        var targetPosition = pathPoints[customerIndices[index]].transform.position;

        customer.transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(currentPosition, targetPosition) < 0.001f && customerIndices[index] != numberOfPoints - 1)
        {
            customerIndices[index]++;
        }
    }

}

