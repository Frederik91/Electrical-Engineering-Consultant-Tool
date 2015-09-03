using EECT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECT.ViewModel
{
    class TransformerDataViewModel : ViewModelBase
    {
        private double _powerRating = 400000;
        private double _TransformerPowerLoss = 4600;
        private double _IkGrid = 15501;
        private double _R0 = 1;
        private double _X0 = 0.95;

        private GridAndTransformerData GTD = new GridAndTransformerData();

        public TransformerDataViewModel(GridDataViewModel gridDataViewModel)
        {

        }


        #region Properties




        public double TransformerPowerLoss
        {
            get { return _TransformerPowerLoss; }
            set
            {
                _TransformerPowerLoss = value;
                OnPropertyChanged("TransformerPowerLoss");
            }
        }

        public double IkGrid
        {
            get { return _IkGrid; }
            set
            {
                _IkGrid = value;
                OnPropertyChanged("IkGrid");
            }
        }



        public double PowerRating
        {
            get { return _powerRating; }
            set
            {
                _powerRating = value;
                OnPropertyChanged("PowerRating");
            }
        }

        #endregion

    }
}
