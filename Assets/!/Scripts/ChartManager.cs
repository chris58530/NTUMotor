using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartManager : MonoBehaviour
{
    [SerializeField] private GameObject chart;
    [SerializeField] private GameObject currentChart;

    public void SpawnChart()
    {
        if (currentChart != null)
        {
            Destroy(currentChart);
        }

        currentChart = Instantiate(chart, transform.position, transform.rotation);
        currentChart.transform.parent = transform;
        currentChart.transform.localScale = Vector3.one;
        Debug.Log("spawn chart");
    }
}