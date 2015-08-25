using EECT.Assets;
using EECT.ElectricalCalculations;
using EECT.Model;
using EECT.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECT.ViewModel
{
    public class GridDataViewModel : ViewModelBase
    {
        private List<CableData> _cableList = new List<CableData>();
        private List<CableData> _filteredCableList1 = new List<CableData>();

        private double _powerRating;
        private double _Ek;
        private double _Vp;
        private double _Vs;
        private double _dimension1;

        private int _conductors1;

        private List<double> _cableDimensionList;
                
        private List<int> _cableConductorList;

        private List<string> _cableTypeList;
        private List<string> _cableMaterialList;

        private string _cableData = ConfigurationManager.AppSettings["CableDataFolderPath"];
        private string _material1;
        private string _type1;



        public GridDataViewModel()
        {
            var CBH = new CableDataHandler();
            FilteredCableList1 = CableList = CBH.GetCableData(_cableData);
            CableConductorList = CBH.GetCableConductorList(CableList);
            CableMaterialList = CBH.GetMaterialList(CableList);
            CableDimensionList = CBH.GetCableSizesList(CableList);
            CableTypeList = CBH.GetCableTypeList(CableList);
        }

        #region Methods


        private void FilterAviliableCableOptions(CableUnits Updated)
        {
            var CBH = new CableDataHandler();

            FilteredCableList1 = CBH.FilterAviliableCables(CableList, Conductors1, Material1, Dimension1, Type1);

                switch (Updated)
                {
                    case CableUnits.Conductors:
                        CableConductorList = CBH.GetCableConductorList(FilteredCableList1);
                    return;                      
                    case CableUnits.Materials:
                        CableMaterialList = CBH.GetMaterialList(FilteredCableList1);
                    return;
                case CableUnits.Dimension:
                        CableDimensionList = CBH.GetCableSizesList(FilteredCableList1);
                    return;
                case CableUnits.Type:
                        CableTypeList = CBH.GetCableTypeList(FilteredCableList1);
                    return;
                default:
                    return;
            }               
        }


        #endregion


        #region Properties

        #region Cable1

        public List<CableData> FilteredCableList1
        {
            get { return _filteredCableList1; }
            set
            {
                _filteredCableList1 = value;
                OnPropertyChanged("FilteredCableList1");
            }
        }

        public int Conductors1
        {
            get { return _conductors1; }
            set
            {
                _conductors1 = value;
                FilterAviliableCableOptions(CableUnits.Conductors);
                OnPropertyChanged("Conductors1");
            }
        }

        public string Material1
        {
            get { return _material1; }
            set
            {
                _material1 = value;
                FilterAviliableCableOptions(CableUnits.Materials);
                OnPropertyChanged("Material1");
            }
        }

        public double Dimension1
        {
            get { return _dimension1; }
            set
            {
                _dimension1 = value;
                FilterAviliableCableOptions(CableUnits.Dimension);
                OnPropertyChanged("Dimension1");
            }
        }

        public string Type1
        {
            get { return _type1; }
            set
            {
                _type1 = value;
                FilterAviliableCableOptions(CableUnits.Type);
                OnPropertyChanged("Type1");
            }
        }

        #endregion

        #region Input data

        public List<int> CableConductorList
        {
            get { return _cableConductorList; }
            set
            {
                _cableConductorList = value;
                OnPropertyChanged("CableConductorList");
            }
        }

        public List<string> CableMaterialList
        {
            get { return _cableMaterialList; }
            set
            {
                _cableMaterialList = value;
                OnPropertyChanged("CableMaterialList");
            }
        }

        public List<string> CableTypeList
        {
            get { return _cableTypeList; }
            set
            {
                _cableTypeList = value;
                OnPropertyChanged("CableTypeList");
            }
        }

        public List<double> CableDimensionList
        {
            get { return _cableDimensionList; }
            set
            {
                _cableDimensionList = value;
                OnPropertyChanged("CableDimensionList");
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

        public double Ek
        {
            get { return _Ek; }
            set
            {
                _Ek = value;
                OnPropertyChanged("Ek");
            }
        }

        public double Vp
        {
            get { return _Vp; }
            set
            {
                _Vp = value;
                OnPropertyChanged("Vp");
            }
        }


        public double Vs
        {
            get { return _Vs; }
            set
            {
                _Vs = value;
                OnPropertyChanged("Vs");
            }
        }

        public List<CableData> CableList
        {
            get { return _cableList; }
            set
            {
                _cableList = value;
                OnPropertyChanged("CableList");
            }
        }

        #endregion

        #endregion
    }
}
