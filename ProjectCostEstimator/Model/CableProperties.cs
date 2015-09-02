using EECT.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EECT.Model
{
    public class CableProperties : ViewModelBase
    {
        private double _length { get; set; }
        private CableData _cableData { get; set; }
        private int _numberOfCables { get; set; }
        private double _ik3p { get; set; }

        public Complex ImpedanceBehind { get; set; }
        public string Name { get; set; }
        public double SkCable { get; set; }
        public double TotalSkCable { get; set; }


        public CableData CableData
        {
            get
            {
                return _cableData;
            }
            set
            {
                _cableData = value;
                OnPropertyChanged("CableData");
            }
        }

        public double Length
        {
            get
            {
                return _length;
            }
            set
            {
                _length = value;
                OnPropertyChanged("Length");
            }
        }

        public int NumberOfCables
        {
            get
            {
                return _numberOfCables;
            }
            set
            {
                _numberOfCables = value;
                OnPropertyChanged("NumberOfCables");
            }
        }

        public double Ik3p
        {
            get
            {
                return _ik3p;
            }
            set
            {
                _ik3p = value;
                OnPropertyChanged("Ik3p");
            }
        }

        

    }
}
