using EECT.Assets;
using EECT.ElectricalCalculations;
using EECT.Model;
using System.Collections.Generic;
using System.Configuration;
using System.Numerics;
using System.Windows;

namespace EECT.ViewModel
{
    public class GridDataViewModel : ViewModelBase
    {

        private ViewModelBase _editCableViewModel { get; set; }
        private ViewModelBase _currentDataVM { get; set; }

        private List<CableProperties> CableList = new List<CableProperties>();

        private List<string> _modeList = new List<string>();

        private GridAndTransformerData _GTD = new GridAndTransformerData();
        private FirstSwitchboardData _FSD = new FirstSwitchboardData();

        private Complex SystemImpedance3pmax;
        private Complex SystemImpedance3pmin;
        private Complex _totalImpedance;

        private double _GridMaxTolerance = 1.1;
        private double _maxTolerance = 1.1;
        private double _minTolerance = 0.95;
        private double _Cmax = 1.05;

        private double _aCU = 0.00393;
        private double _aAL = 0.0039;

        private int _selectedModeIndex = 0;

        public GridDataViewModel()
        {
            EditCableViewModel = new EditCableViewModel(this);

            GTD.TransformerVoltageLow = 400;

            FillModeList();
            ActivateView();
            FirstSwitchboardCalculations();
        }

        #region Methods

        private void ActivateView()
        {
            switch (SelectedModeIndex)
            {
                case (0):
                    CurrentDataVM = new FirstSwitchboardViewModel(this, FSD, _GridMaxTolerance, _minTolerance, GTD.TransformerVoltageLow);
                    break;
                case (1):
                    CurrentDataVM = new TransformerDataViewModel(this, GTD);
                    break;

                default:
                    break;
            }
        }


        private void TransformerCalculations()
        {
            var Calc = new TransformerCalc();
            var PC = new PowerCalc();

            var Zq = PC.GridImpedance(GTD, _maxTolerance);
            var Zt = PC.TransformerImpedance(GTD, _Cmax, Zq.Magnitude);
            SystemImpedance3pmax = Zq + Zt;

            EndOfCableCalculation();
        }

        private void FirstSwitchboardCalculations()
        {
            SystemImpedance3pmax = new Complex(FSD.Rplus3pmax, FSD.Xplus3pmax);
            SystemImpedance3pmin = FSD.Z3pmin;
            EndOfCableCalculation();
        }

        private void FillModeList()
        {
            _modeList.Add("Switchboard");
            _modeList.Add("Transformer");
        }

        private void EndOfCableCalculation()
        {
            var PC = new PowerCalc();
            var CDH = new CableDataHandler();
            var index = 0;
            double tolerance;

            Complex Zcables = new Complex();
            foreach (var cable in CableList)
            {
                if (cable.NumberOfCables > 0)
                {
                    if (index == 0 && cable.Length == 0)
                    {
                        tolerance = _GridMaxTolerance;
                    }
                    else
                    {
                        tolerance = _maxTolerance;
                    }

                    Zcables = Zcables + CDH.GetCableImpedance(cable);
                    cable.ImpedanceBehind = Zcables;
                    cable.Ik3pMax = PC.Ik3p(GTD.TransformerVoltageLow, cable.ImpedanceBehind + SystemImpedance3pmax, tolerance);

                    cable.Ik3pMin = PC.Ik3p(GTD.TransformerVoltageLow, 2*PC.TemperatureCorrectedImpedance(cable.CableData.MaxTemp, cable.ImpedanceBehind, _aAL, _aCU, cable) + SystemImpedance3pmax, _minTolerance);
                }
            }
        }
        

        public void CableSelected(List<CableProperties> UpdatedCableList)
        {
            CableList = UpdatedCableList;
            EndOfCableCalculation();
        }


        #endregion


        #region Properties

        public double Vs
        {
            get { return GTD.TransformerVoltageLow; }
            set
            {
                GTD.TransformerVoltageLow = value;
                OnPropertyChanged("Vs");
                EndOfCableCalculation();
            }
        }

        public GridAndTransformerData GTD
        {
            get { return _GTD; }
            set
            {
                _GTD = value;
                OnPropertyChanged("GTD");
                TransformerCalculations();
            }
        }

        public FirstSwitchboardData FSD
        {
            get { return _FSD; }
            set
            {
                _FSD = value;
                OnPropertyChanged("FSD");
                FirstSwitchboardCalculations();
            }
        }

        public ViewModelBase EditCableViewModel
        {
            get { return _editCableViewModel; }
            set
            {
                _editCableViewModel = value;
                OnPropertyChanged("EditCableViewModel");
            }
        }


        public ViewModelBase CurrentDataVM
        {
            get { return _currentDataVM; }
            set
            {
                _currentDataVM = value;
                OnPropertyChanged("CurrentDataVM");
            }
        }

        #region PowerUnits


        public Complex TotalImpedance
        {
            get { return _totalImpedance; }
            set
            {
                _totalImpedance = value;
                OnPropertyChanged("TotalImpedance");
            }
        }
        

        public List<string> ModeList
        {
            get { return _modeList; }
            set
            {
                _modeList = value;
                OnPropertyChanged("ModeList");
            }
        }

        public int SelectedModeIndex
        {
            get { return _selectedModeIndex; }
            set
            {
                _selectedModeIndex = value;
                OnPropertyChanged("SelectedMode");
                ActivateView();
            }
        }

        #endregion

        #endregion
    }
}
