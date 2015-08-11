using GalaSoft.MvvmLight;
using ProjectCostEstimator.Commands;
using System.Windows.Input;

namespace ProjectCostEstimator.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        public MainViewModel()
        {
            NewProjectCommand = new DelegateCommand(o => OpenNewProjectView());
            ExistingProjectCommand = new DelegateCommand(o => OpenExistingProjectView());
            StartScreenCommand = new DelegateCommand(o => OpenStartScreenView());
            DataFileManagerCommand = new DelegateCommand(o => OpenDataFileManagerView());
            OpenStartScreenView();
        }

        public void OpenNewProjectView()
        {
            CurrentViewModel = new NewProjectViewModel();
        }

        public void OpenExistingProjectView()
        {
            CurrentViewModel = new ExistingProjectViewModel();
        }

        public void OpenStartScreenView()
        {
            CurrentViewModel = new StartScreenViewModel();
        }


        public void OpenDataFileManagerView()
        {
            CurrentViewModel = new DataFileManagerViewModel();
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

        public ICommand NewProjectCommand { get; private set; }
        public ICommand ExistingProjectCommand { get; private set; }
        public ICommand StartScreenCommand { get; private set; }
        public ICommand DataFileManagerCommand { get; private set; }
    }
}