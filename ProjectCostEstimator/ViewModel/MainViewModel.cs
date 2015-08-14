using GalaSoft.MvvmLight;
using ProjectCostEstimator.Commands;
using System.Windows.Input;
using System.Configuration;
using System.IO;
using Microsoft.VisualBasic;
using System.Xml;
using System.Collections.Generic;
using ProjectCostEstimator.Model;
using System;

namespace ProjectCostEstimator.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string _XMLPath = ConfigurationManager.AppSettings["XMLFolderPath"] + ConfigurationManager.AppSettings["XMLFileListName"] + ".xml";



        private ViewModelBase _currentViewModel;


        public MainViewModel()
        {
            #region initialization Command members

            NewProjectCommand = new DelegateCommand(o => OpenNewProjectView());
            ExistingProjectCommand = new DelegateCommand(o => OpenExistingProjectView());
            StartScreenCommand = new DelegateCommand(o => OpenStartScreenView());
            DataFileManagerCommand = new DelegateCommand(o => OpenDataFileManagerView());            

            #endregion
            OpenStartScreenView();
            checkXMLPath();
        }

        private void checkXMLPath()
        {
            if (!File.Exists(_XMLPath))
            {
                _XMLPath = Interaction.InputBox("FileList path in configuration file is wrong or missing, please provide correct path for FileList.xml", "File not found");
                checkXMLPath();
            }
        }




        #region Command members

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

        #endregion
    }
}