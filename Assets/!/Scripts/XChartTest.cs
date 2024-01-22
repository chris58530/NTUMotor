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
    public List<double> PowerIn = new List<double>();
    private List<double> PowerOut = new List<double>();
    private List<double> Speed = new List<double>();
    private List<double> Efficiency = new List<double>();

    void Start()
    {
        _data = ConnectUDP.JsonData;
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

        if (isFirst)
        {
            chart.AddXAxisData(Speed.ToString());
            isFirst = false;
        }

        int batchSize = 5; // 每五个数据点更新一次

        for (int i = 0; i < dataList.Count; i++)
        {

            if ((i + 1) % batchSize == 0)
            {
                chart.AddData(dataIndex, dataList[i]);

                yield return null; // 等待一帧的时间
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