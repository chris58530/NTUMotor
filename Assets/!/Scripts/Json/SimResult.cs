using System.Collections.Generic;

namespace _.Scripts.Json
{
    [System.Serializable]
    public class SimResult
    {
        public List<double> Time_sec_;
        public List<double> PowerIn_W_;
        public List<double> PowerOut_W_;
        public List<double> Speed_rpm_;
        public List<double> Efficiency_percent_;
        public List<double> Voltage_V_;
        public List<double> Current_A_;
        public List<double> Torque_Nm_;
    }
}