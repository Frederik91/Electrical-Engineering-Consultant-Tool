using Microsoft.Win32;
using ProjectCostEstimator.Commands;
using ProjectCostEstimator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;

namespace ProjectCostEstimator.ViewModel
{
    class DataFileManagerViewModel : ViewModelBase
    {
        private List<string> _chapterTypes = new List<string>();
        private List<string> _chapterNumber = new List<string>();

        private List<DisciplineValues> _importedDataList = new List<DisciplineValues>();
        private List<FilePathList> _xmlFilePathsList = new List<FilePathList>();
        private ObservableCollection<DisciplineValues> _projectCostData;

        private DateTime _tenderDate = new DateTime();
        private DateTime _completionDate = new DateTime();

        private string _selectedChapterType;
        private string _selectedImportFilePath;
        private string _projectName;
        private string _atr;
        private string _XMLPath = ConfigurationManager.AppSettings["XMLFolderPath"];
        private int _selectedProjectIndex;

        public DataFileManagerViewModel()
        {
            SaveChangesCommand = new DelegateCommand(o => SaveChanges());
            ImportNewProject = new DelegateCommand(o => ImportProjectFile());
            SelectImportFilePathCommand = new DelegateCommand(o => SelectImportFilePath());

            collectXMLFilePaths();
        }

        private void ImportProjectFile()
        {
            var run = new ImportXML();
            ImportedDataList = run.ImportProjectFile(SelectedChapterType, SelectedImportFilePath);
        }

        private void SelectImportFilePath()
        {
            var fd = new OpenFileDialog();

            fd.DefaultExt = ".xml";
            fd.Filter = "XML files (*.xml)|*.xml";

            bool? result = fd.ShowDialog();

            if (result == true)
            {
                SelectedImportFilePath = fd.FileName;
            }

            if (File.Exists(SelectedImportFilePath))
            {
                PrepareFileImport();
            }

        }

        private void PrepareFileImport()
        {
            var run = new ImportXML();
            ChapterTypes = run.GetChapterTypes(SelectedImportFilePath);

        }

        private void collectXMLFilePaths()
        {
            XmlReader xmlReader = XmlReader.Create(_XMLPath);
            while (xmlReader.Read())
            {
                if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "ProjectFile"))
                {
                    if (xmlReader.HasAttributes)
                    {
                        XMLFilePathList.Add(new FilePathList() { Name = xmlReader.GetAttribute("Name"), FilePath = xmlReader.GetAttribute("Path") });
                    }
                }
            }
        }

        private void SaveChanges()
        {
            XElement xEle = XElement.Load(XMLFilePathList[SelectedProjectIndex].FilePath);

            xEle.Element("BuildingInformation").SetElementValue("ProjectName", ProjectName);
            xEle.Element("BuildingInformation").SetElementValue("ATR", ATR);
            xEle.Element("BuildingInformation").SetElementValue("TenderDate", TenderDate.ToShortDateString());
            xEle.Element("BuildingInformation").SetElementValue("CompletionDate", CompletionDate.ToShortDateString());

            xEle.Element("CostData").RemoveAll();

            foreach (var item in ProjectCostData)
            {
                xEle.Element("CostData").Add(new XElement("Chapter", new XElement("Discipline", item.Discipline), new XElement("ChapterNumber", item.Chapter), new XElement("Cost", item.Cost)));
            }

            xEle.Save(XMLFilePathList[SelectedProjectIndex].FilePath);

        }

        private void getSelectedProjectData()
        {
            _projectCostData = new ObservableCollection<DisciplineValues>();

            XDocument xdocument = XDocument.Load(XMLFilePathList[SelectedProjectIndex].FilePath);
            IEnumerable<XElement> ProjectInfoFile = xdocument.Elements();
            foreach (var info in ProjectInfoFile)
            {
                ProjectName = info.Element("BuildingInformation").Element("ProjectName").Value;
                ATR = info.Element("BuildingInformation").Element("ATR").Value;
                TenderDate = DateTime.ParseExact(info.Element("BuildingInformation").Element("TenderDate").Value, "dd.MM.yyyy", null);
                CompletionDate = DateTime.ParseExact(info.Element("BuildingInformation").Element("CompletionDate").Value, "dd.MM.yyyy", null);
            }

            foreach (XElement element in xdocument.Descendants("Chapter"))
            {
                _projectCostData.Add(new DisciplineValues
                {
                    Discipline = element.Element("Discipline").Value,
                    Chapter = element.Element("ChapterNumber").Value,
                    Cost = Convert.ToInt32(element.Element("Cost").Value)
                });
            }

            ProjectCostData = _projectCostData;
        }

        public List<DisciplineValues> ImportedDataList
        {
            get { return _importedDataList; }
            set
            {
                _importedDataList = value;
                OnPropertyChanged("ImportedDataList");
            }
        }

        public string SelectedChapterType
        {
            get { return _selectedChapterType; }
            set
            {
                _selectedChapterType = value;
                OnPropertyChanged("SelectedChapterType");
                var run = new ImportXML();
                ChapterNumber = run.GetChapterNumber(SelectedChapterType, SelectedImportFilePath);

            }
        }

        public List<string> ChapterNumber
        {
            get { return _chapterNumber; }
            set
            {
                _chapterNumber = value;
                OnPropertyChanged("ChapterNumber");
            }
        }

        public List<string> ChapterTypes
        {
            get { return _chapterTypes; }
            set
            {
                _chapterTypes = value;
                OnPropertyChanged("ChapterTypes");
            }
        }

        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                _projectName = value;
                OnPropertyChanged("ProjectName");
            }
        }

        public string ATR
        {
            get { return _atr; }
            set
            {
                _atr = value;
                OnPropertyChanged("ATR");
            }
        }

        public DateTime TenderDate
        {
            get { return _tenderDate; }
            set
            {
                _tenderDate = value;
                OnPropertyChanged("TenderDate");
            }
        }


        public DateTime CompletionDate
        {
            get { return _completionDate; }
            set
            {
                _completionDate = value;
                OnPropertyChanged("CompletionDate");
            }
        }

        public List<FilePathList> XMLFilePathList
        {
            get { return _xmlFilePathsList; }
            set
            {
                _xmlFilePathsList = value;
                OnPropertyChanged("XMLFilePathList");
            }
        }

        public ObservableCollection<DisciplineValues> ProjectCostData
        {
            get { return _projectCostData; }
            set
            {
                _projectCostData = value;
                OnPropertyChanged("ProjectCostData");
            }
        }

        public int SelectedProjectIndex
        {
            get { return _selectedProjectIndex; }
            set
            {
                _selectedProjectIndex = value;
                OnPropertyChanged("SelectedProject");
                getSelectedProjectData();
            }
        }

        public string SelectedImportFilePath
        {
            get { return _selectedImportFilePath; }
            set
            {
                _selectedImportFilePath = value;
                OnPropertyChanged("SelectedImportFilePath");
            }
        }

        public ICommand SelectImportFilePathCommand { get; private set; }
        public ICommand SaveChangesCommand { get; private set; }
        public ICommand ImportNewProject { get; private set; }

    }
}
