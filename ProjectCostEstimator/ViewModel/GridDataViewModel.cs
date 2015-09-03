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

        private List<CableProperties> CableList = new List<CableProperties>();

        private List<string> _modeList = new List<string>();

        private GridAndTransformerData GTD;

        private Complex Zq;
        private Complex Zt;
        private Complex _totalImpedance;

        private double _Ik3p;
   
        private double _maxTolerance = 1.05;
        private double _minTolerance = 0.95;
        private double _Cmax = 1.05;
        private double _SkTransformer;
        private double _Ek = 0.04;
        private double _Vp = 20000;
        private double _Vs = 410;
        private double _Ik;


        private double _aCU = 0.00393;
        private double _aAL = 0.0039;

        private int _selectedModeIndex = 0;

        public GridDataViewModel()
        {
            EditCableViewModel = new EditCableViewModel(this);


            TransformerCalculations();
        }

        #region Methods

        private void TransformerCalculations()
        {
            var Calc = new TransformerCalc();

            GTD.Ek = _Ek;
            GTD.GridVoltage = _Vp;
            GTD.GridIk = _IkGrid;
            GTD.TransfomerX0 = _X0;
            GTD.TransformerRo = _R0;
            GTD.TransformerFullLoadLoss = _TransformerPowerLoss;
            GTD.TransformerPowerRating = _powerRating;
            GTD.TransformerVoltageLow = _Vs;

            SkTransformer = Calc.Sk(_powerRating, _Ek);
            Ik = Calc.Ik(_SkTransformer, Vs);
            EndOfCableCalculation();

            FillModeList();
        }

        private void FillModeList()
        {
            _modeList.Add("Transformer");
            _modeList.Add("Switchboard");
        }

        private void EndOfCableCalculation()
        {
            var PC = new PowerCalc();
            var CDH = new CableDataHandler();

            Zq = PC.GridImpedance(GTD, _maxTolerance);
            Zt = PC.TransformerImpedance(GTD, _Cmax, Zq.Magnitude);

            Complex Zcables = new Complex();
            foreach (var cable in CableList)
            {
                if (cable.NumberOfCables > 0)
                {
                    Zcables = Zcables + CDH.GetCableImpedance(cable);
                    cable.ImpedanceBehind = Zcables;
                    cable.Ik3pMax = PC.Ik3p(Vs, cable.ImpedanceBehind + Zq + Zt, _maxTolerance);

                    cable.Ik3pMin = PC.Ik3p(Vs, PC.TemperatureCorrectedImpedance(cable.CableData.MaxTemp, cable.ImpedanceBehind, _aAL, _aCU, cable) + Zq + Zt, _minTolerance);
                }
            }

            TotalImpedance = PC.SumImpedances(Zq, Zt, Zcables);

            Ik3p = PC.Ik3p(Vs, TotalImpedance, _maxTolerance);
        }


        public void CableSelected(List<CableProperties> UpdatedCableList)
        {
            CableList = UpdatedCableList;
            EndOfCableCalculation();
        }


        #endregion


        #region Properties

        public double SkTransformer
        {
            get { return _SkTransformer; }
            set
            {
                _SkTransformer = value;
                OnPropertyChanged("Sk");
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


        #region PowerUnits

        public double Ik3p
        {
            get { return _Ik3p; }
            set
            {
                _Ik3p = value;
                OnPropertyChanged("Ik3p");
            }
        }


        public Complex TotalImpedance
        {
            get { return _totalImpedance; }
            set
            {
                _totalImpedance = value;
                OnPropertyChanged("TotalImpedance");
            }
        }


        public double Ik
        {
            get { return _Ik; }
            set
            {
                _Ik = value;
                OnPropertyChanged("Ik");
            }
        }



        public double Ek
        {
            get { return _Ek; }
            set
            {
                if (value > 1)
                {
                    _Ek = value / 100;
                }
                else
                {
                    _Ek = value;
                }
                OnPropertyChanged("Ek");
                TransformerCalculations();
            }
        }

        public double Vp
        {
            get { return _Vp; }
            set
            {
                _Vp = value;
                OnPropertyChanged("Vp");
                TransformerCalculations();
            }
        }


        public double Vs
        {
            get { return _Vs; }
            set
            {
                _Vs = value;
                OnPropertyChanged("Vs");
                TransformerCalculations();
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

        public int SelectedMode
        {
            get { return _selectedModeIndex; }
            set
            {
                _selectedModeIndex = value;
                OnPropertyChanged("SelectedMode");
            }
        }

        #endregion

        #endregion
    }
}
