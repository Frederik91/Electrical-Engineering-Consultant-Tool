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
        private GridDataViewModel GridDataVM;

        private GridAndTransformerData GTD = new GridAndTransformerData();

        public TransformerDataViewModel(GridDataViewModel gridDataViewModel, GridAndTransformerData _gtd)
        {
            GridDataVM = gridDataViewModel;
            GTD = _gtd;


            GTD.TransformerPowerRating = 400000;
            GTD.Ek = 0.04;
            GTD.GridVoltage = 20000;
            GTD.TransformerFullLoadLoss = 4600;
            GTD.GridIk = 15501;
            GTD.TransformerR0 = 1;
            GTD.TransformerX0 = 0.95;
        }

        #region Methods

        private void ValueUpdated()
        {
            GridDataVM.GTD = GTD;
        }

        #endregion


        #region Properties

        public double PowerRating
        {
            get { return GTD.TransformerPowerRating; }
            set
            {
                GTD.TransformerPowerRating = value;
                OnPropertyChanged("PowerRating");
                ValueUpdated();
            }
        }

        public double Ek
        {
            get { return GTD.Ek; }
            set
            {
                if (value > 1)
                {
                    GTD.Ek = value / 100;
                }
                else
                {
                    GTD.Ek = value;
                }
                OnPropertyChanged("Ek");
                ValueUpdated();
            }
        }

        public double Vp
        {
            get { return GTD.GridVoltage; }
            set
            {
                GTD.GridVoltage = value;
                OnPropertyChanged("Vp");
                ValueUpdated();
            }
        }

        public double Vs
        {
            get { return GTD.TransformerVoltageLow; }
            set
            {
                GTD.TransformerVoltageLow = value;
                OnPropertyChanged("Vs");
                ValueUpdated();
            }
        }

        public double TransformerPowerLoss
        {
            get { return GTD.TransformerFullLoadLoss; }
            set
            {
                GTD.TransformerFullLoadLoss = value;
                OnPropertyChanged("TransformerPowerLoss");
                ValueUpdated();
            }
        }

        public double IkGrid
        {
            get { return GTD.GridIk; }
            set
            {
                GTD.GridIk = value;
                OnPropertyChanged("IkGrid");
                ValueUpdated();
            }
        }
        

        #endregion

    }
}
