using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel2 : MonoBehaviour
{
    public TMP_InputField inputField;

    void Start()
    {
        inputField.onValueChanged.AddListener(OnInputValueChanged);
    }

    void OnInputValueChanged(string value)
    {
        if (int.TryParse(value, out int parsedValue))
        {
            ConnectUDP.CmdData.voltage = parsedValue;
        }
        else
        {
            ConnectUDP.CmdData.voltage = 50;
        }

    }

   
}
