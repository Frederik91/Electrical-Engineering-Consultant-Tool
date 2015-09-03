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
        private double _ik3pMax { get; set; }
        private double _ik3pMin { get; set; }
        private double _ik2pMax { get; set; }
        private double _ik2pMin { get; set; }

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

        public double Ik3pMax
        {
            get
            {
                return _ik3pMax;
            }
            set
            {
                _ik3pMax = value;
                OnPropertyChanged("Ik3pMax");
            }
        }

        public double Ik3pMin
        {
            get
            {
                return _ik3pMin;
            }
            set
            {
                _ik3pMin = value;
                OnPropertyChanged("Ik3pMin");
            }
        }

        public double Ik2pMax
        {
            get
            {
                return _ik2pMax;
            }
            set
            {
                _ik2pMax = value;
                OnPropertyChanged("Ik2pMax");
            }
        }


        public double Ik2pMIn
        {
            get
            {
                return _ik2pMin;
            }
            set
            {
                _ik2pMin = value;
                OnPropertyChanged("Ik2pMIn");
            }
        }


    }
}
