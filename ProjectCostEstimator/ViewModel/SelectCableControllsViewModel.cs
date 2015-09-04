using EECT.ElectricalCalculations;
using EECT.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EECT.ViewModel
{
    class SelectCableControllsViewModel : ViewModelBase
    {
        private EditCableViewModel EditCableVM;

        private CableProperties _cable;

        private CableDataHandler CDH = new CableDataHandler();

        private List<CableData> _filteredCableList;
        private List<CableData> _cableDataList;

        private List<double?> _cableDimensionList;
        private List<int?> _cableConductorList;
        private double _cableImpedance;

        private List<string> _cableTypeList;
        private List<string> _cableMaterialList;
        private List<string> _aviliableCableList;

        private ObservableCollection<CableProperties> _cableList = new ObservableCollection<CableProperties>();

        public SelectCableControllsViewModel(EditCableViewModel editCableVM, CableProperties CableProperties, List<CableData> CableDataList)
        {
            EditCableVM = editCableVM;
            _cable = CableProperties;
            _cableDataList = CableDataList;

            CableConductorList = CDH.GetCableConductorList(_cableDataList);
            CableMaterialList = CDH.GetMaterialList(_cableDataList);
            CableDimensionList = CDH.GetCableSizesList(_cableDataList);
            CableTypeList = CDH.GetCableTypeList(_cableDataList);
            AviliableCablesList = CDH.GetCableNameList(_cableDataList);

        }


        #region Methods

        private List<CableData> FilterAviliableCables()
        {
            return CDH.FilterAviliableCables(_cableDataList, _cable.CableData);
        }

        private List<string> GetAviliableCablesList()
        {
            return CDH.GetCableNameList(_filteredCableList);
        }

        private CableData GetSelectedCableID()
        {
            return CDH.GetCableID(_cable.CableData.CableType, _cableDataList);
        }


        private void UpdateCableList()
        {
            EditCableVM.UpdateCableList(_cable);
        }

        private void CableSelected()
        {
            var item = _cableDataList.Find(x => x.Cable == _cable.CableData.Cable);
            _cable.CableData = item;
        }

        #endregion

        public double CableImpedance
        {
            get { return _cableImpedance; }
            set
            {
                _cableImpedance = value;
                OnPropertyChanged("CableImpedance");
            }
        }


        public string CableName
        {
            get { return _cable.Name; }
            set
            {
                _cable.Name = value;
                OnPropertyChanged("CableName");
                Cable.Name = value;
            }
        }


        public List<int?> CableConductorList
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

        public List<double?> CableDimensionList
        {
            get { return _cableDimensionList; }
            set
            {
                _cableDimensionList = value;
                OnPropertyChanged("CableDimensionList");
            }
        }

        #region Properties

        public CableProperties Cable
        {
            get { return _cable; }
            set
            {
                _cable = value;
                OnPropertyChanged("SelectedCable");
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
                    Type = value[0];
                }
            }
        }

        public int? Conductors
        {
            get { return _cable.CableData.Conductors; }
            set
            {
                _cable.CableData.Conductors = value;
                FilteredCableList = FilterAviliableCables();
                AviliableCablesList = GetAviliableCablesList();
                OnPropertyChanged("Conductors");
            }
        }

        public string Material
        {
            get { return _cable.CableData.Material; }
            set
            {
                _cable.CableData.Material = value;
                FilteredCableList = FilterAviliableCables();
                AviliableCablesList = GetAviliableCablesList();
                OnPropertyChanged("Material");
            }
        }

        public double? Dimension
        {
            get { return _cable.CableData.Dimension; }
            set
            {
                _cable.CableData.Dimension = value;
                FilteredCableList = FilterAviliableCables();
                AviliableCablesList = GetAviliableCablesList();
                OnPropertyChanged("Dimension");
            }
        }

        public string Type
        {
            get { return _cable.CableData.CableType; }
            set
            {
                _cable.CableData.CableType = value;
                OnPropertyChanged("Type");
            }
        }

        public string SelectedCable
        {
            get { return _cable.CableData.Cable; }
            set
            {
                _cable.CableData.Cable = value;
                OnPropertyChanged("Type");
                CableSelected();
                UpdateCableList();
            }
        }

        public double Length
        {
            get { return _cable.Length; }
            set
            {
                _cable.Length = value;
                OnPropertyChanged("Length");
                UpdateCableList();
            }
        }

        public int NumberOfCables
        {
            get { return _cable.NumberOfCables; }
            set
            {
                _cable.NumberOfCables = value;
                OnPropertyChanged("NumberOfCables");
                UpdateCableList();
            }
        }
        #endregion
    }
}
