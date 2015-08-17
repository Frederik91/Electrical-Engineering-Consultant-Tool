using Microsoft.Win32;
using ProjectCostEstimator.Commands;
using ProjectCostEstimator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using System.Xml.XPath;

namespace ProjectCostEstimator.ViewModel
{
    class DataFileManagerViewModel : ViewModelBase
    {
        #region initializers

        private List<string> _chapterTypes = new List<string>();
        private List<string> _chapterNumber = new List<string>();

        private List<DisciplineValues> _importedDataList = new List<DisciplineValues>();
        private List<FilePathList> _xmlFilePathsList = new List<FilePathList>();
        private ObservableCollection<DisciplineValues> _projectCostData;

        private DateTime _tenderDate = new DateTime();
        private DateTime _completionDate = new DateTime();
        private DateTime _importTenderDate = new DateTime();
        private DateTime _importCompletionDate = new DateTime();


        private string _importProjectName;
        private string _importATR;
        private int _importArea;


        private string _currentProjectName;
        private string _selectedChapterType;
        private string _selectedImportFilePath;
        private string _projectName;
        private string _atr;
        private string _XMLPath = ConfigurationManager.AppSettings["XMLFolderPath"] + ConfigurationManager.AppSettings["XMLFileListName"] + ".xml";

        private int _selectedProjectIndex;
        private int _area;

        #endregion

        public DataFileManagerViewModel()
        {
            SelectedProjectIndex = 0;

            SaveChangesCommand = new DelegateCommand(o => SaveChanges());
            ImportNewProject = new DelegateCommand(o => ImportProjectFile());
            SelectImportFilePathCommand = new DelegateCommand(o => SelectImportFilePath());
            CompressListCommand = new DelegateCommand(o => CompressList());
            SaveImportDataCommand = new DelegateCommand(o => SaveImportData(SelectedImportFilePath));
            DeleteStoredProjectCommand = new DelegateCommand(o => DeleteStoredProject());

            collectXMLFilePaths();
        }

        #region Import File Methods

        private void SaveImportData(string xmlpath)
        {
            var filename = Interaction.InputBox("Enter new file name", "Save file as") + ".xml";
            var filePath = ConfigurationManager.AppSettings["XMLFolderPath"];
            var file = filePath + filename;

            if (filename == string.Empty)
            {
                MessageBox.Show("Filename cannot be empty");
                return;
            }

            if (File.Exists(file))
            {
                MessageBox.Show("Filename is already in use");
                return;
            }

            XElement xEle =
                new XElement("ProjectFile",
                    new XElement("BuildingInformation",
                        new XElement("ProjectName", ImportProjectName),
                        new XElement("ATR", ImportATR),
                        new XElement("Area", ImportArea),
                        new XElement("TenderDate", ImportTenderDate.ToShortDateString()),
                        new XElement("CompletionDate", ImportCompletionDate.ToShortDateString())
                    )
                );

            xEle.Add(new XElement("CostData", ""));
                foreach (var item in ImportedDataList)
                    {
                        xEle.Element("CostData").Add(new XElement("Chapter",
                                        new XElement("ChapterNumber", item.Chapter),
                                        new XElement("Cost", Math.Round(item.Cost)),
                                        new XElement("SqmCost", item.SqmCost)
                                        )
                        );
                     };



            xEle.Save(file);

            XElement FileList = XElement.Load(_XMLPath);

            FileList.Add(new XElement("ProjectFile", new XAttribute("Name", ImportProjectName), new XAttribute("Path", file)));

            FileList.Save(_XMLPath);            

            

            collectXMLFilePaths();

            MessageBox.Show("File successfully saved");

        }

        private void CompressList()
        {
            var l = ImportedDataList.GroupBy(o => o.Chapter)
                          .Select(grp => new DisciplineValues
                          {
                              Chapter = grp.Key,
                              Cost = grp.Sum(o => o.Cost),
                              SqmCost = grp.Sum(o => o.SqmCost),
                              Comment = ""
                          });

            ImportedDataList = l.ToList();

        }

        private void ImportProjectFile()
        {
            var run = new ImportXML();
            ImportedDataList = run.ImportProjectFile(SelectedChapterType, SelectedImportFilePath, ImportArea);
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
            ImportProjectName = run.GetImportFileName(SelectedImportFilePath);
            ImportTenderDate = ImportCompletionDate = run.GetImportDate(SelectedImportFilePath);

        }



        #endregion


        #region Import File Properties

        public string ImportProjectName
        {
            get { return _importProjectName; }
            set
            {
                _importProjectName = value;
                OnPropertyChanged("ImportProjectName");
            }
        }

        public string ImportATR
        {
            get { return _importATR; }
            set
            {
                _importATR = value;
                OnPropertyChanged("ImportATR");
            }
        }

        public DateTime ImportTenderDate
        {
            get { return _importTenderDate; }
            set
            {
                _importTenderDate = value;
                OnPropertyChanged("ImportTenderDate");
            }
        }


        public DateTime ImportCompletionDate
        {
            get { return _importCompletionDate; }
            set
            {
                _importCompletionDate = value;
                OnPropertyChanged("ImportCompletionDate");
            }
        }


        public int ImportArea
        {
            get { return _importArea; }
            set
            {
                _importArea = value;
                OnPropertyChanged("ImportArea");
            }
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


        public string SelectedImportFilePath
        {
            get { return _selectedImportFilePath; }
            set
            {
                _selectedImportFilePath = value;
                OnPropertyChanged("SelectedImportFilePath");
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

        public ICommand SaveImportDataCommand { get; private set; }
        public ICommand CompressListCommand { get; private set; }
        public ICommand SelectImportFilePathCommand { get; private set; }
        public ICommand ImportNewProject { get; private set; }

        #endregion


        #region Stored Files Methods

        private void collectXMLFilePaths()
        {
            int selectedIndex = SelectedProjectIndex;

            var list = new List<FilePathList>();

            XmlReader xmlReader = XmlReader.Create(_XMLPath);
            while (xmlReader.Read())
            {
                if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "ProjectFile"))
                {
                    if (xmlReader.HasAttributes)
                    {
                        list.Add(new FilePathList() { Name = xmlReader.GetAttribute("Name"), FilePath = xmlReader.GetAttribute("Path") });
                    }
                }
            }

            XMLFilePathList = list;

            SelectedProjectIndex = selectedIndex;

            xmlReader.Close();
        }

        private void DeleteStoredProject()
        {
            var yes = MessageBox.Show("Do you want to delete '" + ProjectName + "'? This can not be undone.", "Warning", MessageBoxButton.YesNo);

            if (yes == MessageBoxResult.No)
            {
                return;
            }

            XElement xdoc = XElement.Load(_XMLPath);

            var elementsToRemove = from element in xdoc.Elements("ProjectFile")
                                   where element.Attribute("Name").Value == _currentProjectName
                                   select element;

            foreach (var e in elementsToRemove)
            {
                File.Delete(e.Attribute("Path").Value);
                e.Remove();
            }
            xdoc.Save(_XMLPath);
            
            collectXMLFilePaths();


        }

        private void getSelectedProjectData()
        {
            _projectCostData = new ObservableCollection<DisciplineValues>();

            if (XMLFilePathList.Count > 0 && SelectedProjectIndex >= 0) 
            {
                XDocument xdocument = XDocument.Load(XMLFilePathList[SelectedProjectIndex].FilePath);
                IEnumerable<XElement> ProjectInfoFile = xdocument.Elements();
                foreach (var info in ProjectInfoFile)
                {
                    ProjectName = info.Element("BuildingInformation").Element("ProjectName").Value;
                    ATR = info.Element("BuildingInformation").Element("ATR").Value;
                    Area = Convert.ToInt32(info.Element("BuildingInformation").Element("Area").Value);
                    TenderDate = DateTime.ParseExact(info.Element("BuildingInformation").Element("TenderDate").Value, "dd.MM.yyyy", null);
                    CompletionDate = DateTime.ParseExact(info.Element("BuildingInformation").Element("CompletionDate").Value, "dd.MM.yyyy", null);
                }

                foreach (XElement element in xdocument.Descendants("Chapter"))
                {
                    _projectCostData.Add(new DisciplineValues
                    {
                        Chapter = element.Element("ChapterNumber").Value,
                        Cost = Convert.ToInt32(element.Element("Cost").Value),
                        SqmCost = Convert.ToDouble(element.Element("SqmCost").Value)
                    });
                }

                ProjectCostData = _projectCostData;

                _currentProjectName = ProjectName;
            }
        }


        private void SaveChanges()
        {
            if (SelectedProjectIndex < 0)
            {
                MessageBox.Show("Project not selected, select project in list view before attempting to save.");
                return;
            }

            XElement xEle = XElement.Load(XMLFilePathList[SelectedProjectIndex].FilePath);

            xEle.Element("BuildingInformation").SetElementValue("ProjectName", ProjectName);
            xEle.Element("BuildingInformation").SetElementValue("ATR", ATR);
            xEle.Element("BuildingInformation").SetElementValue("Area", Area);
            xEle.Element("BuildingInformation").SetElementValue("TenderDate", TenderDate.ToShortDateString());
            xEle.Element("BuildingInformation").SetElementValue("CompletionDate", CompletionDate.ToShortDateString());

            xEle.Element("CostData").RemoveAll();

            xEle.Save(XMLFilePathList[SelectedProjectIndex].FilePath);

            foreach (var item in ProjectCostData)
            {
                xEle.Element("CostData").Add(new XElement("Chapter", new XElement("ChapterNumber", item.Chapter), new XElement("Cost", item.Cost), new XElement("SqmCost", item.Cost / Area)));
            }

            xEle.Save(XMLFilePathList[SelectedProjectIndex].FilePath);


            XElement xdoc = XElement.Load(_XMLPath);

            var elementsToRename = from element in xdoc.Elements("ProjectFile")
                                   where element.Attribute("Name").Value == _currentProjectName
                                   select element;

            foreach (var e in elementsToRename)
            {
                e.SetAttributeValue("Name", ProjectName);
            }
            xdoc.Save(_XMLPath);

            collectXMLFilePaths();

        }

        #endregion


        #region Stored Files properties

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


        public int Area
        {
            get { return _area; }
            set
            {
                _area = value;
                OnPropertyChanged("Area");
            }
        }

        public ICommand DeleteStoredProjectCommand { get; private set; }

        #endregion


        #region Common properties

        public List<FilePathList> XMLFilePathList
        {
            get { return _xmlFilePathsList; }
            set
            {
                _xmlFilePathsList = value;
                OnPropertyChanged("XMLFilePathList");
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

        public ICommand SaveChangesCommand { get; private set; }

        #endregion


    }
}
