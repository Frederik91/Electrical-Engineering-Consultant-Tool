using GalaSoft.MvvmLight;
using EECT.Commands;
using System.Windows.Input;
using System.Configuration;
using System.IO;
using Microsoft.VisualBasic;
using System.Xml;
using System.Collections.Generic;
using EECT.Model;
using System;
using System.Windows;

namespace EECT.ViewModel
{
    public class MainViewModel : ViewModelBase
    {



        private ViewModelBase _currentViewModel;


        public MainViewModel()
        {
            #region initialization Command members
                        
            NewProjectCommand = new DelegateCommand(o => OpenNewProjectView());
            ExistingProjectCommand = new DelegateCommand(o => OpenExistingProjectView());
            StartScreenCommand = new DelegateCommand(o => OpenStartScreenView());
            DataFileManagerCommand = new DelegateCommand(o => OpenDataFileManagerView());
            CableAndProtectionCommand = new DelegateCommand(o => OpenCableAndProtectionView());
            ExitCommand = new DelegateCommand(o => ExitProgram());

            #endregion
            OpenStartScreenView();
        }


        #region Command members

        public void OpenCableAndProtectionView()
        {
            CurrentViewModel = new CableAndProtectionViewModel();
        }

        public void ExitProgram()
        {
            Application.Current.Shutdown();
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

        public ICommand CableAndProtectionCommand { get; private set; }

        public ICommand ExitCommand { get; private set; }
        public ICommand NewProjectCommand { get; private set; }
        public ICommand ExistingProjectCommand { get; private set; }
        public ICommand StartScreenCommand { get; private set; }
        public ICommand DataFileManagerCommand { get; private set; }

        #endregion
    }
}