using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECT.Model
{
    public class CableData
    {
        public int ID { get; set; }
        public string Cable { get; set; }
        public string CableID { get; set; }
        public string CableType { get; set; }
        public string Isolation { get; set; }
        public int OperatingTemp { get; set; }
        public int MaxTemp { get; set; }
        public int Voltage { get; set; }
        public int Phases { get; set; }
        public int? Conductors { get; set; }
        public double? Dimension { get; set; }
        public string Material { get; set; }
        public bool Jacket { get; set; }
        public double Narea { get; set; }
        public string Nmaterial { get; set; }
        public string PEArea { get; set; }
        public string PEmaterial { get; set; }
        public double CenterDistance { get; set; }
        public double ConductiorDiameter { get; set; }
        public double CableDiameter { get; set; }
        public double JacketDiameter { get; set; }
        public double CableWeight { get; set; }
        public double Rpos { get; set; }
        public double Lpos { get; set; }
        public double R0N { get; set; }
        public double L0N { get; set; }
        public double R0PEN { get; set; }
        public double L0PEN { get; set; }
        public double RPE { get; set; }
        public double LPE { get; set; }
        public double RPhasePhase { get; set; }
        public double LPhasePhase { get; set; }
        public double RPhaseN { get; set; }
        public double LPhaseN { get; set; }
        public double RPhasePEN { get; set; }
        public double LPhasePEN { get; set; }
        public double PhasePhaseGmd { get; set; }
        public double PhasePhaseGroupGmd { get; set; }
        public double PhaseNGmd { get; set; }
        public double PhasePEGmd { get; set; }
        public double NGmd { get; set; }
        public double PEGmd { get; set; }
        public double Capacitance { get; set; }
        public int ELnumber { get; set; }
        public string CENELEC { get; set; }

    }
}
