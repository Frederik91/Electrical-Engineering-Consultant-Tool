using EECT.Assets;
using EECT.ElectricalCalculations;
using EECT.Model;
using System.Collections.Generic;
using System.Configuration;

namespace EECT.ViewModel
{
    public class GridDataViewModel : ViewModelBase
    {
        private List<CableData> _cableList = new List<CableData>();
        private List<CableData> _filteredCableList1 = new List<CableData>();

        private CableProperties _cable1 = new CableProperties();

        private double _powerRating;
        private double _Ek;
        private double _Vp;
        private double _Vs;


        private List<double> _cableDimensionList;
                
        private List<int> _cableConductorList;

        private List<string> _cableTypeList;
        private List<string> _cableMaterialList;

        private string _cableData = ConfigurationManager.AppSettings["CableDataFolderPath"];
        private List<string> _aviliableCableList1;
        private string _selectedCable1;
        private CableData _selectedCableData;



        public GridDataViewModel()
        {
            var CBH = new CableDataHandler();
            CableList = CBH.GetCableData(_cableData);
            CableConductorList = CBH.GetCableConductorList(_cableList);
            CableMaterialList = CBH.GetMaterialList(_cableList);
            CableDimensionList = CBH.GetCableSizesList(_cableList);
            CableTypeList = CBH.GetCableTypeList(_cableList);
        }

        #region Methods

        private List<CableData> FilterAviliableCables(CableProperties Cable)
        {
            var CBH = new CableDataHandler();

            return CBH.FilterAviliableCables(CableList, Cable);
        }

        private List<string> GetAviliableCablesList1(List<CableData> ListToSort)
        {
            var CBH = new CableDataHandler();

            return CBH.GetCableNameList(ListToSort);
        }

        private CableData GetSelectedCableID(string cableName)
        {
            var CBH = new CableDataHandler();

            return CBH.GetCableID(cableName, _cableList);
        }

        #endregion


        #region Properties

        #region Cable1

        public CableProperties Cable1
        {
            get { return _cable1; }
            set
            {
                _cable1 = value;                
                OnPropertyChanged("Cable1");
            }
        }

        public List<CableData> FilteredCableList1
        {
            get { return _filteredCableList1; }
            set
            {
                _filteredCableList1 = value;
                OnPropertyChanged("FilteredCableList1");
            }
        }

        public List<string> AviliableCablesList1
        {
            get { return _aviliableCableList1; }
            set
            {
                _aviliableCableList1 = value;
                OnPropertyChanged("AviliableCablesList1");
            }
        }

        public string SelectedCable1
        {
            get { return _selectedCable1; }
            set
            {
                _selectedCable1 = value;
                OnPropertyChanged("SelectedCable1");
                _selectedCableData = GetSelectedCableID(value);
            }
        }


        public int Conductors1
        {
            get { return Cable1.Conductors; }
            set
            {
                Cable1.Conductors = value;
                OnPropertyChanged("Conductors1");
                FilteredCableList1 = FilterAviliableCables(_cable1);
                AviliableCablesList1 = GetAviliableCablesList1(_filteredCableList1);
            }
        }

        public string Material1
        {
            get { return Cable1.Material; }
            set
            {
                Cable1.Material = value;
                OnPropertyChanged("Material1");
                FilteredCableList1 = FilterAviliableCables(_cable1);
                AviliableCablesList1 = GetAviliableCablesList1(_filteredCableList1);
            }
        }

        public double Dimension1
        {
            get { return Cable1.dimension; }
            set
            {
                Cable1.dimension = value;
                OnPropertyChanged("Dimension1");
                FilteredCableList1 = FilterAviliableCables(_cable1);
                AviliableCablesList1 = GetAviliableCablesList1(_filteredCableList1);
            }
        }

        public string Type1
        {
            get { return Cable1.CableType; }
            set
            {
                Cable1.CableType = value;
                OnPropertyChanged("Type1");
                FilteredCableList1 = FilterAviliableCables(_cable1);
                AviliableCablesList1 = GetAviliableCablesList1(_filteredCableList1);
            }
        }

        public double Length1
        {
            get { return Cable1.Length; }
            set
            {
                Cable1.Length = value;
                OnPropertyChanged("Length1");
            }
        }

        public int NumberOfCables1
        {
            get { return Cable1.NumberOfCables; }
            set
            {
                Cable1.NumberOfCables = value;
                OnPropertyChanged("NumberOfCables1");
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
