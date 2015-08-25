using EECT.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EECT.ViewModel
{
    public class ShortCircuitViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;

        private bool _dataGridBool = true;

        public ShortCircuitViewModel()
        {
            OpenGridDataView();
        }

        public void OpenGridDataView()
        {
            if (DataGridBool)
            {
                CurrentViewModel = new GridDataViewModel();
            }

        }

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                this.OnPropertyChanged("CurrentViewModel");
            }
        }

        public bool DataGridBool
        {
            get { return _dataGridBool; }
            set
            {
                _dataGridBool = value;
                OpenGridDataView();
                OnPropertyChanged("DataGridBool");
            }
        }
    }
}
