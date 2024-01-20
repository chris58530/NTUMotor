using System.Collections.Generic;
using UnityEngine;

namespace @_.Scripts.Json
{
    [System.Serializable]
    public class JsonData 
    {
        public List<double> Time_sec_;
        public List<double> PowerIn_W_;
        public List<double> PowerOut_W_;
        public List<double> Speed_rpm_;
        public List<double> Efficiency_percent_;
    }
}