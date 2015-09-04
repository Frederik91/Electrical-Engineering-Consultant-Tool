using EECT.Assets;
using EECT.ElectricalCalculations;
using EECT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EECT.ViewModel
{
    class FirstSwitchboardViewModel : ViewModelBase
    {
        private GridDataViewModel GridDataVM;

        private FirstSwitchboardData FSD = new FirstSwitchboardData();

        private double MinTolerance;
        private double MaxTolerance;
        private double Voltage;

        private bool CalculationsInProgress;

        public FirstSwitchboardViewModel(GridDataViewModel gridDataViewModel, FirstSwitchboardData fsd, double maxTolerance, double minTolerance, double voltage)
        {
            GridDataVM = gridDataViewModel;
            MaxTolerance = maxTolerance;
            Voltage = voltage;
            FSD = fsd;
            MinTolerance = minTolerance;

            //debug-verdier
            FSD.Rplus2pmin = 0.114;
            FSD.Xplus2pmin = 0.0552;
            DataUpdated2p(FirstSwitchboardEnums.Rplus);

            FSD.Rplus3pmax = 0.008;
            FSD.Xplus3pmax = 0.0186;
            DataUpdated3p(FirstSwitchboardEnums.Rplus);
        }

        private void DataUpdated2p(FirstSwitchboardEnums Updated)
        {
            var PC = new PowerCalc();
            var z = new Complex(Rplus2pmin, Xplus2pmin);

            if ((Updated == FirstSwitchboardEnums.Rplus || Updated == FirstSwitchboardEnums.Xplus) && !CalculationsInProgress)
            {
                CalculationsInProgress = true;
                CosPhi2pmin = Rplus2pmin / z.Magnitude;
                Ik2pmin = PC.Ik2p(Voltage, z, 0.95);

                CalculationsInProgress = false;
            }
            if ((Updated == FirstSwitchboardEnums.CosPhi || Updated == FirstSwitchboardEnums.Ik) && !CalculationsInProgress)
            {
                CalculationsInProgress = true;
                var Z = PC.CurrentCosPhiToImpedance3p(Ik2pmin, CosPhi2pmin, Voltage, MaxTolerance);

                Rplus3pmax = Z.Real;
                Xplus3pmax = Z.Imaginary;
                CalculationsInProgress = false;
            }

            FSD.Ik3pmin = Ik2pmin / (0.5 * Math.Sqrt(3));

            GridDataVM.FSD = FSD;
        }

        private void DataUpdated3p(FirstSwitchboardEnums Updated)
        {
            var PC = new PowerCalc();
            var z = new Complex(Rplus3pmax, Xplus3pmax);

            if ((Updated == FirstSwitchboardEnums.Rplus || Updated == FirstSwitchboardEnums.Xplus) && !CalculationsInProgress)
            {
                CalculationsInProgress = true;
                CosPhi3pmax = Rplus3pmax / z.Magnitude;
                Ik3pmax = PC.Ik3p(Voltage, z, MaxTolerance);
                CalculationsInProgress = false;
            }
            if ((Updated == FirstSwitchboardEnums.CosPhi || Updated == FirstSwitchboardEnums.Ik) && !CalculationsInProgress)
            {
                CalculationsInProgress = true;
                var Z = PC.CurrentCosPhiToImpedance3p(Ik3pmax, CosPhi3pmax, Voltage, MaxTolerance);

                Rplus3pmax = Z.Real;
                Xplus3pmax = Z.Imaginary;
                CalculationsInProgress = false;
            }

            FSD.Ik2pmax = Ik3pmax * (0.5 * Math.Sqrt(3));

            GridDataVM.FSD = FSD;
        }

        public double Ik2pmin
        {
            get { return FSD.Ik2pmin; }
            set
            {
                FSD.Ik2pmin = value;
                OnPropertyChanged("Ik2pmin");
                DataUpdated2p(FirstSwitchboardEnums.Ik);
            }
        }

        public double CosPhi2pmin
        {
            get { return FSD.CosPhi2pmin; }
            set
            {
                FSD.CosPhi2pmin = value;
                OnPropertyChanged("CosPhi2pmin");
                DataUpdated2p(FirstSwitchboardEnums.CosPhi);
            }
        }

        public double Rplus2pmin
        {
            get { return FSD.Rplus2pmin; }
            set
            {
                FSD.Rplus2pmin = value;
                OnPropertyChanged("Rplus2pmin");
                DataUpdated2p(FirstSwitchboardEnums.Rplus);
            }
        }

        public double Xplus2pmin
        {
            get { return FSD.Xplus2pmin; }
            set
            {
                FSD.Xplus2pmin = value;
                OnPropertyChanged("Xplus2pmin");
                DataUpdated2p(FirstSwitchboardEnums.Xplus);
            }
        }

        public double Ik3pmax
        {
            get { return FSD.Ik3pmax; }
            set
            {
                FSD.Ik3pmax = value;
                OnPropertyChanged("Ik3pmax");
                DataUpdated3p(FirstSwitchboardEnums.Ik);
            }
        }

        public double CosPhi3pmax
        {
            get { return FSD.CosPhi3pmax; }
            set
            {
                FSD.CosPhi3pmax = value;
                OnPropertyChanged("CosPhi3pmax");
                DataUpdated3p(FirstSwitchboardEnums.CosPhi);
            }
        }

        public double Rplus3pmax
        {
            get { return FSD.Rplus3pmax; }
            set
            {
                FSD.Rplus3pmax = value;
                OnPropertyChanged("Rplus3pmax");
                DataUpdated3p(FirstSwitchboardEnums.Rplus);
            }
        }

        public double Xplus3pmax
        {
            get { return FSD.Xplus3pmax; }
            set
            {
                FSD.Xplus3pmax = value;
                OnPropertyChanged("Xplus3pmax");
                DataUpdated3p(FirstSwitchboardEnums.Xplus);
            }
        }
    }
}
