using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MotorType
{
    PMDC,
    SeriesExcited,
    ShuntExcited,
    SeparatelyExcited
}

public enum LoadTest
{
    Auto,
    Manual
}

public class CmdData
{
    public MotorType motorType;
    public LoadTest loadTest;
    public int voltage;
}