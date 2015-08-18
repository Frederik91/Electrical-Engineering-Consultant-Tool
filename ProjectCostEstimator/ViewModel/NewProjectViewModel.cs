using Microsoft.VisualBasic;
using ProjectCostEstimator.Commands;
using ProjectCostEstimator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace ProjectCostEstimator.ViewModel
{
    class NewProjectViewModel : ViewModelBase
    {
        private string _XMLPath = ConfigurationManager.AppSettings["XMLFolderPath"] + ConfigurationManager.AppSettings["XMLFileListName"] + ".xml";

        private int _projectIndexToAdd;
        private int _projectIndexToRemove;

        private ObservableCollection<FilePathList> _storedProjectsList = new ObservableCollection<FilePathList>();
        private ObservableCollection<FilePathList> _selectedProjectsList = new ObservableCollection<FilePathList>();


        public NewProjectViewModel()
        {
            AddToSelectedProjectsListCommand = new DelegateCommand(o => AddToSelectedProjectsList());
            RemoveFromSelectedProjectsListCommand = new DelegateCommand(o => RemoveFromSelectedProjectsList());


            checkXMLPath();
            FillProjectsList();
        }


        #region Methods

        private void checkXMLPath()
        {
            if (!File.Exists(_XMLPath))
            {
                _XMLPath = Interaction.InputBox("FileList path in configuration file is wrong or missing, please provide correct path for FileList.xml", "File not found");
                checkXMLPath();
            }
        }

        private void RemoveFromSelectedProjectsList()
        {
            var ToList = StoredProjectsList;
            var FromList = SelectedProjectsList;

            ToList.Add(new FilePathList
            {
                Name = SelectedProjectsList[ProjectIndexToRemove].Name,
                FilePath = SelectedProjectsList[ProjectIndexToRemove].FilePath
            });

            FromList.RemoveAt(ProjectIndexToRemove);

            StoredProjectsList = ToList;
            SelectedProjectsList = FromList;
        }

        private void AddToSelectedProjectsList()
        {
            var ToList = SelectedProjectsList;
            var FromList = StoredProjectsList;

            ToList.Add(new FilePathList
            {
                Name = StoredProjectsList[ProjectIndexToAdd].Name,
                FilePath = StoredProjectsList[ProjectIndexToAdd].FilePath
            });

            FromList.RemoveAt(ProjectIndexToAdd);

            StoredProjectsList = FromList;
            SelectedProjectsList = ToList;

        }

        private void FillProjectsList()
        {
            XDocument xDoc = XDocument.Load(_XMLPath);
            var list = new ObservableCollection<FilePathList>();
            var Projects = xDoc.Element("Files").Elements("ProjectFile");

            foreach (var project in Projects)
            {
                list.Add(new FilePathList
                {
                    Name = project.Attribute("Name").Value,
                    FilePath = project.Attribute("Path").Value
                });
            }
            StoredProjectsList = list;
        }

        #endregion

        #region Properties

        public ObservableCollection<FilePathList> StoredProjectsList
        {
            get { return _storedProjectsList; }
            set
            {
                _storedProjectsList = value;
                OnPropertyChanged("StoredProjectsList");
            }
        }

        public int ProjectIndexToAdd
        {
            get { return _projectIndexToAdd; }
            set
            {
                _projectIndexToAdd = value;
                OnPropertyChanged("ProjectIndexToAdd");
            }
        }

        public int ProjectIndexToRemove
        {
            get { return _projectIndexToRemove; }
            set
            {
                _projectIndexToRemove = value;
                OnPropertyChanged("ProjectIndexToRemove");
            }
        }

        public ObservableCollection<FilePathList> SelectedProjectsList
        {
            get { return _selectedProjectsList; }
            set
            {
                _selectedProjectsList = value;
                OnPropertyChanged("SelectedProjectsList");
            }
        }

        public ICommand RemoveFromSelectedProjectsListCommand { get; private set; }
        public ICommand AddToSelectedProjectsListCommand { get; private set; }

        #endregion
    }
}
