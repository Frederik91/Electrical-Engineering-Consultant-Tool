using EECT.Commands;
using EECT.ElectricalCalculations;
using EECT.Model;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
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

        
        private List<CableProperties> _cableList = new List<CableProperties>();
        private List<CableData> _filteredCableList;
        private List<CableData> _cableDataList;


        private List<double> _cableDimensionList;
        private List<int> _cableConductorList;
        private List<string> _cableTypeList;
        private List<string> _cableMaterialList;

        private List<string> _aviliableCableList;

        //////private CableProperties _cable = new CableProperties();

        private CableProperties _chosenCable = new CableProperties { CableData = new CableData() };
        private CableProperties _selectedCableProperties = new CableProperties { CableData = new CableData() };

        private string _cableDataLocation = ConfigurationManager.AppSettings["CableDataFolderPath"];
        private string _selectedCableType;
        private string _cableName;

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


        private void SaveCable()
        {
            if (_cableList.Count == 0)
            {
                MessageBoxResult createNew = MessageBox.Show("No cable exist, do you want to create one?", "Warning", MessageBoxButton.YesNo);

                if (createNew == MessageBoxResult.Yes)
                {
                    AddCable();
                }
                else
                {
                    return;
                }

            }
            //Må finne aktiv kabel i CableList og sette denne lik _chosenCable.
        }

        private void AddCable()
        {
            List<CableProperties> list = new List<CableProperties>();

            list = _cableList;

            string Name = Interaction.InputBox("Enter cable name", "New Cable");

            list.Add(new CableProperties
            {
                CableData = new CableData(),
                Name = Name
            });

            CableName = Name;
            CableList = list;
            SetSelectedCableProperties();
        }

        private void GetSelectedCableData()
        {
            //Når en annen kabel en den som er aktiv i listen CableList velges, skal comboboxer nullstilles, med unntak av combobox for valg av kabel.
            //tekstbokser med navn, antall kabler og lengde skal få verdiene fra valgte kabel.
        }

        private void DeleteSelectedCable()
        {
            //Valgt kabel skal slettes fra listen.
        }

        private void SetSelectedCableProperties()
        {
            var currentCable = CableList.Find(item => item.Name == CableName);

            SelectedCableType = currentCable.CableData.CableType;
            Material = currentCable.CableData.Material;
            Conductors = currentCable.CableData.Conductors;
            Type = currentCable.CableData.CableType;
            Dimension = currentCable.CableData.Dimension;
        }

        private void EditCable()
        {
            _selectedCableProperties.Length = Length;
            _selectedCableProperties.NumberOfCables = NumberOfCables;
            GridDataViewModel.CableSelected(_selectedCableProperties);
        }

        private void RecalculateImpedance()
        {
            SelectedCable.CableData = _selectedCableProperties.CableData;
            CableImpedance = CDH.GetCableImpedance(_selectedCableProperties).Magnitude;
        }

        public string CableName
        {
            get { return _cableName; }
            set
            {
                _cableName = value;
                OnPropertyChanged("CableName");
                SelectedCable.Name = value;
            }
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

        private List<CableData> FilterAviliableCables()
        {
            return CDH.FilterAviliableCables(_cableDataList, _selectedCableProperties.CableData);
        }

        private List<string> GetAviliableCablesList()
        {
            return CDH.GetCableNameList(_filteredCableList);
        }

        private CableData GetSelectedCableID()
        {
            return CDH.GetCableID(_selectedCableType, _cableDataList);
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

        public List<CableProperties> CableList
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
            get { return _selectedCableProperties; }
            set
            {
                _selectedCableProperties = value;
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
                    SelectedCableType = value[0];
                }
            }
        }

        public string SelectedCableType
        {
            get { return _selectedCableType; }
            set
            {
                _selectedCableType = value;
                OnPropertyChanged("SelectedCableType");
                if (value != null)
                {
                    _chosenCable = SelectedCable;
                    RecalculateImpedance();
                    SaveCable();
                }
            }
        }


        public int Conductors
        {
            get { return _selectedCableProperties.CableData.Conductors; }
            set
            {
                _selectedCableProperties.CableData.Conductors = value;
                OnPropertyChanged("Conductors");
                FilteredCableList = FilterAviliableCables();
                AviliableCablesList = GetAviliableCablesList();
            }
        }

        public string Material
        {
            get { return _selectedCableProperties.CableData.Material; }
            set
            {
                _selectedCableProperties.CableData.Material = value;
                OnPropertyChanged("Material");
                FilteredCableList = FilterAviliableCables();
                AviliableCablesList = GetAviliableCablesList();
            }
        }

        public double Dimension
        {
            get { return _selectedCableProperties.CableData.Dimension; }
            set
            {
                _selectedCableProperties.CableData.Dimension = value;
                OnPropertyChanged("Dimension");
                FilteredCableList = FilterAviliableCables();
                AviliableCablesList = GetAviliableCablesList();
            }
        }

        public string Type
        {
            get { return _selectedCableProperties.CableData.CableType; }
            set
            {
                _selectedCableProperties.CableData.CableType = value;
                OnPropertyChanged("Type");
                FilteredCableList = FilterAviliableCables();
                AviliableCablesList = GetAviliableCablesList();
            }
        }

        public double Length
        {
            get { return _selectedCableProperties.Length; }
            set
            {
                _selectedCableProperties.Length = value;
                OnPropertyChanged("Length");
                RecalculateImpedance();
            }
        }

        public int NumberOfCables
        {
            get { return _selectedCableProperties.NumberOfCables; }
            set
            {
                _selectedCableProperties.NumberOfCables = value;
                OnPropertyChanged("NumberOfCables");
                RecalculateImpedance();
            }
        }

        public ICommand DeleteCableCommand { get; set; }
        public ICommand AddCableCommand { get; set; }

        #endregion
    }
}
