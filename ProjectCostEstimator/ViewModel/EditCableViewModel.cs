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
        private ViewModelBase _viewModelBase;

        private CableDataHandler CDH = new CableDataHandler();

        private List<string> _cableNameList = new List<string>();
        private List<CableData> _cableDataList;

        private ObservableCollection<CableProperties> _cableList = new ObservableCollection<CableProperties>();


        private CableProperties _chosenCable = new CableProperties { CableData = new CableData() };

        private string _cableDataLocation = ConfigurationManager.AppSettings["CableDataFolderPath"];

        private int _selectedCableIndex = 0;

        public EditCableViewModel(GridDataViewModel gridDataViewModel)
        {
            GridDataViewModel = gridDataViewModel;

            _cableDataList = CDH.GetCableData(_cableDataLocation);

            DeleteCableCommand = new DelegateCommand(o => DeleteSelectedCable());
            AddCableCommand = new DelegateCommand(o => AddCable());


        }

        #region Methods

        private void OpenSelectCableControlls()
        {
            ViewModelBase = new SelectCableControllsViewModel(this, _chosenCable, _cableDataList);
        }

        private void UpdateUserControlls()
        {
            OpenSelectCableControlls();
        }

        public void UpdateCableList(CableProperties Cable)
        {
            var list = CableList;
            var index = _selectedCableIndex;
            list[_selectedCableIndex] = Cable;

            //CableList = new ObservableCollection<CableProperties>();

            CableList = list;

            SelectedCableIndex = index;

            GridDataViewModel.CableSelected(CableList.ToList());

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

            UpdateUserControlls();
            GridDataViewModel.CableSelected(CableList.ToList());
        }

        private void ChangeCable()
        {
            if (_selectedCableIndex == -1 || _selectedCableIndex == CableList.Count())
            {
                return;
            }

            _chosenCable = CableList[_selectedCableIndex];

            UpdateUserControlls();
            GridDataViewModel.CableSelected(CableList.ToList());
        }

        private void DeleteSelectedCable()
        {
            var list = CableList;
            var index = _selectedCableIndex;

            list.RemoveAt(_selectedCableIndex);

            CableList = list;

            if (index > 0)
            {
                SelectedCableIndex = index - 1;
            }

            if (index == 0)
            {
                SelectedCableIndex = 0;
            }
            GridDataViewModel.CableSelected(CableList.ToList());
        }

        public void UpdateCableList(List<CableProperties> CableList)
        {

        }


        #endregion

        #region Properties


        public ObservableCollection<CableProperties> CableList
        {
            get { return _cableList; }
            set
            {
                _cableList = value;
                OnPropertyChanged("CableList");
            }
        }


        public int SelectedCableIndex
        {
            get { return _selectedCableIndex; }
            set
            {
                _selectedCableIndex = value;
                ChangeCable();
                OnPropertyChanged("SelectedCableIndex");
            }
        }

        public ViewModelBase ViewModelBase
        {
            get { return _viewModelBase; }
            set
            {
                _viewModelBase = value;
                OnPropertyChanged("ViewModelBase");
            }
        }

        public ICommand DeleteCableCommand { get; set; }
        public ICommand AddCableCommand { get; set; }

        #endregion
    }
}
