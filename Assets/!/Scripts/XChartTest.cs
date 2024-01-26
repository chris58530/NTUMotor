using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using _.Scripts.Json;
using UnityEngine;
using UnityEngine.UI;
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


     
        
        double maxPowerOut = _data.SimResult.PowerOut_W_.Max();
        double minPowerOut = _data.SimResult.PowerOut_W_.Min();

        double maxCurrent = _data.SimResult.Current_A_.Max();
        double minCurrent = _data.SimResult.Current_A_.Min();

        double maxEfficiency = _data.SimResult.Efficiency_percent_.Max();
        double minEfficiency = _data.SimResult.Efficiency_percent_.Min();

        double maxVoltage = _data.SimResult.Voltage_V_.Max();
        double minVoltage = _data.SimResult.Voltage_V_.Min();

        double maxTorque =  _data.SimResult.Torque_Nm_.Max();
        double minTorque =  _data.SimResult.Torque_Nm_.Min();

        // 找出最大的最大值和最小的最小值所对应的列表
        List<double> maxValues = new List<double> { maxPowerOut, maxCurrent, maxEfficiency, maxVoltage, maxTorque };
        List<double> minValues = new List<double> { minPowerOut, minCurrent, minEfficiency, minVoltage, minTorque };

        indexOfMaxValue = maxValues.IndexOf(maxValues.Max());
       indexOfMinValue = minValues.IndexOf(minValues.Min());
       
       StartCoroutine(LoadChartData("W", _data.SimResult.PowerOut_W_, 0));
       StartCoroutine(LoadChartData("Amp", _data.SimResult.Current_A_, 1));
       StartCoroutine(LoadChartData("Eff", _data.SimResult.Efficiency_percent_, 2));
       StartCoroutine(LoadChartData("Volt", _data.SimResult.Voltage_V_, 3));
       StartCoroutine(LoadChartData("Kgcm", _data.SimResult.Torque_Nm_, 4));
    }

    private int indexOfMaxValue;
    private int indexOfMinValue;
    private bool _isFirst = true;

    IEnumerator LoadChartData(string serieName, List<double> dataList, int dataIndex)
    {
        chart.ClearData();
        var serie = chart.AddSerie<Line>(serieName);
        serie.serieName = serieName;
        serie.symbol.type = SymbolType.None;
        double minValue = Speed.Min();
        int minIntValue = (int)minValue;
        double maxValue = Speed.Max();
        int maxIntValue = (int)maxValue;
        if (_isFirst)
        {
            var xAxis = chart.EnsureChartComponent<XAxis>();
            // xAxis.min = (int)_data.SimResult.PowerOut_W_[^1];
            xAxis.min = minIntValue;
            xAxis.max = maxIntValue;

            // _isFirst = false;
        }

        int batchSize = 10;

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

    public void OnToggleValueChanged0(bool isOn)
    {
        chart = gameObject.GetComponent<LineChart>();
        if (isOn)
        {          

            chart.series[0].show = true;
        }
        else
        {
            chart.series[0].show = false;
        }
    }

    public void OnToggleValueChanged1(bool isOn)
    {
        chart = gameObject.GetComponent<LineChart>();
        if (isOn)
        {
            chart.series[1].show = true;
        }
        else
        {
            chart.series[1].show = false;
        }
    }

    public void OnToggleValueChanged2(bool isOn)
    {
        chart = gameObject.GetComponent<LineChart>();
        if (isOn)
        {
            chart.series[2].show = true;
        }
        else
        {
            chart.series[2].show = false;
        }
    }

    public void OnToggleValueChanged3(bool isOn)
    {
        chart = gameObject.GetComponent<LineChart>();
        if (isOn)
        {
            chart.series[3].show = true;
        }
        else
        {
            chart.series[3].show = false;
        }
    }

    public void OnToggleValueChanged4(bool isOn)
    {
        chart = gameObject.GetComponent<LineChart>();
        if (isOn)
        {
            chart.series[4].show = true;
            
        }
        else
        {
            chart.series[4].show = false;
        }
    }
}