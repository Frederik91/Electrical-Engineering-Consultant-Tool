using ProjectCostEstimator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EECT.ViewModel
{
    class CableAndProtectionViewModel : ViewModelBase
    {
        private double _selectedPhases = 1;
        private double _selectedCableSize;
        private double _voltage = 230;
        private double _current = 0;
        private double _power = 0;

        private bool _powerLockedInverted = true;
        private bool _currentLockedInverted = true;
        private bool _voltageLockedInverted = false;

        private bool _powerLocked = false;
        private bool _currentLocked= false;
        private bool _voltageLocked = true;

        private string _cableData = ConfigurationManager.AppSettings["CableDataFolderPath"];

        private List<CableData> _cableList = new List<CableData>();
        private List<double> _phasesList = new List<double>();
        private List<double> _cableSizeList = new List<double>();

        public CableAndProtectionViewModel()
        {

            StartupActions();
        }

        private void StartupActions()
        {            
            GetCableData();
            GetPhases();
            GetCableSizes();
        }
        
        private void Recalculate(string lastUpdated)
        {
            var calc = new PowerCalculations();

            if (lastUpdated == "Power")
            {
                if (CurrentLocked)
                {
                    var voltage = calc.Voltage(Current, Power, SelectedPhases);
                    if (Voltage != voltage)
                    {
                        Voltage = voltage;
                    }
                    return;                
                }

                if (VoltageLocked)
                {
                    var current = calc.Current(Voltage, Power, SelectedPhases);
                    if (Current != current)
                    {
                        Current = current;
                    }
                    return;
                }
            }

            if (lastUpdated == "Current")
            {
                if (PowerLocked)
                {
                    var voltage = calc.Voltage(Current, Power, SelectedPhases);
                    if (Voltage != voltage)
                    {
                        Voltage = voltage;
                    }
                    return;
                }

                if (VoltageLocked)
                {
                    var power = calc.Power(Current, Voltage, SelectedPhases);
                    if (Power != power)
                    {
                        Power = power;
                    }
                    return;
                }
                                
            }

            if (lastUpdated == "Voltage")
            {
                if (PowerLocked)
                {
                    var current = calc.Current(Voltage, Power, SelectedPhases);
                    if (Current != current)
                    {
                        Current = current;
                    }
                    return;
                }

                if (CurrentLocked)
                {
                    var power = calc.Power(Current, Voltage, SelectedPhases);
                    if (Power != power)
                    {
                        Power = power;
                    }
                    return;
                }
                
            }
        }


        private void GetCableSizes()
        {
            var list = new List<double>();

            foreach (var item in CableList)
            {
                list.Add(item.Dimension);
            }

            list = list.Distinct().ToList();

            var sortedList = from dbl in list
                         orderby dbl ascending
                         select dbl;

            CableSizeList = sortedList.ToList();            
            
        }

        private void GetPhases()
        {
            var list = new List<double>();

            list.Add(1);
            list.Add(3);

            PhasesList = list;
        }

        private void GetCableData()
        {
            var cableList = new List<CableData>();
            var errorlist = new List<string>();

            using (StreamReader sr = new StreamReader(_cableData))
            {
                sr.ReadLine();
                while (sr.Peek() >= 0)
                {
                    var cable = sr.ReadLine();
                    var cableArray = cable.Split('\t');

                    for (int i = 0; i < cableArray.Length; i++)
                    {
                        if (cableArray[i] == string.Empty)
                        {
                            cableArray[i] = "-1";
                        }
                    }

                    try
                    {
                        cableList.Add(new CableData
                        {
                            ID = Convert.ToInt32(cableArray[0]),
                            Cable = cableArray[1],
                            CableID = cableArray[2],
                            CableType = cableArray[3],
                            Isolation = cableArray[4],
                            OperatingTemp = Convert.ToInt32(cableArray[5]),
                            MaxTemp = Convert.ToInt32(cableArray[6]),
                            Voltage = Convert.ToInt32(cableArray[7]),
                            Phases = Convert.ToInt32(cableArray[8]),
                            Conductors = Convert.ToInt32(cableArray[9]),
                            Dimension = Convert.ToDouble(cableArray[10]),
                            Material = cableArray[11],
                            Jacket = Convert.ToBoolean(cableArray[12]),
                            Narea = Convert.ToDouble(cableArray[13]),
                            Nmaterial = cableArray[14],
                            PEArea = cableArray[15],
                            PEmaterial = cableArray[16],
                            CenterDistance = Convert.ToDouble(cableArray[17]),
                            ConductiorDiameter = Convert.ToDouble(cableArray[18]),
                            CableDiameter = Convert.ToDouble(cableArray[19]),
                            JacketDiameter = Convert.ToDouble(cableArray[20]),
                            CableWeight = Convert.ToDouble(cableArray[21]),
                            Rpos = Convert.ToDouble(cableArray[22]),
                            Lpos = Convert.ToDouble(cableArray[23]),
                            R0N = Convert.ToDouble(cableArray[24]),
                            L0N = Convert.ToDouble(cableArray[25]),
                            R0PEN = Convert.ToDouble(cableArray[26]),
                            L0PEN = Convert.ToDouble(cableArray[27]),
                            RPE = Convert.ToDouble(cableArray[28]),
                            LPE = Convert.ToDouble(cableArray[29]),
                            RPhasePhase = Convert.ToDouble(cableArray[30]),
                            LPhasePhase = Convert.ToDouble(cableArray[31]),
                            RPhaseN = Convert.ToDouble(cableArray[32]),
                            LPhaseN = Convert.ToDouble(cableArray[33]),
                            RPhasePEN = Convert.ToDouble(cableArray[34]),
                            LPhasePEN = Convert.ToDouble(cableArray[35]),
                            PhasePhaseGmd = Convert.ToDouble(cableArray[36]),
                            PhasePhaseGroupGmd = Convert.ToDouble(cableArray[37]),
                            PhaseNGmd = Convert.ToDouble(cableArray[38]),
                            PhasePEGmd = Convert.ToDouble(cableArray[39]),
                            NGmd = Convert.ToDouble(cableArray[40]),
                            PEGmd = Convert.ToDouble(cableArray[41]),
                            Capacitance = Convert.ToDouble(cableArray[42]),
                            ELnumber = Convert.ToInt32(cableArray[43]),
                            CENELEC = cableArray[44]
                        });
                    }
                    catch (Exception)
                    {
                        errorlist.Add(cable);
                    }
                }
                CableList = cableList;
            }
        }




        #region Properties

        public bool PowerLocked
        {
            get { return _powerLocked; }
            set
            {
                PowerLockedInverted = !value;
                _powerLocked = value;
                OnPropertyChanged("PowerLocked");
            }
        }


        public bool CurrentLocked
        {
            get { return _currentLocked; }
            set
            {
                CurrentLockedInverted = !value;
                _currentLocked = value;
                OnPropertyChanged("CurrentLocked");
            }
        }

        public bool VoltageLocked
        {
            get { return _voltageLocked; }
            set
            {
                VoltageLockedInverted = !value;
                _voltageLocked = value;
                OnPropertyChanged("VoltageLocked");
            }
        }

        public bool PowerLockedInverted
        {
            get { return _powerLockedInverted; }
            set
            {
                _powerLockedInverted = value;
                OnPropertyChanged("PowerLockedInverted");
            }
        }


        public bool CurrentLockedInverted
        {
            get { return _currentLockedInverted; }
            set
            {
                _currentLockedInverted = value;
                OnPropertyChanged("CurrentLockedInverted");
            }
        }

        public bool VoltageLockedInverted
        {
            get { return _voltageLockedInverted; }
            set
            {
                _voltageLockedInverted = value;
                OnPropertyChanged("VoltageLockedInverted");
            }
        }

        public double Power
        {
            get { return _power; }
            set
            {
                _power = value;
                OnPropertyChanged("Power");
                Recalculate("Power");
            }
        }
        
        public double Current
        {
            get { return _current; }
            set
            {
                _current = value;
                OnPropertyChanged("Current");
                Recalculate("Current");
            }
        }

        public double Voltage
        {
            get { return _voltage; }
            set
            {
                _voltage = value;
                OnPropertyChanged("Voltage");
                Recalculate("Voltage");
            }
        }

        public double SelectedCableSize
        {
            get { return _selectedCableSize; }
            set
            {
                _selectedCableSize = value;
                OnPropertyChanged("SelectedCableSize");
            }
        }

        public List<double> CableSizeList
        {
            get { return _cableSizeList; }
            set
            {
                _cableSizeList = value;
                OnPropertyChanged("CableSizeList");
            }
        }

        public double SelectedPhases
        {
            get { return _selectedPhases; }
            set
            {
                _selectedPhases = value;
                OnPropertyChanged("SelectedPhases");
            }
        }

        public List<double> PhasesList
        {
            get { return _phasesList; }
            set
            {
                _phasesList = value;
                OnPropertyChanged("Phases");
            }
        }

        public List<CableData> CableList
        {
            get { return _cableList; }
            set
            {
                _cableList = value;
                OnPropertyChanged("CableList");
            }
        }

        #endregion

    }
}
