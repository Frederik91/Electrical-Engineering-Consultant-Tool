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

        private GridAndTransformerData GTD = new GridAndTransformerData();


        private Complex Zq;
        private Complex Zt;
        private Complex _totalImpedance;

        private double _Ik3p;
        private double _R0 = 1;
        private double _X0 = 0.95;
        private double _Tolerance = 1.05;
        private double _Cmax = 1.05;
        private double _powerRating = 400000;
        private double _TransformerPowerLoss = 4600;
        private double _Ek = 0.04;
        private double _Vp = 20000;
        private double _Vs = 410;
        private double _Ik;
        private double _IkGrid = 10000;
        private double _SkTransformer;

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
        }

        private void EndOfCableCalculation()
        {
            var PC = new PowerCalc();
            var CDH = new CableDataHandler();

            Zq = PC.GridImpedance(GTD, _Tolerance);
            Zt = PC.TransformerImpedance(GTD, _Cmax, Zq.Magnitude);

            Complex Zcables = new Complex();
            foreach (var cable in CableList)
            {
                if (cable.NumberOfCables > 0)
                {
                    Zcables = Zcables + CDH.GetCableImpedance(cable);
                    cable.ImpedanceBehind = Zcables;
                }
            }

            TotalImpedance = PC.SumImpedances(Zq, Zt, Zcables);

            Ik3p = PC.Ik3p(Vs, TotalImpedance, _Tolerance);
        }


        public void CableSelected(CableProperties SelectedCable)
        {
            var PC = new PowerCalc();
            var CDH = new CableDataHandler();            
            var list = CableList;

            list.Add(SelectedCable);

            foreach (var Cable in CableList)
            {
                Cable.SkCable = PC.Sk(_Vs, PC.SumImpedances(Zq + Zt, CDH.GetCableImpedance(Cable)));
                Cable.TotalSkCable = PC.SumSk(SkTransformer, Cable.SkCable);
            }

            CableList = list;
            EndOfCableCalculation();
        }


        #endregion


        #region Properties


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

        public double TransformerPowerLoss
        {
            get { return _TransformerPowerLoss; }
            set
            {
                _TransformerPowerLoss = value;
                OnPropertyChanged("TransformerPowerLoss");
                TransformerCalculations();
            }
        }

        public double IkGrid
        {
            get { return _IkGrid; }
            set
            {
                _IkGrid = value;
                OnPropertyChanged("IkGrid");
                TransformerCalculations();
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

        public double SkTransformer
        {
            get { return _SkTransformer; }
            set
            {
                _SkTransformer = value;
                OnPropertyChanged("Sk");
            }
        }

        public double PowerRating
        {
            get { return _powerRating; }
            set
            {
                _powerRating = value;
                OnPropertyChanged("PowerRating");
                TransformerCalculations();
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




        #endregion

        #endregion
    }
}
