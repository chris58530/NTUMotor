using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using _.Scripts.Json;
using UnityEngine;
using XCharts.Runtime;

public class XChartTest : MonoBehaviour
{
    private JsonData _data;
    private LineChart chart;
    private List<double> PowerIn = new List<double>();
    private List<double> PowerOut = new List<double>();
    private List<double> Speed = new List<double>();
    private List<double> Efficiency = new List<double>();

    void Start()
    {
        _data = JsonReader.GetJsonData();
        if (_data == null)
        {
            Debug.LogError("Failed to read Json file.");
            return;
        }

        PowerIn.AddRange(_data.PowerIn_W_);
        PowerOut.AddRange(_data.PowerOut_W_);
        Speed.AddRange(_data.Speed_rpm_);
        Efficiency.AddRange(_data.Efficiency_percent_);
        
        Debug.Log(PowerIn.Count);

        chart = gameObject.GetComponent<LineChart>();
        if (chart == null)
        {
            chart = gameObject.AddComponent<LineChart>();
            chart.Init();
        }

        var title = chart.EnsureChartComponent<Title>();
        title.text = "EMTRC";

        var legend = chart.EnsureChartComponent<Legend>();
        legend.show = true;

        var xAxis = chart.EnsureChartComponent<XAxis>();
        xAxis.type = Axis.AxisType.Value;

        var yAxis = chart.EnsureChartComponent<YAxis>();
        yAxis.type = Axis.AxisType.Value;


        StartCoroutine(LoadChartData("PowerIn", PowerIn, 0));
        StartCoroutine(LoadChartData("PowerOut", PowerOut, 1));
        StartCoroutine(LoadChartData("Speed", Speed, 2));
        StartCoroutine(LoadChartData("Efficiency", Efficiency, 3));
    }

    private bool isFirst = true;

    IEnumerator LoadChartData(string serieName, List<double> dataList, int dataIndex)
    {
        chart.ClearData();

        var line = chart.AddSerie<Line>(serieName);
        line.symbol.type = SymbolType.None;
        line.serieName = serieName;
        Debug.Log(dataList.Count);
        int batchSize = 10;
        float totalSeconds = 10f;
    
        float initialDelay = totalSeconds / dataList.Count; // 初始等待时间
        float minDelay = 0.001f;
        float currentDelay = initialDelay;

        if (isFirst)
        {
            chart.AddXAxisData("" + _data.Time_sec_);
            isFirst = false;
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            chart.AddData(dataIndex, dataList[i]);
            if (i % batchSize == 0)
            {
                yield return new WaitForSeconds(currentDelay);

                // Gradually decrease the delay, ensuring it doesn't go below the minimum
                currentDelay = Mathf.Max(currentDelay * 0.9f, minDelay);
            }
        }

        yield return null;
    }

    public void ReStartLine()
    {
        StartCoroutine(LoadChartData("PowerIn", _data.PowerIn_W_, 0));
        StartCoroutine(LoadChartData("PowerOut", _data.PowerOut_W_, 1));
        StartCoroutine(LoadChartData("Speed", _data.Speed_rpm_, 2));
        StartCoroutine(LoadChartData("Efficiency", _data.Efficiency_percent_, 3));
    }
}