using EECT.Assets;
using EECT.ElectricalCalculations;
using EECT.Model;
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
        private ViewModelBase _gridDataModel;

        private double _selectedPhases = 1;
        private double _selectedCableSize;
        private double _voltage = 230;
        private double _current = 0;
        private double _power = 0;

        private int _numberOfCables = 1;

        private bool _powerLockedInverted = true;
        private bool _currentLockedInverted = true;
        private bool _voltageLockedInverted = false;

        private bool _powerLocked = false;
        private bool _currentLocked = false;
        private bool _voltageLocked = true;

        private string _cableData = ConfigurationManager.AppSettings["CableDataFolderPath"];

        private PowerUnits _lastRecalculation;

        private List<CableData> _cableList = new List<CableData>();

        private List<double> _phasesList = new List<double>();
        private List<double> _cableSizeList = new List<double>();

        public CableAndProtectionViewModel()
        {
            StartupActions();
            GridDataModel = new GridDataViewModel();

        }


        private void StartupActions()
        {
            var CableData = new CableDataHandler();

            CableList = CableData.GetCableData(_cableData);
            CableSizeList = CableData.GetCableSizesList(CableList);
            PhasesList = CableData.GetPhases(CableList);
        }

        private void resetLastLocked(PowerUnits newLocked)
        {
            if (PowerLocked && VoltageLocked || PowerLocked && CurrentLocked || VoltageLocked && CurrentLocked)
            {

                if (newLocked == PowerUnits.Voltage || newLocked == PowerUnits.Power)
                {
                    if (CurrentLocked)
                    {
                        CurrentLocked = !CurrentLocked;
                    }
                }
                if (newLocked == PowerUnits.Current || newLocked == PowerUnits.Voltage)
                {
                    if (PowerLocked)
                    {
                        PowerLocked = !PowerLocked;
                    }
                }
                if (newLocked == PowerUnits.Current || newLocked == PowerUnits.Power)
                {
                    if (VoltageLocked)
                    {
                        VoltageLocked = !VoltageLocked;
                    }
                }
            }
        }

        private void Recalculate(PowerUnits lastUpdated)
        {
            var calc = new PowerCalc();


            if (lastUpdated == PowerUnits.Power)
            {
                if (CurrentLocked)
                {
                    var voltage = calc.Voltage(Current, Power, SelectedPhases);
                    if (Voltage != voltage)
                    {
                        Voltage = voltage;
                        _lastRecalculation = lastUpdated;
                    }
                    return;
                }

                if (VoltageLocked)
                {
                    var current = calc.Current(Voltage, Power, SelectedPhases);
                    if (Current != current)
                    {
                        Current = current;
                        _lastRecalculation = lastUpdated;
                    }
                    return;
                }
            }

            if (lastUpdated == PowerUnits.Current)
            {
                if (PowerLocked)
                {
                    var voltage = calc.Voltage(Current, Power, SelectedPhases);
                    if (Voltage != voltage)
                    {
                        Voltage = voltage;
                        _lastRecalculation = lastUpdated;
                    }
                    return;
                }

                if (VoltageLocked)
                {
                    var power = calc.Power(Current, Voltage, SelectedPhases);
                    if (Power != power)
                    {
                        Power = power;
                        _lastRecalculation = lastUpdated;
                    }
                    return;
                }

            }

            if (lastUpdated == PowerUnits.Voltage)
            {
                if (PowerLocked)
                {
                    var current = calc.Current(Voltage, Power, SelectedPhases);
                    if (Current != current)
                    {
                        Current = current;
                        _lastRecalculation = lastUpdated;
                    }
                    return;
                }

                if (CurrentLocked)
                {
                    var power = calc.Power(Current, Voltage, SelectedPhases);
                    if (Power != power)
                    {
                        Power = power;
                        _lastRecalculation = lastUpdated;
                    }
                    return;
                }

            }
        }


        #region Properties

        public int NumberOfCables
        {
            get { return _numberOfCables; }
            set
            {
                _numberOfCables = value;
                OnPropertyChanged("NumberOfCables");
            }
        }

        public bool PowerLocked
        {
            get { return _powerLocked; }
            set
            {
                PowerLockedInverted = !value;
                _powerLocked = value;
                resetLastLocked(PowerUnits.Power);
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
                resetLastLocked(PowerUnits.Current);
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
                resetLastLocked(PowerUnits.Voltage);
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
                Recalculate(PowerUnits.Power);
            }
        }

        public double Current
        {
            get { return _current; }
            set
            {
                _current = value;
                OnPropertyChanged("Current");
                Recalculate(PowerUnits.Current);
            }
        }

        public double Voltage
        {
            get { return _voltage; }
            set
            {
                _voltage = value;
                OnPropertyChanged("Voltage");
                Recalculate(PowerUnits.Voltage);
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
                Recalculate(_lastRecalculation);
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

        public ViewModelBase GridDataModel
        {
            get { return _gridDataModel; }
            set
            {
                _gridDataModel = value;
                this.OnPropertyChanged("CurrentViewModel");
            }
        }

        #endregion

    }
}
