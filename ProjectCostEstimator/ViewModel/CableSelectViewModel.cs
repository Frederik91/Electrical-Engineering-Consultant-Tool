using EECT.ElectricalCalculations;
using EECT.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECT.ViewModel
{
    public class CableSelectViewModel : ViewModelBase
    {
        public GridDataViewModel GridDataViewModel { get; set; }

        private CableDataHandler CBH = new CableDataHandler();

        private List<CableData> _cableList = new List<CableData>();
        private List<CableData> _filteredCableList;

        private List<double> _cableDimensionList;
        private List<int> _cableConductorList;
        private List<string> _cableTypeList;
        private List<string> _cableMaterialList;
        
        private List<string> _aviliableCableList;

        private CableProperties _cable = new CableProperties();

        private CableData _selectedCableData;

        private string _cableData = ConfigurationManager.AppSettings["CableDataFolderPath"];
        private string _selectedCable;

        private double _cableImpedance;


        public CableSelectViewModel(GridDataViewModel gridDataViewModel)
        {
            GridDataViewModel = gridDataViewModel;
            Cable.CableData = new CableData();

            CableList = CBH.GetCableData(_cableData);
            CableConductorList = CBH.GetCableConductorList(_cableList);
            CableMaterialList = CBH.GetMaterialList(_cableList);
            CableDimensionList = CBH.GetCableSizesList(_cableList);
            CableTypeList = CBH.GetCableTypeList(_cableList);
            AviliableCablesList = CBH.GetCableNameList(_cableList);
        }

        #region Methods

        private void SendSelectedCable()
        {
            _selectedCableData.Length = Length;
            _selectedCableData.NumberOfCables = NumberOfCables;
            GridDataViewModel.CableSelected(this, _selectedCableData);
        }

        private void RecalculateImpedance()
        {
            var PC = new PowerCalc();
            CableImpedance = PC.EqualParallelImpedances(CBH.GetCableImpedance(_selectedCableData) * Length, NumberOfCables).Magnitude;
        }


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

        private List<CableData> FilterAviliableCables(CableProperties Cable)
        {
            return CBH.FilterAviliableCables(CableList, Cable);
        }

        private List<string> GetAviliableCablesList(List<CableData> ListToSort)
        {
            return CBH.GetCableNameList(ListToSort);
        }

        private CableData GetSelectedCableID(string cableName)
        {
            return CBH.GetCableID(cableName, _cableList);
        }

        #endregion

        #region Properties

        public double CableImpedance
        {
            get { return _cableImpedance; }
            set
            {
                _cableImpedance = value;
                OnPropertyChanged("CableImpedance");
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

        public CableProperties Cable
        {
            get { return _cable; }
            set
            {
                _cable = value;
                OnPropertyChanged("Cable");
            }
        }

        public List<CableData> FilteredCableList
        {
            get { return _filteredCableList; }
            set
            {
                _filteredCableList = value;
                OnPropertyChanged("FilteredCableList");
            }
        }

        public List<string> AviliableCablesList
        {
            get { return _aviliableCableList; }
            set
            {
                _aviliableCableList = value;
                OnPropertyChanged("AviliableCablesList");
                if (AviliableCablesList.Count == 1)
                {
                    SelectedCable = value[0];
                }
            }
        }

        public string SelectedCable
        {
            get { return _selectedCable; }
            set
            {
                _selectedCable = value;
                OnPropertyChanged("SelectedCable");
                if (value != null)
                {
                    _selectedCableData = GetSelectedCableID(value);
                    RecalculateImpedance();
                    SendSelectedCable();
                }
            }
        }


        public int Conductors
        {
            get { return Cable.Conductors; }
            set
            {
                Cable.Conductors = value;
                OnPropertyChanged("Conductors");
                FilteredCableList = FilterAviliableCables(_cable);
                AviliableCablesList = GetAviliableCablesList(_filteredCableList);
            }
        }

        public string Material
        {
            get { return Cable.CableData.Material; }
            set
            {
                Cable.CableData.Material = value;
                OnPropertyChanged("Material");
                FilteredCableList = FilterAviliableCables(_cable);
                AviliableCablesList = GetAviliableCablesList(_filteredCableList);
            }
        }

        public double Dimension
        {
            get { return Cable.CableData.Dimension; }
            set
            {
                Cable.CableData.Dimension = value;
                OnPropertyChanged("Dimension");
                FilteredCableList = FilterAviliableCables(_cable);
                AviliableCablesList = GetAviliableCablesList(_filteredCableList);
            }
        }

        public string Type
        {
            get { return Cable.CableData.CableType; }
            set
            {
                Cable.CableData.CableType = value;
                OnPropertyChanged("Type");
                FilteredCableList = FilterAviliableCables(_cable);
                AviliableCablesList = GetAviliableCablesList(_filteredCableList);
            }
        }

        public double Length
        {
            get { return Cable.CableData.Length; }
            set
            {
                Cable.CableData.Length = value;
                OnPropertyChanged("Length");
                RecalculateImpedance();
            }
        }

        public int NumberOfCables
        {
            get { return Cable.NumberOfCables; }
            set
            {
                Cable.NumberOfCables = value;
                OnPropertyChanged("NumberOfCables");
                RecalculateImpedance();
            }
        }

        #endregion
    }
}
