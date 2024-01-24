using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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

    public void Start()
    {
       
        _data = ConnectUDP.JsonData;
        if (_data == null)
        {
            Debug.LogError("Failed to read Json file.");
            return;
        }


        PowerIn.AddRange(_data.SimResult.PowerIn_W_);
        PowerOut.AddRange(_data.SimResult.PowerOut_W_);
        Speed.AddRange(_data.SimResult.Speed_rpm_);
        Efficiency.AddRange(_data.SimResult.Efficiency_percent_);


        Debug.Log(PowerIn.Count);

        chart = gameObject.GetComponent<LineChart>();
        chart.ClearData();

        if (chart == null)
        {
            chart = gameObject.AddComponent<LineChart>();
            chart.Init();      

        }

        var title = chart.EnsureChartComponent<Title>();
        title.text = "EMTRC";

        var legend = chart.EnsureChartComponent<Legend>();
        legend.show = true;

        //
        // var xAxis = chart.EnsureChartComponent<XAxis>();
        // xAxis.type = Axis.AxisType.Value;
        //
        // var yAxis = chart.EnsureChartComponent<YAxis>();
        // yAxis.type = Axis.AxisType.Value;


        StartCoroutine(LoadChartData("W", _data.SimResult.PowerOut_W_, 0));
        StartCoroutine(LoadChartData("Amp", _data.SimResult.Current_A_, 1));
        StartCoroutine(LoadChartData("Eff", _data.SimResult.Efficiency_percent_, 2));
        StartCoroutine(LoadChartData("Volt", _data.SimResult.Voltage_V_, 3));
        StartCoroutine(LoadChartData("Kgcm", _data.SimResult.Torque_Nm_, 4));
    }

    private bool _isFirst = true;

    IEnumerator LoadChartData(string serieName, List<double> dataList, int dataIndex)
    {
        chart.ClearData();
        var line = chart.AddSerie<Line>(serieName);
        line.serieName = serieName;
        line.symbol.type = SymbolType.None;
        
        double minValue = Speed.Min();
        int minIntValue = (int)minValue;
        double maxValue = Speed.Max();
        int maxIntValue = (int)maxValue;
        if (_isFirst)
        {
            var xAxis = chart.EnsureChartComponent<XAxis>();
            xAxis.min = minIntValue;
            xAxis.max = maxIntValue;
            chart.AddXAxisData(Speed.ToString());
            _isFirst = false;
        }

        int batchSize =10;

        for (int i = maxIntValue; i < dataList.Count; i++)
        {
            chart.AddData(dataIndex, Speed[i], Math.Round(dataList[i], 2));
            if ((i - maxIntValue + 1) % batchSize == 0)
            {
                yield return null;
            }
        }

        yield return null;
    }
}