using EECT.Commands;
using EECT.ElectricalCalculations;
using EECT.Model;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EECT.ViewModel
{
    public class EditCableViewModel : ViewModelBase
    {
        public GridDataViewModel GridDataViewModel { get; set; }

        private CableDataHandler CDH = new CableDataHandler();

        private List<string> _cableNameList = new List<string>();


        private ObservableCollection<CableProperties> _cableList = new ObservableCollection<CableProperties>();
        private List<CableData> _filteredCableList;
        private List<CableData> _cableDataList;


        private List<double?> _cableDimensionList;
        private List<int?> _cableConductorList;
        private List<string> _cableTypeList;
        private List<string> _cableMaterialList;

        private List<string> _aviliableCableList;

        private CableProperties _chosenCable = new CableProperties { CableData = new CableData() };

        private string _cableDataLocation = ConfigurationManager.AppSettings["CableDataFolderPath"];

        private double _cableImpedance;

        private int _selectedCableIndex = 0;

        public EditCableViewModel(GridDataViewModel gridDataViewModel)
        {
            GridDataViewModel = gridDataViewModel;

            SelectedCable.CableData = new CableData();

            DeleteCableCommand = new DelegateCommand(o => DeleteSelectedCable());
            AddCableCommand = new DelegateCommand(o => AddCable());

            _cableDataList = CDH.GetCableData(_cableDataLocation);
            CableConductorList = CDH.GetCableConductorList(_cableDataList);
            CableMaterialList = CDH.GetMaterialList(_cableDataList);
            CableDimensionList = CDH.GetCableSizesList(_cableDataList);
            CableTypeList = CDH.GetCableTypeList(_cableDataList);
            AviliableCablesList = CDH.GetCableNameList(_cableDataList);
        }

        #region Methods

        private void UpdateUserControlls()
        {
            Conductors = Conductors;
            Material = Material;
            Type = Type;
            Dimension = Dimension;
            SelectedCable = SelectedCable;
            CableList = CableList;
        }


        private void AddCable()
        {
            ObservableCollection<CableProperties> list = new ObservableCollection<CableProperties>();

            list = _cableList;

            string _name = Interaction.InputBox("Enter cable name", "New Cable");

            if (_name == string.Empty)
            {
                return;
            }

            list.Add(new CableProperties
            {
                CableData = new CableData(),
                Name = _name
            });

            CableList = list;

            _selectedCableIndex = CableList.Count() - 1;
            _chosenCable = CableList[_selectedCableIndex];
            CableName = _name;
        }

        private void ChangeCable()
        {
            //Når en annen kabel en den som er aktiv i listen CableList velges, skal comboboxer nullstilles, med unntak av combobox for valg av kabel.
            //tekstbokser med navn, antall kabler og lengde skal få verdiene fra valgte kabel.
            if (_selectedCableIndex == -1)
            {
                return;
            }

            _chosenCable = CableList[_selectedCableIndex];

            NumberOfCables = _chosenCable.NumberOfCables;
            Length = _chosenCable.Length;
            CableName = _chosenCable.Name;

            _chosenCable.CableData.Conductors = null;
            _chosenCable.CableData.Material = null;
            _chosenCable.CableData.CableType = null;
            _chosenCable.CableData.Dimension = null;
            SelectedCable = _chosenCable;
            FilteredCableList = FilterAviliableCables();
            AviliableCablesList = GetAviliableCablesList();

            UpdateUserControlls();
        }

        private void DeleteSelectedCable()
        {
            //Valgt kabel skal slettes fra listen.
        }



        private void EditCable()
        {

            if (_cableList.Count == 0)
            {
                MessageBoxResult result = MessageBox.Show("No cable exist, do you want to create one?", "Warning", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    AddCable();
                }
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }

            }

            var index = _selectedCableIndex;
            var list = CableList;
            list[_selectedCableIndex] = _chosenCable;

            CableList = list;
            _selectedCableIndex = index;

            GridDataViewModel.CableSelected(_chosenCable);
        }

        private void RecalculateImpedance()
        {
            SelectedCable.CableData = _chosenCable.CableData;
            CableImpedance = CDH.GetCableImpedance(_chosenCable).Magnitude;
        }

        public string CableName
        {
            get { return _chosenCable.Name; }
            set
            {
                _chosenCable.Name = value;
                OnPropertyChanged("CableName");
                SelectedCable.Name = value;
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

        private List<CableData> FilterAviliableCables()
        {
            return CDH.FilterAviliableCables(_cableDataList, _chosenCable.CableData);
        }

        private List<string> GetAviliableCablesList()
        {
            return CDH.GetCableNameList(_filteredCableList);
        }

        private CableData GetSelectedCableID()
        {
            return CDH.GetCableID(_chosenCable.CableData.CableType, _cableDataList);
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

        public ObservableCollection<CableProperties> CableList
        {
            get { return _cableList; }
            set
            {
                _cableList = value;
                OnPropertyChanged("CableList");
            }
        }

        public CableProperties SelectedCable
        {
            get { return _chosenCable; }
            set
            {
                _chosenCable = value;
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

        //public string SelectedCableType
        //{
        //    get { return _chosenCable.CableData.CableType; }
        //    set
        //    {
        //        if (value != null && value != _chosenCable.CableData.CableType)
        //        {
        //            _chosenCable.CableData.CableType = value;
        //            _chosenCable = SelectedCable;
        //            RecalculateImpedance();
        //            EditCable();
        //        }
        //        _chosenCable.CableData.CableType = value;
        //        OnPropertyChanged("SelectedCableType");
        //    }
        //}


        public int? Conductors
        {
            get { return _chosenCable.CableData.Conductors; }
            set
            {
                if (value != null && value != _chosenCable.CableData.Conductors)
                {
                    _chosenCable.CableData.Conductors = value;
                    FilteredCableList = FilterAviliableCables();
                    AviliableCablesList = GetAviliableCablesList();
                }
                _chosenCable.CableData.Conductors = value;
                OnPropertyChanged("Conductors");
            }
        }

        public string Material
        {
            get { return _chosenCable.CableData.Material; }
            set
            {
                if (value != null && value != _chosenCable.CableData.Material)
                {
                    _chosenCable.CableData.Material = value;
                    FilteredCableList = FilterAviliableCables();
                    AviliableCablesList = GetAviliableCablesList();
                }
                _chosenCable.CableData.Material = value;
                OnPropertyChanged("Material");
            }
        }

        public double? Dimension
        {
            get { return _chosenCable.CableData.Dimension; }
            set
            {
                if (value != null && value != _chosenCable.CableData.Dimension)
                {
                    _chosenCable.CableData.Dimension = value;
                    FilteredCableList = FilterAviliableCables();
                    AviliableCablesList = GetAviliableCablesList();
                }
                _chosenCable.CableData.Dimension = value;
                OnPropertyChanged("Dimension");
            }
        }

        public string Type
        {
            get { return _chosenCable.CableData.CableType; }
            set
            {
                if (value != null && value != _chosenCable.CableData.CableType)
                {
                    _chosenCable.CableData.CableType = value;
                    FilteredCableList = FilterAviliableCables();
                    AviliableCablesList = GetAviliableCablesList();
                }
                _chosenCable.CableData.CableType = value;
                OnPropertyChanged("Type");
            }
        }

        public double Length
        {
            get { return _chosenCable.Length; }
            set
            {
                if (value != _chosenCable.Length)
                {
                    _chosenCable.Length = value;
                    RecalculateImpedance();
                    EditCable();
                }
                _chosenCable.Length = value;
                OnPropertyChanged("Length");
            }
        }

        public int NumberOfCables
        {
            get { return _chosenCable.NumberOfCables; }
            set
            {
                if (value != _chosenCable.NumberOfCables)
                {
                    _chosenCable.NumberOfCables = value;
                    RecalculateImpedance();
                    EditCable();
                }
                _chosenCable.NumberOfCables = value;
                OnPropertyChanged("NumberOfCables");

            }
        }

        public int SelectedCableIndex
        {
            get { return _selectedCableIndex; }
            set
            {
                if (_selectedCableIndex != value)
                {
                    _selectedCableIndex = value;
                    ChangeCable();
                }
                _selectedCableIndex = value;
                OnPropertyChanged("SelectedCableIndex");
            }
        }

        public ICommand DeleteCableCommand { get; set; }
        public ICommand AddCableCommand { get; set; }

        #endregion
    }
}
