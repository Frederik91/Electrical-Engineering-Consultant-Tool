using EECT.Assets;
using EECT.ElectricalCalculations;
using EECT.Model;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;

namespace EECT.ViewModel
{
    public class GridDataViewModel : ViewModelBase
    {

        private ViewModelBase _cable1ViewModel { get; set; }
        private ViewModelBase _cable2ViewModel { get; set; }
        private ViewModelBase _cable3ViewModel { get; set; }
        private ViewModelBase _cable4ViewModel { get; set; }

        private CableData _cable1;
        private CableData _cable2;
        private CableData _cable3;
        private CableData _cable4;

        private double _powerRating;
        private double _Ek;
        private double _Vp;
        private double _Vs;
        private double _Ik;
        private double _Sk;
        private double _Zk3p;

        private Visibility _cable1Selected = Visibility.Hidden;
        private Visibility _cable2Selected = Visibility.Hidden;
        private Visibility _cable3Selected = Visibility.Hidden;


        public GridDataViewModel()
        {
            Cable1ViewModel = new CableSelectViewModel(this);
            Cable2ViewModel = new CableSelectViewModel(this);
            Cable3ViewModel = new CableSelectViewModel(this);
            Cable4ViewModel = new CableSelectViewModel(this);
        }

        #region Methods

        public void TransformerCalculations()
        {
            var Calc = new TransformerCalc();

            Sk = Calc.Sk(_powerRating, _Ek);
            Ik = Calc.Ik(_Sk, Vs);
            Zk3p = Calc.Z(Ek, Vs, _powerRating);
        }


        public void CableSelected(CableSelectViewModel CSVM, CableData SelectedCable)
        {
            if (CSVM == Cable1ViewModel)
            {
                Cable1 = SelectedCable;
            }


            if (CSVM == Cable2ViewModel)
            {
                Cable2 = SelectedCable;
            }

            if (CSVM == Cable3ViewModel)
            {
                Cable3 = SelectedCable;
            }

            if (CSVM == Cable4ViewModel)
            {
                Cable4 = SelectedCable;
            }
        }

        #endregion


        #region Properties

        #region Cables

        public CableData Cable1
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

        public CableData Cable2
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

        public CableData Cable3
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

        public CableData Cable4
        {
            get { return _cable4; }
            set
            {
                _cable4 = value;
                OnPropertyChanged("Cable4");
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

        public Visibility Cable2Selected
        {
            get { return _cable2Selected; }
            set
            {
                _cable2Selected = value;
                this.OnPropertyChanged("Cable2Selected");
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

        public ViewModelBase Cable1ViewModel
        {
            get { return _cable1ViewModel; }
            set
            {
                _cable1ViewModel = value;
                OnPropertyChanged("Cable1ViewModel");
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

        public ViewModelBase Cable3ViewModel
        {
            get { return _cable3ViewModel; }
            set
            {
                _cable3ViewModel = value;
                OnPropertyChanged("Cable3ViewModel");
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


        #endregion

        #region PowerUnits

        public double Zk3p
        {
            get { return _Zk3p; }
            set
            {
                _Zk3p = value;
                OnPropertyChanged("Zk3p");
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

        public double Sk
        {
            get { return _Sk; }
            set
            {
                _Sk = value;
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
