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

        private ViewModelBase _cable1ViewModel { get; set; }
        private ViewModelBase _cable2ViewModel { get; set; }
        private ViewModelBase _cable3ViewModel { get; set; }
        private ViewModelBase _cable4ViewModel { get; set; }

        private CableProperties _cable1 = new CableProperties { CableData = new CableData() };
        private CableProperties _cable2 = new CableProperties { CableData = new CableData() };
        private CableProperties _cable3 = new CableProperties { CableData = new CableData() };
        private CableProperties _cable4 = new CableProperties { CableData = new CableData() };


        private GridAndTransformerData GTD = new GridAndTransformerData();


        private Complex Zq;
        private Complex Zt;
        private Complex _totalImpedance;

        private double _Ik3p;
        private double _Ik3pPeak;
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
        private double _skCable1;
        private double _skCable2;
        private double _skCable3;
        private double _skCable4;
        private double _totalSkCable1;
        private double _totalSkCable2;
        private double _totalSkCable3;
        private double _totalSkCable4;



        private Visibility _cable1Selected = Visibility.Hidden;
        private Visibility _cable2Selected = Visibility.Hidden;
        private Visibility _cable3Selected = Visibility.Hidden;

        public GridDataViewModel()
        {
            Cable1ViewModel = new CableSelectViewModel(this);
            Cable2ViewModel = new CableSelectViewModel(this);
            Cable3ViewModel = new CableSelectViewModel(this);
            Cable4ViewModel = new CableSelectViewModel(this);

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
            var CableList = new List<CableProperties>();
            var PC = new PowerCalc();
            var CDH = new CableDataHandler();

            CableList.Add(_cable1);
            CableList.Add(_cable2);
            CableList.Add(_cable3);
            CableList.Add(_cable4);

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
            Ik3pPeak = PC.Ik3pPeak(TotalImpedance, Ik3p);

        }


        public void CableSelected(CableSelectViewModel CSVM, CableProperties SelectedCable)
        {
            var PC = new PowerCalc();
            var CDH = new CableDataHandler();
            if (CSVM == Cable1ViewModel)
            {
                Cable1 = SelectedCable;
                SkCable1 = PC.Sk(_Vs, PC.SumImpedances(Zq+Zt, CDH.GetCableImpedance(Cable1)));
                TotalSkCable1 = PC.SumSk(SkTransformer, SkCable1);
            }

            if (CSVM == Cable2ViewModel)
            {
                Cable2 = SelectedCable;
                SkCable2 = PC.Sk(_Vs, PC.SumImpedances(SkCable1, CDH.GetCableImpedance(Cable2)));
                TotalSkCable2 = PC.SumSk(SkTransformer, TotalSkCable1);
            }

            if (CSVM == Cable3ViewModel)
            {
                Cable3 = SelectedCable;
                SkCable3 = PC.Sk(_Vs, PC.SumImpedances(SkCable2, CDH.GetCableImpedance(Cable3)));
                TotalSkCable3 = PC.SumSk(SkTransformer, TotalSkCable2);
            }

            if (CSVM == Cable4ViewModel)
            {
                Cable4 = SelectedCable;
                SkCable4 = PC.Sk(_Vs, PC.SumImpedances(SkCable3, CDH.GetCableImpedance(Cable4)));
                TotalSkCable4 = PC.SumSk(SkTransformer, TotalSkCable3);
            }
            EndOfCableCalculation();
        }


        #endregion


        #region Properties

        #region Cables

        #region Cable1


        public CableProperties Cable1
        {
            get { return _cable1; }
            set
            {
                _cable1 = value;
                OnPropertyChanged("Cable1");
                if (value != null)
                {
                    Cable1Selected = Visibility.Visible;
                }
            }
        }

        public ViewModelBase Cable1ViewModel
        {
            get { return _cable1ViewModel; }
            set
            {
                _cable1ViewModel = value;
                OnPropertyChanged("Cable1ViewModel");
            }
        }

        public double SkCable1
        {
            get { return _skCable1; }
            set
            {
                _skCable1 = value;
                OnPropertyChanged("SkCable1");
            }
        }

        public double TotalSkCable1
        {
            get { return _totalSkCable1; }
            set
            {
                _totalSkCable1 = value;
                OnPropertyChanged("TotalSkCable1");
            }
        }

        #endregion

        #region Cable2

        public CableProperties Cable2
        {
            get { return _cable2; }
            set
            {
                _cable2 = value;
                OnPropertyChanged("Cable2");
                if (value != null)
                {
                    Cable2Selected = Visibility.Visible;
                }
            }
        }

        public Visibility Cable1Selected
        {
            get { return _cable1Selected; }
            set
            {
                _cable1Selected = value;
                this.OnPropertyChanged("Cable1Selected");
            }
        }




        public ViewModelBase Cable2ViewModel
        {
            get { return _cable2ViewModel; }
            set
            {
                _cable2ViewModel = value;
                OnPropertyChanged("Cable2ViewModel");
            }
        }


        public double SkCable2
        {
            get { return _skCable2; }
            set
            {
                _skCable2 = value;
                OnPropertyChanged("SkCable2");
            }
        }

        public double TotalSkCable2
        {
            get { return _totalSkCable2; }
            set
            {
                _totalSkCable2 = value;
                OnPropertyChanged("TotalSkCable2");
            }
        }

        #endregion

        #region Cable3

        public CableProperties Cable3
        {
            get { return _cable3; }
            set
            {
                _cable3 = value;
                OnPropertyChanged("Cable3");
                if (value != null)
                {
                    Cable3Selected = Visibility.Visible;
                }
            }
        }

        public Visibility Cable2Selected
        {
            get { return _cable2Selected; }
            set
            {
                _cable2Selected = value;
                this.OnPropertyChanged("Cable2Selected");
            }
        }

        public ViewModelBase Cable3ViewModel
        {
            get { return _cable3ViewModel; }
            set
            {
                _cable3ViewModel = value;
                OnPropertyChanged("Cable3ViewModel");
            }
        }

        public double SkCable3
        {
            get { return _skCable3; }
            set
            {
                _skCable3 = value;
                OnPropertyChanged("SkCable3");
            }
        }

        public double TotalSkCable3
        {
            get { return _totalSkCable3; }
            set
            {
                _totalSkCable3 = value;
                OnPropertyChanged("TotalSkCable3");
            }
        }
        #endregion

        #region Cable4

        public CableProperties Cable4
        {
            get { return _cable4; }
            set
            {
                _cable4 = value;
                OnPropertyChanged("Cable4");
            }
        }

        public Visibility Cable3Selected
        {
            get { return _cable3Selected; }
            set
            {
                _cable3Selected = value;
                this.OnPropertyChanged("Cable3Selected");
            }
        }
        
        public ViewModelBase Cable4ViewModel
        {
            get { return _cable4ViewModel; }
            set
            {
                _cable4ViewModel = value;
                OnPropertyChanged("Cable4ViewModel");
            }
        }

        public double SkCable4
        {
            get { return _skCable4; }
            set
            {
                _skCable4 = value;
                OnPropertyChanged("SkCable4");
            }
        }

        public double TotalSkCable4
        {
            get { return _totalSkCable4; }
            set
            {
                _totalSkCable4 = value;
                OnPropertyChanged("TotalSkCable4");
            }
        }
        #endregion

        #endregion

        #region PowerUnits

        public double Ik3pPeak
        {
            get { return _Ik3pPeak; }
            set
            {
                _Ik3pPeak = value;
                OnPropertyChanged("Ik3pPeak");
            }
        }

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
